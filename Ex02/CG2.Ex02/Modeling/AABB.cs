using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Rendering;
using CG2.Mathematics;

namespace CG2.Modeling
{
	public class AABB: Model
	{
		#region Properties

		public Vector4 AA, BB;
		List <Plane> Planes = new List<Plane>();

		#endregion

		#region Init

		public AABB(Vector4 aa, Vector4 bb)
		{
			AA = aa;
			BB = bb;

			Planes.Add(new Plane(aa, new Vector4(1, 0, 0)));
			Planes.Add(new Plane(aa, new Vector4(0, 1, 0)));
			Planes.Add(new Plane(aa, new Vector4(0, 0, 1)));
			Planes.Add(new Plane(bb, new Vector4(0, 0, 1)));
			Planes.Add(new Plane(bb, new Vector4(0, 1, 0)));
			Planes.Add(new Plane(bb, new Vector4(1, 0, 0)));
		}

		#endregion

		#region Ray Tracing

		/// <summary>
		/// Collide ray with object and return intersection and intersected object.
		/// </summary>
		public override void Collide(Ray ray)
		{
			Collide (ray, this);
		}

		private static Boolean isInside(Vector4 p, Vector4 a, Vector4 b)
		{
			if (
				p.X >= Math.Min (a.X, b.X) && p.X <= Math.Max (a.X, b.X) &&
				p.Y >= Math.Min (a.Y, b.Y) && p.Y <= Math.Max (a.Y, b.Y) &&
				p.Z >= Math.Min (a.Z, b.Z) && p.Z <= Math.Max (a.Z, b.Z)
			)
				return true;
			else
				return false;
		}

		public static Vector4 Project(Vector4 point, Plane plane)
		{
			Ray ray = new Ray(point, plane.Normal);

			Double t = Plane.CollideReturning(ray, plane);

			if (t < 0)
				t = Plane.CollideReturning(ray, new Plane(plane.Origin, plane.Normal*-1));

			return ray.Origin + t * ray.Direction;
		}

		public static void Collide(Ray ray, AABB box)
		{
			Double t = Double.PositiveInfinity;

			foreach (Plane plane in box.Planes)
			{
				Double temp = Plane.CollideReturning(ray, plane);			
				Vector4 point = ray.Origin + temp * ray.Direction;
				if (
					temp > 0 &&
					temp < t &&
					isInside(point, Project(box.AA, plane), Project(box.BB, plane))
				)
				{
					t = temp;
				}
			}

			if (t > 0 && ray.HitParam > t)
			{
				ray.HitModel = box;
				ray.HitParam = t;
			}
		}


		#endregion
	}
}

