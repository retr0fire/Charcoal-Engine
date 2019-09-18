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
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows;
using System.Design;
using CharcoalEngine.Object;
using System.Drawing.Design;
using System.Drawing;


namespace SceneTest
{
    class Ground
    {

    }

    class Section : Transform
    {
        float[,] Heights;
        int Height;
        int Width;
        Vector3 Scale;

        VertexPositionNormalTexture[] V;

        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public List<Material> Material_Edit
        {
            get
            {
                List<Material> T = new List<Material>();
                T.Add(Material);
                return T;
            }
            set
            {
                Material = value[0];
            }
        }


        [Editor(typeof(WindowsFormsComponentEditor), typeof(UITypeEditor))]
        public Material Material
        {
            get
            {
                return __material__;
            }
            set
            {
                __material__ = value;
            }
        }
        Material __material__;//sets basic material settings

        public Section(float[,] _Heights, int _Height, int _Width, Vector3 _Scale)
        {
            Material = new Material();

            Heights = _Heights;
            Height = _Height;
            Width = _Width;
            Scale = _Scale;


            ReloadHeights();
        }

        public string HMTextureFileName;

        [BrowsableAttribute(true)]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string _HMTexture
        {
            get { return HMTextureFileName; }
            set
            {
                HMTextureEnabled = true;
                HMTexture = TextureImporter.LoadTextureFromFile(value);
                if (HMTexture == null)
                    HMTextureEnabled = false;
                HMTextureFileName = value;

                Height = HMTexture.Bounds.Height;
                Width = HMTexture.Bounds.Width;

                Heights = getpixeldata(HMTexture);

                ReloadHeights();
            }
        }

        public Texture2D HMTexture;

        public bool _HMTextureEnabled
        {
            get { return HMTextureEnabled; }
            set { HMTextureEnabled = value; }
        }
        public bool HMTextureEnabled;

        public float[,] getpixeldata(Texture2D tex)
        {
            Microsoft.Xna.Framework.Color[] pixeldata;

            pixeldata = new Microsoft.Xna.Framework.Color[tex.Bounds.Width * tex.Bounds.Height];
            tex.GetData(pixeldata, 0, Height * Width);
            //HMTexture.Dispose();

            float[,] retheights = new float[Width, Height];

            for (int i = 0; i < Height * Width; i++)
            {
                int row = i / Width;
                int column = i - (int)(i / Width) * Width;

                retheights[column, row] = 1.0f - (float)pixeldata[i].G / 255.0f;
            }
            pixeldata = null;

            return retheights;
        }

        public void ReloadHeights()
        {
            
            int vnum = 0;
            V = new VertexPositionNormalTexture[6 * (Width) * (Height)];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    V[/*(y * (6 * Width)) + (x * 6) + 0 */ vnum++] = new VertexPositionNormalTexture(new Vector3(x + 0, Heights[roll_index(x + 0, Width - 1), roll_index(y + 0, Height - 1)], y + 0) * Scale, Vector3.Up, new Vector2((float)(x + 0) / (float)Width, (float)(y + 0) / (float)Height));
                    V[/*(y * (6 * Width)) + (x * 6) + 1 */ vnum++] = new VertexPositionNormalTexture(new Vector3(x + 1, Heights[roll_index(x + 1, Width - 1), roll_index(y + 0, Height - 1)], y + 0) * Scale, Vector3.Up, new Vector2((float)(x + 1) / (float)Width, (float)(y + 0) / (float)Height));
                    V[/*(y * (6 * Width)) + (x * 6) + 2 */ vnum++] = new VertexPositionNormalTexture(new Vector3(x + 1, Heights[roll_index(x + 1, Width - 1), roll_index(y + 1, Height - 1)], y + 1) * Scale, Vector3.Up, new Vector2((float)(x + 1) / (float)Width, (float)(y + 1) / (float)Height));
                    V[/*(y * (6 * Width)) + (x * 6) + 3 */ vnum++] = new VertexPositionNormalTexture(new Vector3(x + 0, Heights[roll_index(x + 0, Width - 1), roll_index(y + 0, Height - 1)], y + 0) * Scale, Vector3.Up, new Vector2((float)(x + 0) / (float)Width, (float)(y + 0) / (float)Height));
                    V[/*(y * (6 * Width)) + (x * 6) + 4 */ vnum++] = new VertexPositionNormalTexture(new Vector3(x + 1, Heights[roll_index(x + 1, Width - 1), roll_index(y + 1, Height - 1)], y + 1) * Scale, Vector3.Up, new Vector2((float)(x + 1) / (float)Width, (float)(y + 1) / (float)Height));
                    V[/*(y * (6 * Width)) + (x * 6) + 5 */ vnum++] = new VertexPositionNormalTexture(new Vector3(x + 0, Heights[roll_index(x + 0, Width - 1), roll_index(y + 1, Height - 1)], y + 1) * Scale, Vector3.Up, new Vector2((float)(x + 0) / (float)Width, (float)(y + 1) / (float)Height));
                }
            }
            /*for (int i = 30; i < V.Length; i++)
            {
                if (V[i].Position == Vector3.Zero)
                {
                    Console.WriteLine(i);
                }
                //Console.WriteLine(V[V.Length - 1].Position);
            }*/
            Console.WriteLine(vnum);
            List<Vector3> points = new List<Vector3>();
            for (int i = 0; i < V.Length; i++)
            {
                points.Add(V[i].Position);
                V[i].Normal = Vector3.Cross(Vector3.Normalize(
                    V[i].Position - V[roll_index((i + 6), V.Length-1)].Position), -(V[i].Position - 
                    V[roll_index(i + (Width * 6), V.Length-1)].Position));
            }
            LocalBoundingBox = BoundingBox.CreateFromPoints(points);
        }

        int roll_index(int value, int max)
        {
            if (value < 0)
                return max + value;
            if (value > max)
                return value - max;
            return value;
        }

        public override void Draw(Effect e)
        {
            if (e != null)
            {
                if ((string)e.Tag == "NDT")
                {
                    e.Parameters["FarPlane"].SetValue(Camera.Viewport.MaxDepth);
                    e.Parameters["World"].SetValue(AbsoluteWorld);
                    e.Parameters["View"].SetValue(Camera.View);
                    e.Parameters["Projection"].SetValue(Camera.Projection);
                    e.Parameters["BasicTexture"].SetValue(Material.Texture);
                    e.Parameters["TextureEnabled"].SetValue(Material._TextureEnabled);
                    e.Parameters["DiffuseColor"].SetValue(Material.DiffuseColor);
                    e.Parameters["Alpha"].SetValue(Material.Alpha);

                    e.Parameters["NormalMap"].SetValue(Material.NormalMap);
                    e.Parameters["NormalMapEnabled"].SetValue(Material.NormalMapEnabled);
                    //...
                    e.CurrentTechnique.Passes[0].Apply();

                    if (V != null)
                    {
                        Engine.g.DrawUserPrimitives(PrimitiveType.TriangleList, V, 0, 2 * (Width-1) * (Height-1));
                    }
                }
            }
            base.Draw(e);
        }
    }
}
