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
using Jitter.LinearMath;

namespace SceneTest.Objects
{
    //manages a 50x50x50 chunk of cubes
    public class CubeSection : Transform
    {
        CubeType[,,] Cubes;// = new CubeType[50, 50, 50];
        int Size;
        VertexPositionNormalTexture[] V;

        Material Material;

        Octree CollisionOctree;

        public CubeSection(CubeType[,,] cubes, int size, Vector3 Position)
        {
            this.Position += Position;
            Cubes = cubes;
            Size = size;
            LocalBoundingBox = new BoundingBox(Vector3.Zero-Vector3.One/2, Vector3.One * Size + Vector3.One/2);
            UpdateBoundingBox();
            Update_Cubes();

            //set up the collision octree for this section
            CollisionOctree = new Octree(Vector3.Zero, new BoundingBox(Vector3.Zero, Vector3.One*Size), 4);
            
            Material = new CharcoalEngine.Object.Material();
            Material.Alpha = 1;
            Material.AlphaEnabled = false;
            Material.Ambient = Color.Black.ToVector3();
            //Material.DiffuseColor = Color.Black.ToVector3();
            Material.name = "CubeMat";
            Material._Texture = "C:\\Users\\Michael\\Documents\\XNA\\Hunt-Or-Gatherers\\Models\\Objects\\Cube\\Carpet_Plush_Charcoal.jpg";
            Material.Visible = true;
        }
        
        List<VertexPositionNormalTexture> VList = new List<VertexPositionNormalTexture>();
            
        public void Update_Cubes()
        {
            VList.Clear();

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    for(int z = 0; z < Size; z++)
                    {
                        if (Cubes[x,y,z] != CubeType.Nothing)
                        {
                            if (CubeVisible(x, y, z))
                                VList.AddRange(AddCube(new Vector3(x, y, z)).ToList());
                        }
                    }
                }
            }

