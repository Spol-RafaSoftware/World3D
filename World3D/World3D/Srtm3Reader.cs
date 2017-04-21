using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK;

namespace World3D
{


	/// <summary>
	/// Reads SRTM3 Elevation files.
	/// </summary>
	public class Srtm3Reader : TiledAltitudeMapReader
	{
        float NoData { get { return -32768 * AltitudeScale; } }

        public float AltitudeScale { get; set; } = 1;

        public Srtm3Reader()
        {
            TileWidth = 1200;
            TileHeight = 1200;
            DegreesLatitudePerTile = 1;
            DegreesLongitudePerTile = 1;
        }
        

        
        protected override void FillTile(Vector3[,] ret, Vector2 southWestLatLong)
        {
            int longitude = (int)southWestLatLong.Y;
            int latitude = (int)southWestLatLong.X;
            // World wrap for filename.
            if (longitude < -180) longitude += 360;
            if (longitude >= 180) longitude -= 360;
            southWestLatLong = new Vector2(latitude, longitude);
            string filename = Srtm3PathManager.CreateFilename(latitude, longitude);
            string validFile = Srtm3PathManager.FindFilename(filename);
            if (string.IsNullOrEmpty(validFile))
            {
                Console.WriteLine("   SRTM3 file: '" + filename + "' not found.");
                InsertBlankMap(ret, southWestLatLong);
            }
            else
            {
                Console.WriteLine("Building SRTM: '" + filename+"'");
                InsertSrtmFile(ret, southWestLatLong, validFile);
            }

        }


		void InsertSrtmFile(Vector3[,] ret, Vector2 southWestLatLong, string filename)
		{

			using (FileStream reader = new FileStream(filename, FileMode.Open))
			{
				float alt = 0;
				for (int h = 0; h < TileHeight; h++)
				{
					double latOffset = 1.0 - (double)h / TileHeight;
					for (int w = 0; w < TileWidth; w++)
					{
						alt = (float)ReadShortBE(reader) * AltitudeScale;

                        Vector2 latLong = CreateLatLongFromTileOffsets(southWestLatLong, TileHeight-h-1, w);
                        ret[TileHeight - h - 1, w ] = new Vector3(latLong.X, latLong.Y, (float)alt);
                       
					}
					alt = ReadShortBE(reader);
                }
			}

            // Find NaN values in the SRTM data and fill them in with surrounding terrain.
            for (int i = 0; i < 40; i++)
            {
                List<XY> nanIndices = FindNaNs(ret);
                if (nanIndices.Count <= 0) break;
                RemoveNaNs(ret, nanIndices);
            }
        }

        short ReadShortBE(Stream stream)
        {
            int b0 = stream.ReadByte();
            int b1 = stream.ReadByte();
            int r = (b0 * 256 + b1);
            if (b0 > 127)
            {
                r = r - 65536;
            }
           
            return (short)r; 
        }


        internal class XY { public int x; public int y; }

        /// <summary>
        /// Removes No Data values by filling in from average of surrounding pixels
        /// </summary>
        /// <param name="ret"></param>
        void RemoveNaNs(Vector3[,] ret, List<XY> nanIndices)
        {
            int[] valid = new int[nanIndices.Count];
            for (int i = 0; i < nanIndices.Count; i++)
            {
                XY xy = nanIndices[i];
                int s = 0;
                if ((xy.x > 0) && (ret[xy.y, xy.x - 1].Z != NoData))
                    s++;
                if ((xy.x < ret.GetLength(1) - 1) && (ret[xy.y, xy.x + 1].Z != NoData))
                    s++;
                if ((xy.y > 0) && (ret[xy.y - 1, xy.x].Z != NoData))
                    s++;
                if ((xy.y < ret.GetLength(0) - 1) && (ret[xy.y + 1, xy.x].Z != NoData))
                    s++;
                valid[i] = s;
            }
            for (int i = 0; i < nanIndices.Count; i++)
            {
                if (valid[i] == 0) continue;
                XY xy = nanIndices[i];
                double sum = 0;
                int s = 0;
                if ((xy.x > 0) && ret[xy.y, xy.x - 1].Z != NoData)
                {
                    sum += ret[xy.y, xy.x - 1].Z;
                    s++;
                }
                if ((xy.x < ret.GetLength(1) - 1) && ret[xy.y, xy.x + 1].Z != NoData)
                {
                    sum += ret[xy.y, xy.x + 1].Z;
                    s++;
                }
                if ((xy.y > 0) && ret[xy.y - 1, xy.x].Z != NoData)
                {
                    sum += ret[xy.y - 1, xy.x].Z;
                    s++;
                }
                if ((xy.y < ret.GetLength(0) - 1) && ret[xy.y + 1, xy.x].Z != NoData)
                {
                    sum += ret[xy.y + 1, xy.x].Z;
                    s++;
                }
                ret[xy.y, xy.x].Z = (float)(sum / s);
            }
        }

        List<XY> FindNaNs(Vector3[,] ret)
        {
            List<XY> list = new List<XY>();
            for (int y = 0; y < ret.GetLength(0); y++)
                for (int x = 0; x < ret.GetLength(1); x++)
                    if (ret[y, x].Z  == NoData)
                        list.Add(new XY() { x = x, y = y });
            return list;
        }

        

	}
}
