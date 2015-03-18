using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using CG2.Lighting;

namespace CG2.Shading
{
	public class PhongShader : Shader
	{
		#region Properties

        // ToDo: declare Diffuse, Specular and Ambient Color and Shininees
		Vector4 DiffuseColor;
		Vector4 SpecularColor;
		Vector4 AmbientColor;
		Double Shininess = 20;

		#endregion


		#region Init

        //Hint: We can create phong shader with different parameters
        //      Default is phong with just a color
        //      Try to change specular and ambient color to see the behaviour
		public PhongShader()
		{
		}

		public PhongShader(Vector4 diffuseColor)
		{
			DiffuseColor = diffuseColor;
		}

		public PhongShader(Vector4 diffuseColor, Vector4 specularColor)
		{
            DiffuseColor = diffuseColor;
            SpecularColor = specularColor;
		}

		public PhongShader(Vector4 diffuseColor, Vector4 specularColor, Vector4 ambientColor)
		{
            DiffuseColor = diffuseColor;
            SpecularColor = specularColor;
            AmbientColor = ambientColor;
		}

		#endregion


		#region Shading

        public override Vector4 GetColor(Vector4 point, Vector4 normal, Vector4 eyeDir, Vector4 lightDir, Double lightIntensity, Light light)
		{
			Double diffuseFactor = Math.Max((normal * lightDir) * lightIntensity, 0);
			Vector4 h = (normal % lightDir).Normalized;
			Double specularFactor = Math.Pow ((normal * h), Shininess) * lightIntensity;

			Vector4 color = new Vector4();
			color += (DiffuseColor ^ light.DiffuseColor) * diffuseFactor;
			color += (SpecularColor ^ light.DiffuseColor) * specularFactor;
			color += AmbientColor;

			return color;
		}

		#endregion
	}
}
