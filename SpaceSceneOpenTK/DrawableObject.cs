﻿
using OpenTK;
using System.Drawing.Drawing2D;
using OpenTK.Graphics.OpenGL;

namespace SpaceSceneOpenTK
{
    public abstract class DrawableObject
    {
        protected float[] _vertices;
        protected float[] _normals;
        protected float[] _texCoords;
        protected uint[]  _indices;
        protected Texture _texture;
        protected Matrix4 _state_matrix;
        protected Shader _shader;
        public Matrix4 State { get { return _state_matrix; } }

        protected DrawableObject()
        {
            _shader = Resources.Shader;
            _state_matrix = Matrix4.Identity;
        }
        public virtual void Draw()
        {
            GL.UniformMatrix4(_shader.GetAttrib("state"), true, ref _state_matrix);
        }
        public virtual void ResetState()
        {
            _state_matrix = Matrix4.Identity;
        }
        public virtual void SetRotation(Vector3 axis, float angle)
        {
            _state_matrix = _state_matrix.ClearRotation() * Matrix4.CreateFromAxisAngle(axis, angle);
        }
        public virtual void SetScale(Vector3 scale)
        {
            _state_matrix = _state_matrix.ClearScale() * Matrix4.CreateScale(scale);
        }
        public virtual void SetTranslation(float x, float y, float z)
        {
            _state_matrix = _state_matrix.ClearTranslation() * Matrix4.CreateTranslation(x, y, z);
        }
        public virtual void Rotate(Vector3 axis, float angle)
        {
            _state_matrix *= Matrix4.CreateFromAxisAngle(axis, angle);
        }
        public virtual void Scale(Vector3 scale)
        {
            _state_matrix *= Matrix4.CreateScale(scale);
        }
        public virtual void Translate(float x, float y, float z)
        {
            _state_matrix *= Matrix4.CreateTranslation(x, y, z);
        }
    }
}
