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
    public class ParticleGenerator : UI_Object
    {
        List<Particle> Particles = new List<Particle>();

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

        VertexPositionNormalTexture[] V;

        public Vector3 Scale
        {
            get; set;
        } = Vector3.One;

        public float Speed
        {
            get; set;
        } = 0.1f;

        public int MaxLife
        {
            get; set;
        } = 100;


        public ParticleGenerator()
        {
            V = new VertexPositionNormalTexture[6];

            V[0] = new VertexPositionNormalTexture(new Vector3(-1, -1, 0), Vector3.Up, Vector2.Zero);
            V[1] = new VertexPositionNormalTexture(new Vector3(1, -1, 0), Vector3.Up, Vector2.UnitX);
            V[2] = new VertexPositionNormalTexture(new Vector3(1, 1, 0), Vector3.Up, Vector2.One);

            V[3] = new VertexPositionNormalTexture(new Vector3(-1, -1, 0), Vector3.Up, Vector2.Zero);
            V[4] = new VertexPositionNormalTexture(new Vector3(1, 1, 0), Vector3.Up, Vector2.One);
            V[5] = new VertexPositionNormalTexture(new Vector3(-1, 1, 0), Vector3.Up, Vector2.UnitY);
            
            Material = new Material();

            Material.AlphaEnabled = true;

            
        }
        Random r = new Random();
        public void Init()
        {
            
            for (int i = 0; i < 1000; i++)
            {
                Particles.Add(new Particle());
                Particles[i].Position = this.AbsolutePosition;
                Particles[i].Alpha = 1.0f;
                Particles[i].Scale = Vector3.One;
                Particles[i].Velocity = new Vector3(RandomFloat(), RandomFloat(), RandomFloat());
                Particles[i].Velocity.Normalize();
                Particles[i].Velocity *= Speed;
                Particles[i].LifeTime = MaxLife + r.Next(-MaxLife/2, MaxLife/2);
                Particles[i].MaxLife = Particles[i].LifeTime;
            }
        }

        public float RandomFloat()
        {
            return (float)(r.Next(-100000, 100000)) / 100000.0f;
        }

        public override void Update()
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].LifeTime--;
                Particles[i].Position += Particles[i].Velocity;
                Particles[i].Alpha = (float)Particles[i].LifeTime / (float)Particles[i].MaxLife;
                if (Particles[i].LifeTime <= 0)
                {
                    Particles.RemoveAt(i);
                    Particles.Add(new Particle());
                    Particles[i].Position = this.AbsolutePosition;
                    Particles[i].Alpha = 1.0f;
                    Particles[i].Scale = Vector3.One;
                    Particles[i].Velocity = new Vector3(RandomFloat(), RandomFloat(), RandomFloat());
                    Particles[i].Velocity.Normalize();
                    Particles[i].Velocity *= Speed;
                    Particles[i].LifeTime = MaxLife + r.Next(-MaxLife / 2, MaxLife / 2);
                    Particles[i].MaxLife = Particles[i].LifeTime;
                    i--;
                }

            }

            base.Update();

        }

        public override void Draw(Effect e)
        {
            if (Material.Visible)
            {
                if (e != null)
                {
                    if ((string)e.Tag == "NDT" && Material.AlphaEnabled == true)
                    {
                        base.Draw(e);
                        return;
                    }

                    if ((string)e.Tag == "ALPHANDT" && Material.AlphaEnabled == false)
                    {
                        base.Draw(e);
                        return;
                    }
                    Particles.Sort(new ParticleSorter());
                    for (int i = 0; i < Particles.Count; i++)
                    {
                        e.Parameters["World"].SetValue(Matrix.CreateScale(Scale * Particles[i].Scale) * Matrix.CreateBillboard(Particles[i].Position, Camera.Position, Camera.Up, null));
                        e.Parameters["View"].SetValue(Camera.View);
                        e.Parameters["Projection"].SetValue(Camera.Projection);
                        e.Parameters["BasicTexture"].SetValue(Material.Texture);
                        e.Parameters["TextureEnabled"].SetValue(Material._TextureEnabled);
                        e.Parameters["NormalMap"].SetValue(Material.NormalMap);
                        e.Parameters["NormalMapEnabled"].SetValue(Material.NormalMapEnabled);
                        e.Parameters["DiffuseColor"].SetValue(Material.DiffuseColor);
                        e.Parameters["Alpha"].SetValue(Material.Alpha * Particles[i].Alpha);
                        e.Parameters["AlphaEnabled"].SetValue(Material.AlphaEnabled);

                        //...
                        e.CurrentTechnique.Passes[0].Apply();
                        e.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                        Engine.g.DrawUserPrimitives(PrimitiveType.TriangleList, V, 0, 2);
                    }
                }
            }
        }
    }

    public class ParticleSorter : IComparer<Particle>
    {
        public int Compare(Particle x, Particle y)
        {
            return (Vector3.Distance(x.Position, Camera.Position) < Vector3.Distance(y.Position, Camera.Position)) ? 1 : 0;
        }
    }

    public class Particle
    {
        public Particle()
        {
           
        }
        
        public Vector3 Scale;
        public Vector3 Position;
        public float Alpha;

        public Vector3 Velocity;

        public int LifeTime;
        public int MaxLife;

        /*
        
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
        }*/
    }
}


