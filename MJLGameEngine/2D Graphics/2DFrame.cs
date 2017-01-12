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
using MJLGameEngine._2D_Graphics.Objects;

namespace MJLGameEngine._2D_Graphics
{
    public class _2DFrame
    {
        public Texture2D Background;
        public Color BackgroundColor;

        public Rectangle FrameRectangle;
        MouseState m;
            
        public SpriteFont Font;

        #region Colors
        public Color ItemBackgroundColor,
                     ItemForegroundColor,
                     ItemHoverColor,
                     ItemPressedColor,
                     ItemTextColor,
                     ItemCheckedColor,
                     ItemArrowColor;
        #endregion
        int originalheight;
                
        private Texture2D Pixel;
        private MouseState lm;

        #region Controls
        public List<Button> Buttons = new List<Button>();
        public List<Checkbox> Checkboxes = new List<Checkbox>();
        public List<Label> Labels = new List<Label>();
        public List<NumberSelect> NumberSelects = new List<NumberSelect>();
        public List<SliderBar> SliderBars = new List<SliderBar>();
        #endregion

        public List<_2DFrame> ChildFrames = new List<_2DFrame>();
        public string Name;
        public bool Collapsed = true;
        GraphicsDevice g;

        #region _2DFrame
        public _2DFrame(string name,
                        Texture2D _Background, Color _BackgroundColor,
                        Rectangle _FrameRectangle,
                        SpriteFont _Font,
                        Color _ItemBackgroundColor,
                        Color _ItemForegroundColor,
                        Color _ItemHoverColor,
                        Color _ItemPressedColor,
                        Color _ItemTextColor,
                        Color _ItemCheckedColor,
                        Color _ItemArrowColor)
        {
            Name = name;

            g = GameEngineData.game.GraphicsDevice;

            Background         = _Background;
            BackgroundColor    = _BackgroundColor;
            FrameRectangle     = _FrameRectangle;
            Font = _Font;

            ItemBackgroundColor = _ItemBackgroundColor;
            ItemForegroundColor = _ItemForegroundColor;
            ItemHoverColor      = _ItemHoverColor;
            ItemPressedColor    = _ItemPressedColor;
            ItemTextColor       = _ItemTextColor;
            ItemCheckedColor    = _ItemCheckedColor;
            ItemArrowColor      = _ItemArrowColor;

            Pixel = new Texture2D(g, 1, 1,false, SurfaceFormat.Color);
            Color[] pix = new Color[1];
            pix[0] = new Color((float)1, (float)1, (float)1, (float)1);
            Pixel.SetData(pix, 0, 1);
        }
#endregion
        #region _2DFrameBasic
        public _2DFrame(string name,
                        Texture2D _Background, Color _BackgroundColor,
                        Rectangle _FrameRectangle,
                        SpriteFont _Font)
        {
            Name = name;
            g = GameEngineData.game.GraphicsDevice;

            Background = _Background;
            BackgroundColor = _BackgroundColor;
            FrameRectangle = _FrameRectangle;
            Font = _Font;

            SetColorsToDefault();

            Pixel = new Texture2D(g, 1, 1, false, SurfaceFormat.Color);
            Color[] pix = new Color[1];
            pix[0] = new Color((float)1, (float)1, (float)1, (float)1);
            Pixel.SetData(pix, 0, 1);
        }
        #endregion
        public void SetColorsToDefault()
        {
            ItemBackgroundColor = Color.Black;
            ItemForegroundColor = Color.DarkGray;
            ItemHoverColor = Color.White;
            ItemPressedColor = new Color((int)60, (int)60, (int)60);
            ItemTextColor = Color.Black;
            ItemCheckedColor = Color.LightBlue;
            //ItemArrowColor = ;
        }
        #region SetColors
        public void SetColors(
                        Color _ItemBackgroundColor,
                        Color _ItemForegroundColor,
                        Color _ItemHoverColor,
                        Color _ItemPressedColor,
                        Color _ItemTextColor,
                        Color _ItemCheckedColor,
                        Color _ItemArrowColor)
        {

            ItemBackgroundColor = _ItemBackgroundColor;
            ItemForegroundColor = _ItemForegroundColor;
            ItemHoverColor = _ItemHoverColor;
            ItemPressedColor = _ItemPressedColor;
            ItemTextColor = _ItemTextColor;
            ItemCheckedColor = _ItemCheckedColor;
            ItemArrowColor = _ItemArrowColor;

        }
        #endregion
        public void AddButton(Vector2 Position, Vector2 Size, string Text)
        {
            Rectangle ButtonRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            Button b = new Button();
            b.load(Pixel, Font, ButtonRectangle, ItemBackgroundColor, ItemForegroundColor, Text, ItemHoverColor, ItemPressedColor, ItemTextColor);
            Buttons.Add(b);
        }
        public void AddCheckBox(string Text, Vector2 Position, Vector2 Size, bool Checked)
        {
            Rectangle CheckRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            Checkbox c = new Checkbox();
            c.load(Text, CheckRectangle, Checked, Pixel, ItemTextColor, ItemForegroundColor, ItemCheckedColor, ItemHoverColor, Font, ItemTextColor);
            Checkboxes.Add(c);
        }
        public void AddLabel(Vector2 Position, string Text)
        {
            Label l = new Label();
            l.load(Font, Text, Position, ItemForegroundColor);
            Labels.Add(l);
        }
        public void AddNumberSelect(Vector2 Position, Vector2 Size, int Number, Texture2D Arrow, string Text)
        {
            NumberSelect n = new NumberSelect();
            n.load(Position, Arrow, Number, Font, ItemTextColor, ItemTextColor, ItemHoverColor, ItemTextColor, Text);
            NumberSelects.Add(n);
        }
        public void AddSliderBar(Rectangle pointer, Rectangle bar, string Text)
        {
            SliderBar s = new SliderBar();
            s.load(Text, bar, pointer, ItemTextColor, ItemForegroundColor, ItemForegroundColor, 0, Font, Pixel, ItemTextColor, ItemHoverColor, ItemPressedColor);
            SliderBars.Add(s);
        }
        public void Update(GameTime gameTime)
        {
            m = Mouse.GetState();
            if (!Collapsed)
            {
                for (int i = 0; i < Buttons.Count; i++)
                {
                    Buttons[i].buttonrect.X += FrameRectangle.X;
                    Buttons[i].buttonrect.Y += FrameRectangle.Y;
                    Buttons[i].update(m);
                    Buttons[i].buttonrect.X -= FrameRectangle.X;
                    Buttons[i].buttonrect.Y -= FrameRectangle.Y;
                }
                for (int i = 0; i < Checkboxes.Count; i++)
                {
                    Checkboxes[i].checkrect.X += FrameRectangle.X;
                    Checkboxes[i].checkrect.Y += FrameRectangle.Y;
                    Checkboxes[i].update(m);
                    Checkboxes[i].checkrect.X -= FrameRectangle.X;
                    Checkboxes[i].checkrect.Y -= FrameRectangle.Y;
                }
                for (int i = 0; i < NumberSelects.Count; i++)
                {
                    NumberSelects[i].position.X += FrameRectangle.X;
                    NumberSelects[i].position.Y += FrameRectangle.Y;
                    NumberSelects[i].update();
                    NumberSelects[i].position.X -= FrameRectangle.X;
                    NumberSelects[i].position.Y -= FrameRectangle.Y;
                }
                for (int i = 0; i < SliderBars.Count; i++)
                {
                    SliderBars[i].pointerrect.X += FrameRectangle.X;
                    SliderBars[i].pointerrect.Y += FrameRectangle.Y;
                    SliderBars[i].sliderrect.X += FrameRectangle.X;
                    SliderBars[i].sliderrect.Y += FrameRectangle.Y;
                    SliderBars[i].Update();
                    SliderBars[i].sliderrect.X -= FrameRectangle.X;
                    SliderBars[i].sliderrect.Y -= FrameRectangle.Y;
                    SliderBars[i].pointerrect.X -= FrameRectangle.X;
                    SliderBars[i].pointerrect.Y -= FrameRectangle.Y;
                }
                int h;
                for (int i = 0; i < ChildFrames.Count; i++)
                {
                    if (ChildFrames[i].FrameRectangle.Height + ChildFrames[i].FrameRectangle.Y > FrameRectangle.Height)
                        if (!ChildFrames[i].Collapsed)
                            FrameRectangle.Height = ChildFrames[i].FrameRectangle.Height + ChildFrames[i].FrameRectangle.Y;
                    if (i < ChildFrames.Count - 1)
                    {
                        if (ChildFrames[i].Collapsed == false)
                            ChildFrames[i + 1].FrameRectangle.Y = ChildFrames[i].FrameRectangle.Y + ChildFrames[i].FrameRectangle.Height + 10;
                        else
                            ChildFrames[i + 1].FrameRectangle.Y = ChildFrames[i].FrameRectangle.Y + 30;
                    }
                    ChildFrames[i].FrameRectangle.X += FrameRectangle.X;
                    ChildFrames[i].FrameRectangle.Y += FrameRectangle.Y;
                    ChildFrames[i].Update(gameTime);
                    ChildFrames[i].FrameRectangle.X -= FrameRectangle.X;
                    ChildFrames[i].FrameRectangle.Y -= FrameRectangle.Y;
                }
            }
            int height = (FrameRectangle.Height - 20);
            FrameRectangle.Height -= height;

            if (FrameRectangle.Contains(m.X, m.Y))
            {
                if (m.LeftButton == ButtonState.Pressed)
                    if (lm.LeftButton == ButtonState.Released)
                        Collapsed = !Collapsed;
            }

            FrameRectangle.Height += height;

            lm = m;
        }
        public void Draw(SpriteBatch s)
        {

            if (!Collapsed)
            {
                s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                
                s.Draw(Pixel, FrameRectangle, ItemBackgroundColor);
                FrameRectangle.Inflate(-2, -2);
                s.Draw(Pixel, FrameRectangle, BackgroundColor);
                FrameRectangle.Inflate(2, 2);
                if (Background != null)
                    s.Draw(Background, Background.Bounds, Color.White);
                s.Draw(Pixel, new Rectangle(FrameRectangle.X, FrameRectangle.Y + 20, FrameRectangle.Width, 2), ItemBackgroundColor);
                s.DrawString(Font, Name, new Vector2(FrameRectangle.X + 3, FrameRectangle.Y + 2), ItemTextColor);
                foreach (Button button in Buttons)
                {
                    button.buttonrect.X += FrameRectangle.X;
                    button.buttonrect.Y += FrameRectangle.Y;
                    button.draw(s);
                    button.buttonrect.X -= FrameRectangle.X;
                    button.buttonrect.Y -= FrameRectangle.Y;
                }
                foreach (Checkbox checkbox in Checkboxes)
                {
                    checkbox.checkrect.X += FrameRectangle.X;
                    checkbox.checkrect.Y += FrameRectangle.Y;
                    checkbox.draw(s);
                    checkbox.checkrect.X -= FrameRectangle.X;
                    checkbox.checkrect.Y -= FrameRectangle.Y;
                }
                foreach (Label label in Labels)
                {
                    label.position.X += FrameRectangle.X;
                    label.position.Y += FrameRectangle.Y;
                    label.draw(s);
                    label.position.X -= FrameRectangle.X;
                    label.position.Y -= FrameRectangle.Y;
                }
                foreach (NumberSelect numberselect in NumberSelects)
                {
                    numberselect.position.X += FrameRectangle.X;
                    numberselect.position.Y += FrameRectangle.Y;
                    numberselect.draw(s);
                    numberselect.position.X -= FrameRectangle.X;
                    numberselect.position.Y -= FrameRectangle.Y;
                }
                for (int i = 0; i < SliderBars.Count; i++)
                {
                    SliderBars[i].pointerrect.X += FrameRectangle.X;
                    SliderBars[i].pointerrect.Y += FrameRectangle.Y;
                    SliderBars[i].sliderrect.X += FrameRectangle.X;
                    SliderBars[i].sliderrect.Y += FrameRectangle.Y;
                    SliderBars[i].Draw(s);
                    SliderBars[i].sliderrect.X -= FrameRectangle.X;
                    SliderBars[i].sliderrect.Y -= FrameRectangle.Y;
                    SliderBars[i].pointerrect.X -= FrameRectangle.X;
                    SliderBars[i].pointerrect.Y -= FrameRectangle.Y;
                }
                s.End();
                for (int i = 0; i < ChildFrames.Count; i++)
                {
                    ChildFrames[i].FrameRectangle.X += FrameRectangle.X;
                    ChildFrames[i].FrameRectangle.Y += FrameRectangle.Y;
                    ChildFrames[i].Draw(s);
                    ChildFrames[i].FrameRectangle.X -= FrameRectangle.X;
                    ChildFrames[i].FrameRectangle.Y -= FrameRectangle.Y;
                }
            }
            else
            {
                s.Begin();
                int height = (FrameRectangle.Height - 20);
                FrameRectangle.Height -= height;

                s.Draw(Pixel, FrameRectangle, ItemBackgroundColor);
                FrameRectangle.Inflate(-2, -2);
                s.Draw(Pixel, FrameRectangle, BackgroundColor);
                FrameRectangle.Inflate(2, 2);

                FrameRectangle.Height += height;
                s.DrawString(Font, Name, new Vector2(FrameRectangle.X + 3, FrameRectangle.Y + 2), ItemTextColor);
                s.End();
            }
        }
        public void DrawString(SpriteBatch s, string Text, Vector2 Position, Color TextColor)
        {
            s.Begin();
            s.DrawString(Font, Text, Position, TextColor);
            s.End();
        }
        public void DrawString(SpriteBatch s, string Text, Vector2 Position)
        {
            s.Begin();
            s.DrawString(Font, Text, Position, ItemTextColor);
            s.End();
        }
    }
}
