using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CharcoalEngine.Object.ParticleGenerator
{
    public class Particle_Generator
    {
        public Vector3 _Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector3 position;

        public int life { get; set; }
        public int maxlife { get; set; }
        public Texture2D particletex;
        public Model particlemodel;

        public Vector3 _wind_dir
        {
            get { return wind_dir; }
            set { wind_dir = value; }
        }
        public Vector3 wind_dir;

        public float wind_speed { get; set; }
        public float scale { get; set; }
        public List<Particle> Particles = new List<Particle>();
        public bool auto_add { get; set; }
        Effect BTEffect;

        public Vector3 _color
        {
            get { return color; }
            set { color = value; }
        }
        Vector3 color;

        public float _deflation
        {
            get { return deflation; }
            set { deflation = value; }
        }
        float deflation;

        GraphicsDevice g;

        public Particle_Generator(Vector3 spawnpos, int mlife, Texture2D ptex, Model pmodel, Vector3 forcedir, float forcepower, bool should_auto_add, Vector3 colour, float D)
        {
            scale = 0.6f;
            deflation = D;
            g = GameEngineData.game.GraphicsDevice;
            position = spawnpos;
            life = mlife;
            particletex = ptex;
            particlemodel = pmodel;
            wind_dir = forcedir;
            wind_speed = forcepower;
            auto_add = should_auto_add;
            BTEffect = Utilities.EngineContent.Content.Load<Effect>("Effects/BasicTextureEffect");
            color = colour;
        }

        public void Update(GameTime Gametime, Vector3 Camera_Position, Quaternion Camera_Rotation)
        {
            if (auto_add==true)
                Particles.Add(new Particle(position, life, particletex, particlemodel, wind_dir, wind_speed, g, scale, color, deflation));

            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].update(Camera_Position, Vector3.Cross(Vector3.Transform(Vector3.Forward, Matrix.CreateFromQuaternion(Camera_Rotation)), Vector3.Transform(Vector3.Left, Matrix.CreateFromQuaternion(Camera_Rotation))));

                if (Particles[i].life == Particles[i].maxlife)
                {
                    Particles.Remove(Particles[i]);

                }
            }
        }
        public void Draw(Matrix View, Matrix Projection)
        {
            g.BlendState = BlendState.AlphaBlend;
            g.DepthStencilState = DepthStencilState.DepthRead;

            for (int p = 0; p < Particles.Count; p++ )
            {
                for (int mesh = 0; mesh < Particles[p].particlemodel.Meshes.Count; mesh++)
                {
                    foreach (ModelMeshPart part in Particles[p].particlemodel.Meshes[mesh].MeshParts)
                    {
                        part.Effect = BTEffect;
                        BTEffect.Parameters["World"].SetValue(Particles[p].world);
                        BTEffect.Parameters["View"].SetValue(View);
                        BTEffect.Parameters["Projection"].SetValue(Projection);
                        BTEffect.Parameters["BasicTexture"].SetValue(Particles[p].particletex);
                        BTEffect.Parameters["Alpha"].SetValue(1 - (float)((float)Particles[p].life / (float)Particles[p].maxlife));
                    }
                    Particles[p].particlemodel.Meshes[mesh].Draw();
                }
            }
        }

        public void Add()
        {
            Particles.Add(new Particle(position, life, particletex, particlemodel, wind_dir, wind_speed, g, scale, color, deflation));
        }
    }
}
