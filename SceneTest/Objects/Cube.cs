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
using CharcoalEngine._2D;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;

namespace SceneTest.Objects
{
    public class Cube : Transform
    {
        //public CharcoalEngine.Object.Material Material;

        public VertexPositionNormalTexture[] V;
        public ushort[] Indices;
        public VertexBuffer B;
        public IndexBuffer I;

        public Cube()
        {
            V = new VertexPositionNormalTexture[8];
            Indices = new ushort[12];
            B = new VertexBuffer(Engine.g, typeof(VertexPositionNormalTexture), 8, BufferUsage.WriteOnly);
            I = new IndexBuffer(Engine.g, IndexElementSize.SixteenBits, 12, BufferUsage.WriteOnly);

            V[0] = new VertexPositionNormalTexture(new Vector3(0, 0, 0), Vector3.Forward, new Vector2(0, 0));
            V[1] = new VertexPositionNormalTexture(new Vector3(1, 0, 0), Vector3.Forward, new Vector2(1, 0));
            V[2] = new VertexPositionNormalTexture(new Vector3(1, 1, 0), Vector3.Forward, new Vector2(1, 1));
            V[3] = new VertexPositionNormalTexture(new Vector3(0, 1, 0), Vector3.Forward, new Vector2(0, 1));

            V[4] = new VertexPositionNormalTexture(new Vector3(1, 0, 0), Vector3.Right, new Vector2(0, 0));
            V[5] = new VertexPositionNormalTexture(new Vector3(1, 0, 1), Vector3.Right, new Vector2(1, 0));
            V[6] = new VertexPositionNormalTexture(new Vector3(1, 1, 1), Vector3.Right, new Vector2(1, 1));
            V[7] = new VertexPositionNormalTexture(new Vector3(1, 1, 0), Vector3.Right, new Vector2(0, 1));

            Indices[0] = 0;
            Indices[1] = 1;
            Indices[2] = 2;

            Indices[3] = 2;
            Indices[4] = 3;
            Indices[5] = 0;

            Indices[6] = 4;
            Indices[7] = 5;
            Indices[8] = 6;

            Indices[9] = 6;
            Indices[10] = 7;
            Indices[11] = 4;

            B.SetData(V);
            I.SetData(Indices);

            /*Material = new CharcoalEngine.Object.Material();
            Material.Alpha = 1;
            Material.AlphaEnabled = false;
            Material.Ambient = Color.Black.ToVector3();
            //Material.DiffuseColor = Color.Black.ToVector3();
            Material.name = "CubeMat";
            Material._Texture = "C:\\Users\\Michael\\Documents\\XNA\\Hunt-Or-Gatherers\\Models\\Objects\\Cube\\Carpet_Plush_Charcoal.jpg";
            Material.Visible = true;*/
        }

        public override void Draw(Effect e)
        {
            /*if (Material.Visible)
            {*/
                //if (Camera.Viewport.frAbsoluteBoundingBox)
                //fill basic parameters
                if (e != null)
                {
                    /*if ((string)e.Tag == "NDT" && Material.AlphaEnabled == true)
                    {
                        base.Draw(e);
                        return;
                    }*/

                    if ((string)e.Tag == "ALPHANDT"/* && Material.AlphaEnabled == false*/)
                    {
                        base.Draw(e);
                        return;
                    }

                    e.Parameters["World"].SetValue(AbsoluteWorld);
                    e.Parameters["View"].SetValue(Camera.View);
                    e.Parameters["Projection"].SetValue(Camera.Projection);
                    //e.Parameters["BasicTexture"].SetValue(Material.Texture);
                    e.Parameters["TextureEnabled"].SetValue(/*Material._TextureEnabled*/false);
                    //e.Parameters["BumpMap"].SetValue(Material.BumpMap);
                    //e.Parameters["BumpMapEnabled"].SetValue(Material.BumpMapEnabled);
                    e.Parameters["DiffuseColor"].SetValue(/*Material.DiffuseColor*/Vector3.One);
                    e.Parameters["Alpha"].SetValue(/*Material.Alpha*/1.0f);
                    e.Parameters["AlphaEnabled"].SetValue(/*Material.AlphaEnabled*/false);
                    //...
                    e.CurrentTechnique.Passes[0].Apply();

                    //e.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                    Engine.g.SetVertexBuffer(B);
                    Engine.g.Indices = I;
                    Engine.g.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4);
                }
            /*}*/
            base.Draw(e);
        }
    }
}
