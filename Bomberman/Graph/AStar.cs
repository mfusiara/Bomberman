using System.Linq;

namespace MyGraph
{
    public static class AStar
    {
        public static int?[] FindPath(this Graph graph, int start, int end)
        {
            var distances = new int[graph.VerticesCount];
            var last = new int?[graph.VerticesCount];
            var estimations = new int[graph.VerticesCount];
            int weight = 1;

            for (int i = 0; i < graph.VerticesCount; i++)
            {
                estimations[i] = graph.VerticesCount;
                distances[i] = int.MaxValue/2;
                last[i] = null;
            }

            distances[start] = 0;
            last[start] = start;

            var T = Enumerable.Range(0, graph.VerticesCount).ToList();

            while (T.Any())
            {
                int u = T[0];
                int min = int.MaxValue;
                foreach (var i in T)
                {
                    if (min > distances[i] + estimations[i])
                    {
                        min = distances[i] + estimations[i];
                        u = i;
                    }
                }

                T.Remove(u);
                if(u == end) break;

                foreach (int w in graph.EdgesList[u])
                {
                    if (distances[w] > distances[u] + weight)
                    {
                        distances[w] = distances[u] + weight;
                        last[w] = u;
                    }
                }
            }

            return last;

        }
    }
}