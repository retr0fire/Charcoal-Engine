using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using CharcoalEngine.Scene;
using CharcoalEngine;
using CharcoalEngine.Utilities;
using CharcoalEngine.Object;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;

namespace SceneTest.Objects
{
    class Landscape
    {
        public float[,] Heights;
        public int Width;
        public int Height;

        public Landscape(float[,] heights, int width, int height)
        {
            Heights = heights;
            Width = width;
            Height = height;
        }

        public Vector3 MoveCharacter(BoundingBox b, Vector3 Position, Matrix Orientation, Vector3 Direction, float amount)
        {
            float original_height = Position.Y;
            float next_height = Position.Y + Direction.Y * amount;

            Vector2 original_xy = new Vector2(Position.X, Position.Z);
            Vector2 next_xy = new Vector2(Position.X + Direction.X * amount, Position.Z + Direction.Z * amount);

            if ((int)(next_xy.X) < 0)
                return Vector3.Zero;
            if ((int)(next_xy.X) >= Width)
                return Vector3.Zero;

            if ((int)(next_xy.Y) < 0)
                return Vector3.Zero;
            if ((int)(next_xy.Y) >= Height)
                return Vector3.Zero;

            float change_in_height = (int)(Heights[(int)(next_xy.X), (int)(next_xy.Y)]) - (int)(Heights[(int)(original_xy.X), (int)(original_xy.Y)]);
            //change_in_height *= height_multiplier;
            //change_in_height = (int)change_in_height;
            Console.WriteLine((int)(Heights[(int)(next_xy.X), (int)(next_xy.Y)]) + " + " + (int)(Heights[(int)(original_xy.X), (int)(original_xy.Y)]) + " + " + change_in_height);
            if (change_in_height > 1)
                return Vector3.Zero;
            else
            {
                
                return Direction * amount + Vector3.Up*change_in_height;
            }
        }
    }
}
