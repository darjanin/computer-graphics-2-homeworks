using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using CG2.Rendering;

namespace CG2.Modeling
{
	public class Ring : Model
	{
		#region Properties

		public Vector4 Origin;
		public Vector4 Normal;
		public Double Radius;

		#endregion


		#region Init

		public Ring()
		{
		}

		public Ring(Vector4 origin, Vector4 normal, Double radius)
		{
			Origin = origin;
			Normal = normal;
			Radius = radius;
		}

		#endregion


		#region Raytracing


		public override void Collide(Ray ray)
		{
			Collide(ray, this);
		}

		public static void Collide(Ray ray, Ring ring)
		{
			// find out intersection of ring's plane with ray
			Plane plane = new Plane (ring.Origin, ring.Normal);

			Double nv = plane.Normal * ray.Direction;
			if (nv >= -0.00001 && nv <= 0.00001) return;

			Double t = (plane.Normal * (plane.Origin - ray.Origin)) / nv;
			if (t < ray.Bias || t >= (ray.HitParam + 0.0000001)) return;

			// calculate coordinates of intersections
			Vector4 coordinates = ray.Origin + t * ray.Direction;

			// calculate distance of intersection to ring's radius
			Double distance = (coordinates - ring.Origin).Length;

			// set the HitModel & HitParam when intersections is in ring's radius
			if (distance <= ring.Radius && ray.HitParam > t) {
				ray.HitModel = ring;
				ray.HitParam = t;
			}
		}

		#endregion
	}
}
