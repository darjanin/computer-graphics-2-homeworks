using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using CG2.Rendering;

namespace CG2.Lighting
{
    public class PointLight : Light
    {
        #region Properties

        // ToDo: declare Origin and Range of point light
        // ToDo: declare Linear and Quadratic attenuation factors
        public Vector4 Origin;
        public Double Range = 1;
        public Double LinearAttenuation = 1.0;
        public Double QuadraticAttenuation = 0.0;

        #endregion

        #region Lighting

        public override Double GetIntensityAt(Vector4 point)
        {
            // ToDo: Implement linear/quadratic light attenuation based on distance of point from light
            // Hint: Use > http://goo.gl/j9I6t 
			Vector4 v = point - Origin;
			double r = v.Length;

			return Intensity * (Range / (Range + LinearAttenuation * r)) * ((Range*Range) / ((Range*Range) + QuadraticAttenuation * r*r)) ;  
        }

        public override void SetLightRayAt(Vector4 point, Ray ray)
        {
            // ToDo: Set light vector from point to Origin of the light
            //       It is important to set the lenght of the direction vector in ray
            //       It is also useful to set the hit parameter in ray
			Vector4 v = Origin - point;
			ray.Set(point, v);
        }

        #endregion
    }
}
