using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace Skeleton
{
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
}
