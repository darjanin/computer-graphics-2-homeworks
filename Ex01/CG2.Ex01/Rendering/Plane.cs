using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;

namespace CG2.Rendering
{
	public class Plane
	{
		#region Properties

        //ToDo: Define object properties: Origin, Normal, Color
		public Vector4 Origin;
		public Vector4 Normal;
		public Vector4 Color;

		#endregion

        public Plane()
        {
        }

        public Plane(Vector4 origin, Vector4 normal)
        {
            //ToDo: Init
			Origin = origin;
			Normal = normal;
        }

		#region Ray Tracing

        public void Collide(Ray ray)
        {
            Collide(ray, this);
        }

        /// <summary>
        /// Collide ray with object and return intersection and intersected object.
        /// </summary>
        public static void Collide(Ray ray, Plane plane)
		{
            //ToDo: Compute ray-plane intersection
			Double t = ((plane.Origin - ray.Origin) * plane.Normal) / (ray.Direction * plane.Normal);
			ray.Hit (t, plane);


		}

		#endregion
	}
}
