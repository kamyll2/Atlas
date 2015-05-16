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
        public string name;
        public List<int> faces;

        public Model()
        {
            faces = new List<int>();
        }

        public Model(string param1)
        {
            name = param1;
            faces = new List<int>();
        }

        public void drawModel(List<Vector3> vertices)
        {
            GL.Begin(BeginMode.Triangles);

            GL.Color3(System.Drawing.Color.Silver);

            foreach (int x in faces)
            {
                GL.Vertex3(vertices[x - 1]);
            }
            GL.End();
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

        private int Program = 0;
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

            if (Program > 0)
                GL.DeleteProgram(Program);

            Variables.Clear();

            Program = GL.CreateProgram();

            if (vertexSource != "")
            {
                int vertexShader = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vertexShader, vertexSource);
                GL.CompileShader(vertexShader);
                GL.GetShaderInfoLog(vertexShader, out info);
                GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out status_code);

                if (status_code != 1)
                {
                    Console.WriteLine("Failed to Compile Vertex Shader Source." +
                        Environment.NewLine + info + Environment.NewLine + "Status Code: " + status_code.ToString());

                    GL.DeleteShader(vertexShader);
                    GL.DeleteProgram(Program);
                    Program = 0;

                    return false;
                }

                GL.AttachShader(Program, vertexShader);
                GL.DeleteShader(vertexShader);
            }

            if (fragmentSource != "")
            {
                int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(fragmentShader, fragmentSource);
                GL.CompileShader(fragmentShader);
                GL.GetShaderInfoLog(fragmentShader, out info);
                GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out status_code);

                if (status_code != 1)
                {
                    Console.WriteLine("Failed to Compile Fragment Shader Source." +
                        Environment.NewLine + info + Environment.NewLine + "Status Code: " + status_code.ToString());

                    GL.DeleteShader(fragmentShader);
                    GL.DeleteProgram(Program);
                    Program = 0;

                    return false;
                }

                GL.AttachShader(Program, fragmentShader);
                GL.DeleteShader(fragmentShader);
            }

            GL.LinkProgram(Program);
            GL.GetProgramInfoLog(Program, out info);
            GL.GetProgram(Program, GetProgramParameterName.LinkStatus, out status_code);

            if (status_code != 1)
            {
                Console.WriteLine("Failed to Link Shader Program." +
                    Environment.NewLine + info + Environment.NewLine + "Status Code: " + status_code.ToString());

                GL.DeleteProgram(Program);
                Program = 0;

                return false;
            }

            return true;
        }

        private int GetVariableLocation(string name)
        {
            if (Variables.ContainsKey(name))
                return Variables[name];

            int location = GL.GetUniformLocation(Program, name);

            if (location != -1)
                Variables.Add(name, location);
            else
                Console.WriteLine("Failed to retrieve Variable Location." +
                    Environment.NewLine + "Variable Name not found.", "Error");

            return location;
        }

        /// <summary>
        /// Change a value Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="x">Value</param>
        public void SetVariable(string name, float x)
        {
            if (Program > 0)
            {
                GL.UseProgram(Program);

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
            if (Program > 0)
            {
                GL.UseProgram(Program);

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
            if (Program > 0)
            {
                GL.UseProgram(Program);

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
            if (Program > 0)
            {
                GL.UseProgram(Program);

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
            if (Program > 0)
            {
                GL.UseProgram(Program);

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
            if (shader != null && shader.Program > 0)
            {
                GL.UseProgram(shader.Program);
            }
            else
            {
                GL.UseProgram(0);
            }
        }

        public void Dispose()
        {
            if (Program != 0)
                GL.DeleteProgram(Program);
        }
    }
}
