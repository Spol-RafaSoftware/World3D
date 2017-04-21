using Microsoft.VisualStudio.TestTools.UnitTesting;
using World3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World3D.Tests
{
    [TestClass()]
    public class TriangleEnumeratorTests
    {
        [TestMethod()]
        public void TriangleEnumeratorTest()
        {
            Cube cube = new Cube();
            int num = 0;
            int[] touchedVertices = new int[cube.Vertices.Length];
            foreach(IndexedTriangle t in cube)
            {
                num++;
                touchedVertices[t.i0]++;
                touchedVertices[t.i1]++;
                touchedVertices[t.i2]++;
            }
            Assert.AreEqual(num, 2 * 6);
            foreach (int i in touchedVertices)
            {
                Assert.AreNotEqual(0, i);
                Assert.IsTrue(i % 3 == 0);
            }
        }

        [TestMethod()]
        public void TriangleStripEnumeratorTest()
        {
            Cube cube = new ColouredCube();
            int num = 0;
            int[] touchedVertices = new int[cube.Vertices.Length];
            foreach (IndexedTriangle t in cube)
            {
                num++;
                touchedVertices[t.i0]++;
                touchedVertices[t.i1]++;
                touchedVertices[t.i2]++;
            }
            Assert.AreEqual(num, 2 * 6);
            foreach (int i in touchedVertices)
            {
                Assert.AreNotEqual(0, i);
            }
        }

        

    }
}