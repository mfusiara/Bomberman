using System.Collections.Generic;
using System.Linq;
using Domain.Weapons;
using Domain.WorldElements;

namespace Domain
{
    public class Board
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int FieldSize { get; set; }
        public int Yoffset { get; set; }
        private WorldElement[,] _board;
        public BoardGraph Graph { get; private set; }

        public Board(int width, int height)
        {
            FieldSize = 1;
            Width = width;
            Height = height;
            _board = new WorldElement[Width, Height];
        }

        public WorldElement this[Coordinates coordinates]
        {
            get
            {
                var c = GetCoordinates(coordinates);
                return _board[c.X, c.Y];
            }
            set
            {
                var c = GetCoordinates(coordinates);
                _board[c.X, c.Y] = value;
                if(value is Bomb) Graph.RemoveEdges(c);
            }
        }

        public WorldElement this[int x, int y]
        {
            get { return this[new Coordinates(x, y)]; }
            set { this[new Coordinates(x, y)] = value; }
        }

        public void Add(WorldElement worldElement)
        {
            this[worldElement.Coordinates] = worldElement;
        }

        public Coordinates GetCoordinates(Coordinates coordinates)
        {
            var c = coordinates + FieldSize/2;
            var result = new Coordinates(c.X/FieldSize, (c.Y - Yoffset)/FieldSize);
            return result;
        }

        public void BuildGraph(IEnumerable<Wall> walls)
        {
            Graph = new BoardGraph(Width, Height);

            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    if (i + 1 < Width) Graph.AddEdge(i, j, i + 1, j);
                    if (j + 1 < Height) Graph.AddEdge(i, j, i, j + 1);
                }
            }

            foreach (var wall in walls)
                Graph.RemoveEdges(wall.Coordinates);
        }

        public List<Coordinates> FindPath(Coordinates getBoardCoordinates, Coordinates coordinates)
        {
            return Graph.FindPath(getBoardCoordinates, coordinates);
        }

        public IEnumerable<WorldElement> GetNeighborhood(Coordinates coordinates)
        {
            var adjacentCootdinates = GetCoordinates(coordinates)
                                        .GetAdjacents()
                                        .Where(CheckIfNotOutOfRange)
                                        .ToList();

            foreach (var cootdinate in adjacentCootdinates)
            {
                var result = _board[cootdinate.X, cootdinate.Y];
                if(result != null) yield return result;
            }
        }

        public void AddEdgesToGraph(Coordinates coordinates)
        {
            var start = GetCoordinates(coordinates);

            var adjacentCootdinates = start.GetAdjacents()
                                           .Where(CheckIfNotOutOfRange)
                                           .ToList();
            foreach (var c in adjacentCootdinates)
            {
                var element = _board[c.X, c.Y];
                if(!(element is Wall)) Graph.AddEdge(start.X, start.Y, c.X, c.Y);
            }
        }

        private bool CheckIfNotOutOfRange(Coordinates coordinates)
        {
            return coordinates.X >= 0 && coordinates.X < Width && coordinates.Y >= 0 && coordinates.Y < Height;
        }

        public void Remove(WorldElement worldElement)
        {
            this[worldElement.Coordinates] = null;
            if(worldElement is Bomb) AddEdgesToGraph(worldElement.Coordinates);
        }
    }

}