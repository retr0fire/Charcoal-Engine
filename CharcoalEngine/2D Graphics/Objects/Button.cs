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

namespace CharcoalEngine._2D_Graphics.Objects
{
    enum bstate
    {
        hover,
        away,
        pressed,
        released
    }
    public class Button
    {
        bstate bs = bstate.away;
        bstate lbs = bstate.away;
        public Rectangle buttonrect;
        public Color bordercolor;
        public Color fillcolor;
        public Color pressedColor;
        public string text;
        public Rectangle mousepos = new Rectangle(0, 0, 1, 1);
        public Color hovercolor;
        public Color textcolor;

        public Texture2D border;
        public Texture2D fill;

        public SpriteFont sf;

        /// <summary>
        /// Loads images(Note: can be used to load custom button face images)
        /// </summary>
        /// <param name="g">The GraphicsDevice in use</param>
        public void load(Texture2D p, SpriteFont f, Rectangle br, Color bc, Color fc, string t, Color hc, Color pc, Color tc)
        {
            buttonrect = br;
            bordercolor = bc;
            fillcolor = fc;
            text = t;
            hovercolor = hc;
            pressedColor = pc;
            textcolor = tc;

            border = p;
            fill = p;
            sf = f;
        }

        public void update(MouseState m)
        {
            mousepos.X = m.X;
            mousepos.Y = m.Y;

            if (buttonrect.Intersects(mousepos))
            {
                bs = bstate.hover;
                if (m.LeftButton == ButtonState.Pressed)
                {
                    bs = bstate.pressed;
                }
                if (m.LeftButton == ButtonState.Released)
                {
                    if (lbs == bstate.pressed)
                    {
                        bs = bstate.released;
                    }
                }
            }
            else
            {
                bs = bstate.away;
            }






            lbs = bs;
        }
        public void draw(SpriteBatch s)
        {
            

            //====
            s.Draw(border, buttonrect, bordercolor);
            //====
            Rectangle frect = new Rectangle(buttonrect.X + 2, buttonrect.Y + 2, buttonrect.Width - 4, buttonrect.Height - 4);

            if (bs == bstate.hover)
                s.Draw(fill, frect, hovercolor);

            if (bs == bstate.away)
                s.Draw(fill, frect, fillcolor);

            if (bs == bstate.pressed)
                s.Draw(fill, frect, pressedColor);

            if (bs == bstate.released)
            {
                s.Draw(fill, frect, hovercolor); ;
            }

            s.DrawString(sf, text, new Vector2(buttonrect.X + buttonrect.Width / 2 - sf.MeasureString(text).X/2, buttonrect.Y + buttonrect.Height / 2 - sf.LineSpacing / 2), textcolor);

        }

        public bool isreleased()
        {
            if (bs == bstate.released)
                return true;
            else
                return false;
        }
    }
}
