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
using CG2.Shading;
using CG2.Lighting;

namespace CG2.Ex03
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
            World world = new World();

            #region Shaders

            Shader black = new PhongShader(new Vector4(0, 0, 0));
            Shader white = new PhongShader(new Vector4(1, 1, 1));
            Shader red = new PhongShader(new Vector4(1, 0, 0));
            Shader green = new PhongShader(new Vector4(0, 1, 0));
            Shader blue = new PhongShader(new Vector4(0, 0, 1));
            Shader yellow = new PhongShader(new Vector4(1, 1, 0));
            Shader pink = new PhongShader(new Vector4(1, 0, 1));
            Shader cyan = new PhongShader(new Vector4(0, 1, 1));
            Shader bluePhong = new PhongShader(new Vector4(0, 0, 1));
            Shader gray = new PhongShader(new Vector4(0.5, 0.5, 0.5));

            Shader unsatGreen = new PhongShader(new Vector4(124 / 255.0, 157 / 255.0, 123 / 255.0)); // From RGB

            Shader checker = new CheckerShader()
            {
                Shader0 = unsatGreen,
                Shader1 = white,
                CubeSize = 5
            };

            #endregion

            #region Models

            world.Models.Add(new Plane(checker, new Vector4(0, 0, 0), new Vector4(0, 0, 1)));
            world.Models.Add(new Sphere(red, new Vector4(0, 0, 4), 2));

            

            #endregion

            #region Lights

            SunLight sun = new SunLight()
            {
                Intensity = 1,
                Direction = new Vector4(-5, -5, -10)
            };

            world.Lights.Add(sun);

            #endregion

            camera = new Camera(500, 500)
            {
                Position = new Vector4(25, -4, 10),
                Target = new Vector4(0, 0, 3),
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

        private void bRender_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            DateTime t0 = DateTime.Now;
            camera.UseShadows = cbUseShadows.Checked;

            ReadValues();
            camera.Render();

            DateTime t1 = DateTime.Now;
            Cursor = Cursors.Default;
            lRenderTime.Text = "Rendering: " + (t1 - t0).TotalSeconds.ToString("F3") + " s";
            Invalidate();
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
