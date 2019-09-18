using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using CharcoalEngine.Scene;
using CharcoalEngine;
using CharcoalEngine.Utilities;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Windows;
using System.Design;

namespace CharcoalEngine.Object
{
    public class PhysicsObject : Transform
    {
        public Vector3 Velocity
        {
            get; set;
        }

        public float Mass
        {
            get; set;
        } = 1;

        public bool PhysicsEnabled
        {
            get
            {
                return Engine.PhysicsEnabled;
            }
            set
            {
                Engine.PhysicsEnabled = value;
            }
        }

        public bool IsStatic
        {
            get;
            set;
        } = false;

        public bool CanCollide
        {
            get;
            set;
        } = true;

        public float Friction
        {
            get;
            set;
        } = 0.6f;

        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public List<Force> ActiveForces { get; set; } = new List<Force>();

        public void AddForce(Force f)
        {
            ActiveForces.Add(f);
        }

        /*public void AddTorque(Torque t)
        {

        }*/

        public static void Collide(PhysicsObject p1, PhysicsObject p2)
        {
            if (p1.IsStatic) p1.Velocity = Vector3.Zero;
            if (p2.IsStatic) p2.Velocity = Vector3.Zero;

            if (p1.CanCollide == false || p2.CanCollide == false) return;

            Vector3 segment = p1.AbsolutePosition - p2.AbsolutePosition;
            segment.Normalize();

            Console.WriteLine("Segment: " + segment);

            Vector3 p1_cv = Vector3.Dot(p1.Velocity, segment) * segment;
            Vector3 p2_cv = Vector3.Dot(p2.Velocity, segment) * segment;

            Console.WriteLine("p1_cv: " + p1_cv);
            Console.WriteLine("p2_cv: " + p2_cv);

            Vector3 p1_uv = p1.Velocity - p1_cv;
            Vector3 p2_uv = p2.Velocity - p2_cv;

            Console.WriteLine("p1_uv: " + p1_uv);
            Console.WriteLine("p2_uv: " + p2_uv);

            Vector3 p1_pcv = (2 * p2.Mass * p2_cv + p1_cv * (p1.Mass - p2.Mass)) / (p1.Mass + p2.Mass);
            Vector3 p2_pcv = (2 * p1.Mass * p1_cv + p2_cv * (p2.Mass - p1.Mass)) / (p1.Mass + p2.Mass);
            
            Console.WriteLine("p1_pcv: " + p1_pcv);
            Console.WriteLine("p2_pcv: " + p2_pcv);

            p1.Velocity = p1_uv + p1_pcv;
            p2.Velocity = p2_uv + p2_pcv;
            
            /*Vector3 Momentum = Mass * Velocity;
            Vector3 segment = AbsolutePosition - objPosition;
            segment.Normalize();

            Console.WriteLine("Momentum1: " + Momentum);
            Console.WriteLine("Momentum2: " + objMomentum);

            float M1_collision = Vector3.Dot(Momentum, segment);
            float M2_collision = Vector3.Dot(objMomentum, segment);

            Console.WriteLine("m1_collision: " + M1_collision);
            Console.WriteLine("m2_collision: " + M2_collision);

            Vector3 M1 = M2_collision * segment + (Momentum - (M1_collision * segment));
            Vector3 M2 = M1_collision * segment + (objMomentum - (M2_collision * segment));

            Console.WriteLine("m1: " + M1);
            Console.WriteLine("m2: " + M2);

            Velocity = M1 / Mass;

            return M2;*/
        }

        public override void Update()
        {
            base.Update();

            if (PhysicsEnabled && !IsStatic)
            {
                Vector3 NetForce = Vector3.Zero;
                if (Engine.gameTime == null) return;

                foreach (Force f in ActiveForces)
                {
                    f.Time -= (float)Engine.gameTime.ElapsedGameTime.TotalSeconds;
                    if (f.Time <= 0.0f)
                        ActiveForces.Remove(f);
                }

                for (int i = 0; i < ActiveForces.Count; i++)
                {
                    NetForce += ActiveForces[i].ForceVector;
                }

                NetForce += -Velocity * Mass * Friction;

                Velocity += ((float)Engine.gameTime.ElapsedGameTime.TotalSeconds) * (NetForce / Mass);

                Position += ((float)Engine.gameTime.ElapsedGameTime.TotalSeconds) * Velocity;
            }

        }

        public override void Draw(Effect e)
        {
            base.Draw(e);

            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.White, this.AbsolutePosition, this.AbsolutePosition);
            //LineUtility3D.DrawAABoundingBox(Engine.g, Camera.View, Camera.Projection, this.AbsoluteBoundingBox, Matrix.Identity);
            //LineUtility3D.DrawAABoundingBox(Engine.g, Camera.View, Camera.Projection, this.boundingBox, this.AbsoluteWorld);
        }
    }
    public class Force
    {
        public Vector3 ForceVector
        {
            get; set;
        }
        public float Time
        {
            get; set;
        }

        public Force()
        {
            ForceVector = Vector3.Zero;
            Time = float.PositiveInfinity;
        }

        public Force(Vector3 _ForceVector, float _Time)
        {
            ForceVector = _ForceVector;
            Time = _Time;
        }

        public override String ToString()
        {
            return "Force: { ForceVector: " + ForceVector + " Time: " + Time + " }";
        }
    }
    public class Spring : PhysicsObject
    {
        PhysicsObject obj1;
        PhysicsObject obj2;

        Force f1;
        Force f2;

        public float springConstant { get; set; } = 1.0f;
        public float springZeroDistance { get; set; } = 1.0f;

        public Spring(PhysicsObject o1, PhysicsObject o2, float _springConstant, float _springZeroDistance)
        {
            IsStatic = true;
            CanCollide = false;

            obj1 = o1;
            obj2 = o2;

            springConstant = _springConstant;
            springZeroDistance = _springZeroDistance;

            f1 = new Force(Vector3.Zero, float.PositiveInfinity);
            f2 = new Force(Vector3.Zero, float.PositiveInfinity);

            obj1.AddForce(f1);
            obj2.AddForce(f2);
        }

        public override void Update()
        {
            base.Update();

            Vector3 Direction = obj2.Position - obj1.Position;
            Direction.Normalize();

            float Distance = (obj2.Position - obj1.Position).Length();

            Position = obj1.Position + Direction * Distance * 0.5f;

            float Dx = Distance - springZeroDistance;

            f1.ForceVector = Direction * Dx * springConstant;
            f2.ForceVector = -Direction * Dx * springConstant;

            //Console.WriteLine(f1);
            //Console.WriteLine(f2);
        }
    }

}