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
    public class Transform
    {
        public Transform Parent;

        public string Name = "Transform";

        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public List<Transform> Children { get; set; } = new List<Transform>();

        /// <summary>
        /// The position relative to the parent's position
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return __position__;
            }
            set
            {
                __position__ = value;
                Invalidate();
            }
        }
        public Vector3 __position__ = Vector3.Zero;

        public Vector3 AbsolutePosition
        {
            get
            {
                if (!AbsolutePositionValid)
                {
                    if (Parent != null)
                    {
                        //Console.WriteLine(Parent.AbsolutePosition);
                        __absoluteposition__ =  Vector3.Transform(Position, Parent.AbsoluteWorld);
                    }
                    else
                    {
                        //Console.WriteLine("null");
                        __absoluteposition__ = Position;
                    }
                    AbsolutePositionValid = true;
                }
                return __absoluteposition__;
            }
        }
        private bool AbsolutePositionValid = false;
        private Vector3 __absoluteposition__;

        /// <summary>
        /// Yaw, Pitch, and Roll for this node
        /// </summary>
        public Vector3 YawPitchRoll
        {
            get
            {
                return __yawpitchroll__;
            }
            set
            {
                __yawpitchroll__ = value;
                //UpdateMatrix();
                Up = Vector3.Transform(Vector3.Up, Matrix.CreateFromYawPitchRoll(__yawpitchroll__.X, __yawpitchroll__.Y, __yawpitchroll__.Z));
                Forward = Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(__yawpitchroll__.X, __yawpitchroll__.Y, __yawpitchroll__.Z));
                //Update_AfterRotation();

                Invalidate();

                /*TODO this is I think a hack*/
                UpValid = true;
                ForwardValid = true;
            }
        }
        Vector3 __yawpitchroll__;

        public Matrix AbsoluteYawPitchRoll
        {
            get
            {
                if (!AbsoluteYawPitchRollValid)
                {
                    if (Parent != null)
                        __absoluteyawpitchroll__ = Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z) * Parent.AbsoluteYawPitchRoll;
                    else
                        __absoluteyawpitchroll__ = Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z);
                    AbsoluteYawPitchRollValid = true;
                }
                return __absoluteyawpitchroll__;
            }
        }
        private bool AbsoluteYawPitchRollValid = false;
        private Matrix __absoluteyawpitchroll__;

        
        /// <summary>
        /// boundingBox that contains this node and all of its children, untranslated
        /// </summary>
        public BoundingBox boundingBox
        {
            get
            {
                if (!boundingBoxValid)
                {
                    UpdateBoundingBox();
                    boundingBoxValid = true;
                }
                return __boundingbox__;
            }
            //this shouldn't be written to as its a generated value
        }
        private BoundingBox __boundingbox__ = new BoundingBox(-Vector3.One, Vector3.One);
        private bool boundingBoxValid = false;

        /// <summary>
        /// boundingbox that contains this node's local geometry, untranslated
        /// </summary>
        public BoundingBox LocalBoundingBox
        {
            get
            {
                return __localboundingbox__;
            }
            set
            {
                __localboundingbox__ = value;
                InvalidateBBox();
            }
        }
        public BoundingBox __localboundingbox__ = new BoundingBox(-Vector3.One, Vector3.One);

        public BoundingBox AbsoluteBoundingBox
        {
            get
            {
                if (!AbsoluteBoundingBoxValid)
                {
                    BoundingBox b = boundingBox;

                    b.Min += AbsolutePosition;
                    b.Max += AbsolutePosition;
                    __absoluteboundingbox__ = b;
                    AbsoluteBoundingBoxValid = true;
                }
                return __absoluteboundingbox__;
            }
        }
        private bool AbsoluteBoundingBoxValid = false;
        private BoundingBox __absoluteboundingbox__;
        
        private bool JustCreated = true;

        public bool DebugDraw
        {
            get;
            set;
        }

        public Transform()
        {
            
        }

        public void Invalidate()
        {
            AbsolutePositionValid = false;
            AbsoluteYawPitchRollValid = false;
            LocalWorldValid = false;
            AbsoluteWordValid = false;
            InvalidateBBox();

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Invalidate();
            }

            if (Parent != null) Parent.InvalidateBBox();
        }

        public void InvalidateBBox()
        {
            AbsoluteBoundingBoxValid = false;
            boundingBoxValid = false;
            if (Parent != null) Parent.InvalidateBBox();
        }
        
        /// <summary>
        /// updates the boundingBox after changes in orientation of child nodes
        /// </summary>
        public virtual void UpdateBoundingBox()
        {
            //Console.WriteLine("getting updated" + Children.Count);
            BoundingBox b = LocalBoundingBox;
            for (int i = 0; i < Children.Count; i++)
            {
                b = BoundingBox.CreateMerged(b, GetBBox(Children[i].boundingBox,Children[i].Position));
            }
            __boundingbox__ = b;
        }

        public BoundingBox GetBBox(BoundingBox b)
        {
            b.Max += Position;
            b.Min += Position;
            return b;
        }
        public BoundingBox GetBBox(BoundingBox b, Vector3 Offset)
        {
            b.Max += Offset;
            b.Min += Offset;
            return b;
        }


        public Matrix LocalWorld
        {
            get
            {
                if (!LocalWorldValid)
                {
                    UpdateMatrix();
                    LocalWorldValid = true;
                }
                return __localworld__;
            }
        }
        private Matrix __localworld__ = Matrix.Identity;
        private bool LocalWorldValid = false;

        public Matrix AbsoluteWorld
        {
            get
            {
                if (!AbsoluteWordValid)
                {
                    if (Parent != null)
                        __absoluteworld__ = LocalWorld * Parent.AbsoluteWorld;
                    else
                        __absoluteworld__ = LocalWorld;
                    AbsoluteWordValid = true;
                }
                return __absoluteworld__;
            }
        }
        private Matrix __absoluteworld__;
        private bool AbsoluteWordValid = false;

        public Vector3 Forward {
            get
            {
                if (!ForwardValid)
                {
                    if (Parent != null)
                        __forward__ = Vector3.Transform(__forward__, Parent.AbsoluteYawPitchRoll);
                    ForwardValid = true;
                }
                return __forward__;
            }
            set
            {
                ForwardValid = true;
                __forward__ = value;
            }
        }
        private Vector3 __forward__= Vector3.Forward;
        private bool ForwardValid = false;

        public Vector3 Up
        {
            get
            {
                if (!UpValid)
                {
                    if (Parent != null)
                        __up__ = Vector3.Transform(__up__, Parent.AbsoluteYawPitchRoll);
                    UpValid = true;
                }
                return __up__;
            }
            set
            {
                ForwardValid = true;
                __up__ = value;
            }
        }
        private Vector3 __up__ = Vector3.Up;
        private bool UpValid = false;

        public Vector3 Center
        {
            get
            {
                return __center__;
            }
            set
            {
                __center__ = value;
                LocalWorldValid = false;
                AbsoluteWordValid = false;
            }
        }
        private Vector3 __center__;

        public virtual void UpdateMatrix()
        {
            //World = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(-Center) * Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z) * Matrix.CreateTranslation(Center) * Matrix.CreateTranslation(Position);
            __localworld__ = Matrix.CreateTranslation(-Center) * Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z) * Matrix.CreateTranslation(Center) * Matrix.CreateTranslation(Position);
        }

        public float? Select(Ray selectionRay)
        {
            selectionRay.Position -= AbsolutePosition+Center;
            selectionRay.Position = Vector3.Transform(selectionRay.Position, Matrix.Transpose(AbsoluteYawPitchRoll));
            selectionRay.Direction = Vector3.Transform(selectionRay.Direction, Matrix.Transpose(AbsoluteYawPitchRoll));
            selectionRay.Position += AbsolutePosition+Center;

            return selectionRay.Intersects(AbsoluteBoundingBox);
        }
        public virtual void Update()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].Parent == null)
                    Children[i].Parent = this;
                Children[i].Update();
            }
        }

        /// <summary>
        /// _Not_ to be used for main drawing of 3D entities - should be done by scene manager.
        /// This handles the drawing of any UI graphics
        /// </summary>
        public virtual void Draw(Effect e)
        {
            if (DebugDraw)
            {
                LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.White, AbsolutePosition, AbsolutePosition + Vector3.Right * 0.5f);
                LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.White, AbsolutePosition, AbsolutePosition + Vector3.Up * 0.5f);
                LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.White, AbsolutePosition, AbsolutePosition + Vector3.Forward * 0.5f);
            }
            //if (AbsoluteBoundingBox.Intersects(Camera.Frustum))
            //{
                for (int i = 0; i < Children.Count; i++)
                {
                    Children[i].Draw(e);
                }
            //}
        }
    }
}