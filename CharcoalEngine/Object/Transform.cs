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
                //Console.WriteLine("set" + this.GetType().ToString());
                __position__ = value;
                if (Parent != null) Parent.UpdateBoundingBox();
                UpdateMatrix();
            }
        }
        public Vector3 __position__ = Vector3.Zero;
        public Vector3 AbsolutePosition
        {
            get
            {
                if (Parent != null)
                {
                    //Console.WriteLine(Parent.AbsolutePosition);
                    return Vector3.Transform(Position, Parent.AbsoluteWorld);
                }
                else
                {
                    //Console.WriteLine("null");
                    return Position;
                }
            }
            set
            {

            }
        }
        
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
                UpdateMatrix();
                Up = Vector3.Transform(Vector3.Up, Matrix.CreateFromYawPitchRoll(__yawpitchroll__.X, __yawpitchroll__.Y, __yawpitchroll__.Z));
                Forward = Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(__yawpitchroll__.X, __yawpitchroll__.Y, __yawpitchroll__.Z));
                //Update_AfterRotation();
            }
        }
        Vector3 __yawpitchroll__;

        public Matrix AbsoluteYawPitchRoll
        {
            get
            {
                if (Parent != null)
                {
                    return Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z)*Parent.AbsoluteYawPitchRoll;
                }
                return Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z);
            }
        }

        /*  
        /// <summary>
        /// relative scale for this node, and its children
        /// </summary>
        public Vector3 Scale
        {
            get
            {
                return __scale__;
            }
            set
            {
                __scale__ = value;
                UpdateBoundingBox();
            }
        }
        Vector3 __scale__;
        public Vector3 AbsoluteScale
        {
            get
            {
                return Parent.AbsoluteScale + Scale;
            }
        }
        */
        /// <summary>
        /// boundingBox that contains this node and all of its children, untranslated
        /// </summary>
        public BoundingBox boundingBox
        {
            get
            {
                return __boundingbox__;
            }
            set
            {
                __boundingbox__ = value;

            }
        }
        public BoundingBox __boundingbox__ = new BoundingBox(-Vector3.One, Vector3.One);

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
                UpdateBoundingBox();
            }
        }
        public BoundingBox __localboundingbox__ = new BoundingBox(-Vector3.One, Vector3.One);

        public BoundingBox AbsoluteBoundingBox
        {
            get
            {
                BoundingBox b = boundingBox;

                b.Min += AbsolutePosition;
                b.Max += AbsolutePosition;
                return b;
            }
            set
            {

            }
        }


        private bool JustCreated = true;

        public Transform()
        {
            //UpdateBoundingBox();

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
                boundingBox = b;
                
                if (Parent != null)
                    Parent.UpdateBoundingBox();
            
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

        public Matrix LocalWorld = Matrix.Identity;
        public Matrix AbsoluteWorld
        {
            get
            {
                if (Parent != null)
                    return LocalWorld * Parent.AbsoluteWorld;
                else
                    return LocalWorld;
            }
        }

        public Vector3 Forward {
            get
            {
                if (Parent != null)
                    return Vector3.Transform(__forward__, Parent.AbsoluteYawPitchRoll);
                return __forward__;
            }
            set
            {
                __forward__ = value;
            }
        }
        Vector3 __forward__= Vector3.Forward;
        public Vector3 Up
        {
            get
            {
                if (Parent != null)
                    return Vector3.Transform(__up__, Parent.AbsoluteYawPitchRoll);
                return __up__;
            }
            set
            {
                __up__ = value;
            }
        }
        Vector3 __up__ = Vector3.Up;

        public Vector3 Center
        {
            get
            {
                return __center__;
            }
            set
            {
                __center__ = value;
                UpdateMatrix();
            }
        }
        private Vector3 __center__;

        public virtual void UpdateMatrix()
        {
            //World = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(-Center) * Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z) * Matrix.CreateTranslation(Center) * Matrix.CreateTranslation(Position);
            LocalWorld = Matrix.CreateTranslation(-Center) * Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z) * Matrix.CreateTranslation(Center) * Matrix.CreateTranslation(Position);
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
            if (JustCreated)
            {
                UpdateBoundingBox();
                JustCreated = false;
                for (int i = 0; i < Children.Count; i++)
                {
                    if (Children[i].Parent == null)
                        Children[i].Parent = this;
                    Children[i].Update();
                }
            }
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
            //LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.White, AbsolutePosition, AbsolutePosition + Vector3.Right * 0.5f);
            //LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.White, AbsolutePosition, AbsolutePosition + Vector3.Up * 0.5f);
            //LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.White, AbsolutePosition, AbsolutePosition + Vector3.Forward * 0.5f);
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