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
		public const double NoData = -32768;

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
            string filename = Srtm3PathManager.CreateFilename(latitude, longitude);
            string validFile = Srtm3PathManager.FindFilename(filename);
            if (string.IsNullOrEmpty(validFile))
            {
                Console.WriteLine("   SRTM3 file: '" + filename + "' not found.");
                InsertBlankMap(ret, new Vector2(latitude,longitude));
            }
            else
            {
                Console.WriteLine("Building SRTM: '" + filename+"'");
                InsertSrtmFile(ret, latitude, longitude, validFile);
            }

        }


		void InsertSrtmFile(Vector3[,] ret, int latitude, int longitude, string filename)
		{

			using (FileStream reader = new FileStream(filename, FileMode.Open))
			{
				short alt = 0;
				for (int h = 0; h < TileHeight; h++)
				{
					double latOffset = 1.0 - (double)h / TileHeight;
					for (int w = 0; w < TileWidth; w++)
					{
						alt = ReadShortBE(reader);
                        ret[h, w ] = new Vector3()
                        {
                            Z = alt,
                            X = (float)(latitude + latOffset),
                            Y = (float)(longitude + (double)w / TileWidth)
                        };
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
            return 0;
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
                    if (ret[y, x].Z == NoData)
                        list.Add(new XY() { x = x, y = y });
            return list;
        }

        

	}
}
