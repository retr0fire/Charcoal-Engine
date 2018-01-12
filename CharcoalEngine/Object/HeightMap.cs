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
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Windows;
using System.Design;

namespace CharcoalEngine.Object
{
    public class HeightMap : UI_Object
    {
        
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

        public HeightMap()
        {
            Material = new Material();
        }

        public bool Reload
        {
            set
            {
                _HMTexture = HMTextureFileName;
                //ReloadHeights();
            }
            get { return false; }
        }

        VertexPositionNormalTexture[] V;
        public int Width;
        public int Height;
        public float[,] Heights;
        public Vector3 scale
        {
            get
            {
                return __scale__;
            }
            set
            {
                __scale__ = value;
                ReloadHeights();
            }
        }
        public float tile
        {
            get
            {
                return __tile__;
            }
            set
            {
                __tile__ = value;
                ReloadHeights();
            }
        }
        private float __tile__ = 10;
        private Vector3 __scale__ = new Vector3(0.1f, 2.0f, 0.1f);
        
        public void ReloadHeights()
        {
            Height = HMTexture.Bounds.Height;
            Width = HMTexture.Bounds.Width;

            Heights = getpixeldata(HMTexture);

            V = new VertexPositionNormalTexture[36 * (Width - 1) * (Height - 1)];
            for (int y = 0; y < Height - 1; y++)
            {
                for (int x = 0; x < Width - 1; x++)
                {
                    V[(y * (6 * Width)) + (x * 6) + 0] = new VertexPositionNormalTexture(new Vector3(x + 0, Heights[x + 0, y + 0], y + 0) * scale, Vector3.Up, new Vector2((float)(x + 0) / (float)Width * tile, (float)(y + 0) / (float)Height * tile));
                    V[(y * (6 * Width)) + (x * 6) + 1] = new VertexPositionNormalTexture(new Vector3(x + 1, Heights[x + 1, y + 0], y + 0) * scale, Vector3.Up, new Vector2((float)(x + 1) / (float)Width * tile, (float)(y + 0) / (float)Height * tile));
                    V[(y * (6 * Width)) + (x * 6) + 2] = new VertexPositionNormalTexture(new Vector3(x + 1, Heights[x + 1, y + 1], y + 1) * scale, Vector3.Up, new Vector2((float)(x + 1) / (float)Width * tile, (float)(y + 1) / (float)Height * tile));
                    V[(y * (6 * Width)) + (x * 6) + 3] = new VertexPositionNormalTexture(new Vector3(x + 0, Heights[x + 0, y + 0], y + 0) * scale, Vector3.Up, new Vector2((float)(x + 0) / (float)Width * tile, (float)(y + 0) / (float)Height * tile));
                    V[(y * (6 * Width)) + (x * 6) + 4] = new VertexPositionNormalTexture(new Vector3(x + 1, Heights[x + 1, y + 1], y + 1) * scale, Vector3.Up, new Vector2((float)(x + 1) / (float)Width * tile, (float)(y + 1) / (float)Height * tile));
                    V[(y * (6 * Width)) + (x * 6) + 5] = new VertexPositionNormalTexture(new Vector3(x + 0, Heights[x + 0, y + 1], y + 1) * scale, Vector3.Up, new Vector2((float)(x + 0) / (float)Width * tile, (float)(y + 1) / (float)Height * tile));
                }
            }
            List<Vector3> points = new List<Vector3>();
            for (int i = 6; i < V.Length; i++)
            {
                points.Add(V[i].Position);
                if (i > (Width*6)-1)
                {
                    V[i].Normal = Vector3.Cross(Vector3.Normalize(V[i].Position - V[i - 6].Position), -(V[i].Position - V[i - (Width*6)].Position));
                }
                else
                {
                    V[i].Normal = Vector3.Cross(Vector3.Normalize(V[i].Position - V[i - 6].Position), Vector3.Forward);
                }
            }
            LocalBoundingBox = BoundingBox.CreateFromPoints(points);
        }

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
                    //...
                    e.CurrentTechnique.Passes[0].Apply();

                    if (V != null)
                    {
                        Engine.g.DrawUserPrimitives(PrimitiveType.TriangleList, V, 0, 2 * Width * Height);
                    }
                }
            }
            base.Draw(e);
        }
    }
}
