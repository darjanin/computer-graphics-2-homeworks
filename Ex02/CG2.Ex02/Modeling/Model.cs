using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Rendering;
using CG2.Mathematics;

namespace CG2.Modeling
{
	public class Model
	{
		#region Properties

		public Vector4 Color = new Vector4(0, 0, 1);

		#endregion


		#region Ray Tracing

        /// <summary>
        /// Collide ray with object and return intersection and intersected object.
        /// </summary>
		public virtual void Collide(Ray ray)
		{
		}

		#endregion
	}
}
