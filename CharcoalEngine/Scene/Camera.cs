using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CharcoalEngine.Scene
{
    public sealed class Camera
    {
        public static Vector3 Position;
        public static Quaternion Rotation;

        public static Matrix View;
        public static Matrix Projection;
        public static Viewport Viewport;

        //private static KeyboardState last_k;
        private static MouseState last_m;
        public static float FoV = MathHelper.Pi / 3.2f;
        public static Vector3 look;
        public static Vector3 Up;
        //arc_ball
        public static float start_distance_arc_ball = 10;
        public static float distance_arc_ball=10;
        public static float r_x_arc_ball = 0;
        public static float r_y_arc_ball = 0;
        public static Vector3 dynamictarget = Vector3.Zero;

        public static BoundingFrustum Frustum = new BoundingFrustum(Matrix.Identity);

        private Camera()
        { }

        public static void Initialize()
        {
            Position = new Vector3(0, 0, 0);
            Rotation = new Quaternion(0, 0, 0, 1);
            
        }

        public static void Initialize_WithDefaults()
        {
            Position = new Vector3(0, 0, 0);
            Rotation = new Quaternion(0, 0, 0, 1);

            Viewport = Engine.g.Viewport;
            Viewport.MinDepth = 0.1f;
            Viewport.MaxDepth = 5000;
            Viewport.Width = Engine.g.PresentationParameters.BackBufferWidth;
            Viewport.Height = Engine.g.PresentationParameters.BackBufferHeight;
            Position = new Vector3(0, 2, 6);
            LookAt(Vector3.Zero, 0);
            Update();
            Update_WASD(0, 0);
        }

        public static void LookAt(Vector3 target, float speed)
        {
            Vector3 tminusp = target - Position;
            Vector3 ominusp = Vector3.Forward;

            if (tminusp == Vector3.Zero)
                return;

            tminusp.Normalize();

            float theta = (float)System.Math.Acos(Vector3.Dot(tminusp, ominusp));
            Vector3 cross = Vector3.Cross(ominusp, tminusp);

            if (cross == Vector3.Zero)
                return;

            cross.Normalize();

            Quaternion targetQ = Quaternion.CreateFromAxisAngle(cross, theta);
            Rotation = Quaternion.Slerp(Rotation, targetQ, speed);
        }

        public static void Update()
        {
            look = Vector3.Transform(Vector3.Forward, Matrix.CreateFromQuaternion(Rotation));
            look.Normalize();
            Up = Vector3.Cross(Vector3.Transform(Vector3.Forward, Matrix.CreateFromQuaternion(Rotation)), Vector3.Transform(Vector3.Left, Matrix.CreateFromQuaternion(Rotation)));
            View = Matrix.CreateLookAt(Position, Position + Vector3.Transform(Vector3.Forward, Matrix.CreateFromQuaternion(Rotation)), Vector3.Cross(Vector3.Transform(Vector3.Forward, Matrix.CreateFromQuaternion(Rotation)), Vector3.Transform(Vector3.Left, Matrix.CreateFromQuaternion(Rotation))));
            //View = Matrix.Invert(Matrix.CreateFromQuaternion(Rotation) *
            //                       Matrix.CreateTranslation(Position));

            Projection = Matrix.CreatePerspectiveFieldOfView(FoV, (float)Engine.Game.Window.ClientBounds.Width / (float)Engine.Game.Window.ClientBounds.Height, Viewport.MinDepth, Viewport.MaxDepth);

            Frustum = new BoundingFrustum(View * Projection);
        }
        public static void Rotate(Vector3 axis, float angle)
        {
            axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(Rotation));
            look = axis;
            Rotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, angle) * Rotation);

            Update();
        }
        public static void Translate(Vector3 distance)
        {
            Position += Vector3.Transform(distance, Matrix.CreateFromQuaternion(Rotation));
            Update();
        }
        public static void Translate_AxisAligned(Vector3 distance)
        {
            Position += distance;
            Update();
        }

        public static void Update_WASD(float Rotation_Speed, float Translation_Speed)
        {
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.Left))
                Rotate(Vector3.Up, Rotation_Speed);
            if (k.IsKeyDown(Keys.Right))
                Rotate(Vector3.Up, -Rotation_Speed);
            if (k.IsKeyDown(Keys.Up))
                Rotate(Vector3.Left, -Rotation_Speed);
            if (k.IsKeyDown(Keys.Down))
                Rotate(Vector3.Left, Rotation_Speed);
            /*
            if (k.IsKeyDown(Keys.W))
                Translate(Vector3.Forward * Translation_Speed);
            if (k.IsKeyDown(Keys.S))
                Translate(-Vector3.Forward * Translation_Speed);
            if (k.IsKeyDown(Keys.A))
                Translate(Vector3.Right * Translation_Speed);
            if (k.IsKeyDown(Keys.D))
                Translate(-Vector3.Right * Translation_Speed);*/
            /*
            if (k.IsKeyDown(Keys.Z))
                Translate(Vector3.Up * Translation_Speed);
            if (k.IsKeyDown(Keys.X))
                Translate(-Vector3.Up * Translation_Speed);
            */
            Update();
        }
        public static void Update_Arc_Ball(Vector3 Target)
        {
            look = Target - Position;
            look.Normalize();
            KeyboardState k = Keyboard.GetState();
            MouseState m = Mouse.GetState();

            distance_arc_ball -= (m.ScrollWheelValue - last_m.ScrollWheelValue)/10;
            if (m.LeftButton == ButtonState.Pressed)
            {
                r_x_arc_ball -= ((float)m.X - (float)last_m.X) / 150;
                r_y_arc_ball += ((float)m.Y - (float)last_m.Y) / 150;
            }

            last_m = m;

            // Calculate rotation matrix from rotation values
            Matrix rotation = Matrix.CreateFromYawPitchRoll(r_x_arc_ball, -
            r_y_arc_ball, 0);
            
            // Translate down the Z axis by the desired distance
            // between the camera and object, then rotate that
            // vector to find the camera offset from the target
            Vector3 translation = new Vector3(0, 0, distance_arc_ball);
            translation = Vector3.Transform(translation, rotation);
            Position = Target + translation;
            // Calculate the up vector from the rotation matrix
            Up = Vector3.Transform(Vector3.Up, rotation);
            View = Matrix.CreateLookAt(Position, Target, Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 3.2f, (float)Engine.Game.Window.ClientBounds.Width / (float)Engine.Game.Window.ClientBounds.Height, Viewport.MinDepth, Viewport.MaxDepth);
            Rotation = Quaternion.CreateFromRotationMatrix(rotation);
            
            Frustum = new BoundingFrustum(View * Projection);
        }
        public static void Update_Chase_Camera(Vector3 Target, float Distance, float Speed)
        {
            LookAt(Target, Speed);
            Translate(Vector3.Forward * (Vector3.Distance(Position, Target)-Distance) * Speed);
            Update();
        }
    }
}
