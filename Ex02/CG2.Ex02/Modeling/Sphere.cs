using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Rendering;
using CG2.Mathematics;

namespace CG2.Modeling
{
	public class Sphere: Model
	{
		#region Properties

		public Vector4 Center;
		public Double Radius;

		#endregion

		#region Init

		public Sphere(Vector4 center, Double radius)
		{
			Center = center;
			Radius = radius;
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

		public void Collide(Ray ray, Sphere sphere)
		{
			Double b = 2 * ray.Direction * (ray.Origin - sphere.Center);
			Double c = (ray.Origin - sphere.Center) * (ray.Origin - sphere.Center) - Math.Pow (Radius, 2);
			Double discriminant = Math.Pow (b, 2) - 4 * c;
			Double t;
			if (discriminant >= 0)
			{
				t = (-b - Math.Sqrt (discriminant)) / 2;
				if (t < 0)
					t = (-b + Math.Sqrt (discriminant)) / 2;
			
				if (t < ray.Bias || t >= (ray.HitParam + 0.0000001)) return;

				if (t < ray.HitParam)
				{
					ray.HitModel = sphere;
					ray.HitParam = t;
				}
			}

		}

		#endregion
	}
}

