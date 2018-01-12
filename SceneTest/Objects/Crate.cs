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
using CharcoalEngine._2D;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;

namespace SceneTest.Objects
{
    class Crate : GameObject
    {
        public Crate()
        {
            OBJ_File obj = new OBJ_File();
            Name = "Crate";
            obj.Load("C:\\Users\\Michael\\Documents\\XNA\\Hunt-Or-Gatherers\\Models\\Objects\\Crate.obj", Engine.g, new Vector3(0, 0, 0), Vector3.Zero, 1f, false, false, this);


            Display = new CharcoalEngine._2D.Panel();

            Display.Bounds = new Vector2(300, 300);
            ((Panel)Display).Position = new Vector2(300, 300);
            Panel p = (Panel)Display;

            p.BGColor = Color.Gray;
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    p.Children.Add(new Panel());
                    p.Children[p.Children.Count - 1].Position = new Vector2(13 + i * 100, 13 + j * 100);
                    ((Panel)p.Children[p.Children.Count - 1]).BGColor = new Color(i/3.0f, j/3.0f, 1.0f);
                    p.Children[p.Children.Count - 1].Bounds = new Vector2(75, 75);
                    p.Children[p.Children.Count - 1].Visible = true;
                }
            }
        }

        public override void Select()
        {

            base.Select();
        }

        public override void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.C))
                Display.Visible = true;
            else
                Display.Visible = false;
            base.Update();
        }

        public override void Draw(Effect e)
        {
            Display.Draw(Engine.spriteBatch);
            base.Draw(e);
        }
        
    }
}
