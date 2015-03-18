using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using System.Drawing;
using CG2.Modeling;
using CG2.Shading;
using CG2.Lighting;

namespace CG2.Rendering
{
	public class Camera
	{
		#region Properties

		public Vector4 Position;
		public Vector4 Target;
		public Vector4 Up = new Vector4(0, 0, 1);
		public Double FovY = 45;
        public Double zFar;
        public Double zNear;
		public Vector4 U, V, W; // camera to world space
		public Vector4 BgColor = new Vector4(0, 0, 0);

        //Info: Result image
        public Bitmap Bitmap;
        public Int32 Width;
        public Int32 Height;
        public Vector4[] Pixels;
        public World World;

        //ToDo : declare UseShadows to control shadow rendering
        public Boolean UseShadows = false;

		#endregion


		#region Init

		public Camera(Int32 width, Int32 height)
		{
			Width = width;
			Height = height;
            Bitmap = new Bitmap(Width, Height);
            Pixels = new Vector4[Width * Height];
		}

		#endregion


		#region Buffer Acess

		public Vector4 GetPixel(Int32 i, Int32 j)
		{
			return Pixels[i + j * Width];
		}

		public void SetPixel(Int32 i, Int32 j, Vector4 color)
		{
			Pixels[i + j * Width] = color;
		}

		#endregion


		#region Rendering

		public void Render()
		{
            InitRender();
			RayTrace();
			PresentFrame();
		}

        public void InitRender()
        {
            Bitmap = new Bitmap(Width, Height);
            Pixels = new Vector4[Width * Height];
        }

        /// <summary>Derived from Computer Graphics - David Mount. /n
        /// Implementations can differ - make your own from scratch. 
        /// See http://goo.gl/q6Sz0 and http://goo.gl/rB8J6
        /// </summary>
		public void RayTrace()
		{
            W = -1 * (Target - Position).Normalized;
            U = (Up % W).Normalized;
            V = (W % U);

            Double AspectRatio = (Double)Width / (Double)Height;
            Double h = 2 * Math.Tan(MathEx.DegToRad(FovY / 2.0));
            Double w = h * AspectRatio;

            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
                {
                    Double ur = h * ((Double)r / (Double)Height - 0.5);
                    Double uc = w * ((Double)c / (Double)Width - 0.5);
                    Vector4 rayDir = Position + (uc * U + ur * V - W);
                    rayDir = (rayDir - Position).Normalized;
                    Ray ray = new Ray(Position, rayDir, zNear, zFar);
                    Vector4 color = RayTrace(ray);
                    SetPixel(c, Height - 1 - r, color);
                }
            }
		}

        public Vector4 RayTrace(Ray ray)
		{
			Vector4 color = Vector4.Zero;
			World.Collide (ray);
			if (ray.HitModel == null)
				return BgColor; // no collision return
            
			// Lighting
			// ToDo: for each light in the world do
			// 1) setup light ray to current light
			// 2) collide light ray with scene to check for shadows
			// 3) evaluate local shading (e.g. phong-model) and accumulate color

			foreach (Light light in World.Lights) {
				Ray LightRay = new Ray ();
				Vector4 hitPoint = ray.GetHitPoint ();
				light.SetLightRayAt (hitPoint, LightRay);
				World.Collide (LightRay);
				if (LightRay.HitModel == null || !UseShadows) {
					// Lightray.Direction has to be normalized
					color += ray.HitModel.Shader.GetColor (hitPoint, ray.HitNormal, -ray.Direction, LightRay.Direction.Normalized, light.GetIntensityAt (hitPoint), light);
				}

			}
           
			return color;
		}

        /// <summary>
        /// Copy all the pixels from pixel buffer to the Bitmap.
        /// Color is clamped in post process.
        /// </summary>
		public void PresentFrame()
		{
			for (Int32 y = 0; y < Height; y++)
			{
				for (Int32 x = 0; x < Width; x++)
				{
					Vector4 c = GetPixel(x, y);
                    c = Vector4.Clamp(c, 0, 1);
					Bitmap.SetPixel(x, y, Color.FromArgb(
						(Int32)(255 * c.X),
						(Int32)(255 * c.Y),
						(Int32)(255 * c.Z))
					);
				}
			}
		}

		#endregion
	}
}
