using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enemies;
using Domain.Enemies.Motion;
using Domain.Players;
using Domain.Treatment;
using Domain.Weapons;
using Domain.WorldElements;

namespace Domain
{
    public class World
    {
        private BoardGraph _graph;
        public int Width { get; set; }
        public int Height { get; set; }
        public int FieldSize
        {
            get { return Board.FieldSize; }
            set { Board.FieldSize = value; }
        }
        public int Yoffset
        {
            get { return Board.Yoffset; }
            set { Board.Yoffset = value; }
        }

        public PlayerBase Player { get; private set; }
        public ICollection<Enemy> Enemies { get; private set; }
        public ICollection<Wall> Walls { get; private set; }
        public ICollection<Bomb> Bombs { get; private set; }
        public ICollection<BombSet> BombSets { get; private set; }
        public ICollection<AidKit> AidKits { get; private set; }
        public Door Door { get; private set; }
        public Key Key { get; private set; }
        public Board Board { get; private set; }

        public IEnumerable<WorldElement> WorldElements
        {
            get
            {
                foreach (var enemy in Enemies)
                    yield return enemy;
                foreach (var wall in Walls)
                    yield return wall;
                yield return Door;
                if(Key != null) yield return Key;
                foreach (var bomb in Bombs)
                    yield return bomb;
                foreach (var bombSet in BombSets)
                    yield return bombSet;
                foreach (var aidKit in AidKits)
                    yield return aidKit;
                yield return Player;
            }
        }

        public event Action<WorldElement> WorldElementRemoved;
        public event Action GameWin;
        public event Action GameLost;
        public event Action KeyCollected;
        public event Action BombExploding;
        public event Action PlayerAttacked;
        protected World()
        {
            Bombs = new List<Bomb>();
            BombSets = new List<BombSet>();
            AidKits = new List<AidKit>();
        }

        public World(int width, int height, 
            PlayerBase player, 
            ICollection<Enemy> enemies, 
            ICollection<Wall> walls, 
            ICollection<BombSet> bombSets,
            ICollection<AidKit> aidKits, 
            Key key,
            Door door) : this()
        {
            BombSets = bombSets ?? new List<BombSet>();
            AidKits = aidKits ?? new List<AidKit>();
            Width = width;
            Height = height;
            Player = player;
            Enemies = enemies;
            Walls = walls;
            Key = key;
            Door = door;

            Board = new Board(Width, Height);
            foreach (var worldElement in WorldElements)
                Board[worldElement.Coordinates] = worldElement;

            Board.BuildGraph(Walls);
            foreach (var enemy in Enemies)
            {
                enemy.Dead += e =>
                {
                    Enemies.Remove(e as Enemy);
                    RemoveFromBoard(e);
                };
            }

            Player.Dead += p => OnGameLost();
        }

        public IEnumerable<WorldElement> DetectCollision(WorldElement worldElement, Coordinates coordinates)
        {
            return WorldElements.Where(w => w != worldElement).Where(element => Intersected(coordinates, element));
        }

        public void MovePlayer(Coordinates direction)
        {
            var sum = Player.Coordinates + direction;
            if(sum.X < 0 || sum.Y < Yoffset || sum.X > Width - FieldSize || sum.Y > Height - FieldSize)
                return;

            var worldElements = DetectCollision(Player, Player.Coordinates + direction*Player.Speed).ToList();
            if(worldElements.OfType<Wall>().Any() || worldElements.OfType<Koala>().Any()) return;

            foreach (var worldElement in worldElements)
            {
                var key = worldElement as Key;
                if (key != null)
                {
                    Player.CollectKey(key);
                    RemoveFromBoard(key);
                    Key = null;
                    OnKeyCollected();
                }

                var door = worldElement as Door;
                if (door != null && Player.Key != null && door.Open(Player.Key)) OnGameWin();

                var enemy = worldElement as Enemy;
                if (enemy != null)
                {
                    if(enemy.Attack(Player) > 0)OnPlayerAttacked();
                }

                var bombSet = worldElement as BombSet;
                if (bombSet != null)
                {
                    Player.CollectBombs(bombSet.Bombs);
                    BombSets.Remove(bombSet);
                    RemoveFromBoard(bombSet);
                }

                var aidKit = worldElement as AidKit;
                if (aidKit != null)
                {
                    Player.UseAidKit(aidKit);
                    AidKits.Remove(aidKit);
                    RemoveFromBoard(aidKit);
                }
            }

            Player.Move(direction);
        }

