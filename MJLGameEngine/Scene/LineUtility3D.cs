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

namespace MJLGameEngine.Scene
{
    public static class LineUtility3D
    {
        private static BasicEffect effect;
        private static VertexPositionColor[] vertices = new VertexPositionColor[2];
        public static void Draw3DLine(GraphicsDevice g, Matrix View, Matrix Projection, Color LineColor, Vector3 Start, Vector3 End)
        {
            if (effect == null)
                effect = new BasicEffect(g);
            effect.View = View;
            effect.Projection = Projection;
            effect.DiffuseColor = LineColor.ToVector3();
            effect.CurrentTechnique.Passes[0].Apply();
            vertices[0] = new VertexPositionColor(Start, LineColor);
            vertices[1] = new VertexPositionColor(End, LineColor);
            g.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
        }
        public static void DrawPlane(GraphicsDevice g, Matrix View, Matrix Projection, int size, float[,] heights, float scale)
        {
            if (effect == null)
                effect = new BasicEffect(g);
            effect.View = View;
            effect.Projection = Projection;
            effect.DiffuseColor = Color.White.ToVector3();
            effect.VertexColorEnabled = true;
            effect.CurrentTechnique.Passes[0].Apply();
            VertexPositionColor[] V = new VertexPositionColor[36*size*size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    V[(y * (6 * size)) + (x * 6) + 0] = new VertexPositionColor(new Vector3(x + 0, heights[x + 0, y + 0], y + 0) * scale, new Color((float)x / (float)size, (float)y / (float)size, 0));
                    V[(y * (6 * size)) + (x * 6) + 1] = new VertexPositionColor(new Vector3(x + 1, heights[x + 1, y + 0], y + 0) * scale, new Color((float)x / (float)size, (float)y / (float)size, 0));
                    V[(y * (6 * size)) + (x * 6) + 2] = new VertexPositionColor(new Vector3(x + 1, heights[x + 1, y + 1], y + 1) * scale, new Color((float)x / (float)size, (float)y / (float)size, 0));
                    V[(y * (6 * size)) + (x * 6) + 3] = new VertexPositionColor(new Vector3(x + 0, heights[x + 0, y + 0], y + 0) * scale, new Color((float)x / (float)size, (float)y / (float)size, 0));
                    V[(y * (6 * size)) + (x * 6) + 4] = new VertexPositionColor(new Vector3(x + 1, heights[x + 1, y + 1], y + 1) * scale, new Color((float)x / (float)size, (float)y / (float)size, 0));
                    V[(y * (6 * size)) + (x * 6) + 5] = new VertexPositionColor(new Vector3(x + 0, heights[x + 0, y + 1], y + 1) * scale, new Color((float)x / (float)size, (float)y / (float)size, 0));
                }
            }
            g.DrawUserPrimitives(PrimitiveType.TriangleList, V, 0, 2*size*size);
        }
    }
}
