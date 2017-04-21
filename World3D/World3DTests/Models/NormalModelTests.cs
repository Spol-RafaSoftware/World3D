using Microsoft.VisualStudio.TestTools.UnitTesting;
using World3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace World3D.Tests
{
    [TestClass()]
    public class NormalModelTests
    {
        [TestMethod()]
        public void CalculateNormalsGridTest()
        {
            Model grid = new SquareGrid(2, 2, 1, 1);
            Vector3[] normals = NormalModel.CalculateNormals(grid);
            foreach (Vector3 v in normals)
                if (float.IsNaN(v.X))
                {
                    Assert.IsTrue(float.IsNaN(v.Y));
                    Assert.IsTrue(float.IsNaN(v.Z));
                }
                else
                    VectorAssertions.AreEqual(Vector3.UnitY, v, 0.0001);
        }


        [TestMethod()]
        public void CalculateNormalsCubeTest()
        {
            Model cube = new ColouredCube();
            Vector3[] normals = NormalModel.CalculateNormals(cube);
            Vector3 sum = new Vector3();
            foreach (Vector3 v in normals)
                sum += v;

            VectorAssertions.AreEqual(new Vector3(), sum, 0.0001);
        }
    }
}