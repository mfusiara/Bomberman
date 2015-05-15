using System;
using MyGraph;
using NUnit.Framework;


namespace NUnitTests
{
    [TestFixture]
    public class GraphTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NewGraph_NumberOfVerticesLowerThan0()
        {
            var graph = new Graph(-1);
        }

        [Test]
        public void AddEdge()
        {
            var graph = new Graph(2);
            graph.AddEdge(0, 1);

            Assert.AreEqual(graph.EdgesCount, 1);
        }

        [Test]
        public void RemoveEdge()
        {
            var graph = new Graph(2);
            var result = graph.RemoveEdge(0, 1);

            Assert.IsFalse(result);
        }
    }
}