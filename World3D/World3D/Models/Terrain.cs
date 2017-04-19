using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World3D
{
    public class Terrain : SquareGrid
    {
        public TerrainInfo Info { get; set; }

        public void Recreate(TerrainInfo info, float [][] altInMetres)
        {

        }
    }

    public class TerrainInfo
    {
        public int LatRows { get; set; }
        public int LongColumns { get; set; }
        public double BottomLatitude { get; set; }
        public double LeftLongitude { get; set; }
        public double MetresPerDegreeLatitude { get; set; }
        
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
