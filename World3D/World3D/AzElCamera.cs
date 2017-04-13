using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using OpenTK;

namespace World3D
{
	public interface Camera
	{
		Vector3 Eye { get;  }
		Vector3 Up { get; }
		Vector3 Target { get;  }
		Matrix4 View { get; }
        void LookAt(Vector3 eye, Vector3 target);
	}

	public class AzElCamera : Camera
	{
		double distance = 1.0;
		double elevation = 0.0;
		double azimuth = 0.0;
		/// <summary>
		/// Distance between Eye and Target.
		/// Changing distance adjusts position of the Eye while keeping target the same.
		/// </summary>
		public double Distance { get { return distance; } set { SetDistance(value); } }
		/// <summary>
		/// Elevation direction in radians. 0 = horizontal. +90 = +Y.
		/// Changes the camera Eye position to rotate around the Target.
		/// </summary>
		public double Elevation { get { return elevation; } set { elevation = Math3D.Clamp((float)value, (float)-Math.PI / 2 + 0.1f, (float)Math.PI / 2 - 0.1f); } }
		/// <summary>
		/// Azimuth direction in radians. 0 = north = +Z, pi = east = +X.
		/// Changes the camera Eye position to rotate around the Target.
		/// </summary>
		public double Azimuth
		{
			get { return azimuth; }
			set
			{
				azimuth = Math3D.Wrap((float)value, (float)-Math.PI, (float)Math.PI);
			}
		}
		/// <summary>
		/// The eye position in world coordinates of the camera
		/// </summary>
		public Vector3 Eye { get; set; }// { return CalculateEye(); } }


        /// <summary>
        /// The target position that is one unit distant from the eye.
        /// </summary>
        public Vector3 Target { get { return CalculateTarget(); } }
		public Vector3 Up { get { return Vector3.UnitY; } }

		public Matrix4 View { get { return Matrix4.LookAt(-Eye, -Target, -Up); } }


        public void LookAt(Vector3 eye, Vector3 target)
        {
            Eye = eye;
            Vector3 diff = (eye - target);
            float distance = diff.Length;
            Elevation = -Math.Atan2(diff.Y, Math.Sqrt(diff.X * diff.X + diff.Z * diff.Z));
            Azimuth = Math.Atan2(-diff.X, -diff.Z);
            Distance = distance;
        }

        Vector3 CalculateTarget()
        {
            Vector3 dir = -GetDirection();
            dir = Vector3.Multiply(dir, (float)distance);
            return Vector3.Subtract(Eye, dir);
        }
        
		/// <summary>
		/// Return camera direction as a unit vector.
		/// </summary>
		/// <returns></returns>
		public Vector3 GetDirection()
		{
			double r = 1.0;
			double y = Math.Sin(Elevation) * r;
			double d = Math.Sqrt(r * r - y * y);
			double x = Math.Sin(-Azimuth) * d;
			double z = Math.Cos(-Azimuth) * d;
			Vector3 dir = new Vector3((float)x, (float)y, (float)z);
			return dir;
		}

		/// <summary>
		/// Sets the distance between eye and target. Eye remains the same, but target moves backwards/forwards.
		/// </summary>
		/// <param name="d"></param>
		public void SetDistance(double d)
		{
			Vector3 dir = GetDirection();
			Vector3 tar = Target;
			distance = Math.Max(0.25, d);
		}

		/// <summary>
		/// Moves the target location sideways relative to the camera.
		/// </summary>
		/// <param name="d"></param>
		public void MoveSideways(double d)
		{
			double x = -Math.Cos(Azimuth) * d;
			double z = -Math.Sin(Azimuth) * d;
            Eye = Vector3.Add(Eye, new Vector3((float)x, 0, (float)z));
        }

		/// <summary>
		/// Moves the target location forwards or backwards relative to the camera.
		/// </summary>
		/// <param name="d"></param>
		public void MoveForwards(double d)
		{
			double x = Math.Sin(-Azimuth) * d;
			double z = Math.Cos(-Azimuth) * d;
            Eye = Vector3.Add(Eye, new Vector3((float)x, 0, (float)z));
        }

		public void MoveUpwards(double d)
		{
            Eye = Vector3.Add(Eye, new Vector3(0, (float)d, 0));
        }

	}
}
