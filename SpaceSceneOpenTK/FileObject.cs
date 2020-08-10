using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Drawing;


namespace SpaceSceneOpenTK
{
    class FileObject : DrawableObject
    {
        private int _vertexBufferObject;
        private int _elementBufferObject;
        private int _vertexArray;
        private Color _color;

        public FileObject(string filepath)
        {
            Random random = new Random();
            _color = Color.FromArgb(random.Next());
            Console.WriteLine(_color);

            ImporterOBJ import = new ImporterOBJ(filepath);
            _vertices = import.vertices;
            _texCoords = import.textCoord;
            _indices = import.index; 

            _vertexBufferObject = GL.GenBuffer();
            _elementBufferObject = GL.GenBuffer();
            _vertexArray = GL.GenVertexArray();

            GL.BindVertexArray(_vertexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            //GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            GL.TexCoordPointer(3, TexCoordPointerType.Float, sizeof(float) * 6, sizeof(float) * 3);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
        }

        public override void Draw()
        {
            GL.BindVertexArray(_vertexArray);
            GL.Color3(_color);
            GL.EnableVertexAttribArray(0);
            //GL.EnableVertexAttribArray(1);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.DisableVertexAttribArray(0);
        }
    }
}
