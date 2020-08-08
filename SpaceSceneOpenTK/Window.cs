using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using SpaceOpenGL;

namespace SpaceSceneOpenTK
{
    public class Window : GameWindow
    {
        RenderObject render;

        protected override void OnLoad(EventArgs e)
        {
            Title = "Hello OpenTK!";
            GL.ClearColor(Color.CornflowerBlue);
            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.Texture2D);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            //Enable face culling. This function doesnt show polygons that the back/face side to viewer
            GL.CullFace(CullFaceMode.Back); //culls only back side polygon
            GL.FrontFace(FrontFaceDirection.Ccw); //determine face side of the polygon
            GL.Enable(EnableCap.CullFace);


            render = ImporterOBJ.Import("man.obj");

            var _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, render._vertices.Length * sizeof(float), render._vertices, BufferUsageHint.StaticDraw);

            var _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, render._indices.Length * sizeof(uint), render._indices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            //_sphere = new Sphere();

            
            //_sphere = new Sphere();
            //_cube = new Cube();


            Matrix4 modelview = Matrix4.LookAt(
                    new Vector3(0.5f, 0.5f, 1.0f) * 3.0f,
                    Vector3.Zero,
                    Vector3.UnitY);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);


            base.OnLoad(e);
        }

        private Cube _cube;
        private Sphere _sphere;
        private float _camera_move_var = 0.0f;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit
                   | ClearBufferMask.DepthBufferBit);

            //_cube.Draw();
            //_sphere.Draw();
            //_sphere.DrawSphere();

            GL.DrawElements(PrimitiveType.Lines, render._indices.Length, DrawElementsType.UnsignedInt, 0);


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

            if (isMyKey)
            {
                Matrix4 modelview = Matrix4.LookAt(
                        new Vector3(0.5f + _camera_move_var, 0.5f + _camera_move_var, 1.0f) * 3.0f,
                        Vector3.Zero,
                        Vector3.UnitY);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelview);
            }
            base.OnKeyDown(e);
        }
    }
}
