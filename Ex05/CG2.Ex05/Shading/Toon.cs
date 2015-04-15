using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using CG2.Lighting;

namespace CG2.Shading
{
    class Toon : Phong
    {       

		#region Init

        public Toon()
		{
		}

        public Toon(Phong shader)
        {
            AmbientColor = shader.AmbientColor;
            DiffuseColor = shader.DiffuseColor;
            SpecularColor = shader.SpecularColor;
            Shininess = shader.Shininess;
        }

        #endregion

        #region Shading

        Double DiscretizeDiffuse(Double factor)
        {
            //ToDo: return discretized diffuse factor ~4 intensities should be enough
			if (factor > 0.75) {
				return 1;
			} else if (factor > 0.5) {
				return 0.75;
			} else if (factor > 0.25) {
				return 0.5;
			} else if (factor > 0) {
				return 0.25;
			} else {
				return 0;
			}
        }

        Double DiscretizeSpecular(Double factor)
        {
            //ToDo: return discretized specular color ~3 intensities should be enough
			if (factor > 0.66) {
				return 1;
			} else if (factor > 0.33) {
				return 0.5;
			} else {
				return 0;
			}
        }

        public override Vector4 GetColor(Vector4 point, Vector4 normal, Vector4 eyeDir, Vector4 lightDir, Double lightIntensity, Light light, Boolean InShadow)
        {
            Double diffuseFactor = lightIntensity * (normal * lightDir);
            if (diffuseFactor < 0) diffuseFactor = 0;

            diffuseFactor = DiscretizeDiffuse(diffuseFactor);

            Vector4 half = (lightDir + eyeDir).Normalized;
            Double specularFactor = lightIntensity * Math.Pow(normal * half, Shininess);

            specularFactor = DiscretizeSpecular(specularFactor);

            Vector4 color = new Vector4();
            color = diffuseFactor * (DiffuseColor ^ light.DiffuseColor);
            color += specularFactor * (SpecularColor ^ light.DiffuseColor);
            color += AmbientColor;

            if (InShadow)
                return diffuseFactor * (DiffuseColor ^ light.DiffuseColor) + AmbientColor;
            else
                return color;
        }

        #endregion
    }
}
