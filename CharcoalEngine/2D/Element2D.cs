using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using CharcoalEngine.Object;
using CharcoalEngine.Editing;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;
/// <summary>
/// This stuff is pretty untested so far, I haven't really had a chance to use it yet
/// </summary>
namespace CharcoalEngine._2D
{
    public class Element2D
    {
        public List<Element2D> Children = new List<Element2D>();
        public Element2D Parent;

        public Vector2 Bounds;//x & y are always 0
        public Vector2 Position;
        public Vector2 AbsolutePosition
        {
            get
            {
                if (Parent != null)
                    return Parent.AbsolutePosition + Position;
                else
                    return Position;
            }
        }

        public Texture2D Pixel;

        public bool Visible = true;

        public Element2D()
        {
            Pixel = Engine.Content.Load<Texture2D>("ModelTextures/Pixel");
        }

        public virtual void Update()
        {
            foreach (Element2D e in Children)
            {
                e.Parent = this;
                e.Update();
            }
        }

        public virtual void Draw(SpriteBatch s)
        {
            foreach (Element2D e in Children)
            {
                e.Draw(s);
            }
        }
    }
}
