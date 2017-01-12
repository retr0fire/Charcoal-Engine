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
    public class SliderBar
    {
        string label;
        public Rectangle sliderrect;
        public Rectangle pointerrect;
        Vector2 mousepos;
        Color textcolor;
        Color barcolor;
        Color pointercolor;
        float value;
        SpriteFont sf;
        Texture2D pixel;
        Color barbordercolor;
        Color hovercolor;
        Color grabbedcolor;

        MouseState LastMouseState;
        MouseState MouseState;

        private bool grabbed = false;
        private bool hovering = false;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">Slider Bar label</param>
        /// <param name="slider">size of Slider Bar</param>
        /// <param name="pointer">size Pointer that mouse drags</param>
        /// <param name="tc">Text Color</param>
        /// <param name="bc">Bar Color</param>
        /// <param name="pc">Pointer Color</param>
        /// <param name="defaultvalue">Default value/ X position of Pointer</param>
        /// <param name="font">SpriteFont</param>
        /// <param name="p">Pixel</param>
        /// <param name="bbc">Border Color of Bar</param> 
        /// <param name="hc">Hover Color</param>
        /// <param name="gc">Grabbed Color</param>
        public void load(string text, Rectangle slider, Rectangle pointer, Color tc, Color bc, Color pc, float defaultvalue, SpriteFont font, Texture2D p, Color bbc, Color hc, Color gc)
        {
            label = text;
            sliderrect = slider;
            pointerrect = pointer;
            textcolor = tc;
            barcolor = bc;
            pointercolor = pc;
            value = defaultvalue;
            sf = font;
            pixel = p;
            barbordercolor = bbc;
            hovercolor = hc;
            grabbedcolor = gc;
        }
        public void Update()
        {
            MouseState = Mouse.GetState();
            if (pointerrect.Contains(MouseState.X, MouseState.Y))
            {
                hovering = true;
                if (MouseState.LeftButton == ButtonState.Pressed)
                {
                    if (LastMouseState.LeftButton != ButtonState.Pressed)
                        grabbed = true;

                }
                else
                {
                    grabbed = false;
                }

            }
            else
            {
                hovering = false;
            }
            if (MouseState.LeftButton != ButtonState.Pressed)
                grabbed = false;
            if (grabbed == true)
            {
                pointerrect.X = MouseState.X;
            }
            if (pointerrect.X < sliderrect.X)
                pointerrect.X = sliderrect.X;
            if (pointerrect.X+pointerrect.Width > sliderrect.X+sliderrect.Width)
                pointerrect.X = sliderrect.X + sliderrect.Width - pointerrect.Width;
            
            LastMouseState = MouseState;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            int buffer = 2;
            spriteBatch.Draw(pixel, sliderrect, barcolor);
            spriteBatch.Draw(pixel, new Rectangle(sliderrect.X, sliderrect.Y + sliderrect.Height - buffer, sliderrect.Width, buffer), barbordercolor);
            spriteBatch.Draw(pixel, pointerrect, textcolor); 
            spriteBatch.Draw(pixel, new Rectangle(pointerrect.X + buffer, pointerrect.Y + buffer, pointerrect.Width - buffer * 2, pointerrect.Height - buffer * 2), pointercolor);
            if (hovering)
                spriteBatch.Draw(pixel, new Rectangle(pointerrect.X + buffer, pointerrect.Y + buffer, pointerrect.Width - buffer*2, pointerrect.Height - buffer*2), hovercolor);
            if (grabbed)
                spriteBatch.Draw(pixel, new Rectangle(pointerrect.X + buffer, pointerrect.Y + buffer, pointerrect.Width - buffer*2, pointerrect.Height - buffer*2), grabbedcolor);
        
        }
    }
}
