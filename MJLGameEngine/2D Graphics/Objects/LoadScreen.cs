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
    public class LoadScreen
    {
        Texture2D[] p;
        Texture2D arbpix;
        float index = 0;
        float speed;
        int fadelength;
        public void load(Texture2D[] images, float sp, Texture2D apix, int fade)
        {
            p = images;
            arbpix = apix;
            speed = sp;
            fadelength = fade;
        }

        public void draw(SpriteBatch s, Rectangle screenbounds)
        {

            if (index < p.Length)
            {
                
                
                int di = (int)index;
                s.Draw(arbpix, new Rectangle(0, 0, screenbounds.Width, screenbounds.Height), Color.Black);
                s.Draw(p[di], new Rectangle(screenbounds.Width / 2 - p[di].Width / 2, screenbounds.Height / 2 - p[di].Height / 2, p[di].Width, p[di].Height), Color.White);
                
            
                index += speed;
            }
            
            if (index < p.Length + fadelength)
            {
                if (index >= p.Length)
                {
                    s.Draw(arbpix, new Rectangle(0, 0, screenbounds.Width, screenbounds.Height), new Color(0, 0, 0, 1-((index-57) / fadelength)));
                    index += speed;
                }
            }

        }
    }
}
