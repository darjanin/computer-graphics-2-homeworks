using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using CG2.Lighting;

namespace CG2.Shading
{
	public class Shader
	{
        //Info: Each shader should return the color of the hitpoint with the hitpoint normal, in current lighting conditions
        public virtual Vector4 GetColor(Vector4 point, Vector4 normal, Vector4 eyeDir, Vector4 lightDir, Double lightIntensity, Light light)
		{
            return Vector4.Zero;
		}
	}
}
