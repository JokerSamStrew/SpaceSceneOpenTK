using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace SpaceSceneOpenTK
{
    class FontTestWindow : GameWindow
    {
        protected override void OnLoad(EventArgs e)
        {
            //TextDrawer.init("qwertyuiop[]asdfghjkl;\n'zxcvbnm,./", this.Width, this.Height);
            TextDrawer.init("LUL", this.Width, this.Height);
            
            base.OnLoad(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Resources.Shader.Use();
            TextDrawer.Draw();
            base.OnRenderFrame(e);
            SwapBuffers();
        }
    }
}
