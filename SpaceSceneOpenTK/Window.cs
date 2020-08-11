using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SixLabors.ImageSharp.Formats.Gif;

namespace SpaceSceneOpenTK
{
    public class Window : GameWindow
    {
        FileObject _man;
        FileObject _cone;
        Shader shader;
        Texture _texture;

        protected override void OnLoad(EventArgs e)
        {
            Title = "Hello OpenTK!";
            GL.ClearColor(Color.CornflowerBlue);

            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.Texture2D);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            //Enable face culling. This function doesnt show polygons that the back/face side to viewer
            GL.CullFace(CullFaceMode.Back); //culls only back side polygon
            GL.FrontFace(FrontFaceDirection.Ccw); //determine face side of the polygon
            GL.Enable(EnableCap.CullFace);

            shader = new Shader("./Shaders/shader.vert", "./Shaders/shader.frag");
            _man = new FileObject("man.obj", shader);
            _cone = new FileObject("cone.obj", shader);

            shader.Use();
            
            _texture = new Texture("cone_texture.png");

            Matrix4 modelview = Matrix4.LookAt(
                    new Vector3(0.5f, 0.5f, 1.0f) * 3.0f,
                    Vector3.Zero,
                    Vector3.UnitY);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                   (float)Math.PI / 4,
                   Width / (float)Height,
                   1.0f, 64.0f);

            GL.UniformMatrix4(shader.GetAttrib("view"), true, ref modelview);
            GL.UniformMatrix4(shader.GetAttrib("projection"), true, ref projection);


            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            shader.Dispose();
            base.OnUnload(e);
        }

        private float _camera_move_var = 0.0f;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit
                   | ClearBufferMask.DepthBufferBit);


            //_cube.Draw();
            //_sphere.Draw();
            //shader.Use();
            _man.Draw();
            _cone.Draw();

            
            base.OnRenderFrame(e);
            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {

            GL.Viewport(
                    ClientRectangle.X,
                    ClientRectangle.Y,
                    ClientRectangle.Width,
                    ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                    (float)Math.PI / 4,
                    Width / (float)Height,
                    1.0f, 64.0f);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            GL.UniformMatrix4(shader.GetAttrib("projection"), true, ref projection);


            base.OnResize(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            bool isMyKey = false;
            if (e.Key == Key.Up)
            {
                isMyKey = true;
                _camera_move_var += 0.05f;
            }
            else if (e.Key == Key.Down)
            {
                isMyKey = true;
                _camera_move_var -= 0.05f;
            }
            else if (e.Key == Key.Right)
            {
                _man.Translate(0.05f,0.0f,0.0f);
            }
            else if (e.Key == Key.Left)
            {
                _man.Translate(-0.05f,0.0f,0.0f);
            }

            if (isMyKey)
            {
                Matrix4 modelview = Matrix4.LookAt(
                        new Vector3(0.5f + _camera_move_var, 0.5f + _camera_move_var, 1.0f) * 3.0f,
                        Vector3.Zero,
                        Vector3.UnitY);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelview);

                GL.UniformMatrix4(shader.GetAttrib("view"), true, ref modelview);

            }
            base.OnKeyDown(e);
        }
    }
}
