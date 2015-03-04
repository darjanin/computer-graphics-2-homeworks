using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CG2.Mathematics;
using System.Drawing;

namespace CG2.Rendering
{
	public class Camera
	{
		#region Properties

		// Camera properties
		public Vector4 Position;
		public Vector4 Target;
		public Vector4 U;
		public Vector4 V;
		public Vector4 W;
		public Vector4 Up = new Vector4 (0,0,1);
		public Double FovY = 45;

		// Frame properties
		public Int32 Width, Height;
		public Bitmap Bitmap;
		public Vector4[] Pixels;
		public Color BgColor;

		// Models
		public List<Plane> Planes = new List<Plane>();

		#endregion

		#region Init

		public Camera(Int32 width, Int32 height)
		{
			BgColor = Color.Black;
			Bitmap = new Bitmap(width, height);
			Pixels = new Vector4 [width * height];

			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					Bitmap.SetPixel(i, j, BgColor);
				}
			}

			Width = width;
			Height = height;
		}

		#endregion

		#region Buffer Acess

		public Vector4 GetPixel(Int32 i, Int32 j)
		{
			return Pixels [i + j * Convert.ToInt32(Width)];
		}

		public void SetPixel(Int32 i, Int32 j, Vector4 color)
		{
			Pixels [i + j * Convert.ToInt32(Width)] = color;
		}

		#endregion

		#region Rendering

		public void Render()
		{
			RayTrace();
			PresentFrame();
		}

		/// <summary>Derived from Computer Graphics - David Mount. /n
		/// Implementations can differ - make your own from scratch. 
		/// See http://goo.gl/q6Sz0 and http://goo.gl/rB8J6
		/// </summary>
		public void RayTrace()
		{
			//ToDo: Initialize camera (u, v, w)
			U = (Target - Position).Normalized;
			V = Up;
			W = (U % V).Normalized;

			//ToDo: Compute perspective with FovY as a field of view
			Double angle = Math.PI * FovY / 180.0; // convert degres to radians
			Double tan = Math.Tan (angle / 2.0); // Calculate tan(FovY/2)
			Double aspect = Width / Height; // Calculate aspect ratio of bitmap

			Double w = -aspect * tan;
			Double h = tan;

			Double dx = 2.0 * aspect * tan / Width;
			Double dy = 2.0 * tan / Height;

			for (int i = 0; i < Width; i++)
			{
				w += dx; // increase w of the dx
				h = tan; // reset the h to it's intial value

				for (int j = 0; j < Height; j++)
				{
					Vector4 direction = (w * W + h * V + U).Normalized; // calculate direction vector
					Ray ray = new Ray (Position, direction); // create ray to i,j

					h -= dy; // decrement of the h

					foreach(Plane p in Planes){
						// check every plane if it collides with ray
						p.Collide(ray);
					}

					// if ray hits plane than save it's color to Pixels buffer
					if (ray.HitObject != null)
					{
						SetPixel (i, j, ray.HitObject.Color);
					}
				}
			}
		}

		/// <summary>
		/// Copy all the pixels from pixel buffer to the Bitmap
		/// </summary>
		public void PresentFrame()
		{
			for (int i = 0; i < Width; i++) {
				for (int j = 0; j < Height; j++) {
					Bitmap.SetPixel (i, j, GetPixel (i, j).ToColor ());
				}
			}
		}

		#endregion
	}
}
