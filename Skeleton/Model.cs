using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using OpenTK.Input;

namespace Skeleton
{
    class MouseHelper
    {
        public float rotX;
        public float rotY;
        public int mouseXclick;
        public int mouseYclick;
        public bool mouseRBpaststate;
        public bool mouseRBstate;
        public bool isMMBDown;
        public float mouseWHEELstate;


        public MouseHelper()
        {
            rotX = 0.0F;
            rotY = 0.0F;
            mouseRBpaststate = false;
            mouseRBstate = false;
            mouseWHEELstate = 0;
        }
        public void updateMouseParams(MouseState param1)
        {
            mouseRBstate = param1[MouseButton.Right] ? true : false;
            isMMBDown = param1[MouseButton.Middle] ? true : false;
            if (mouseRBstate && !mouseRBpaststate)
            {
                mouseXclick = param1.X - 3 * (int)rotX;
                mouseYclick = param1.Y - 3 * (int)rotY;
            }
            if (mouseRBstate)
            {
                rotY = (param1.Y - mouseYclick) / 3.0F;
                rotX = (param1.X - mouseXclick) / 3.0F;
            }
            mouseRBpaststate = mouseRBstate;

            mouseWHEELstate = 3.0F * param1.Wheel;
        }
    }

    class Model
    {
        public static int DEFAULT_TEX;
        public static int SELECTED_TEX;
        public System.Drawing.Color color;
        public bool isSelected = false;

        int[] vao=new int[1];
        int bufVertices; //Uchwyt na bufor VBO przechowujšcy tablicę wsp. wierzch
        //GLuint bufColors;  //Uchwyt na bufor VBO przechowujšcy tablicę kolorów
        int bufNormals; //Uchwyt na bufor VBO przechowujšcy tablicę wektorów norm.
        int bufTexCoords;
        public int tex0;
        public int tex1;
        float[] vertices;
        float[] normals;
        float[] texCoords;
        int vertexCount;

        public string name;
        public String namePL;
        public String wikipediaURL;
        public List<int> faces;

        public void setupVBO()
        {
            bufVertices = makeBuffer(vertices, vertexCount, sizeof(float) * 4);
            bufNormals = makeBuffer(normals, vertexCount, sizeof(float) * 4);
            bufTexCoords = makeBuffer(texCoords, vertexCount, sizeof(float) * 2);
        }

        public void setupVAO(Shader shaderProgram)
        {
            GL.GenVertexArrays(1, vao);
            GL.BindVertexArray(vao[0]);
            assignVBOtoAttribute("vertex", bufVertices, 4,shaderProgram);
            assignVBOtoAttribute("normal", bufNormals, 4, shaderProgram);
            assignVBOtoAttribute("texCoord", bufTexCoords, 2, shaderProgram);
            GL.BindVertexArray(0);
        }

        public void freeVBO()
        {
            GL.DeleteBuffer(bufVertices);
            GL.DeleteBuffer(bufNormals);
            GL.DeleteBuffer(bufTexCoords);
        }

        public void freeVAO()
        {
            GL.DeleteVertexArray(vao[0]);
        }

        public Model()
        {
            faces = new List<int>();
        }

        public Model(string param1)
        {
            name = param1;
            faces = new List<int>();
        }

        public int makeBuffer(float[] data, int vertexCount, int vertexSize)
        {
            int[] handle = new int[1];
            GL.GenBuffers(1, handle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (System.IntPtr)(vertexCount*vertexSize), data, BufferUsageHint.StaticDraw);
            return handle[0];
        }


        public void assignVBOtoAttribute(string attributeName, int bufVBO, int variableSize, Shader shaderProgram)
        {
            int location = shaderProgram.GetAttribLocation(attributeName);
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufVBO);
            GL.EnableVertexAttribArray(location);
            GL.VertexAttribPointer(location, variableSize, VertexAttribPointerType.Float, false, 0, 0/*NULL*/); 
        }

        public void drawModel()
        {
            Shader.Bind(null);
            GL.Begin(BeginMode.Triangles);

            GL.Color3(System.Drawing.Color.Silver);

            for (int i = 0; i < vertexCount; i += 4)
            {
                GL.Vertex3(vertices[i], vertices[i + 1], vertices[i + 2]);
                
            }
            GL.End();
        }
        
