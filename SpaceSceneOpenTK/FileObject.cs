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

            _vertexBufferObject = GL.GenBuffer(); //get index for vetex bao
            _elementBufferObject = GL.GenBuffer(); //get index for indicies bao
            _vertexArray = GL.GenVertexArray(); //get index for vao

            GL.BindVertexArray(_vertexArray); //set current vao

            //bind bao for vertex
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            //bind bao for indicies. for indicies not need to call VertexAttribPointer, its bind to current vao when calls BindBuffer. 
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            //bind vertex bao to current vao. We can change current vertex bao while setting vao. Only VertexAttribPointer bind vertex bao to current vao.
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));


        }

        public override void Draw()
        {
            GL.BindVertexArray(_vertexArray);
            GL.Color3(_color);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            // GL.DrawArrays(PrimitiveType.Triangles, 0, _indices.Length); //in some cases this work faster. Need more info
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
        }
    }
}
