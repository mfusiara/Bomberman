using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGraph
{
    public class Graph
    {
        public int VerticesCount { get; private set; }

        public int EdgesCount
        {
            get
            {
                var list = EdgesList.SelectMany(s => s);
                return list.Count()/2;
            }
        }

        public List<int>[] EdgesList { get; private set; }

        public Graph(int verticesCount)
        {
            if(verticesCount < 1)
                throw new ArgumentOutOfRangeException("The vertice is out of range");

            VerticesCount = verticesCount;
            EdgesList = new List<int>[verticesCount];
            for (int i = 0; i < VerticesCount; i++)
                EdgesList[i] = new List<int>();
        }

        public void AddEdge(int first, int second)
        {
            if (first >= VerticesCount || second >= VerticesCount)
                throw new ArgumentOutOfRangeException("The vertice is out of range");
            EdgesList[first].Add(second);
            EdgesList[second].Add(first);
        }

        public bool RemoveEdge(int first, int second)
        {
            if (first >= VerticesCount || second >= VerticesCount)
                throw new ArgumentOutOfRangeException("The vertice is out of range");

            if (!EdgesList[first].Contains(second) || !EdgesList[second].Contains(first))
                return false;

            EdgesList[first].Remove(second);
            EdgesList[second].Remove(first);

            return true;
        }

        public void RemoveEdges(int v)
        {
            if(v >= VerticesCount)
                throw new ArgumentOutOfRangeException("The vertice is out of range");

            foreach (var edge in EdgesList[v])
                EdgesList[edge].Remove(v);
            EdgesList[v].Clear();
        }
    }
}