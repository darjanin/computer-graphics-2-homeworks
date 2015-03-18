using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using CG2.Rendering;

namespace CG2.Modeling
{
	public class Plane : Model
	{
		#region Properties

		public Vector4 Origin;
		public Vector4 Normal;

		#endregion


		#region Init

		public Plane()
		{
		}

		public Plane(Vector4 origin, Vector4 normal)
		{
			Origin = origin;
			Normal = normal;
		}

		#endregion


		#region Raytracing


		public override void Collide(Ray ray)
		{
			Collide(ray, this);
		}

		public static void Collide(Ray ray, Plane plane)
		{
            Double nv = plane.Normal * ray.Direction;
            if (nv >= -0.00001 && nv <= 0.00001) return;

            Double param = (plane.Normal * (plane.Origin - ray.Origin)) / nv;
            if (param < ray.Bias || param >= (ray.HitParam + 0.0000001)) return;

            ray.HitModel = plane;
            ray.HitParam = param;
		}

		public static Double CollideReturning(Ray ray, Plane plane)
		{
			double nv = plane.Normal * ray.Direction;

			if (nv != 0) {
				Double param = (plane.Normal * (plane.Origin - ray.Origin)) / nv;
				return (param > 0) ? param : -1;
			}				
			return -1;
		}

	#endregion
	}
}
