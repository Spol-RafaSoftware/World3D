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
    public class ArrayFillerTests
    {


        [TestMethod()]
        public void Fill2DArrayTest()
        {
            int[,] a1 = new int[5, 3];
            int[,] a2 = new int[5, 3];
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 5; y++)
                {
                    a1[y, x] = y * 10 + x;
                    a2[y, x] = y * 10 + x + 3;
                }
            int[,] b = new int[10, 3];
            ArrayFiller.Fill2DArray(b, a1, 0, 0);
            ArrayFiller.Fill2DArray(b, a2, 5, 0);

            for(int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 5; y++)
                    Assert.AreEqual(y * 10 + x, b[y, x]);
            }
        }
    }
}