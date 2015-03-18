using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using CG2.Modeling;

namespace CG2.Rendering
{
	public class Ray
	{
		#region Properties
		public Vector4 Origin;
		public Vector4 Direction;

        public Double Bias = 0.01;
        public Double HitParam;
		public Model HitModel = null;

        //ToDo: Declare HitNormal
        public Vector4 HitNormal;

		#endregion


		#region Init

		public Ray()
		{
		}

		public Ray(Vector4 origin, Vector4 direction, Double zNear = 0.01, Double zFar = Double.MaxValue)
		{
            Set(origin, direction, zNear, zFar);
		}

        //Info: Set ray properties for already created ray
        public void Set(Vector4 origin, Vector4 direction, Double zNear = 0.01, Double zFar = Double.MaxValue)
        {
            Origin = origin;
            Direction = direction;
            HitParam = zFar;
            Bias = zNear;
        }

        public Vector4 GetHitPoint()
        {
			return Origin + HitParam * Direction;
        }

		#endregion
	}
}
