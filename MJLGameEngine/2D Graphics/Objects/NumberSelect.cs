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
    public class NumberSelect
    {
        public Vector2 position;
        public int number;
        public string label;
        public SpriteFont sf;
        public Color textcolor;
        public Color arrowcolor;
        public Color hovercolor;
        public Color valuecolor;

        public Texture2D arrow;
        public Rectangle uprect;
        public Rectangle downrect;
        public MouseState m;
        public MouseState lm;
        public KeyboardState k;//for later use;

        public void load(Vector2 pos, Texture2D ar, int dfn, SpriteFont font, Color tc, Color ac, Color hc, Color vc, string l)
        {
            position = pos;
            arrow = ar;
            number = dfn;
            sf = font;
            textcolor = tc;
            arrowcolor = ac;
            hovercolor = hc;
            valuecolor = vc;
            label = l;
            
            int buffer = 2;
            //=========================================================================
            uprect.X = (int)(position.X + sf.MeasureString(label).X + buffer);
            uprect.Y = (int)(position.Y - buffer - arrow.Height/2);

            downrect.X = (int)(position.X + sf.MeasureString(label).X + buffer);
            downrect.Y = (int)(position.Y + buffer + sf.MeasureString(label).Y);

            uprect.Width = arrow.Width/2;
            uprect.Height = arrow.Height/2;

            downrect.Width = arrow.Width/2;
            downrect.Height = arrow.Height/2;
            
        }
        public void update()
        {
            m = Mouse.GetState();

            if (uprect.Contains(new Point(m.X, m.Y)))
            {
                if (m.LeftButton == ButtonState.Pressed)
                {
                    if (lm.LeftButton == ButtonState.Released)
                    {
                        number++;
                    }
                }
            }

            if (downrect.Contains(new Point(m.X, m.Y)))
            {
                if (m.LeftButton == ButtonState.Pressed)
                {
                    if (lm.LeftButton == ButtonState.Released)
                    {
                        number--;
                    }
                }
            }

            int buffer = 2;
            //=========================================================================
            uprect.X = (int)(position.X + sf.MeasureString(label).X + buffer);
            uprect.Y = (int)(position.Y - buffer - arrow.Height / 2);

            downrect.X = (int)(position.X + sf.MeasureString(label).X + buffer);
            downrect.Y = (int)(position.Y + buffer + sf.MeasureString(label).Y);

            uprect.Width = arrow.Width / 2;
            uprect.Height = arrow.Height / 2;

            downrect.Width = arrow.Width / 2;
            downrect.Height = arrow.Height / 2;
            

            lm = m;
        }
        public void draw(SpriteBatch s)
        {
            s.DrawString(sf, label, position, textcolor);
            s.DrawString(sf, number.ToString(), new Vector2(position.X + sf.MeasureString(label).X + 4, position.Y), valuecolor);
            if (uprect.Contains(new Point(m.X, m.Y)))
            {
                s.Draw(arrow, uprect, hovercolor);
            }
            else
            {
                s.Draw(arrow, uprect, arrowcolor);
            }
            //-------------------------------------------
            if (downrect.Contains(new Point(m.X, m.Y)))
            {
                s.Draw(arrow, downrect, null, hovercolor, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
            }
            else
            {
                s.Draw(arrow, downrect, null, arrowcolor, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
            }
            

        }
    }
}
