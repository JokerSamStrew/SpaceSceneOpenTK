using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace SpaceSceneOpenTK
{
    public class Window : GameWindow
    {
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

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _cube_vertices.Length * sizeof(float), _cube_vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _cube_indices.Length * sizeof(uint), _cube_indices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            _sphere = new Sphere();

            Matrix4 modelview = Matrix4.LookAt(
                    new Vector3(0.5f, 0.5f, 1.0f) * 3.0f,
                    Vector3.Zero,
                    Vector3.UnitY);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            base.OnLoad(e);
        }

        private Sphere _sphere;
        private int _vertexBufferObject;
        private int _elementBufferObject;
        private float _camera_move_var = 0.0f;


        private readonly float[] _cube_vertices = new float[]{
             0.0f, 0.0f, 0.0f,
             0.0f, 0.0f, 1.0f,
             0.0f, 1.0f, 0.0f,
             0.0f, 1.0f, 1.0f,
             1.0f, 0.0f, 0.0f,
             1.0f, 0.0f, 1.0f,
             1.0f, 1.0f, 0.0f,
             1.0f, 1.0f, 1.0f,
        };

        private readonly uint[] _cube_indices = new uint[]{
            0, 1,
            0, 2,
            0, 4,
            3, 1,
            3, 2,
            3, 7,
            5, 1,
            5, 4,
            5, 7,
            6, 2,
            6, 4,
            6, 7
        };

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit
                    | ClearBufferMask.DepthBufferBit);

            GL.Color3(0.0f, 0.0f, 0.0f);
            GL.DrawElements(PrimitiveType.Lines, _cube_indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.Color3(1.0f, 1.0f, 1.0f);
            _sphere.DrawSphere();
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
