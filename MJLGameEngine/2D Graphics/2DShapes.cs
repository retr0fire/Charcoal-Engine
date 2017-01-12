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

namespace MJLGameEngine._2D_Graphics
{
    public static class _2DShapes
    {
        public static SpriteFont Font;
        private static Texture2D Pixel;
        private static GraphicsDevice g;
        public static void Init(SpriteFont _Font)
        {
            Font = _Font;
            g = GameEngineData.game.GraphicsDevice;

            Pixel = new Texture2D(g, 1, 1, false, SurfaceFormat.Color);
            Color[] pix = new Color[1];
            pix[0] = new Color((float)1, (float)1, (float)1, (float)1);
            Pixel.SetData(pix, 0, 1);
        }
        public static void DrawString(SpriteBatch s, string Text, Vector2 Position, Color TextColor)
        {
            s.Begin();
            s.DrawString(Font, Text, Position, TextColor);
            s.End();
        }
        public static void DrawLine(SpriteBatch s, Vector2 Start, Vector2 End, Color LineColor)
        {
            s.Begin();
            Vector2 Direction = End - Start;
            Direction.Normalize();
            Vector2 Position = Start;
            while ((Position-Start).Length() < (End-Start).Length())
            {
                s.Draw(Pixel, Position, null, LineColor);
                Position += Direction;
            }
            s.End();
        }
    }
}
