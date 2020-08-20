using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL;



namespace SpaceSceneOpenTK
{
    //
    // Text drawing methods for testing
    //

    // At this moment breaks current texture which leads to showing black models (if comment fragment shader)
    static class TextDrawer
    {
        static int _texture = -1;
        static Bitmap bmp;
        static public void init(string text, int width, int height)
        {
            //GL.Ortho(0, width, 0, height, -1000, 1000);
            //GL.Ortho(0, width, 0, height, -1000, 1000);
            GL.Scale(1, -1, 1); // I work with a top/left image and openGL is bottom/left
            GL.Viewport(0, 0, width, height);
            GL.ClearColor(Color.LightGray);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.AutoNormal);

            bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var gfx = Graphics.FromImage(bmp);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            _texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, bmp.Width, bmp.Height, 0,
            OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            gfx.Clear(Color.ForestGreen);
            //gfx.DrawString(text, new Font("Arial", 20), Brushes.White, new PointF(bmp.Width / 2, bmp.Height));
            gfx.DrawString(text, new Font("Arial", 20), Brushes.White, new PointF(0.0f, 0.0f));
            gfx.DrawImage(bmp, new PointF(0.0f, 0.0f));
            bmp.Save("Atlas.bmp", ImageFormat.Bmp);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int)bmp.Width, (int)bmp.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);
        }
        static public void Draw()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _texture);

            float realWidth = 1.0f;
            float realHeight = 1.0f;

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord3(0.0f, 0.0f, 0f); GL.Vertex3(-1.0f, -1.0f, 0f);
            GL.TexCoord3(1.0f, 0.0f, 0f); GL.Vertex3(realWidth, -1.0f, 0f);
            GL.TexCoord3(1.0f, 1.0f, 0f); GL.Vertex3(realWidth, realHeight, 0f);
            GL.TexCoord3(0.0f, 1.0f, 0f); GL.Vertex3(-1.0f, realHeight, 0f);

            GL.End();

            GL.Disable(EnableCap.Texture2D);
        }
    }
}
