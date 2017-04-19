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
    public class TerrainTests
    {
        [TestMethod()]
        public void RecreateTest()
        {
            TerrainInfo info = new TerrainInfo()
            {
                BottomLatitude = -37.0,
                LeftLongitude = 174.0,
                LatRows = 1201,
                LongColumns = 1201,
                MetresPerDegreeLatitude = 90
            };
        }
    }
}