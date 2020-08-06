using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace SpaceSceneOpenTK
{
 
	static class Program
	{
		static void Main()
		{
			using (Window game = new Window())
			{
			   game.Run(30.0);
			}
		}
	}
}

