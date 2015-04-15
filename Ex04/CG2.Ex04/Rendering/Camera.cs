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

		public struct HitPoint
		{
			public Vector4 Position;
			public Vector4 Color;
			public Vector4 Normal;
			public Double ShadowCount;
		}

		#region Properties

		public Vector4 Position;
		public Vector4 Target;
		public Vector4 Up = new Vector4(0, 0, 1);
		public Double FovY = 45;
		public Double zFar;
		public Double zNear;
		public Vector4 U, V, W; // camera to world space
		public Vector4 BgColor = new Vector4(0, 0, 0);

		//Result
		public Bitmap Bitmap;
		public Int32 Width;
		public Int32 Height;
		public Vector4[] Pixels;
		public World World;

		// TODO: declare UseShadows to control shadow rendering
		public Boolean UseShadows = true;

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
			Vector4 color = Vector4.Zero, TempColor = Vector4.Zero;
			World.Collide(ray);
			if (ray.HitModel == null) return BgColor; 

			// ToDo: Create hitpoint to store Position, Color, ShadowCount and Normal
			HitPoint hitPoint;
			hitPoint.Position = ray.GetHitPoint();
			hitPoint.Color = Vector4.Zero;
			hitPoint.ShadowCount = 0;
			hitPoint.Normal = ray.HitNormal;

			Ray lightRay = new Ray();
			Vector4 viewDir = -ray.Direction; 

			Double lights = 0;

			// ToDo: 
			// Go through each light
			// Count number of shadows in hit point
			// Count number of lights in the scene
			// Update hitpoint color
			// Phong should return different hitpoint color for point in shadow and for points not in shadow

			foreach (Light l in World.Lights)
			{

				if (l is AreaLight)
					foreach (Light light in ((AreaLight)l).Lights)
					{
						lights++;
						Double nl;

						lightRay.HitModel = null;
						light.SetLightRayAt(hitPoint.Position, lightRay);
						nl = hitPoint.Normal * lightRay.Direction;
						if (UseShadows && nl > 0.0f)
							World.Collide(lightRay);

						if (lightRay.HitModel != null) {
							hitPoint.Color += ray.HitModel.Shader.GetColor (hitPoint.Position, hitPoint.Normal, viewDir, lightRay.Direction, light.GetIntensityAt (hitPoint.Position), light, true);
							hitPoint.ShadowCount++;
						} else {
							hitPoint.Color += ray.HitModel.Shader.GetColor (hitPoint.Position, hitPoint.Normal, viewDir, lightRay.Direction, light.GetIntensityAt (hitPoint.Position), light, false);
						}
					}
				else
				{
					lights++;
					Double nl;

					lightRay.HitModel = null;
					l.SetLightRayAt (hitPoint.Position, lightRay);
					nl = hitPoint.Normal * lightRay.Direction;
					if (UseShadows && nl > 0.0f)
						World.Collide (lightRay);

					if (lightRay.HitModel != null) {
						hitPoint.Color += ray.HitModel.Shader.GetColor (hitPoint.Position, hitPoint.Normal, viewDir, lightRay.Direction, l.GetIntensityAt (hitPoint.Position), l, true);
						hitPoint.ShadowCount++;
					} else {
						hitPoint.Color += ray.HitModel.Shader.GetColor (hitPoint.Position, hitPoint.Normal, viewDir, lightRay.Direction, l.GetIntensityAt (hitPoint.Position), l, false);
					}
				}
			}

			//ToDo: Return color due to interpolation (1 - ShadowsNumber * (1 / LightsNumber))

			return hitPoint.Color * (0.2 + 0.8 * (1 - hitPoint.ShadowCount * (1 / (lights))));
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
