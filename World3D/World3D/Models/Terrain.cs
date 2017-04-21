using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World3D
{
    public class Terrain : SquareGrid, INormalModel
    {
        public TerrainInfo Info { get; set; }
        public int LatRows { get { return xColumns; } }
        public int LongColumns { get { return zRows; } }
        public Vector3 [] Normals { get; set; }

        public void Recreate(Vector3[,] lla)
        {
            double dxSum = 0;
                double dzSum = 0;
            for (int i = 1; i < lla.GetLength(0);i++)
                dxSum = dxSum+lla[i, 0].X - lla[i - 1, 0].X;
            for (int i = 1; i < lla.GetLength(1); i++)
                dzSum = dzSum + lla[0, i].Y - lla[0,i - 1].Y;
            double dLat = dxSum / (lla.GetLength(0) - 1);
            double dLong = dzSum / (lla.GetLength(1) - 1);
            this.Info = new TerrainInfo()
            {
                DegreesLatitudePerPixel = dLat,
                DegreesLongitudePerPixel = dLong,
                SouthWestLatLong = lla[0, 0].Xy,
                NorthEastLatLong = lla[lla.GetLength(0) - 1, lla.GetLength(1) - 1].Xy + new Vector2((float)dLat, (float)dLong)
            };
            CheckGridSize(lla);
            Vector3 refr = new Vector3(Info.CentreLatLong);
            Vector3 refe = EarthConverter.LatLongAltToECEF(refr);
            CreateVerticesFromVectors(lla, refr, refe);
            Normals = NormalModel.CalculateNormals(this);
        }


        private void CreateVerticesFromVectors(Vector3[,] lla, Vector3 refr, Vector3 refe)
        {
            for (int x = 0; x <= LatRows; x++)
            {
                for (int z = 0; z <= LongColumns; z++)
                {
                    int ix = z * (LatRows + 1) + x;
                    Vector3 e = EarthConverter.LatLongAltToECEF(lla[x, z]);
                    Vector3 d = e - refe;
                    Vector3 loc = EarthConverter.RotateECEF(d, refr.X, refr.Y);
                    Vertices[ix] = loc;
                }
            }
        }


        public void Recreate(TerrainInfo info, float [,] altInMetres)
        {
            this.Info = info;
            CheckGridSize(altInMetres);
            Vector3 refr = new Vector3(info.CentreLatLong);
            Vector3 refe = EarthConverter.LatLongAltToECEF(refr);
            CreateVerticesFromFloats(altInMetres, refr, refe);
            Normals = NormalModel.CalculateNormals(this);
        }

        private void CreateVerticesFromFloats(float[,] altInMetres, Vector3 refr, Vector3 refe)
        {
            float xo = (float)Info.DegreesLatitudePerPixel;
            float zo = (float)Info.DegreesLongitudePerPixel;

            for (float x = 0; x <= LatRows; x++)
            {
                for (float z = 0; z <= LongColumns; z++)
                {
                    int ix = (int)(z * (LatRows + 1) + x);
                    Vector3 lla = new Vector3(Info.SouthWestLatLong.X + x * xo, Info.SouthWestLatLong.Y + z * zo, altInMetres[(int)x, (int)z]);
                    Vector3 e = EarthConverter.LatLongAltToECEF(lla);
                    Vector3 d = e - refe;
                    Vector3 loc = EarthConverter.RotateECEF(d, refr.X, refr.Y);
                    Vertices[ix] = loc;
                }
            }
        }

        void CheckGridSize<T>(T[,] grid)
        {

            int latRows = grid.GetLength(0) - 1;
            int longColumns = grid.GetLength(1) - 1;
            if (latRows != LatRows || LongColumns != LongColumns)
            {
                base.RecreateGrid(latRows, longColumns, 1, 1);
            }
        }
    }

    public class TerrainInfo
    {
        public Vector2 SouthWestLatLong { get; set; }
        public Vector2 NorthEastLatLong { get; set; }
        public double DegreesLatitudePerPixel { get; set; }
        public double DegreesLongitudePerPixel { get; set; }
        public Vector2 CentreLatLong { get { return (NorthEastLatLong + SouthWestLatLong) * 0.5f; } }

        public float[,] CreateFlatAltitudes(int latRows, int longCols)
        {
            float[,] altInMetres = new float[latRows + 1,longCols+1];
            return altInMetres;
        }
    }

    public class EarthConverter
    {
        /// <summary>
        /// Earth radius at equator
        /// </summary>
        public  const double A = 6378.137 * 1000;
        /// <summary>
        /// Earth radius at poles
        /// </summary>
        public const double B = 6356.752 * 1000;

        /// <summary>
        /// Converts Latitude (deg), Longitude (deg), Altitude (metres) to 
        /// Earth-Centred-Earth-Fixed cartesian coordinates in metres.
        /// +x = out via 0 longitude. +y = out via 90 longitude. +z = up towards north pole.
        /// </summary>
        /// <param name="latLongAlt"></param>
        /// <returns></returns>
        public static Vector3 LatLongAltToECEF(Vector3 latLongAlt)
        {
            double latRad = latLongAlt.X * Math.PI / 180;
            double longRad = latLongAlt.Y * Math.PI / 180;
            double altMetres = latLongAlt.Z;

            // Curvature in prime vertical
            double N = A * A / Math.Sqrt(
                A * A * Math.Cos(latRad) * Math.Cos(latRad)
                + B * B * Math.Sin(latRad) * Math.Sin(latRad)
                );

            // Earth - Fixed Earth - Centred(ECEF) coordinates
            double x = (N + altMetres) * Math.Cos(latRad) * Math.Cos(longRad);
            double y = (N + altMetres) * Math.Cos(latRad) * Math.Sin(longRad);
            double z = ((B * B / (A * A)) * N + altMetres) * Math.Sin(latRad);
            return new Vector3((float)x, (float)y, (float)z);
        }

        /// <summary>
        /// Rotates Earth-Centred-Earth-Fixed (metres) to flat earth coordinates.
        /// +x = north. +y = up. +z = east.
        /// </summary>
        /// <param name="ecef"></param>
        /// <param name="refLatitude"></param>
        /// <param name="refLongitude"></param>
        /// <returns></returns>
        public static Vector3 RotateECEF(Vector3 ecef, double refLatitude, double refLongitude)
        {
            double alpha = (90 - refLatitude) * Math.PI / 180;
            double beta = refLongitude * Math.PI / 180;
            

            double x = Math.Cos(alpha) * Math.Cos(beta) * ecef.X + Math.Cos(alpha) * Math.Sin(beta) * ecef.Y - Math.Sin(alpha) * ecef.Z;

            double z = -Math.Sin(beta) * ecef.X + Math.Cos(beta) * ecef.Y;

            double y = Math.Sin(alpha) * Math.Cos(beta) * ecef.X + Math.Sin(alpha) * Math.Sin(beta) * ecef.Y + Math.Cos(alpha) * ecef.Z;

            return new Vector3((float)-x, (float)y, (float)z);
        }
    }
}
