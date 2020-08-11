using SpaceSceneOpenTK;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace SpaceOpenGL
{
    class Cube : DrawableObject
    {
        private int _vertexBufferObject;
        private int _elementBufferObject;
        private int _vertexArray;

        public Cube()
        {
            _indices = new uint[]{
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

            _vertices = new float[]{
                 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 1.0f,
                 0.0f, 1.0f, 0.0f,
                 0.0f, 1.0f, 1.0f,
                 1.0f, 0.0f, 0.0f,
                 1.0f, 0.0f, 1.0f,
                 1.0f, 1.0f, 0.0f,
                 1.0f, 1.0f, 1.0f,
            };

            _vertexBufferObject = GL.GenBuffer();
            _elementBufferObject = GL.GenBuffer();
            _vertexArray = GL.GenVertexArray();

            GL.BindVertexArray(_vertexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
           
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        public override void Draw()
        {
            GL.BindVertexArray(_vertexArray);
			GL.Color3(Color.BlueViolet);
            GL.EnableVertexAttribArray(0);
            GL.DrawElements(PrimitiveType.Lines, _indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.DisableVertexAttribArray(0);
        }
    }
}
