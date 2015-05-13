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
}
