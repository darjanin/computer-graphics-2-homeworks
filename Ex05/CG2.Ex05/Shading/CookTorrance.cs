using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using CG2.Lighting;

namespace CG2.Shading
{
    class CookTorrance : Phong
    {
        // set important material values
        Double roughnessValue = 0.3; // 0 : smooth, 1: rough
        Double F0 = 0.7; // Fresnel reflectance at normal incidence

        #region Init

        public CookTorrance()
        {
        }

        public CookTorrance(Phong shader)
        {
            AmbientColor = shader.AmbientColor;
            DiffuseColor = shader.DiffuseColor;
            SpecularColor = shader.SpecularColor;
            Shininess = shader.Shininess;
        }

        #endregion

        #region Shading

        public override Vector4 GetColor(Vector4 point, Vector4 normal, Vector4 eyeDir, Vector4 lightDir, Double lightIntensity, Light light, Boolean InShadow)
        {
            //ToDo: calculate diffuse factor do not forget to clamp negative values
			Double NdotL = Math.Max(0, normal * lightDir);
            //Cook-Torrance shader calculates new value for specular factor
            Double specular = 0.0;
            //calculate only if diffuse factor is greater than zero
            if (NdotL > 0.0)
            {
                //ToDo: calculate intermediary values of dot products used later don't forget to clamp negative values
                //ToDo: calculate half vector
				Vector4 half = (eyeDir + lightDir) *  (1 / (eyeDir + lightDir).Length);
                //ToDo: calculate normal dot half
				Double NdotH = Math.Max(0, normal * half);
                //ToDo: calculate normal dot view_direction
				Double NdotV = Math.Max(0, normal * eyeDir);
                //ToDo: calculate view_direction dot half
				Double VdotH = Math.Max(0, eyeDir * half);
                //ToDo: square roughness of the material
				Double squareRoughness = roughnessValue * roughnessValue;

                //ToDo: calculate geometric attenuation
				Double Ga = 1;
				Double Gb = 2 * NdotH * NdotV / VdotH;
				Double Gc = 2 * NdotH * NdotL / (lightDir * half);
				Double geoAtt = MathEx.Min3 (Ga, Gb, Gc);
                // Hint: more info in slides

                //ToDo: implement roughness (or: microfacet distribution function)
				Double exponent = ((NdotH * NdotH - 1) / (squareRoughness * NdotH * NdotH));
				Double roughness = (1 / (Math.PI * squareRoughness * Math.Pow (NdotH, 4))) * Math.Pow (Math.E, exponent);
                // Hint: you can use Beckmann distribution function from slides

                // ToDo: implement Fresnel effect with Schlick approximation
				Double fresnel = F0 + (1 - F0) * Math.Pow((1 - (half * eyeDir)), 5);
                // Hint: more info in slides

                //resulting specular factor is equal to (fresnel * geoAtt * roughness) / (NdotV * NdotL * Math.PI)
				specular = (fresnel * geoAtt * roughness) / (NdotV * NdotL * Math.PI);
            }

            Vector4 fColor = AmbientColor;
            fColor += lightIntensity * NdotL * (specular * SpecularColor + DiffuseColor) ^ light.DiffuseColor;
            return fColor;
        }

        #endregion
    }
}
