#region --- License ---
/* This source file is released under the MIT license. See License.txt for more information.
 * Coded by Erik Ylvisaker and Stefanos Apostolopoulos.
 */
#endregion

#region --- Using directives ---

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;

using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using OpenTK;

#endregion

namespace Examples.WinForms
{
    //[Example("Simple GLControl Game Loop", ExampleCategory.OpenTK, "GLControl", 2, Documentation = "GLControlGameLoop")]
    public partial class GameLoopForm : Form
    {
        static float angle = 0.0f;
        List<Vector3> vertices = new List<Vector3>();
        List<int> faces = new List<int>();

        #region --- Constructor ---

        public GameLoopForm()
        {
            InitializeComponent();
        }

        #endregion

        #region OnLoad

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            glControl.KeyDown += new KeyEventHandler(glControl_KeyDown);
            glControl.KeyUp += new KeyEventHandler(glControl_KeyUp);
            glControl.Resize += new EventHandler(glControl_Resize);
            glControl.Paint += new PaintEventHandler(glControl_Paint);

            Text =
                GL.GetString(StringName.Vendor) + " " +
                GL.GetString(StringName.Renderer) + " " +
                GL.GetString(StringName.Version);

            GL.ClearColor(Color.MidnightBlue);
            GL.Enable(EnableCap.DepthTest);

            Application.Idle += Application_Idle;

            // Ensure that the viewport and projection matrix are set correctly.
            glControl_Resize(glControl, EventArgs.Empty);
        }

        void glControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
                glControl.GrabScreenshot().Save("screenshot.png");
        }

        #endregion

        #region OnClosing

        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Idle -= Application_Idle;

            base.OnClosing(e);
        }

        #endregion

        #region Application_Idle event

        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl.IsIdle)
            {
                Render();
            }
        }

        #endregion

        #region GLControl.Resize event handler

        void glControl_Resize(object sender, EventArgs e)
        {
            OpenTK.GLControl c = sender as OpenTK.GLControl;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new System.Drawing.Size(c.ClientSize.Width, 1);

            GL.Viewport(0, 0, c.ClientSize.Width, c.ClientSize.Height);

            float aspect_ratio = Width / (float)Height;
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);
        }

        #endregion

        #region GLControl.KeyDown event handler

        void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        #endregion

        #region GLControl.Paint event handler

        void glControl_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        #endregion

        #region private void Render()

        private void Render()
        {
            Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            GL.Rotate(angle, 0.0f, 1.0f, 0.0f);
            angle += 0.2f;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            DrawCube();

            glControl.SwapBuffers();
        }

        #endregion

        #region private void DrawCube()

        private void DrawCube()
        {
            GL.Begin(BeginMode.Triangles);

            GL.Color3(Color.Silver);

            foreach (int x in faces)
            {
                //Console.WriteLine("face\t" + x);
                GL.Vertex3(vertices[x-1]);
            }

            GL.End();
        }

        #endregion

        #region public static void Main()

        /// <summary>
        /// Entry point of this example.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            
            using (GameLoopForm example = new GameLoopForm())
            {
                // Get the title and category  of this example using reflection.
                //ExampleAttribute info = ((ExampleAttribute)example.GetType().GetCustomAttributes(false)[0]);
                //example.Text = String.Format("OpenTK | {0} {1}: {2}", info.Category, info.Difficulty, info.Title);
                example.readObjVertices("..\\..\\Models\\skeleton.obj");
                
                example.ShowDialog();
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        public void readObjVertices(string path)
        {
            StreamReader file = new StreamReader(path);
            while (!file.EndOfStream)
            {
                string temp = file.ReadLine();
                string[] temp2 = temp.Split(' ');
                if (temp2[0] == "v")
                {
                    Vector3 x = new Vector3((float)System.Convert.ChangeType(temp2[1],typeof(float)), 
                                            (float)System.Convert.ChangeType(temp2[2],typeof(float)), 
                                            (float)System.Convert.ChangeType(temp2[3],typeof(float)));
                    vertices.Add(x);
                    
                }
                else if (temp2[0] == "f")
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        string[] temp3 = temp2[i].Split('/');
                        faces.Add(System.Convert.ToInt32(temp3[0]));
                        //faces.Add(System.Convert.ToInt32(temp3[1]));
                        //faces.Add(System.Convert.ToInt32(temp3[2]));
                    }
                }

            }
            file.Close();
        }
    }
}
