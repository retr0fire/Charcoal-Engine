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
using System.IO;
using CharcoalEngine.Scene;
using CharcoalEngine;
using CharcoalEngine.Utilities;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows;
using System.Design;
using CharcoalEngine.Utilities.MapGeneration;

namespace CharcoalEngine.Object
{
    class VaporTracing : Transform
    {
        Effect effect;
        VertexPositionColor[] V;
        //density voxels per unit
        int Granularity = 100;
        Texture2D DensityMap;

        public float Brightness { get; set; } = 0.01f;
        public float Power { get; set; } = 4;
        
        public VaporTracing()
        {
            effect = Engine.Content.Load<Effect>("Effects/VaporTracing");
            V = new VertexPositionColor[6];

            Random r = new Random();

            V[0] = new VertexPositionColor(new Vector3(-1, -1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[1] = new VertexPositionColor(new Vector3(-1, 1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[2] = new VertexPositionColor(new Vector3(1, 1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[3] = new VertexPositionColor(new Vector3(-1, -1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[4] = new VertexPositionColor(new Vector3(1, 1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[5] = new VertexPositionColor(new Vector3(1, -1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));

            LocalBoundingBox = new BoundingBox(-Vector3.One, Vector3.One);

            DensityMap = new Texture2D(Engine.g, Granularity, Granularity * Granularity, false, SurfaceFormat.Vector4);

            Vector4[] pixels = new Vector4[Granularity * Granularity * Granularity];

            //float[,] height = _2DPerlinMap.Create_2D_Perlin_Map_W_Octaves(Granularity, new Random(), 2);
            float[,,] height = _3DPerlinMap.Create_3D_Perlin_Map_W_Octaves(Granularity, new Random(), 2);

            for (int i = 0; i < Granularity; i++)
            {
                for (int j = 0; j < Granularity; j++)
                {
                    for (int k = 0; k < Granularity; k++)
                    {
                        pixels[i + j * Granularity + k * Granularity * Granularity] = new Vector4((float)Math.Pow((double)((height[i, j, k] + 1.0f) / (double)2.0), (double)Power), 0, 0, 1);//new Vector4(((float)i / (float)Granularity), ((float)j / Granularity), 0, 1);
                    }
                }
            }

            DensityMap.SetData(pixels, 0, Granularity*Granularity* Granularity);
        }

        public float t = 0.0f;
        public override void Draw()
        {
            t += 0.01f;
            effect.Parameters["World"].SetValue(AbsoluteWorld);
            effect.Parameters["InverseWorld"].SetValue(Matrix.Invert(AbsoluteWorld));

            effect.Parameters["CornerMin"].SetValue(-Vector3.One);
            effect.Parameters["CornerMax"].SetValue(Vector3.One);

            effect.Parameters["ViewProjection"].SetValue(Camera.View * Camera.Projection);
            effect.Parameters["InverseViewProjection"].SetValue(Matrix.Invert(Camera.View * Camera.Projection));

            effect.Parameters["w"].SetValue((float)Camera.Viewport.Width);
            effect.Parameters["h"].SetValue((float)Camera.Viewport.Height);
            effect.Parameters["NearClip"].SetValue(Camera.Viewport.MinDepth);
            effect.Parameters["FarClip"].SetValue(Camera.Viewport.MaxDepth);

            effect.Parameters["CameraPosition"].SetValue(Camera.Position);
            effect.Parameters["BackgroundColor"].SetValue(Color.Black.ToVector3());
            effect.Parameters["DensityMap"].SetValue(DensityMap);
            effect.Parameters["Granularity"].SetValue(Granularity);
            effect.Parameters["Brightness"].SetValue(Brightness);
            effect.Parameters["t"].SetValue(t);

            effect.CurrentTechnique.Passes[0].Apply();

            Engine.g.DrawUserPrimitives(PrimitiveType.TriangleList, V, 0, 2);

            base.Draw();
        }                                               
    }
}
