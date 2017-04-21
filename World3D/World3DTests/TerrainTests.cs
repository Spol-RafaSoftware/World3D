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
    public class TerrainTests
    {
        [TestMethod()]
        public void RecreateTest()
        {
            int rows = 8;
            int cols = 10;
            TerrainInfo info = new TerrainInfo()
            {
                SouthWestLatLong = new Vector2(-37, 174),
                NorthEastLatLong = new Vector2(-36.999f, 174.001f),
                DegreesLatitudePerPixel = 0.001/(double)rows,
                DegreesLongitudePerPixel = 0.001/(double)cols
            };
            float[,] altInMetres = info.CreateFlatAltitudes(rows, cols);
            Terrain terrain = new Terrain();
            terrain.Recreate(info, altInMetres);

            SquareGrid grid = new SquareGrid(rows, cols, 1, 1);

            Assert.AreEqual(rows, terrain.LatRows);
            Assert.AreEqual(cols, terrain.LongColumns);
            Vector3 centreVertex = terrain.Vertices[terrain.Vertices.Length/2];
            VectorAssertions.AreEqual(new Vector3(), centreVertex, 0.1, "Centre vertex does not match");
        }
        
    }
}