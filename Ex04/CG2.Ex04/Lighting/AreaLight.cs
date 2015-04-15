using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using CG2.Rendering;

namespace CG2.Lighting
{
    public class AreaLight : Light
    {
        public List<PointLight> Lights = new List<PointLight>();
        public Vector4 Origin = new Vector4(0, 0, 0);
        public Double nx, ny;
        public Int32 sx, sy;

        public void Set()
        {
            //ToDo: Set point lights on the plane sx times sy. Number of light should be nx and ny. 
            //      Point light could be stored in Lights property.
            
            //Hint: In the camera class you should go through all the light in the scene 
            //      and if there is area light, you should go through all the light in the are light ect. ect.

			for (double x = -sx / 2; x < sx / 2; x += sx / nx)
			{
				for (double y = -sy / 2; y < sy / 2; y += sy / ny)
				{
					Lights.Add(new PointLight() { Origin = Origin - new Vector4(x, y, 0), Intensity = Intensity });
				}
			}

        }
    }
}
