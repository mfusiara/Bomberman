using System.Collections.Generic;
using System.Linq;
using Domain.WorldElements;
using MyGraph;

namespace Domain
{
    public class BoardGraph
    {
        private readonly int _boardWidth;
        private readonly int _boardHeight;
        protected Graph Graph;

        public BoardGraph(int boardWidth, int boardHeight)
        {
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;
            Graph = new Graph(_boardWidth * _boardHeight);
        }

        public void AddEdge(int startX, int startY, int endX, int endY)
        {
            int first = ConvertToVerticeNumber(startX, startY);
            int second = ConvertToVerticeNumber(endX, endY);
            Graph.AddEdge(first, second);
        }

        public void RemoveEdges(Coordinates coordinates)
        {
            int v = ConvertToVerticeNumber(coordinates.X, coordinates.Y);
            Graph.RemoveEdges(v);
        }

        public IEnumerable<Coordinates> GetEdges(int x, int y)
        {
            int v = ConvertToVerticeNumber(x, y);
            return Graph.EdgesList[v].Select(ConvertVerticeNumberToCoordinates);
        }

        public List<Coordinates> FindPath(Coordinates start, Coordinates end)
        {
            int s = ConvertToVerticeNumber(start);
            int e = ConvertToVerticeNumber(end);

            var path = Graph.FindPath(s, e);

            var boardPath = new List<Coordinates>();
            for (int i = e; i != s; i = path[i].Value)
            {
                if (path[i] == null) return null;
                boardPath.Add(ConvertVerticeNumberToCoordinates(path[i].Value));
            }

            return boardPath;
        }

        private int ConvertToVerticeNumber(Coordinates coordinates)
        {
            return ConvertToVerticeNumber(coordinates.X, coordinates.Y);
        }

        private int ConvertToVerticeNumber(int x, int y)
        {
            return y * _boardWidth + x;
        }

        private Coordinates ConvertVerticeNumberToCoordinates(int v)
        {
            int y = v/_boardWidth;
            int x = v - y*_boardWidth;
            return new Coordinates(x, y);
        }

        
    }
}