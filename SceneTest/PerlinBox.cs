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
    public class PerlinBox : Transform
    {
        float[,,] DataArray;
        int Size;
        int Frequency;
        public PerlinBox(int size, int frequency)
        {
            Size = size;
            Frequency = frequency;
            DataArray = CharcoalEngine.Utilities.MapGeneration._3DPerlinMap.Create_3D_Perlin_Map(size, frequency, new Random());
        }

        public override void Draw(Effect e)
        {
            if (e != null && (string)e.Tag == "NDT")
            {
                for (int x = 0; x < Size; x++)
                {
                    for (int y = 0; y < Size; y++)
                    {
                        for (int z = 0; z < Size; z++)
                        {
                            Effect effect = Engine.Content.Load<Effect>("Effects/NDT_Effect");
                            VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[6];
                            effect.Parameters["World"].SetValue(Matrix.CreateScale(DataArray[x,y,z]) * Matrix.CreateBillboard(AbsolutePosition+ new Vector3(x, y, z), Camera.Position, Camera.Up, Camera.look));
                            effect.Parameters["View"].SetValue(Camera.View);
                            effect.Parameters["Projection"].SetValue(Camera.Projection);
                            effect.Parameters["FarPlane"].SetValue(Camera.Viewport.MaxDepth);

                            effect.CurrentTechnique.Passes[0].Apply();
                            vertices[0] = new VertexPositionNormalTexture(Vector3.Zero, Vector3.Forward, Vector2.Zero);
                            vertices[1] = new VertexPositionNormalTexture(Vector3.Right, Vector3.Forward, Vector2.UnitX);
                            vertices[2] = new VertexPositionNormalTexture(Vector3.Right + Vector3.Up, Vector3.Forward, Vector2.One);

                            vertices[4] = new VertexPositionNormalTexture(Vector3.Right + Vector3.Up, Vector3.Forward, Vector2.One);
                            vertices[3] = new VertexPositionNormalTexture(Vector3.Zero, Vector3.Forward, Vector2.Zero);
                            vertices[5] = new VertexPositionNormalTexture(Vector3.Up, Vector3.Forward, Vector2.UnitY);

                            Engine.g.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 2);
                        }
                    }
                }
            }
            base.Draw(e);
        }
    }
}