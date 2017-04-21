using Microsoft.VisualStudio.TestTools.UnitTesting;
using World3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace World3D.Tests
{
    [TestClass()]
    public class SquareGridTests
    {
        [TestMethod()]
        public void CreateSquareGridTest_1x1()
        {
            int xColumns = 1;
            int zRows = 1;
            float xPitch = 10;
            float zPitch = 10;

            Vector3[] vertices;
            int[] indices;
            BeginMode drawMode;

            SquareGrid.CreateSquareGrid(xColumns, zRows, xPitch, zPitch, out vertices, out indices, out drawMode);

            Assert.AreEqual(BeginMode.TriangleStrip, drawMode);
            Assert.AreEqual(4, vertices.Length);
            Assert.AreEqual(4, indices.Length);

            Vector3[] expectedV = new Vector3[]
            {
                new Vector3(0,0,0),
                new Vector3(xPitch, 0, 0),
                new Vector3(0, 0, zPitch),
                new Vector3(xPitch,0,zPitch)
            };
            int[] expectedI = new int[] { 0, 2, 1, 3 };


            for (int i = 0; i < vertices.Length; i++)
            {
                Assert.AreEqual(expectedV[i], vertices[i]);
                Assert.AreEqual(expectedI[i], indices[i]);
            }
        }
        [TestMethod()]
        public void SquareGridEnumeratorTest()
        {
            int xColumns = 2;
            int zRows = 2;
            float xPitch = 10;
            float zPitch = 10;

            SquareGrid grid = new SquareGrid(xColumns, zRows, xPitch, zPitch);

            int [] touchedVerts = new int[grid.Vertices.Length];

            foreach (IndexedTriangle tri in grid)
            {
                touchedVerts[tri.i0]++;
                touchedVerts[tri.i1]++;
                touchedVerts[tri.i2]++;
            }
            foreach(int i in touchedVerts)
            {
                Assert.AreNotEqual(0, i);
            }
        }

        [TestMethod()]
        public void SquareGridTest_2x3()
        {
            int xColumns = 2;
            int zRows = 3;
            float xPitch = 10;
            float zPitch = 10;

            SquareGrid grid = new SquareGrid(xColumns, zRows, xPitch, zPitch);

            int[] touchedVerts = new int[grid.Vertices.Length];

            foreach (IndexedTriangle tri in grid)
            {
                touchedVerts[tri.i0]++;
                touchedVerts[tri.i1]++;
                touchedVerts[tri.i2]++;
            }
            foreach (int i in touchedVerts)
            {
                Assert.AreNotEqual(0, i);
            }
        }

        [TestMethod()]
        public void SquareGridTest_11x12()
        {
            int xColumns = 11;
            int zRows = 12;
            float xPitch = 10;
            float zPitch = 10;

            SquareGrid grid = new SquareGrid(xColumns, zRows, xPitch, zPitch);

            int[] touchedVerts = new int[grid.Vertices.Length];

            foreach (IndexedTriangle tri in grid)
            {
                touchedVerts[tri.i0]++;
                touchedVerts[tri.i1]++;
                touchedVerts[tri.i2]++;
            }
            foreach (int i in touchedVerts)
            {
                Assert.AreNotEqual(0, i);
            }
        }
    }


}