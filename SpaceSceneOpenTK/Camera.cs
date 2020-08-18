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
        public Vector3 Up { get; private set; }
        public Vector3 Right 
        { 
            get { return Vector3.Normalize(Vector3.Cross(this.Up, this.Front)); }
        }
        public bool TargetLocked { get; set; }
        public Vector3 Front { 
            get{ 
                if (this.TargetLocked)
                    return Vector3.Normalize(this.Target - this.Location);
                else 
                    return  new Vector3(
                        (float) Math.Cos(this.Pitch) * (float) Math.Cos(this.Yaw),
                        (float) Math.Sin(this.Pitch),
                        (float) Math.Cos(this.Pitch) * (float) Math.Sin(this.Yaw)
                    );
            } 
        }
        public Vector3 Location { get; private set; }
        public Vector3 Target { get; set; }
        public float MoveRate { get; set; }
        public float RotationRate { get; set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }

        public Camera()
        {
            this.Location = new Vector3(7.0f, 9.0f, 9.0f);
            this.Up = Vector3.UnitY;

            this.Yaw = -2.3f;
            this.Pitch = -0.7f;
            this.Target = Vector3.Zero;
            this.TargetLocked = false;
             

            this.MoveRate = 0.15f;
            this.RotationRate = 0.05f;
        }

        public void Reset()
        {
            this.Location = new Vector3(7.0f, 9.0f, 9.0f);
            this.Up = Vector3.UnitY;
            this.Yaw = -2.3f;
            this.Pitch = -0.7f;
        }

        public void Rotate(float deltaX, float deltaY)
        {
            if (this.TargetLocked)
                return;

            this.Yaw += deltaX;
            this.Pitch -=  deltaY;
        }

        public void Rotate(Rotation r)
        { 
            if (this.TargetLocked)
                return;

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
        }
        public void Move(Direction d)
        {
            switch(d)
            {
                case Direction.UP:      
                    this.Location += this.Up * this.MoveRate;
                    break;
                case Direction.DOWN:    
                    this.Location -= this.Up * this.MoveRate;
                    break;
                case Direction.FORWARD: 
                    this.Location += this.Front * this.MoveRate;
                    break;
                case Direction.BACK:    
                    this.Location -= this.Front * this.MoveRate;
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
            Matrix4 state_modelview;
            if (this.TargetLocked) {
                state_modelview = Matrix4.LookAt(this.Location, this.Target, this.Up);
            } else { 
                state_modelview = Matrix4.LookAt(this.Location, this.Front + this.Location, this.Up);
            }

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref state_modelview);
            GL.UniformMatrix4(Resources.Shader.GetAttrib("view"), true, ref state_modelview);
        }
    }
}
