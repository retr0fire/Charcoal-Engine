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

namespace CharcoalEngine._2D
{
    public class Panel : Element2D
    {
        public Texture2D Background;
        public Vector2 BackgroundBounds;
        public Vector2 BackgroundOffset;
        public Color BGColor;

        public Panel()
        {

        }

        public override void Draw(SpriteBatch s)
        {
            if (!Visible)
                return;
            s.Begin();
            s.Draw(Pixel, Pos_Size(AbsolutePosition, Bounds), BGColor);
            if (Background != null) s.Draw(Background, Pos_Size(AbsolutePosition+ BackgroundOffset, BackgroundBounds), Color.White);
            s.End();

            base.Draw(s);
        }

        Rectangle Pos_Size(Vector2 Position, Vector2 Size)
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }
    }
}
