using MyGraph;
using NUnit.Framework;

namespace NUnitTests
{
    [TestFixture]
    public class AStartTest
    {
        [Test]
        public void FindPath_5_Vertives_1_Path()
        {
            var graph = new Graph(5);
            graph.AddEdge(0,3);
            graph.AddEdge(3,2);
            graph.AddEdge(2,1);
            graph.AddEdge(3,4);
            graph.AddEdge(2,4);
            graph.AddEdge(1,4);

            var result = graph.FindPath(0, 4);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result[4]);
            Assert.AreEqual(0, result[3]);
        }

        [Test]
        public void FindPath_5_Vertives_2_Path()
        {
            var graph = new Graph(5);
            graph.AddEdge(0, 3);
            graph.AddEdge(3, 2);
            graph.AddEdge(2, 1);
            graph.AddEdge(3, 4);
            graph.AddEdge(2, 4);
            graph.AddEdge(1, 4);
            graph.AddEdge(0, 2);

            var result = graph.FindPath(0, 4);

            Assert.IsNotNull(result);
            Assert.IsTrue((3 == result[4] && 0 == result[3]) || (2 == result[4] && 0 == result[2]));
        }

        [Test]
        public void FindPath_5_Vertices_0_Path()
        {
            var graph = new Graph(5);
            graph.AddEdge(3, 2);
            graph.AddEdge(2, 1);
            graph.AddEdge(3, 4);
            graph.AddEdge(2, 4);
            graph.AddEdge(1, 4);

            var result = graph.FindPath(0, 4);

            Assert.IsNotNull(result);
            Assert.IsNull(result[4]);
        }
    }
}