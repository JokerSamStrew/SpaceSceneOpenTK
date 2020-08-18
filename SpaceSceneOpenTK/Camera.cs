using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices.ComTypes;

namespace SpaceSceneOpenTK
{
    public enum Direction { UP, DOWN, FORWARD, BACK, LEFT, RIGHT }
    public enum Rotation { UP, DOWN, LEFT, RIGHT }
    public class Camera
    {
        public Vector3 Orientation { get; private set; }
        public Vector3 Right 
        { 
            get { return Vector3.Normalize(Vector3.Cross(this.Orientation, this.Directon)); }
        }
        public Vector3 Target { get; private set; }
        public Vector3 Location { get; private set; }
        public Vector3 Directon 
        {
           get { return Vector3.Normalize(this.Location - this.Target); }
        }
        public float MoveRate { get; set; }
        public float RotationRate { get; set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        //public float Roll { get; private set; }

        public Camera()
        {
            this.Location = new Vector3(7.0f, 9.0f, 9.0f);
            this.Orientation = Vector3.UnitY;

            this.Yaw = -2.3f;
            this.Pitch = -0.7f;
            //this.Roll = 0;
            this.Target = new Vector3(
                (float) Math.Cos(this.Pitch) * (float) Math.Cos(this.Yaw),
                (float) Math.Sin(this.Pitch),
                (float) Math.Cos(this.Pitch) * (float) Math.Sin(this.Yaw)
                );

            this.MoveRate = 0.15f;
            this.RotationRate = 0.05f;
        }

        public void Reset()
        {
            this.Location = new Vector3(7.0f, 9.0f, 9.0f);
            this.Orientation = Vector3.UnitY;
            this.Yaw = -2.3f;
            this.Pitch = -0.7f;
            this.Target = new Vector3(
                (float) Math.Cos(this.Pitch) * (float) Math.Cos(this.Yaw),
                (float) Math.Sin(this.Pitch),
                (float) Math.Cos(this.Pitch) * (float) Math.Sin(this.Yaw)
                );
        }

        public void Rotate(float deltaX, float deltaY)
        {
            //yaw += deltaX * sensitivity;
            //pitch -= deltaY * sensitivity; 
            this.Yaw += deltaX;
            this.Pitch -=  deltaY;
            this.Target = new Vector3(
                (float) Math.Cos(this.Pitch) * (float) Math.Cos(this.Yaw),
                (float) Math.Sin(this.Pitch),
                (float) Math.Cos(this.Pitch) * (float) Math.Sin(this.Yaw)
                );
        }

        public void Rotate(Rotation r)
        { 
            switch(r)
            {
                case Rotation.UP:      
                    this.Pitch += RotationRate;
                    break;
                case Rotation.DOWN:    
                    this.Pitch -= RotationRate;
                    break;
                case Rotation.LEFT:    
                    this.Yaw -= RotationRate;
                    break;
                case Rotation.RIGHT:    
                    this.Yaw += RotationRate;
                    break;
            }
            this.Target = new Vector3(
                (float) Math.Cos(this.Pitch) * (float) Math.Cos(this.Yaw),
                (float) Math.Sin(this.Pitch),
                (float) Math.Cos(this.Pitch) * (float) Math.Sin(this.Yaw)
                );
        }
        public void Move(Direction d)
        {
            switch(d)
            {
                case Direction.UP:      
                    this.Location += this.Orientation * this.MoveRate;
                    break;
                case Direction.DOWN:    
                    this.Location -= this.Orientation * this.MoveRate;
                    break;
                case Direction.FORWARD: 
                    this.Location -= this.Directon * this.MoveRate;
                    break;
                case Direction.BACK:    
                    this.Location += this.Directon * this.MoveRate;
                    break;
                case Direction.LEFT:    
                    this.Location -= this.Right * this.MoveRate;
                    break;
                case Direction.RIGHT:    
                    this.Location += this.Right * this.MoveRate;
                    break;
            }
        }

        public void Load()
        {
            //Console.WriteLine(this.Location);
            Console.WriteLine(this.Directon);
            //Console.WriteLine("Yaw: {0}; Pitch: {1}", Yaw, Pitch);
            var state_modelview = Matrix4.LookAt(this.Location, this.Target + this.Location, this.Orientation);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref state_modelview);
            GL.UniformMatrix4(Resources.Shader.GetAttrib("view"), true, ref state_modelview);
        }
    }
}