        public void MoveEnemies()
        {
            foreach (var enemy in Enemies)
            {
                var c = enemy.Coordinates.Clone() as Coordinates;
                Board[c] = null;
                IList<Coordinates> path = null;
                var kangaroo = enemy as Kangaroo;
                if (kangaroo != null)
                {
                    kangaroo.Speed = SpeedLevel.Normal;
                    path = Board.FindPath(Board.GetCoordinates(Player.Coordinates), Board.GetCoordinates(enemy.Coordinates));
                }

                if (path != null && path.Any())
                {
                    kangaroo.Speed = SpeedLevel.Fast;
                    enemy.Move(new List<Direction> { Board.GetCoordinates(c).GetDirection(path[0]) });
                }
                else enemy.Move(GetAvailableDirection(c));

                Board[enemy.Coordinates] = enemy;
            }
        }

        public bool PlaceBomb(Bomb bomb, Coordinates coordinates)
        {
            var element = Board[coordinates];
            if(element != null && element != Player) return false;

            Bombs.Add(bomb);
            bomb.Coordinates = QuantizeCoordinates(coordinates);
            Board.Add(bomb);

            bomb.Exploded += (sender, args) =>
            {
                OnBombExploding();
                Bombs.Remove(bomb);
                RemoveFromBoard(bomb);

                var neighbourhood = Board.GetNeighborhood(bomb.Coordinates);
                var playerCoord = Board.GetCoordinates(Player.Coordinates);
                if (Board.GetCoordinates(bomb.Coordinates).GetAdjacents().Any(c => c.Equals(playerCoord)))
                {
                    Player.ReceiveAttack(bomb.DestructionField);
                }
                foreach (var wall in neighbourhood.OfType<DestructibleWall>())
                {
                    RemoveFromBoard(wall);
                    Walls.Remove(wall);
                    Board.AddEdgesToGraph(wall.Coordinates);
                }
                foreach (var enemy in neighbourhood.OfType<Enemy>())
                {
                    enemy.ReceiveAttack(bomb.DestructionField);
                }
            };

            bomb.SetUp();
            return true;
        }

        private bool Intersected(Coordinates coordinates, WorldElement other)
        {
            return (other.Coordinates.X < coordinates.X + FieldSize &&
                     other.Coordinates.X + FieldSize > coordinates.X &&
                     other.Coordinates.Y < coordinates.Y + FieldSize &&
                     other.Coordinates.Y + FieldSize > coordinates.Y);
        }

        private void RemoveFromBoard(WorldElement worldElement)
        {
            Board.Remove(worldElement);
            OnWorldElementRemoved(worldElement);
        }

        private Coordinates QuantizeCoordinates(Coordinates coordinates)
        {
            int x = coordinates.X + FieldSize/2;
            int y = coordinates.Y + FieldSize/2;
            x /= FieldSize;
            y = (y - Yoffset)/FieldSize;

            return new Coordinates(x*FieldSize, y*FieldSize + Board.Yoffset);
        }

        private IEnumerable<Direction> GetAvailableDirection(Coordinates start)
        {
            var coordinates = Board.GetCoordinates(start);
            var availableCoordinates = Board.Graph.GetEdges(coordinates.X, coordinates.Y);
            foreach (var coordinate in availableCoordinates)
            {
                var direction = coordinates.GetDirection(coordinate);
                if (direction != Direction.None) yield return direction;
            }
        }

        protected virtual void OnWorldElementRemoved(WorldElement obj)
        {
            var handler = WorldElementRemoved;
            if (handler != null) handler(obj);
        }

        protected virtual void OnGameWin()
        {
            var handler = GameWin;
            if (handler != null) handler();
        }

        protected virtual void OnKeyCollected()
        {
            var handler = KeyCollected;
            if (handler != null) handler();
        }

        protected virtual void OnGameLost()
        {
            var handler = GameLost;
            if (handler != null) handler();
        }

        protected virtual void OnBombExploding()
        {
            var handler = BombExploding;
            if (handler != null) handler();

        }

        protected virtual void OnPlayerAttacked()
        {
            var handler = PlayerAttacked;
            if (handler != null) handler();
        }
    }
}