using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain;
using Domain.DTO;
using Domain.Enemies;
using Domain.Enemies.Factories;
using Domain.Players;
using Domain.Serialization;
using Domain.Treatment;
using Domain.Weapons;
using Domain.WorldElements;

namespace GameLibrary.WorldManagement
{
    public interface IWorldBuilder
    {
        PlayerBase Player { get; set; }
        ICollection<Enemy> Enemies { get; set; }
        ICollection<Wall> Walls { get; set; }
        int NumberOfEnemies { get; set; }
        int NumberOfWalls { get; set; }
        WorldDTO WorldDto { get; set; }
        ushort? PlayerHitpoints { get; set; }
        World Build();
        void FromFile();
        bool FromFile(String path);
        void GenerateBombSets();
        void GenerateAidKits();
    }

    public class WorldBuilder : IWorldBuilder
    {
        private String _path;
        private readonly IWorldSerializer _worldSerializer;
        private readonly IEnemyFactory _enemiesFactory;
        private bool _fromFile;
        private WorldDTO _worldDto;
        public PlayerBase Player { get; set; }
        public ICollection<Enemy> Enemies { get; set; }
        public ICollection<Wall> Walls { get; set; }
        public int NumberOfEnemies { get; set; }
        public IList<BombSet> BombSets { get; set; }
        public List<AidKit> AidKits { get; set; }
        public int NumberOfWalls { get; set; }

        public WorldDTO WorldDto
        {
            get { return _worldDto; }
            set
            {
                _worldDto = value;
                _fromFile = _worldDto == null;
            }
        }

        public ushort? PlayerHitpoints { get; set; }

        public WorldBuilder(String path, IEnemyFactory enemyFactory, IWorldSerializer worldSerializer)
        {
            _path = path;
            _worldSerializer = worldSerializer;
            _enemiesFactory = enemyFactory;
            Enemies = new List<Enemy>();
            Walls = new List<Wall>();
        }

        public World Build()
        {
            if (_fromFile) WorldDto = _worldSerializer.Deserialize(_path);
            if(PlayerHitpoints != null) WorldDto.Player.Hitpoints = PlayerHitpoints.Value;
            var player = BuildPlayer(WorldDto.Player);
            var enemies = BuildEnemies(WorldDto.Enemies).ToList();
            var walls = BuildWalls(WorldDto.Walls).ToList();
            Key key = null;
            if(WorldDto.Key != null) key = BuildKey(WorldDto.Key);
            var door = BuildDoor(WorldDto.Door);
            if (BombSets != null) PlaceBombSets();
            if (WorldDto.BombSets != null) BombSets = BuildBombSets(WorldDto.BombSets).ToList();
            if (AidKits != null) PlaceAidKits();
            if (WorldDto.AidKits != null) AidKits = BuildAidKits(WorldDto.AidKits).ToList();

            return new World(WorldDto.Width, WorldDto.Height, player, enemies, walls, BombSets, AidKits, key, door);
        }

        private void PlaceAidKits()
        {
            PlaceElements(AidKits);
        }

        private void PlaceBombSets()
        {
            PlaceElements(BombSets);
        }

        private void PlaceElements(IEnumerable<WorldElement> worldElements)
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            foreach (var worldElement in worldElements)
            {
                Coordinates c = null;
                do
                {
                    c = new Coordinates(rand.Next(WorldDto.Width), rand.Next(WorldDto.Height));
                } while (WorldDto.Walls.Any(w => w.Coordinates.Equals(c)) ||
                    WorldDto.Player.Coordinates.Equals(c) ||
                    WorldDto.Door.Coordinates.Equals(c) ||
                    (WorldDto.Key != null && WorldDto.Key.Coordinates.Equals(c)) || 
                    (BombSets != null && BombSets.Any(w => w.Coordinates.Equals(c))) ||
                    (AidKits != null && AidKits.Any(w => w.Coordinates.Equals(c))));
                worldElement.Coordinates = c;
            }
        }

        private Player BuildPlayer(PlayerDTO playerDto)
        {
            var player = new Player(new PlayerMotion(),
                playerDto.Coordinates,
                playerDto.Bombs.Select(b => new Bomb()),
                playerDto.Hitpoints,
                playerDto.Key != null ? BuildKey(playerDto.Key) : null);

            return player;
        }

        private Key BuildKey(KeyDTO keyDto)
        {
            return new Key(keyDto.Key, keyDto.Coordinates);
        }

        private Door BuildDoor(DoorDTO doorDto)
        {
            return new Door(doorDto.Key, doorDto.Coordinates);
        }

        private IEnumerable<Wall> BuildWalls(IEnumerable<WallDTO> wallDtos)
        {
            return wallDtos.Select(w => w.IsDestructible ? new DestructibleWall(w.Coordinates) : new Wall(w.Coordinates));
        }

        private IEnumerable<Enemy> BuildEnemies(IEnumerable<EnemyDTO> enemyDtos)
        {
            return enemyDtos.Select(e => _enemiesFactory.Create(e)).ToList();
        }

        private IEnumerable<BombSet> BuildBombSets(IEnumerable<BombSetDTO> bombSets)
        {
            return bombSets.Select(bs => new BombSet(bs.Coordinates, bs.Bombs.Select(b => new Bomb())));
        }

        private IEnumerable<AidKit> BuildAidKits(IEnumerable<AidKitDTO> aidKits)
        {
            return aidKits.Select(ak => new AidKit(ak.Coordinates, new CureStrategy()));
        } 

        public void FromFile()
        {
            _fromFile = true;
        }

        public bool FromFile(string path)
        {
            _path = path;
            _fromFile = true;
            var file = new FileInfo(_path);
            return file.Exists;
        }

        public void GenerateBombSets()
        {
            BombSets = new List<BombSet>();
            var numberOfBombSets = new Random((int) DateTime.Now.Ticks).Next(3);
            for (int i = 0; i < numberOfBombSets; i++)
            {
                var bombSet = new BombSet(new Coordinates(), 3);
                BombSets.Add(bombSet);
            }
        }

        public void GenerateAidKits()
        {
            AidKits = new List<AidKit>();
            var numberOfBombSets = new Random((int)DateTime.Now.Ticks).Next(3);
            for (int i = 0; i < numberOfBombSets; i++)
            {
                var aidKit = new AidKit(new Coordinates(), new CureStrategy());
                AidKits.Add(aidKit);
            }
        }
    }
}