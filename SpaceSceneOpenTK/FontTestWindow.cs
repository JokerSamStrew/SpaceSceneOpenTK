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
            TextDrawer.init("L", this.Width, this.Height);
            base.OnLoad(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            TextDrawer.Draw();
            base.OnRenderFrame(e);
            SwapBuffers();
        }
    }
}
