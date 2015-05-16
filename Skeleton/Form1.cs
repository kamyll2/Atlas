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
using OpenTK.Input;


#endregion

namespace Skeleton
{
    //[Example("Simple GLControl Game Loop", ExampleCategory.OpenTK, "GLControl", 2, Documentation = "GLControlGameLoop")]
    public partial class GameLoopForm : Form
    {
        static float angle = 0.0f;
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> texCoords = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();

        List<Model> Models = new List<Model>();
        int modelToShow = 0;
        int displayType = 1;
        MouseHelper mouseHelper = new MouseHelper();
        
        Shader shader;
        string vertexShader;
        string fragmentShader;

        int texture;

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

            //glEnable(GL_LIGHTING);
            //glEnable(GL_LIGHT0);
            //glEnable(GL_DEPTH_TEST);

            //GL.Enable(EnableCap.Lighting);
            //GL.Enable(EnableCap.Light0);
            //GL.Enable(EnableCap.DepthTest);

            
            vertexShader = System.IO.File.ReadAllText("..\\..\\Shaders\\vshader.txt");
            fragmentShader = System.IO.File.ReadAllText("..\\..\\Shaders\\fshader.txt");
            shader = new Shader(vertexShader, fragmentShader);
            //texture = Load(new Bitmap(Image.FromFile("D:\\texture.png")));
            Bitmap x1 = new Bitmap(1,1);
            x1.SetPixel(0, 0, Color.Gray);
            Model.DEFAULT_TEX = Load(x1);
            x1.SetPixel(0, 0, Color.Green);
            Model.SELECTED_TEX = Load(x1);
            int argb = 0;
            foreach (Model x in Models)
            //for(int i=0;i<68;i++)
            {
                
                //Model x = Models[i];
                x.setupVBO();
                x.setupVAO(shader);
                x.color = Color.FromArgb(argb);
                argb += 200000;
                x1.SetPixel(0, 0, x.color);
                x.tex0=Load(x1);
                x.tex1 = Load(x1);
            }
            Models[15].isSelected = true;
            
        }

