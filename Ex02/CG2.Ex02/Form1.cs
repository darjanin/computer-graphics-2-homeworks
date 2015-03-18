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
using CG2.Modeling;

namespace CG2.Ex02
{
    public partial class Form1 : Form
    {
        Camera camera;
        World world = new World();
        CultureInfo provider = new CultureInfo("en-US");

        public Form1()
        {
            InitializeComponent();
            cbScene.SelectedIndex = 0;
        }

        public void InitScene1()
        {
            world = new World();
            
            #region Models
            //zFar
            world.Models.Add(new Plane(new Vector4(-5, 0, 0), new Vector4(1, 0, 0)) { Color = new Vector4(1, 0, 0) });
            //Left
            world.Models.Add(new Plane(new Vector4(0, -5, 0), new Vector4(0, 1, 0)) { Color = new Vector4(0, 1, 0) });
            //Right
            world.Models.Add(new Plane(new Vector4(0, 5, 0), new Vector4(0, -1, 0)) { Color = new Vector4(0, 1, 0) });
            //Bottom
            world.Models.Add(new Plane(new Vector4(0, 0, -5), new Vector4(0, 0, 1)) { Color = new Vector4(0, 0, 1) });
            //Top
            world.Models.Add(new Plane(new Vector4(0, 0, 5), new Vector4(0, 0, -1)) { Color = new Vector4(0, 0, 1) });

            //ToDo: Do you have the guts to uncomment me? Go on, try.
            world.Models.Add(new Sphere(new Vector4(0, 4, -4), 4) { Color = new Vector4(1, 1, 0) });
            
            world.Models.Add(new AABB(new Vector4(-5, -5, -5), new Vector4(2, 0, -3)) { Color = new Vector4(1, 0, 1) });

            world.Models.Add(new Triangle(new Vector4(8, -4, 0), new Vector4(-2, -2, -1), new Vector4(-2, -1, 4)) { Color = new Vector4(1, 0.7, 0) });

            world.Models.Add(new Ring(new Vector4(3, 4, 0), new Vector4(0, 1, -0.2), 2.5) { Color = new Vector4(0.9, 0.7, 0.5) });

            #endregion

            camera = new Camera(500, 500)
            {
                Position = new Vector4(20, 0, 0),
                Target = new Vector4(0, 0, 0),
                World = world,
                zNear = 0.01,
                zFar = 100.0,
            };
            WriteValues();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawImage(camera.Bitmap, 0, 0);
        }

        private void bRender_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            DateTime t0 = DateTime.Now;

            ReadValues();
            camera.Render();

            DateTime t1 = DateTime.Now;
            Cursor = Cursors.Default;
            lRenderTime.Text = "Rendering: " + (t1 - t0).TotalSeconds.ToString("F3") + " s";
            Invalidate();
        }

        Double Parse(String text)
        {
            NumberStyles styles = NumberStyles.Integer | NumberStyles.AllowDecimalPoint;
            double res = 0;
            float rr = 0;
            double.TryParse(text, styles, provider, out res);
            float.TryParse(text, styles, provider, out rr);
            return res;
        }

        private void ReadValues()
        {
            camera.FovY = Parse(textBox1.Text);
            camera.zNear = Parse(textBox8.Text);
            camera.zFar = Parse(textBox9.Text);

            camera.Width = (int)Parse(textBox11.Text);
            camera.Height = (int)Parse(textBox10.Text);

            camera.Position.X = Parse(textBox2.Text);
            camera.Position.Y = Parse(textBox3.Text);
            camera.Position.Z = Parse(textBox4.Text);

            camera.Target.X = Parse(textBox5.Text);
            camera.Target.Y = Parse(textBox6.Text);
            camera.Target.Z = Parse(textBox7.Text);
        }

        private void WriteValues()
        {
            textBox1.Text = camera.FovY.ToString(provider);
            textBox8.Text = camera.zNear.ToString(provider);
            textBox9.Text = camera.zFar.ToString(provider);

            textBox11.Text = camera.Width.ToString(provider);
            textBox10.Text = camera.Height.ToString(provider);

            textBox2.Text = camera.Position.X.ToString(provider);
            textBox3.Text = camera.Position.Y.ToString(provider);
            textBox4.Text = camera.Position.Z.ToString(provider);

            textBox5.Text = camera.Target.X.ToString(provider);
            textBox6.Text = camera.Target.Y.ToString(provider);
            textBox7.Text = camera.Target.Z.ToString(provider);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Images|*.png;*.bmp;*.jpg";
            sfd.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\Output";
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                camera.Bitmap.Save(sfd.FileName, format);
        }

        private void cbScene_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((String)cbScene.SelectedItem)
            {
                case "Scene1": InitScene1(); break;
            }
        }
    }
}
