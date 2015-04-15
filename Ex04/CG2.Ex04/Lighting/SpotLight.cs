using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using CG2.Rendering;

namespace CG2.Lighting
{
	public class SpotLight : PointLight
	{
		#region Properties

		// Hint: Spot Light is very similar to point light. You can use point light class
		// ToDo: Declare Direction of light (pointing to the light target)
		// ToDo: Declare CutoffDeg=45 as the angle in degrees of the visible light cone
		// ToDo: Declare Exp = 3 as the exponent of the radial falloff
		// Hint: See > http://goo.gl/z0wtO
		public Vector4 Direction = new Vector4(-5, -5, -10);
		public Double CutoffDeg = 45;
		public Double Exp = 3;

		#endregion

		#region Lighting

		public override double GetIntensityAt(Vector4 point)
		{
			// ToDo: get the intensity of the point light (= base.GetIntensityAt(point))
			// - calculate lightVector being direction from light origin to the given point
			// - calculate lightAngle being angle between lightVector and light direction
			// - if this angle is more than Cutoff return zero intensity
			// - calculate decay being one minus ratio between lightAngle and cutoff angle powered by Exp
			// - return decay * intensity
			PointLight pl = new PointLight() { Origin = Origin, Intensity = Intensity };           
			Vector4 v = point - Origin;

			double deg = Math.Acos( v.Normalized * Direction.Normalized );
			double cutDeg = CutoffDeg * Math.PI / 180;
			if (cutDeg - deg > 0)
			{
				double decay = 1 - Math.Pow((deg / cutDeg), Exp);
				return decay * pl.GetIntensityAt(point);
			}
			return 0;
		}

		public override void SetLightRayAt(Vector4 point, Ray ray)
		{
			// ToDo: same as in the point light
			ray.Set(point, Origin - point);
		}

		#endregion
	}

}
