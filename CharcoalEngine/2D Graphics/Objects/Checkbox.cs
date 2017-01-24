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
    public class Checkbox
    {
        public string label;
        public Rectangle checkrect;
        public bool ischecked;
        public Texture2D pixel;
        public Color bordercolor;
        public Color fillcolor;
        public Color checkcolor;
        public Color hovercolor;
        public Color textcolor;
        public Point mousepos;
        public MouseState state;
        public MouseState lstate;
        public SpriteFont sf;

        public void load(string text, Rectangle rect, bool default_to_checked, Texture2D arbpic, Color bc, Color fc, Color cc, Color hc, SpriteFont font, Color tc)
        {
            label = text;
            checkrect = new Rectangle(rect.X + (int)font.MeasureString(text).X + 4, rect.Y, rect.Width, rect.Height);
            ischecked = default_to_checked;
            pixel = arbpic;
            bordercolor = bc;
            fillcolor = fc;
            checkcolor = cc;
            hovercolor = hc;
            sf = font;
            textcolor = tc;
        }

        public void update(MouseState m)
        {
            state = m;

            mousepos.X = state.X;
            mousepos.Y = state.Y;

            if (checkrect.Contains(mousepos))
            {
                if (state.LeftButton == ButtonState.Pressed)
                {
                    if (lstate.LeftButton == ButtonState.Released)
                    {
                        if (ischecked == false)
                        {
                            ischecked = true;
                        }
                        else
                        {
                            ischecked = false;
                        }
                    }
                }
            }
            lstate = state;
        }
        public void draw(SpriteBatch s)
        {

            s.DrawString(sf, label, new Vector2(checkrect.X - sf.MeasureString(label).X - 4, checkrect.Y + checkrect.Height / 2 - sf.MeasureString(label).Y/2), textcolor);
            s.Draw(pixel, new Rectangle(checkrect.X - 2, checkrect.Y - 2, checkrect.Width + 4, checkrect.Height + 4), bordercolor);
            if (checkrect.Contains(mousepos))
            {
                s.Draw(pixel, checkrect, hovercolor);
            }
            else
            {
                s.Draw(pixel, checkrect, fillcolor);
            }
            if (ischecked == true)
            {
                s.Draw(pixel, new Rectangle(checkrect.X + 4, checkrect.Y + 4, checkrect.Width - 8, checkrect.Height - 8), checkcolor);
            }

        }
    }
}
