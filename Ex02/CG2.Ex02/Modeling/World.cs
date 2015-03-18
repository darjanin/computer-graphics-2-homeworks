using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Rendering;

namespace CG2.Modeling
{
	public class World
	{
		#region Properties

		public List<Model> Models = new List<Model>();

		#endregion


		#region Ray Tracing

		public void Collide(Ray ray)
		{
			foreach (Model model in Models)
			{
				model.Collide(ray);
			}
		}
		
		#endregion
	}
}
