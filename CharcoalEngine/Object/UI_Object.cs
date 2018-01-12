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
    public class UI_Object : Transform
    {
        MouseState m;
        MouseState lm;

        Model Arrow;

        public UI_Object()
        {

        }

        public override void Update()
        {
           // m = Mouse.GetState();

            

            //lm = m;
            base.Update();
        }
        public override void Draw(Effect e)
        {


            /*LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.White, Vector3.Transform(Vector3.Zero, AbsoluteWorld), Vector3.Transform(Vector3.Zero, AbsoluteWorld) + Vector3.Right *0.5f);
            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.White, Vector3.Transform(Vector3.Zero, AbsoluteWorld), Vector3.Transform(Vector3.Zero, AbsoluteWorld) + Vector3.Up * 0.5f);
            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.White, Vector3.Transform(Vector3.Zero, AbsoluteWorld), Vector3.Transform(Vector3.Zero, AbsoluteWorld) + Vector3.Forward * 0.5f);
           */ base.Draw(e);
        }
    }
}