        public void drawColor(Shader param1)
        {
            GL.Uniform1(param1.GetUniformLocation("textureMap0"), 0);
            GL.Uniform1(param1.GetUniformLocation("textureMap1"), 1);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, tex0);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, tex1);

            GL.BindVertexArray(vao[0]);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
            GL.BindVertexArray(0);
        }

        public void drawDefault(Shader param1)
        {
            GL.Uniform1(param1.GetUniformLocation("textureMap0"), 0);
            GL.Uniform1(param1.GetUniformLocation("textureMap1"), 1);
            if (isSelected)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, SELECTED_TEX);
                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture2D, SELECTED_TEX);
            }
            else
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, DEFAULT_TEX);
                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture2D, DEFAULT_TEX);
            }
            GL.BindVertexArray(vao[0]);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
            GL.BindVertexArray(0);
        }

        public void parseArrays(List<Vector3>vertices, List<Vector3> normals, List<Vector2>texCoords)
        {
            List<float> tempVertices = new List<float>();
            List<float> tempNormals = new List<float>();
            List<float> tempTexCoords = new List<float>();

            for (int i = 0; i < faces.Count; i += 3)
            {
                tempVertices.Add(vertices[faces[i]-1].X);
                tempVertices.Add(vertices[faces[i]-1].Y);
                tempVertices.Add(vertices[faces[i]-1].Z);
                tempVertices.Add(1.0F);

                tempTexCoords.Add(texCoords[faces[i + 1] - 1].X);
                tempTexCoords.Add(texCoords[faces[i + 1] - 1].Y);

                tempNormals.Add(normals[faces[i+2]-1].X);
                tempNormals.Add(normals[faces[i + 2] - 1].Y);
                tempNormals.Add(normals[faces[i + 2] - 1].Z);
                tempNormals.Add(0.0F);
            }

            vertexCount = tempVertices.Count/4;
            this.vertices = new float[tempVertices.Count];
            for (int i = 0; i < tempVertices.Count; i++)
            {
                this.vertices[i] = tempVertices[i];
            }

            this.normals = new float[tempNormals.Count];
            for (int i = 0; i < tempNormals.Count; i++)
            {
                this.normals[i] = tempNormals[i];
            }

            this.texCoords = new float[tempTexCoords.Count];
            for (int i = 0; i < tempTexCoords.Count; i++)
            {
                this.texCoords[i] = tempTexCoords[i];
            }

        }
        
    }


    public class Shader : IDisposable
    {

        /// <summary>
        /// Type of Shader
        /// </summary>
        public enum Type
        {
            Vertex = 0x1,
            Fragment = 0x2
        }

        /// <summary>
        /// Get Whether the Shader function is Available on this Machine or not
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                return (new Version(GL.GetString(StringName.Version).Substring(0, 3)) >= new Version(2, 0) ? true : false);
            }
        }

        private int shaderProgram = 0;
        public int vertexShader;
        public int fragmentShader;
        private Dictionary<string, int> Variables = new Dictionary<string, int>();

        /// <summary>
        /// Create a new Shader
        /// </summary>
        /// <param name="source">Vertex or Fragment Source</param>
        /// <param name="type">Type of Source Code</param>
        public Shader(string source, Type type)
        {
            if (!IsSupported)
            {
                Console.WriteLine("Failed to create Shader." +
                    Environment.NewLine + "Your system doesn't support Shader.", "Error");
                return;
            }

            if (type == Type.Vertex)
                Compile(source, "");
            else
                Compile("", source);
        }

        /// <summary>
        /// Create a new Shader
        /// </summary>
        /// <param name="source">Vertex or Fragment Source</param>
        /// <param name="type">Type of Source Code</param>
        public Shader(string vsource, string fsource)
        {
            if (!IsSupported)
            {
                Console.WriteLine("Failed to create Shader." +
                    Environment.NewLine + "Your system doesn't support Shader.", "Error");
                return;
            }

            Compile(vsource, fsource);
        }

        // I prefer to return the bool rather than throwing an exception lol
        private bool Compile(string vertexSource = "", string fragmentSource = "")
        {
            int status_code = -1;
            string info = "";

            if (vertexSource == "" && fragmentSource == "")
            {
                Console.WriteLine("Failed to compile Shader." +
                    Environment.NewLine + "Nothing to Compile.", "Error");
                return false;
            }

            if (shaderProgram > 0)
                GL.DeleteProgram(shaderProgram);

            Variables.Clear();

            shaderProgram = GL.CreateProgram();

            if (vertexSource != "")
            {
                vertexShader = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vertexShader, vertexSource);
                GL.CompileShader(vertexShader);
                GL.GetShaderInfoLog(vertexShader, out info);
                GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out status_code);

                if (status_code != 1)
                {
                    Console.WriteLine("Failed to Compile Vertex Shader Source." +
                        Environment.NewLine + info + Environment.NewLine + "Status Code: " + status_code.ToString());

                    GL.DeleteShader(vertexShader);
                    GL.DeleteProgram(shaderProgram);
                    shaderProgram = 0;

                    return false;
                }

                GL.AttachShader(shaderProgram, vertexShader);
                GL.DeleteShader(vertexShader);
            }

            if (fragmentSource != "")
            {
                fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(fragmentShader, fragmentSource);
                GL.CompileShader(fragmentShader);
                GL.GetShaderInfoLog(fragmentShader, out info);
                GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out status_code);

                if (status_code != 1)
                {
                    Console.WriteLine("Failed to Compile Fragment Shader Source." +
                        Environment.NewLine + info + Environment.NewLine + "Status Code: " + status_code.ToString());

                    GL.DeleteShader(fragmentShader);
                    GL.DeleteProgram(shaderProgram);
                    shaderProgram = 0;

                    return false;
                }

                GL.AttachShader(shaderProgram, fragmentShader);
                GL.DeleteShader(fragmentShader);
            }

            GL.LinkProgram(shaderProgram);
            GL.GetProgramInfoLog(shaderProgram, out info);
            GL.GetProgram(shaderProgram, GetProgramParameterName.LinkStatus, out status_code);

            if (status_code != 1)
            {
                Console.WriteLine("Failed to Link Shader Program." +
                    Environment.NewLine + info + Environment.NewLine + "Status Code: " + status_code.ToString());

                GL.DeleteProgram(shaderProgram);
                shaderProgram = 0;

                return false;
            }

            return true;
        }

        private int GetVariableLocation(string name)
        {
            if (Variables.ContainsKey(name))
                return Variables[name];

            int location = GL.GetUniformLocation(shaderProgram, name);

            if (location != -1)
                Variables.Add(name, location);
            else
                Console.WriteLine("Failed to retrieve Variable Location." +
                    Environment.NewLine + "Variable Name not found.", "Error");

            return location;
        }

        public int GetAttribLocation(string name)
        {
            return GL.GetAttribLocation(shaderProgram,name);
        }

        public int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(shaderProgram, name);
        }

        /// <summary>
        /// Change a value Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="x">Value</param>
        public void SetVariable(string name, float x)
        {
            if (shaderProgram > 0)
            {
                GL.UseProgram(shaderProgram);

                int location = GetVariableLocation(name);
                if (location != -1)
                    GL.Uniform1(location, x);

                GL.UseProgram(0);
            }
        }

        /// <summary>
        /// Change a 2 value Vector Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="x">First Vector Value</param>
        /// <param name="y">Second Vector Value</param>
        public void SetVariable(string name, float x, float y)
        {
            if (shaderProgram > 0)
            {
                GL.UseProgram(shaderProgram);

                int location = GetVariableLocation(name);
                if (location != -1)
                    GL.Uniform2(location, x, y);

                GL.UseProgram(0);
            }
        }

        /// <summary>
        /// Change a 3 value Vector Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="x">First Vector Value</param>
        /// <param name="y">Second Vector Value</param>
        /// <param name="z">Third Vector Value</param>
        public void SetVariable(string name, float x, float y, float z)
        {
            if (shaderProgram > 0)
            {
                GL.UseProgram(shaderProgram);

                int location = GetVariableLocation(name);
                if (location != -1)
                    GL.Uniform3(location, x, y, z);

                GL.UseProgram(0);
            }
        }

        /// <summary>
        /// Change a 4 value Vector Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="x">First Vector Value</param>
        /// <param name="y">Second Vector Value</param>
        /// <param name="z">Third Vector Value</param>
        /// <param name="w">Fourth Vector Value</param>
        public void SetVariable(string name, float x, float y, float z, float w)
        {
            if (shaderProgram > 0)
            {
                GL.UseProgram(shaderProgram);

                int location = GetVariableLocation(name);
                if (location != -1)
                    GL.Uniform4(location, x, y, z, w);

                GL.UseProgram(0);
            }
        }

        /// <summary>
        /// Change a Matrix4 Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="matrix">Matrix</param>
        public void SetVariable(string name, Matrix4 matrix)
        {
            if (shaderProgram > 0)
            {
                GL.UseProgram(shaderProgram);

                int location = GetVariableLocation(name);
                if (location != -1)
                {
                    // Well cannot use ref on lambda expression Lol
                    // So we need to call Check error manually
                    GL.UniformMatrix4(location, false, ref matrix);
                    
                }

                GL.UseProgram(0);
            }
        }

        /// <summary>
        /// Change a 2 value Vector Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="vector">Vector Value</param>
        public void SetVariable(string name, Vector2 vector)
        {
            SetVariable(name, vector.X, vector.Y);
        }

        /// <summary>
        /// Change a 3 value Vector Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="vector">Vector Value</param>
        public void SetVariable(string name, Vector3 vector)
        {
            SetVariable(name, vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Change a Color Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="color">Color Value</param>
        public void SetVariable(string name, System.Drawing.Color color)
        {
            SetVariable(name, color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }

        /// <summary>
        /// Bind a Shader for Rendering
        /// </summary>
        /// <param name="shader">Shader to bind</param>
        public static void Bind(Shader shader)
        {
            if (shader != null && shader.shaderProgram > 0)
            {
                GL.UseProgram(shader.shaderProgram);
            }
            else
            {
                GL.UseProgram(0);
            }
        }

        public void Dispose()
        {
            if (shaderProgram != 0)
                GL.DeleteProgram(shaderProgram);
        }
    }
}
