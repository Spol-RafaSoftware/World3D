using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace World3D
{
	public class Math3D
	{
		public static Vector3 CalculateNormal(Triangle t) { return CalculateNormal(t.v0, t.v1, t.v2); }
		public static Vector3 CalculateNormal(Vector3 v0, Vector3 v1, Vector3 v2)
		{
			var dir = Vector3.Cross(v1 - v0, v2 - v1);
			var norm = Vector3.Normalize(dir);
			return norm;
		}
		public static Vector3 DiffuseLighting(Vector3 colour, Vector3 light, Vector3 norm) { return DiffuseLighting(colour, light, norm, 0.5f); }
		public static Vector3 DiffuseLighting(Vector3 colour, Vector3 light, Vector3 norm, float ambient)
		{
			float n = 0.5f * (Vector3.Dot(light, norm)+1);
			n *= (1.0f - ambient);
			colour = colour * (n+ambient);
			return colour;
		}

		public static float Clamp(float value, float min, float max)
		{
			if (value > max) value = max;
			if (value < min) value = min;
			return value;
		}

		public static float Wrap(float value, float min, float max)
		{
			float d = max-min;
			while (value < min) value += d;
			while (value > max) value -= d;
			return value;
		}

		public static Vector2 ClampAzimuthElevation(Vector2 azEl)
		{
			float el = Clamp(azEl.Y,(float)-Math.PI/2, (float)Math.PI/2);
			float az = Wrap(azEl.X, (float)-Math.PI, (float)Math.PI);
			return new Vector2(az, el);
		}

		/// <summary>
		/// Converts from Cartesian (X,Y,Z) to Spherical coordinates (Azimuth,Elevation,Radius)
		/// Y = +Elevation = Up, +X = -pi/2 Azimuth = West, +Z = 0 Azimuth = North.
		/// </summary>
		/// <param name="xyz"></param>
		/// <returns></returns>
		public static Vector3 Cart2Sph(Vector3 xyz)
		{
			double r = xyz.LengthSquared;
			double az = -Math.Atan2(xyz.X, xyz.Z);
			r = Math.Sqrt(r);
			double el = Math.Asin(xyz.Y / r);

			Vector3 ret = new Vector3((float)az, (float)el, (float)r);
			return ret;
		}

		/// <summary>
		/// Converts from Spherical coordinates (Azimuth,Elevation,Radius) to Cartesian (X,Y,Z).
		/// Y = +Elevation = Up, +X = -pi/2 Azimuth = West, +Z = 0 Azimuth = North.
		/// </summary>
		/// <param name="azimuthElevation"></param>
		/// <returns></returns>
		public static Vector3 Sph2Cart(Vector2 azimuthElevation)
		{
			double elRadians = azimuthElevation.Y;
			double azRadians = azimuthElevation.X;
			double r = 1;
			double y = Math.Sin(elRadians) * r;
			double d = Math.Sqrt(r * r - y * y);
			double x = -Math.Sin(azRadians) * d;
			double z = Math.Cos(azRadians) * d;
			Vector3 ret = new Vector3((float)x, (float)y, (float)z);
			return ret;
		}

		/// <summary>
		/// In MATLAB code, Vector3.Z = up, but in OpenGL Vector3.Y = up.
		/// This corrects that by swapping .Z and .Y.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static Vector3 ConvertToMatlabOrientation(Vector3 v)
		{
			return new Vector3(v.X, v.Z, v.Y);
		}

		public static Vector3 ConvertFromMatlabOrientation(Vector3 v)
		{
			return new Vector3(v.X, v.Z, v.Y);
		}

		public static Vector4 UnProject(Matrix4 projection, Matrix4 view, System.Drawing.Size viewport, Vector3 point)
		{
			Vector4 vec;

			vec.X = 2.0f * point.X / (float)viewport.Width - 1;
			vec.Y = -(2.0f * point.Y / (float)viewport.Height - 1);
			vec.Z = point.Z;
			vec.W = 1.0f;

			Matrix4 viewInv = Matrix4.Invert(view);
			Matrix4 projInv = Matrix4.Invert(projection);

			Vector4.Transform(ref vec, ref projInv, out vec);
			Vector4.Transform(ref vec, ref viewInv, out vec);

			if (vec.W > float.Epsilon || vec.W < float.Epsilon)
			{
				vec.X /= vec.W;
				vec.Y /= vec.W;
				vec.Z /= vec.W;
				vec.W = 1;
			}


			return vec;
		}

		public static int IntersectionTests = 0;

		/// <summary>
		/// Calculates if a Ray intersects the plane represented by the a triangle of points.
		/// http://www.lighthouse3d.com/tutorials/maths/ray-triangle-intersection/
		/// Returns float.NaN if ray does not intersect.
		/// </summary>
		/// <param name="rayOrigin">Origin of the ray</param>
		/// <param name="rayDirection">Direction vector of the ray</param>
		/// <param name="tri">Triangle of points representing a plane</param>
		/// <returns>The distance in multiples of rayDirection from rayOrigin to the intersecting point of the tri (aka, t)</returns>
		public static float RayIntersectsTriangle(Vector3 rayOrigin, Vector3 rayDirection, Triangle tri)
		{
			IntersectionTests++;
			Vector3 v0 = tri.v0, v1 = tri.v1, v2 = tri.v2, p = rayOrigin, d = rayDirection;
			Vector3 e1, e2, h, s, q;
			float a, f, t, u, v;


			e1 = v1 - v0;
			e2 = v2 - v0;

			h = Vector3.Cross(d, e2);
			a = Vector3.Dot(e1, h);

			if (a > -0.00001 && a < 0.00001)
				return float.NaN;

			f = 1 / a;
			s = p - v0;

			u = f * Vector3.Dot(s, h);

			if (u < 0.0 || u > 1.0)
				return (float.NaN);

			q = Vector3.Cross(s, e1);
			v = f * Vector3.Dot(d, q);

			if (v < 0.0 || u + v > 1.0)
				return (float.NaN);

			// at this stage we can compute t to find out where
			// the intersection point is on the line
			t = f * Vector3.Dot(e2, q);

			if (t > 0.00001) // ray intersection
				//return(true);
				return t;
			else // this means that there is a line intersection
				// but not a ray intersection
				return (float.NaN);
		}

		/// <summary>
		/// Calculates if a Ray intersects the plane represented by the a triangle of points.
		/// </summary>
		/// <param name="rayOrigin">Origin of the ray</param>
		/// <param name="rayDirection">Direction vector of the ray</param>
		/// <param name="tri">Triangle of points representing a plane</param>
		/// <param name="intersectLocation">Location of intersection, if any. If none, returns (NaN,NaN,NaN)</param>
		/// <returns>The distance in multiples of rayDirection from rayOrigin to the intersecting point of the box (aka, t)</returns>
		public static float RayIntersectsTriangle(Vector3 rayOrigin, Vector3 rayDirection, Triangle tri, out Vector3 intersectLocation)
		{
			intersectLocation = new Vector3(float.NaN, float.NaN, float.NaN);

			float t = RayIntersectsTriangle(rayOrigin, rayDirection, tri);
			if (t == float.NaN)
				return t;

			intersectLocation = GetIntersectLocation(rayOrigin, rayDirection, t);

			return t;
		}

		public static Vector3 GetIntersectLocation(Vector3 rayOrigin, Vector3 rayDirection, float t)
		{
			Vector3 intersectLocation = rayOrigin + rayDirection * t;
			return intersectLocation;
		}

		/// <summary>
		/// Calculates if a ray intersects with an axis-aligned bounding box.
		/// Returns float.NaN if ray does not intersect.
		/// </summary>
		/// <param name="rayOrigin">Origin of the ray</param>
		/// <param name="rayDirection">Direction vector of the ray</param>
		/// <param name="bbMin">Point representing the minimum x,y,z values of the bounding box</param>
		/// <param name="bbMax">Point representing the maximum of the x,y,z values of the bounding box</param>
		/// <returns>The distance in multiples of rayDirection from rayOrigin to the intersecting point of the box (aka, t)</returns>
		public static float RayIntersectsBoundingBox(Vector3 rayOrigin, Vector3 rayDirection, Vector3 bbMin, Vector3 bbMax)
		{
			int[] inds;
			Vector3[] verts;
			OpenTK.Graphics.OpenGL.BeginMode m;
			Cube.CreateCube(bbMin, bbMax, out verts, out inds, out m);

			for (int i = 2; i < inds.Length; i++)
			{
				Triangle tri = new Triangle(verts[inds[i-2]], verts[inds[i - 1]], verts[inds[i]]);
				float t = RayIntersectsTriangle(rayOrigin, rayDirection, tri);
				if (!float.IsNaN(t))
					return t;
			}
			
			return float.NaN;
		}


		public static void Copy2DTo1D<T>(T[] dest, T[,] src)
		{
			for (int h = 0; h < src.GetLength(0); h++)
				for (int w = 0; w < src.GetLength(1); w++)
					dest[h * src.GetLength(1) + w] = src[h, w];
			
		}

		public static void Fill2DArray<T>(T[,] dest, T[,] src, int yDestOffset, int xDestOffset) 
		{ Fill2DArray(dest, src, yDestOffset, xDestOffset, 0, 0); }

		public static void Fill2DArray<T>(T[,] dest, T[,] src, int yDestOffset, int xDestOffset, int ySrcOffset, int xSrcOffset)
		{ Fill2DArray(dest, src, yDestOffset, xDestOffset, 0, 0, src.GetLength(0), src.GetLength(1)); }

		public static void Fill2DArray<T>(T[,] dest, T[,] src, int yDestOffset, int xDestOffset, int ySrcOffset, int xSrcOffset, int yCount, int xCount)
		{
			for (int h = 0; h < yCount; h++)
			{
				int sourceIndex = (ySrcOffset + h) * src.GetLength(1) + xSrcOffset;
				int destIndex = (yDestOffset + h) * dest.GetLength(1) + xDestOffset;
				Array.Copy(src, sourceIndex, dest, destIndex, xCount);
			}
		}

		public static void Fill2DArray<T>(T[] dest, T[] src, int yDestOffset, int xDestOffset, int ySrcOffset, int xSrcOffset, 
			int yCount, int xCount, int destWidth, int srcWidth)
		{
			for (int h = 0; h < yCount; h++)
			{
				int sourceIndex = (ySrcOffset + h) * srcWidth + xSrcOffset;
				int destIndex = (yDestOffset + h) * destWidth + xDestOffset;
				Array.Copy(src, sourceIndex, dest, destIndex, xCount);
			}
		}
	}
	
	public class Triangle
	{
		public Vector3 v0;
		public Vector3 v1;
		public Vector3 v2;
		public Triangle(Vector3 v0, Vector3 v1, Vector3 v2)
		{
			this.v0 = v0;
			this.v1 = v1;
			this.v2 = v2;
		}

		public override string ToString()
		{
			return "T: " + v0.ToString() + "_" + v1.ToString() + "_" + v2.ToString();
		}
	}

}
