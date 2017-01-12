using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MJLGameEngine._2D_Graphics.Objects
{
    public class Label
    {
        public string text;
        public SpriteFont sf;
        public Vector2 position;
        public Color lcolor;

        public void load(SpriteFont font, string caption, Vector2 pos, Color color)
        {
            sf = font;
            text = caption;
            position = pos;
            lcolor = color;
        }
        public void draw(SpriteBatch s)
        {

            s.DrawString(sf, text, position, lcolor);

        }
    }
}
