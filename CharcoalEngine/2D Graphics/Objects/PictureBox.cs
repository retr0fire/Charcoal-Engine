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
    public class PictureBox
    {
        public Rectangle picturerect;
        public Texture2D border;
        public Color bordercolor;
        public float fade;

        public Texture2D image;

        public void Load(Texture2D pic, Vector2 position, Color bc, float fadeval, Texture2D arbpic)
        {
            image = pic;
            border = arbpic;
            bordercolor = bc;
            fade = fadeval;
            picturerect = image.Bounds;
            picturerect.X = (int)position.X;
            picturerect.Y = (int)position.Y;
        }
        public void draw(SpriteBatch s)
        {

            s.Draw(border, new Rectangle(picturerect.X - 2, picturerect.Y - 2, picturerect.Width + 4, picturerect.Height + 4), bordercolor);
            
            s.Draw(border, picturerect, Color.Black);
            
            s.Draw(image, picturerect, new Color(1 - fade, 1 - fade, 1 - fade, 1-fade));

        }
    }
}
