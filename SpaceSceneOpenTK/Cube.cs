using SpaceSceneOpenTK;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System;

namespace SpaceOpenGL
{
    class Cube : DrawableObject
    {
        private int _vertexBufferObject;
        private int _elementBufferObject;

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
            //GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            //GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            //var vertexLocation = 0;
            //GL.EnableVertexAttribArray(vertexLocation);
            //GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            //Console.WriteLine($"EB {_elementBufferObject} - VB {_vertexBufferObject}");
        }

        public override void Draw()
        {

            //GL.BindVertexArray()

            //_vertexBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            //GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            //_elementBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            //var vertexLocation = 0;
            //GL.EnableVertexAttribArray(vertexLocation);
            //GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.Color3(0.0f, 0.0f, 0.0f);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

            Console.WriteLine($"Cube - EB {_elementBufferObject} - VB {_vertexBufferObject}");
            GL.DrawElements(PrimitiveType.Lines, _indices.Length, DrawElementsType.UnsignedInt, 0);
            
        }
    }
}