            V = VList.ToArray();
            VList.Clear();
        }

        public void Remove_Block(int x, int y, int z)
        {
            Cubes[x, y, z] = CubeType.Nothing;
            Update_Cubes();
        }

        public bool CubeVisible(int x, int y, int z)
        {
            if (x == 0 || y == 0 || z == 0)
                return true;

            if (x == Size-1 || y == Size - 1 || z == Size - 1)
                return true;

            if (Cubes[x - 1, y, z] != CubeType.Nothing)
            {
                if (Cubes[x + 1, y, z] != CubeType.Nothing)
                {
                    if (Cubes[x, y - 1, z] != CubeType.Nothing)
                    {
                        if (Cubes[x, y + 1, z] != CubeType.Nothing)
                        {
                            if (Cubes[x, y, z - 1] != CubeType.Nothing)
                            {
                                if (Cubes[x, y, z + 1] != CubeType.Nothing)
                                {
                                    //Console.WriteLine("skipped");
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        VertexPositionNormalTexture[] AddCube(Vector3 Position)
        {
            VertexPositionNormalTexture[] cube = new VertexPositionNormalTexture[6 * 6];
            AddFace(Position, Vector3.Zero).CopyTo(cube, 0);
            AddFace(Position, new Vector3(MathHelper.ToRadians(90), 0, 0)).CopyTo(cube, 6);
            AddFace(Position, new Vector3(MathHelper.ToRadians(180), 0, 0)).CopyTo(cube, 12);
            AddFace(Position, new Vector3(MathHelper.ToRadians(270), 0, 0)).CopyTo(cube, 18);
            AddFace(Position, new Vector3(0, MathHelper.ToRadians(90), 0)).CopyTo(cube, 24);
            AddFace(Position, new Vector3(0, MathHelper.ToRadians(-90), 0)).CopyTo(cube, 30);

            return cube;
        }

        VertexPositionNormalTexture[] AddFace(Vector3 Position, Vector3 YawPitchRoll)
        {
            VertexPositionNormalTexture[] Face = new VertexPositionNormalTexture[6];
            Vector3[] Positions = new Vector3[6];

            Positions[0] = Vector3.Transform(new Vector3(0, 0, 0) - Vector3.One / 2, Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z)) + Position;
            Positions[1] = Vector3.Transform(new Vector3(1, 0, 0) - Vector3.One / 2, Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z)) + Position;
            Positions[2] = Vector3.Transform(new Vector3(1, 1, 0) - Vector3.One / 2, Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z)) + Position;
            Positions[3] = Vector3.Transform(new Vector3(1, 1, 0) - Vector3.One / 2, Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z)) + Position;
            Positions[4] = Vector3.Transform(new Vector3(0, 1, 0) - Vector3.One / 2, Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z)) + Position;
            Positions[5] = Vector3.Transform(new Vector3(0, 0, 0) - Vector3.One / 2, Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z)) + Position;

            Vector3 Normal = Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z));

            Face[0] = new VertexPositionNormalTexture(Positions[0], Normal, new Vector2(0, 0));
            Face[1] = new VertexPositionNormalTexture(Positions[1], Normal, new Vector2(1, 0));
            Face[2] = new VertexPositionNormalTexture(Positions[2], Normal, new Vector2(1, 1));
            Face[3] = new VertexPositionNormalTexture(Positions[3], Normal, new Vector2(1, 1));
            Face[4] = new VertexPositionNormalTexture(Positions[4], Normal, new Vector2(0, 1));
            Face[5] = new VertexPositionNormalTexture(Positions[5], Normal, new Vector2(0, 0));

            return Face;
        }

        public override void Draw(Effect e)
        {
            CollisionOctree.Draw(AbsoluteWorld);

            if (e != null)
            {
                if ((string)e.Tag == "NDT" && Material.AlphaEnabled == true)
                {
                    base.Draw(e);
                    return;
                }

                if ((string)e.Tag == "ALPHANDT"/* && Material.AlphaEnabled == false*/)
                {
                    base.Draw(e);
                    return;
                }

                

                e.Parameters["World"].SetValue(AbsoluteWorld);
                e.Parameters["View"].SetValue(Camera.View);
                e.Parameters["Projection"].SetValue(Camera.Projection);
                e.Parameters["BasicTexture"].SetValue(Material.Texture);
                e.Parameters["TextureEnabled"].SetValue(Material._TextureEnabled);
                //e.Parameters["BumpMap"].SetValue(Material.BumpMap);
                //e.Parameters["BumpMapEnabled"].SetValue(Material.BumpMapEnabled);
                e.Parameters["DiffuseColor"].SetValue(Material.DiffuseColor);
                e.Parameters["Alpha"].SetValue(Material.Alpha);
                e.Parameters["AlphaEnabled"].SetValue(Material.AlphaEnabled);
                //...
                e.CurrentTechnique.Passes[0].Apply();

                //e.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                
                Engine.g.DrawUserPrimitives(PrimitiveType.TriangleList, V, 0, V.Length/3);
            }
            base.Draw(e);
        }
    }
    public class Octree
    {
        public Octree Parent;//position within parent node/octree
        public Vector3 Position;
        public BoundingBox boundingBox; //translated. Seriously.
        public Octree[] Octrees = new Octree[8];

        public bool IsEnd = false;

        Vector3 AbsolutePosition
        {
            get
            {
                if (Parent != null)
                    return Parent.AbsolutePosition + Position;
                else
                    return Position;
            }
        }

        public Octree(Vector3 _Position, BoundingBox _boundingBox, int levels)
        {
            Position = _Position;
            boundingBox = _boundingBox;
            if (levels > 0)
            {
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 2; z++)
                        {
                            Vector3 Pos = _boundingBox.Min - _Position;
                            Vector3 Min = _boundingBox.Min - _Position;
                            Vector3 Max = (_boundingBox.Max - _Position) / 2;

                            Vector3 Offset = new Vector3(x, y, z)*Max;

                            Octrees[x*4+y*2+z] = new Octree(_Position + Pos + Offset, new BoundingBox(_Position + Min + Offset, _Position + Max + Offset), levels - 1);
                        }
                    }
                }
            }
            else
                IsEnd = true;
        }

        public void UpdateOctree()
        {
            if (IsEnd)
                return;
            for (int i = 0; i < 8; i++)
            {
                Octrees[i].Parent = this;
                Octrees[i].UpdateOctree();
            }
        }

        public void Draw(Matrix ParentWorld)
        {
            LineUtility3D.DrawAABoundingBox(Engine.g, Camera.View, Camera.Projection, boundingBox, ParentWorld);
            if (IsEnd == false)
            {
                for (int i = 0; i < 8; i++)
                {
                    Octrees[i].Draw(ParentWorld);
                }
            }
        }
    }
    public enum CubeType
    {
        Nothing,
        Dirt,
        Stone
    }
}
