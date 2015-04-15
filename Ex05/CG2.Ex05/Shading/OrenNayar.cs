using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using CG2.Lighting;

namespace CG2.Shading
{
    class OrenNayar : Phong
    {
        // set value depending on material; let the user set this from the outside instead...
        Double roughness = 1.0;

        #region Init

        public OrenNayar()
        {
        }

        public OrenNayar(Phong shader)
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
            //ToDo: calculate intermediary values
            //ToDo: calculate normal dot light_direction
			Double NdotLight = normal * lightDir;
            //ToDo: calculate normal dot view_direction
			Double NdotEye = normal * eyeDir;

            //ToDo: calculate angle between normal and view_direction
			Double neAngle = NdotEye / (normal.Length * eyeDir.Length);
            //ToDo: calculate angle between normal and light_direction
			Double nlAngle = NdotLight / (normal.Length * lightDir.Length);

            //ToDo: set alpha to max of the calculated angles
			Double alfa = Math.Max(neAngle, nlAngle);
            //ToDo: set beta as min of the calculated angles
			Double beta = Math.Min(neAngle, nlAngle);
            //ToDo: calculate gamma value
            // Hint: more info in slides
			Double gamma = (eyeDir - normal * (NdotEye)) * (lightDir - normal * (NdotLight));

            //ToDo: square roughness of the material
			Double squareRoughness = Math.Pow(roughness, 2);

            //ToDo: calculate A
			Double A = 1 - 0.5 * (squareRoughness / (squareRoughness + 0.57));
            // Hint: more info in slides
            //ToDo: calculate B
			Double B = 0.45 * (squareRoughness / (squareRoughness + 0.09));
            // Hint: more info in slides
            //ToDo: calculate C
			Double C = Math.Sin(alfa) * Math.Tan(beta);
            // Hint: more info in slides

            //ToDo: put it all together
            // Hint: more info in slides
			Double L1 = Math.Max(0, NdotLight) * (A + B * Math.Max(0, gamma) * C);

            // get the final color by summing both components
            Vector4 finalValue = AmbientColor;
            finalValue += DiffuseColor ^ light.DiffuseColor * (L1);

            return finalValue;
        }

        #endregion
    }
}
