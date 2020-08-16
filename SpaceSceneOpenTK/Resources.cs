using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceSceneOpenTK
{
    static class Resources
    {
        private static Shader _shader; 
        public static Shader  Shader {
           get { 
                if (_shader == null)
                    _shader = new Shader("./Shaders/shader.vert", "./Shaders/shader.frag"); 

                return _shader;
           }
        }
    }
}
