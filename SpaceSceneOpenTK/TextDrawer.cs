using System;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL;



namespace SpaceSceneOpenTK
{
    //
    // Text drawing methods for testing
    //

    // Green texture with `text` on the screen
    static class TextDrawer
    {
        static private int _vertexBufferObject;
        static private int _elementBufferObject;
        static private int _vertexArray;
        static private int _texture = -1;
        static private SizeF _textureSize;
        static int _height, _width;

        private static int new_StringTexture(string text, Font font, out SizeF textureSize)
        {
            int texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            //Font font = new Font("Courier", 16);
            Image fakeImage = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(fakeImage);
            textureSize = g.MeasureString(text, font);
            var bmp = new Bitmap((int)_textureSize.Width, (int)_textureSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var gfx = Graphics.FromImage(bmp);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            //gfx.Clear(Color.ForestGreen);
            gfx.DrawString(text, font, Brushes.Black, new PointF(0.0f, 0.0f));
            gfx.DrawImage(bmp, new PointF(0.0f, 0.0f));

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0,
            OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                (int)bmp.Width, (int)bmp.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
                PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            return texture;
        }
        
        static public void init(string text, int width, int height)
        {
            _texture = new_StringTexture(text, new Font("Courier", 140), out _textureSize);

            float realWidth = _textureSize.Width / width;
            float realHeight = _textureSize.Height / height;

            uint[] _indices = new uint[] { 0, 1, 2, 3 };
            float[] _vertices = new float[] {
               -realWidth / 2, -realHeight / 2, 0f,
               0.0f, 1.0f,
               realWidth / 2, -realHeight / 2, 0f,
               1.0f, 1.0f,
               realWidth / 2, realHeight / 2, 0f,
               1.0f, 0.0f,
               -realWidth / 2, realHeight / 2, 0f,
               0.0f, 0.0f
            };

            foreach(float vertex in _vertices)
            {
                Console.WriteLine($"{vertex}");
            }

            _width = width;
            _height = height;

            var scale = 1.0f;
            GL.Scale(scale, scale, scale); // I work with a top/left image and openGL is bottom/left
            GL.Viewport(0, 0, width, height);
            
            _vertexArray = GL.GenVertexArray();
            _vertexBufferObject = GL.GenBuffer();
			_elementBufferObject = GL.GenBuffer();
            GL.BindVertexArray(_vertexArray);

            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
			GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);


            GL.ClearColor(Color.LightGray);
            //GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            //GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            //GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            //GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            //GL.Enable(EnableCap.PointSmooth);
            //GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.Blend);
            //GL.Enable(EnableCap.DepthTest);
            //GL.ShadeModel(ShadingModel.Smooth);
            //GL.Enable(EnableCap.AutoNormal);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        }
        static public void Draw()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Texture2D);
            //GL.ActiveTexture(TextureUnit.Texture1);            
            GL.BindVertexArray(_vertexArray);
            //GL.MatrixMode(MatrixMode.Modelview);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.BindTexture(TextureTarget.Texture2D, _texture);

            //GL.DrawArrays(PrimitiveType.Quads, 0, 4);
            GL.DrawElements(PrimitiveType.Quads, 4, DrawElementsType.UnsignedInt, 0);
            //float realWidth = _textureSize.Width / _width;
            //float realHeight = _textureSize.Height / _height;

            //GL.Begin(PrimitiveType.Quads);

            //GL.TexCoord3(0.0f, 1.0f, 0f); GL.Vertex3(-realWidth / 2, -realHeight / 2, 0f);
            //GL.TexCoord3(1.0f, 1.0f, 0f); GL.Vertex3(realWidth / 2, -realHeight / 2, 0f);
            //GL.TexCoord3(1.0f, 0.0f, 0f); GL.Vertex3(realWidth / 2, realHeight / 2, 0f);
            //GL.TexCoord3(0.0f, 0.0f, 0f); GL.Vertex3(-realWidth / 2, realHeight / 2, 0f);

            //GL.End();

            GL.Disable(EnableCap.Texture2D);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
        }
    }
}
