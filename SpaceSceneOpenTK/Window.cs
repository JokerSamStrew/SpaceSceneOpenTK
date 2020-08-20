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
        Texture _texture;
        Camera _camera;

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


            _man = new FileObject("man.obj");
            _cone = new FileObject("cone.obj");
            
            Resources.Shader.Use();
            
            _texture = new Texture("cone_texture.png");
            _camera = new Camera();
            _camera.Load();

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                   (float)Math.PI / 4,
                   Width / (float)Height,
                   1.0f, 64.0f);

            GL.UniformMatrix4(Resources.Shader.GetAttrib("projection"), true, ref projection);
            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            Resources.Shader.Dispose();
            base.OnUnload(e);
        }


        float time = 0;


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit
                   | ClearBufferMask.DepthBufferBit);

            GL.Uniform1(Resources.Shader.GetAttrib("time"), time * 1.6f);
            time += (float)e.Time;
            
            //_cube.Draw();
            //_sphere.Draw();
            _man.Draw();
            //_cone.Draw();

            base.OnRenderFrame(e);
            SwapBuffers();
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (e.Mouse.LeftButton == ButtonState.Pressed) { 
                // TODO: Fix relation between mouse movement and camera rotation
                _camera.Rotate(e.XDelta / 250.0f, e.YDelta / 250.0f );
                _camera.Load();
            }
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
            GL.UniformMatrix4(Resources.Shader.GetAttrib("projection"), true, ref projection);
            base.OnResize(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            bool isMyKey = false;
            if (e.Key == Key.Up) {
                isMyKey = true;
                _camera.Move(Direction.UP);
            } else if (e.Key == Key.Down) {
                isMyKey = true;
                _camera.Move(Direction.DOWN);
            } else if (e.Key == Key.Right || e.Key == Key.D) {
                isMyKey = true;
                _camera.Move(Direction.RIGHT);
            } else if (e.Key == Key.Left || e.Key == Key.A) {
                isMyKey = true;
                _camera.Move(Direction.LEFT);
            } else if (e.Key == Key.W) {
                isMyKey = true;
                _camera.Move(Direction.FORWARD);
            } else if (e.Key == Key.S) {
                isMyKey = true;
                _camera.Move(Direction.BACK);
            } else if (e.Key == Key.C) {
                isMyKey = true;
                _camera.Rotate(Rotation.RIGHT);
            } else if (e.Key == Key.Z) {
                isMyKey = true;
                _camera.Rotate(Rotation.LEFT);
            } else if (e.Key == Key.R) {
                isMyKey = true;
                _camera.Rotate(Rotation.UP);
            } else if (e.Key == Key.F) {
                isMyKey = true;
                _camera.Rotate(Rotation.DOWN);
            } else if (e.Key == Key.Q) {
                isMyKey = true;
                _camera.Reset();
            } else if (e.Key == Key.T) {
                isMyKey = true;
                _camera.TargetLocked = !_camera.TargetLocked;
            }

            if (isMyKey) {
                Console.WriteLine(e.Key);
                _camera.Load();
            }
            base.OnKeyDown(e);
        }
    }
}