        void glControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                displayType = displayType == 0 ? 1 : 0;
            }
        }

        #endregion

        #region OnClosing

        protected override void OnClosing(CancelEventArgs e)
        {
            /*foreach (Model x in Models)
            {
                x.freeVAO();
                x.freeVBO();
            }*/
            Application.Idle -= Application_Idle;
            Shader.Bind(null);
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
            Matrix4 matV = Matrix4.LookAt(0, 0, 30, 0, -1, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref matV);
            
            //GL.Rotate(angle, 0.0f, 1.0f, 0.0f);
            angle += 0.5f;
            
            Matrix4 matP = Matrix4.Perspective(-50.0F,glControl.Width/glControl.Height,1.0F,50.0F);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            MouseState mouse = Mouse.GetState();

            mouseHelper.updateMouseParams(mouse);

            boneNameENG.Text = mouse.Wheel.ToString();
            
            //matM=glm::translate(matM,glm::vec3(0.0,0.0,odleglosc));
            //GL.MatrixMode(MatrixMode.Projection);

                Matrix4 matM = Matrix4.Identity;
                //matM = matM * Matrix4.Scale(0.1F, 0.1F, 0.1F);
                matM = matM * Matrix4.CreateScale(0.1F, 0.1F, 0.1F);

                matM = matM * Matrix4.Rotate(new Vector3(1.0F, 0.0F, 0.0F), 0.05f*mouseHelper.rotY);
                matM = matM * Matrix4.Rotate(new Vector3(0.0F, 1.0F, 0.0F), 0.05f * mouseHelper.rotX);
                //matM = matM * Matrix4.Translation(0.0f, 0.0f, 0.1F * mouseHelper.mouseWHEELstate);
                matM = matM * Matrix4.CreateTranslation(0.0f, 0.0f, 0.3f*mouseHelper.mouseWHEELstate);


                shader.SetVariable("type1", (float)displayType);
                shader.SetVariable("V", matV);
                shader.SetVariable("P", matP);
                shader.SetVariable("M", matM);

                Shader.Bind(shader);
                DrawCube();
            

            
            glControl.SwapBuffers();
        }
        
        #endregion

        #region private void DrawCube()

        private void DrawCube()
        {
            if (displayType == 1)
            {
                foreach (Model x in Models)
                {
                    //x.drawModel();
                    x.drawColor(shader);
                }
            }
            else
            {
                foreach (Model x in Models)
                {
                    //x.drawModel();
                    x.drawDefault(shader);
                }
            }
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
                example.readObjVertices("..\\..\\Models\\skeleton_export_2.obj");
                example.ShowDialog();
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            modelToShow++;
        }

        public void readObjVertices(string path)
        {
            List<int> tempFaces=null;

            StreamReader file = new StreamReader(path);
            while (!file.EndOfStream)
            {
                string temp = file.ReadLine();
                string[] temp2 = temp.Split(' ');

                switch(temp2[0]){
                    case "o":
                    {
                        Model tempModel = new Model(temp2[1]);
                        Models.Add(tempModel);
                        tempFaces = Models[Models.Count - 1].faces;
                    }break;
                    case "v":
                    {
                        Vector3 x = new Vector3((float)System.Convert.ChangeType(temp2[1], typeof(float)),
                                                (float)System.Convert.ChangeType(temp2[2], typeof(float)),
                                                (float)System.Convert.ChangeType(temp2[3], typeof(float)));
                        vertices.Add(x);

                    }break;
                    case "vt":
                    {
                        Vector2 x = new Vector2((float)System.Convert.ChangeType(temp2[1], typeof(float)),
                        -1.0F*(float)System.Convert.ChangeType(temp2[2], typeof(float)));
                        texCoords.Add(x);
                    }break;
                    case "vn":
                        {
                            Vector3 x = new Vector3((float)System.Convert.ChangeType(temp2[1], typeof(float)),
                                                    (float)System.Convert.ChangeType(temp2[2], typeof(float)),
                                                    (float)System.Convert.ChangeType(temp2[3], typeof(float)));
                            normals.Add(x);

                        } break;
                    case "f":
                    {
                        string[] s = new string[1];
                        s[0] = "//";
                        for (int i = 1; i < 4; i++)
                        {
                            string[] temp3 = temp2[i].Split(s, StringSplitOptions.None);
                            tempFaces.Add(System.Convert.ToInt32(temp3[0]));
                            tempFaces.Add(i);
                            tempFaces.Add(System.Convert.ToInt32(temp3[1]));
                        }
                    }break;
                }

            }
            file.Close();

            foreach (Model x in Models)
            {
                x.parseArrays(vertices, normals, texCoords);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            displayType = displayType==0 ? 1:0;
        }

        private int Load(Bitmap bitmap, bool IsRepeated = false, bool IsSmooth = true)
        {
            try
            {
                int TextureID = 0;
                GL.GenTextures(1, out TextureID);
                
                GL.BindTexture(TextureTarget.Texture2D, TextureID);

                System.Drawing.Imaging.BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bitmap.UnlockBits(data);

                // Setup filtering
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, IsRepeated ? Convert.ToInt32(TextureWrapMode.Repeat) : Convert.ToInt32(TextureWrapMode.ClampToEdge));
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, IsRepeated ? Convert.ToInt32(TextureWrapMode.Repeat) : Convert.ToInt32(TextureWrapMode.ClampToEdge));
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, IsSmooth ? Convert.ToInt32(TextureMagFilter.Linear) : Convert.ToInt32(TextureMagFilter.Nearest));
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, IsSmooth ? Convert.ToInt32(TextureMinFilter.Linear) : Convert.ToInt32(TextureMinFilter.Nearest));

                return TextureID;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating new Texture:" + Environment.NewLine + ex.Message, "Error");
                return 0;
            }
        }

        private void glControl_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Console.WriteLine(ColorPicker.getInstance().readPixelColor(new Point(e.X, e.Y)));
            }
        }
    }
}
