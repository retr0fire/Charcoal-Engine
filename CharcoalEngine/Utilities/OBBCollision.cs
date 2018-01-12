using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Design;
using System.Reflection;
using System.Diagnostics;
using CharcoalEngine.Scene;
using System.IO;

namespace CharcoalEngine.Utilities
{
    public static class OBBCollision
    {
        public static bool Intersects(BoundingBox a, Matrix orientation_a, Vector3 position_a, BoundingBox b, Matrix orientation_b, Vector3 position_b)
        {
            BoundingSphere a_s = new BoundingSphere((a.Max + a.Min) / 2 + position_a, (a.Max - a.Min).Length() / 2);
            BoundingSphere b_s = new BoundingSphere((b.Max + b.Min) / 2 + position_b, (b.Max - b.Min).Length() / 2);

            if (!a_s.Intersects(b_s))
            {
                return false;
            }

            if (InternalIntersects(a, orientation_a, position_a, b, orientation_b, position_b))
                return true;
            else
                return InternalIntersects(b, orientation_b, position_b, a, orientation_a, position_a);
        }

        private static bool InternalIntersects(BoundingBox a, Matrix orientation_a, Vector3 position_a, BoundingBox b, Matrix orientation_b, Vector3 position_b)
        {
            Vector3[] a_points = new Vector3[8];
            a_points = a.GetCorners();

            for (int i = 0; i < 8; i++)
            {
                a_points[i] = Vector3.Transform(a_points[i], orientation_a );
                a_points[i] += position_a;
                a_points[i] -= position_b;
                a_points[i] = Vector3.Transform(a_points[i], Matrix.Invert(orientation_b));
            }

            Line[] line = new Line[12];

            line[0] = new Line(a_points[0], a_points[1]);
            line[1] = new Line(a_points[1], a_points[2]);
            line[2] = new Line(a_points[2], a_points[3]);
            line[3] = new Line(a_points[3], a_points[0]);
            line[4] = new Line(a_points[4], a_points[5]);
            line[5] = new Line(a_points[5], a_points[6]);
            line[6] = new Line(a_points[6], a_points[7]);
            line[7] = new Line(a_points[7], a_points[4]);
            line[8] = new Line(a_points[0], a_points[4]);
            line[9] = new Line(a_points[1], a_points[5]);
            line[10] = new Line(a_points[2], a_points[6]);
            line[11] = new Line(a_points[3], a_points[7]);

            OBBBoundingBoxUtilities.DrawBoundingBox(a, position_a, orientation_a);
            OBBBoundingBoxUtilities.DrawBoundingBox(b, position_b, orientation_b);

            for (int i = 0; i < 12; i++)
            {
                float? intersection = b.Intersects(line[i].ray);
                if (intersection != null)
                {
                    if (intersection <= line[i].length)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
    public sealed class OBBBoundingBoxUtilities
    {
        public static void DrawBoundingBox(BoundingBox box, Vector3 Position, Matrix Orientation)
        {
            Vector3[] points = new Vector3[8];
            points = box.GetCorners();

            for (int i = 0; i < 8; i++)
            {
                points[i] = Vector3.Transform(points[i], Orientation);
                points[i] += Position;
            }

            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Color.White, points[0], points[1]);
            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Color.White, points[1], points[2]);
            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Color.White, points[2], points[3]);
            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Color.White, points[3], points[0]);
            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Color.White, points[4], points[5]);
            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Color.White, points[5], points[6]);
            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Color.White, points[6], points[7]);
            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Color.White, points[7], points[4]);
            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Color.White, points[0], points[4]);
            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Color.White, points[1], points[5]);
            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Color.White, points[2], points[6]);
            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Color.White, points[3], points[7]);

            SpriteFont font = Engine.Content.Load<SpriteFont>("Fonts/Font");
            Engine.spriteBatch.Begin();
            for (int i = 0; i < 8; i++)
            {
                Vector3 initial_position = Engine.g.Viewport.Project(points[i], Camera.Projection, Camera.View, Matrix.Identity);

                Engine.spriteBatch.DrawString(font, i.ToString(), new Vector2(initial_position.X, initial_position.Y), Color.Red);
            }
            Engine.spriteBatch.End();
        }
    }
    public class Line
    {
        public Ray ray;
        public Vector3 end;
        public float length;

        public Line(Vector3 start, Vector3 _end)
        {
            ray = new Ray(start, Vector3.Normalize(_end - start));
            end = _end;
            length = (_end - start).Length();
        }
    }
}
