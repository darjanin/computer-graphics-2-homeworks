using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using CG2.Lighting;

namespace CG2.Shading
{
	public class CheckerShader : Shader
	{
		#region Properties

        //Info: Even material
		public Shader Shader0;

        //Info: Odd material
		public Shader Shader1;

        //Info: Cube size should be included to calculations
		public Double CubeSize = 1;

		#endregion


		#region Shading

        public override Vector4 GetColor(Vector4 point, Vector4 normal, Vector4 eyeDir, Vector4 lightDir, double lightIntensity, Light light)
		{
            //ToDo: return correct color for the hitpoint in the checker shader
			bool even = ((Math.Floor ((point.X + 0.00001) / CubeSize) +
						  Math.Floor ((point.Y + 0.00001) / CubeSize) +
						  Math.Floor ((point.Z + 0.00001) / CubeSize)) % 2 == 0);
			if (even)
				return Shader0.GetColor(point, normal, eyeDir, lightDir, lightIntensity, light);
			else
				return Shader1.GetColor(point, normal, eyeDir, lightDir, lightIntensity, light);
		}

		#endregion
	}
}
