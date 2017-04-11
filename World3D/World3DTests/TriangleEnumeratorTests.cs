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
            foreach(Triangle t in cube)
            {
                num++;
            }
            Assert.AreEqual(num, 2 * 6);
        }

        [TestMethod()]
        public void TriangleStripEnumeratorTest()
        {
            Cube cube = new ColouredCube();
            int num = 0;
            foreach (Triangle t in cube)
            {
                num++;
            }
            Assert.AreEqual(num, 2 * 6);
        }

    }
}