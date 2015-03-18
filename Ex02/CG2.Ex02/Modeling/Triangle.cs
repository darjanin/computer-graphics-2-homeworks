using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Rendering;
using CG2.Mathematics;

namespace CG2.Modeling
{
	public class Triangle: Model
	{
		#region Properties

		public Vector4 A, B, C;

		#endregion

		#region Init

		public Triangle(Vector4 a, Vector4 b, Vector4 c)
		{
			A = a;
			B = b;
			C = c;
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

		public void Collide(Ray ray, Triangle triangle)
		{
			// equation from document
			// http://courses.cs.washington.edu/courses/cse457/07sp/lectures/triangle_intersection.pdf

			Vector4 n = (triangle.B - triangle.A) % (triangle.C - triangle.A);

			// find out intersection of triangle's plane with ray
			Plane plane = new Plane (triangle.A, n);

			Double nv = plane.Normal * ray.Direction;
			if (nv >= -0.00001 && nv <= 0.00001) return;

			Double t = (plane.Normal * (plane.Origin - ray.Origin)) / nv;
			if (t < ray.Bias || t >= (ray.HitParam + 0.0000001)) return;

			if (t < 0)
				return;

			// calculate coordinates of intersection
			Vector4 coordinates = ray.Origin + t * ray.Direction;

			// test if intersections is in triangle
			if ( (((triangle.A - triangle.C) % (coordinates - triangle.C)) * n) < 0) return;
			if ( (((triangle.B - triangle.A) % (coordinates - triangle.A)) * n) < 0) return;
			if ( (((triangle.C - triangle.B) % (coordinates - triangle.B)) * n) < 0) return;

			ray.HitModel = triangle;
			ray.HitParam = t;
		}

		#endregion
	}
}

