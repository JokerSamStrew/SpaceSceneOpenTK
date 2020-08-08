
namespace SpaceSceneOpenTK
{
    public abstract class DrawableObject
    {
        protected float[] _vertices;
        protected float[] _normals;
        protected float[] _texCoords;
        protected uint[]  _indices;
        protected Texture _texture;

        public abstract void Draw();
    }
}
