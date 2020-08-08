using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace SpaceSceneOpenTK
{
    public class RenderObject
    {
        public float[] _vertices;
        public float[] _texCoord;
        public uint[] _indices;

        public RenderObject(float[] ver, float[] tex, uint[] ind)
        {
            _vertices = ver;
            _texCoord = tex;
            _indices = ind;
        }
    }
}
