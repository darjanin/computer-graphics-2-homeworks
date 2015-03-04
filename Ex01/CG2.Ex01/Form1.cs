using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using CG2.Rendering;
using CG2.Mathematics;

namespace CG2.Ex01
{
    public partial class Form1 : Form
    {
        Camera camera;

        public Form1()
        {
            InitializeComponent();

            //Info: Init scene
            //ToDo: Do you have the guts to uncomment me? Go on, try.
            
            camera = new Camera(500, 500)
            {
                Position = new Vector4(20, 0, 0),
                Target = new Vector4(0, 0, 0),
            };

            WriteValues();
            //Info: zFar
            camera.Planes.Add(new Plane(new Vector4(-5, 0, 0), new Vector4(1, 0, 0)) { Color = new Vector4(1, 0, 0) });

            //Info: Left
            camera.Planes.Add(new Plane(new Vector4(0, -5, 0), new Vector4(0, 1, 0)) { Color = new Vector4(0, 1, 0) });

            //Info: Right
            camera.Planes.Add(new Plane(new Vector4(0, 5, 0), new Vector4(0, -1, 0)) { Color = new Vector4(0, 1, 0) });

            //Info: Bottom
            camera.Planes.Add(new Plane(new Vector4(0, 0, -5), new Vector4(0, 0, 1)) { Color = new Vector4(0, 0, 1) });

            //Info: Top
            camera.Planes.Add(new Plane(new Vector4(0, 0, 5), new Vector4(0, 0, -1)) { Color = new Vector4(0, 0, 1) });
             
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //ToDo: Do you have the guts to uncomment me? Go on, try.
            g.DrawImage(camera.Bitmap, 0, 0);
        }

        private void bRender_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            DateTime t0 = DateTime.Now;

            ReadValues();

            //ToDo: Do you have the guts to uncomment me? Go on, try.
            camera.Render();

            DateTime t1 = DateTime.Now;
            Cursor = Cursors.Default;
            lRenderTime.Text = "Rendering: " + (t1 - t0).TotalMilliseconds.ToString("F0") + " ms";
            Invalidate();
        }

        Double Parse(String text)
        {
            NumberStyles styles = NumberStyles.Integer | NumberStyles.AllowDecimalPoint;
            CultureInfo provider = new CultureInfo("en-US");
            double res = 0;
            float rr = 0;
            double.TryParse(text, styles, provider, out res);
            float.TryParse(text, styles, provider, out rr);
            return res;
        }

        private void ReadValues()
        {
            //ToDo: Do you have the guts to uncomment me? Go on, try.
            
            camera.FovY = Parse(textBox1.Text);
            camera.Position.X = Parse(textBox2.Text);
            camera.Position.Y = Parse(textBox3.Text);
            camera.Position.Z = Parse(textBox4.Text);

            camera.Target.X = Parse(textBox5.Text);
            camera.Target.Y = Parse(textBox6.Text);
            camera.Target.Z = Parse(textBox7.Text);
             
        }

        private void WriteValues()
        {
            //ToDo: Do you have the guts to uncomment me? Go on, try.
            
            textBox1.Text = camera.FovY.ToString();

            textBox2.Text = camera.Position.X.ToString();
            textBox3.Text = camera.Position.Y.ToString();
            textBox4.Text = camera.Position.Z.ToString();

            textBox5.Text = camera.Target.X.ToString();
            textBox6.Text = camera.Target.Y.ToString();
            textBox7.Text = camera.Target.Z.ToString();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ToDo: Do you have the guts to uncomment me? Go on, try.
            
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Images|*.png;*.bmp;*.jpg";
            sfd.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\Output";
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                camera.Bitmap.Save(sfd.FileName, format);
            
        }
    }
}
