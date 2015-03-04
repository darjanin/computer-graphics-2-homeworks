using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;

namespace CG2.Rendering
{
	public class Ray
	{
		#region Properties

        //ToDo: Define ray properties: Origin, Direction, HitParameter, HitObject, ZNear and ZFar clipping
		public Vector4 Origin;
		public Vector4 Direction;
		public Double HitParameter;
		public Plane HitObject = null;
        
        #endregion

        public Ray()
		{
		}

		public Ray(Vector4 origin, Vector4 direction)
		{
            //ToDo: Init ray
			Origin = origin;
			Direction = direction;
		}

		public void Hit(Double t, Plane plane)
		{
			if (t < 0)
				return;

			if (HitObject == null) {
				HitParameter = t;
				HitObject = plane;
			} else if (t < HitParameter) {
				HitParameter = t;
				HitObject = plane;
			}
		}
	}
}
