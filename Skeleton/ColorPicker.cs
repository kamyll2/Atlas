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


namespace Skeleton
{
    class ColorPicker
    {
        private static ColorPicker instance;

        public static ColorPicker getInstance()
        {
            if (instance == null)
            {
                instance = new ColorPicker();
            }

            return instance;
        }

        public Color readPixelColor(Point pixel) {
                            /*
                GL.GetInteger(GetPName.Viewport, viewport);
                GL.SelectBuffer(BUFSIZE, selectBuffer);
                GL.RenderMode(RenderingMode.Select);
                GL.InitNames();
                GL.PushName(0);
                GL.MatrixMode(MatrixMode.Projection);
                GL.PushMatrix();
                GL.LoadIdentity();
                Byte4 Pixel = new Byte4();
                GL.ReadPixels(e.X, viewport[3] - e.Y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, ref Pixel);
                uint SelectedTriangle = SelectedTriangle = Pixel.ToUInt32();
                GL.Ortho(0, 3, 0, 3, 1, -1); // Bottom-left corner pixel has coordinate (0, 0)                 
                DrawSquares(GL.RenderMode(RenderingMode.Select));
                GL.MatrixMode(MatrixMode.Projection);
                GL.PopMatrix();
                GL.Flush();
                hits = GL.RenderMode(RenderingMode.Render);
                ProcessHits(hits, selectBuffer);
                glControl1.SwapBuffers();
                */
                byte[] color = new byte[300];
                int[] viewport = new int[4];
                GL.GetInteger(GetPName.Viewport, viewport);

                // Read color of pixel at the given position of the active/current viewport (the -4 was added because picking was off by 4 pixels.
                // Also, the Y coordinate is transformed from screen coordinates to OpenGL coordinates
                GL.ReadPixels(pixel.X, viewport[3] - pixel.Y - 4, 10, 10, PixelFormat.Rgb, PixelType.UnsignedByte, color);

                // Convert to Color type
                Color pixelColor = Color.FromArgb(color[0], color[1], color[2]);
                //Console.WriteLine(pixelColor);
                //int index = IntFromColor(pixelColor);
                //return index;
                return pixelColor;
        }
    }
}
