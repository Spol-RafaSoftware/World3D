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
    public class Srtm3ReaderTests
    {


        [TestMethod()]
        public void GetMapTest_1x1()
        {
            AltitudeMapReader reader = new Srtm3Reader();

            Vector2 southWest = new Vector2(-37, 173);
            Vector2 northEast = new Vector2(-36, 174);

            Vector2 diff = northEast - southWest;


            Vector3[,] h = reader.GetMap(southWest, northEast);

            Assert.AreEqual(diff.X * reader.TileHeight, (float)h.GetLength(0), 0.01);
            Assert.AreEqual(diff.Y * reader.TileWidth, (float)h.GetLength(1), 0.01);

        }


        [TestMethod()]
        public void GetTiledAltitudeMapTest()
        {
            TiledAltitudeMapReader reader = new TiledAltitudeMapReader();
     

            Vector2 southWest = new Vector2(-37, 173);
            Vector2 northEast = new Vector2(-36.3f, 173.6f);

            Vector2 diff = northEast - southWest;

            

            Vector3[,] h = reader.GetMap(southWest, northEast);
            Assert.AreEqual(diff.X * reader.TileHeight / reader.DegreesLatitudePerTile, (float)h.GetLength(0), 0.01);
            Assert.AreEqual(diff.Y * reader.TileWidth / reader.DegreesLongitudePerTile, (float)h.GetLength(1), 0.01);
        }


        [TestMethod()]
        public void GetSrtmNZMapTest()
        {
            TiledAltitudeMapReader reader = new Srtm3Reader();


            Vector2 southWest = new Vector2(-37, 173);
            Vector2 northEast = new Vector2(-36f, 174);

            Vector2 diff = northEast - southWest;



            Vector3[,] h = reader.GetMap(southWest, northEast);
            Assert.AreEqual(diff.X * reader.TileHeight / reader.DegreesLatitudePerTile, (float)h.GetLength(0), 0.01);
            Assert.AreEqual(diff.Y * reader.TileWidth / reader.DegreesLongitudePerTile, (float)h.GetLength(1), 0.01);
        }
    }
}