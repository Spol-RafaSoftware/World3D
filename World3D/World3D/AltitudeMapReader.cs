using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World3D
{
    public interface AltitudeMapReader
    {
        Vector3[,] GetMap(Vector2 bottomLeftLatLong, Vector2 topRightLatLong);
        int TileWidth { get; }
        int TileHeight { get; }
        double DegreesLatitudePerTile { get; }
        double DegreesLongitudePerTile { get; }
    }

    public class LLA 
    {
        Vector3 lla;
    }

    public class TiledAltitudeMapReader : AltitudeMapReader
    {
        /// <summary>
        /// Height in pixels per Tile
        /// </summary>
        public int TileHeight { get; protected set; } = 50;
        /// <summary>
        /// Width in pixels per Tile
        /// </summary>
        public int TileWidth { get; protected set; } = 80;
        public double DegreesLatitudePerTile { get; protected set; } = 0.1;
        public double DegreesLongitudePerTile { get; protected set; } = 0.2;
        



        /// <summary>
        /// Gets a map that covers the given latitude/longitudes all as a single 2D array.
        /// </summary>
        /// <param name="bottomLatitude"></param>
        /// <param name="topLatitude"></param>
        /// <param name="leftLongitude"></param>
        /// <param name="rightLongitude"></param>
        /// <returns></returns>
        public virtual Vector3[,] GetMap(Vector2 southWestLatLong, Vector2 northEastLatLong)
        {

            int xTiles = (int)Math.Max(1.0f, (northEastLatLong - southWestLatLong).X / DegreesLatitudePerTile);
            int zTiles = (int)Math.Max(1.0f, (northEastLatLong - southWestLatLong).Y / DegreesLongitudePerTile);
            Vector3[,] ret = new Vector3[xTiles * TileHeight, zTiles * TileWidth];
            for (int z = 0; z < zTiles; z++)
                for (int x = 0; x < xTiles; x++)
                {
                    Vector2 src = southWestLatLong + new Vector2(x * (float)DegreesLatitudePerTile, z * (float)DegreesLongitudePerTile);
                    Vector3[,] temp = GetTile(src);
                    ArrayFiller.Fill2DArray(ret, temp, x * TileHeight, z * TileWidth);
                }

            return ret;
        }

        /// <summary>
        /// Gets a map covering 1 degree from the given latitude/longitude.
        /// </summary>
        /// <param name="bottomLatitude"></param>
        /// <param name="leftLongitude"></param>
        /// <returns></returns>
        public virtual Vector3[,] GetTile(Vector2 southWestLatLong)
        {
            Vector3[,] ret = new Vector3[TileHeight, TileWidth];
            FillTile(ret, southWestLatLong);


            return ret;
        }

        protected virtual void FillTile(Vector3[,] tile, Vector2 southWestLatLong)
        {
            InsertBlankMap(tile, southWestLatLong);
        }

        protected void InsertBlankMap(Vector3[,] tile, Vector2 southWestLatLong)
        {
            short alt = 0;
            for (int h = 0; h < TileHeight; h++)
            {
                for (int w = 0; w < TileWidth; w++)
                {
                    Vector2 latLong = CreateLatLongFromTileOffsets(southWestLatLong, h, w);

                    tile[h, w] = new Vector3(latLong.X, latLong.Y, alt);
                }
            }
        }

        protected Vector2 CreateLatLongFromTileOffsets(Vector2 southWestLatLong, int h, int w)
        {
            double latOffset = h * DegreesLatitudePerTile / TileHeight;
            double longOffset = w * DegreesLongitudePerTile / TileWidth;
            Vector2 latLong = new Vector2((float)(southWestLatLong.X + latOffset),
                (float)(southWestLatLong.Y + longOffset));
            return latLong;
        }
    }

    public class CosineAltitudeMapReader : TiledAltitudeMapReader
    {
        protected override void FillTile(Vector3[,] tile, Vector2 southWestLatLong)
        {
            for (int h = 0; h < TileHeight; h++)
            {
                for (int w = 0; w < TileWidth; w++)
                {
                    Vector2 latLong = CreateLatLongFromTileOffsets(southWestLatLong, h, w);
                    double alt = 1000*Math.Cos((double)h / TileHeight * Math.PI * 2) * Math.Cos((double)w / TileWidth * Math.PI * 2);

                    tile[h, w] = new Vector3(latLong.X,  latLong.Y, (float)alt);
                }
            }
        }
    }
}
