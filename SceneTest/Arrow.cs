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
using CharcoalEngine.Object;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows;

namespace SceneTest
{
    class Arrow : Transform
    {
        float Velocity = 0.9f;
        float distance = 0.0f;
        bool Fired = false;
        public void Fire(Transform NewRoot)
        {
            Fired = true;
            NewRoot.Children.Add(this);
            Parent.Children.Remove(this);
        }
        public override void Update()
        {
            if (Fired)
            {
                Position += Vector3.Transform(Vector3.Right * Velocity, Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z));
                distance += Velocity;
                if (distance > 500)
                    Fired = false;
                for (int i = 0; i < Parent.Children.Count; i++)
                {
                    if (OBBCollision.Intersects(this.boundingBox, this.AbsoluteYawPitchRoll, this.AbsolutePosition, Parent.Children[i].boundingBox, Parent.Children[i].AbsoluteYawPitchRoll, Parent.Children[i].AbsolutePosition))
                    {
                        if (Parent.Children[i] != this)
                            Fired = false;
                    }
                }
            }
            base.Update();
        }
    }
}
