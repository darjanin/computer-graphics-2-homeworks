using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using CG2.Lighting;

namespace CG2.Shading
{
    class GradientShader : Shader
    {
        #region Properties

        // Even material
        public Shader Shader0;
        // Odd material
        public Shader Shader1;
        public Vector4 Q;
        public Vector4 v;

        #endregion


        #region Shading

        public override Vector4 GetColor(Vector4 point, Vector4 normal, Vector4 eyeDir, Vector4 lightDir, Double lightIntensity, Light light, Boolean InShadow)
        {
            //ToDo: create gradient shader
            // Hint: Use additional material > Computer Graphics - David Mount > http://goo.gl/q6Sz0
            //ToDo: gradient is done by alfa blending
            //ToDo: vector from point to gradient center is projected on gradient direction vector v
            //ToDo: then we can calculate the alfa value in 1D
			Q = new Vector4 (0, 0, 0);
			v = new Vector4 (0, 1, 0);
			Double beta = ((point - Q) * v) / (v * v);
			Double alfa = (1 - Math.Cos (beta * Math.PI)) / 2;

			return (1 - alfa) * Shader0.GetColor (point, normal, eyeDir, lightDir, lightIntensity, light, InShadow) +
			alfa * Shader1.GetColor (point, normal, eyeDir, lightDir, lightIntensity, light, InShadow);

        }

        #endregion
    }
}
