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

namespace MJLGameEngine.Object.ParticleGenerator
{
    public class Particle
    {
        public Vector3 position;
        public Matrix world;
        public int life;
        public int maxlife;
        public Texture2D particletex;
        public Model particlemodel;
        public Vector3 spos;
        public float size;
        public Random r;
        public Vector3 wind_dir;
        public float wind_speed;
        public GraphicsDevice g;
        public float p_scale = 1;
        public float alpha;
        public Vector3 color;
        public float deflation = 100;

        public Particle(Vector3 spawnpos, int mlife, Texture2D ptex, Model pmodel, Vector3 winddir, float winds, GraphicsDevice gr, float Particle_scale, Vector3 colour, float D)
        {
            deflation = D;
            p_scale = Particle_scale;
            r = new Random();
            position = spawnpos + (new Vector3((float)r.Next(-200, 200) / 1000, (float)r.Next(-200, 200) / 1000, (float)r.Next(-200, 200) / 1000))*p_scale;
            maxlife = mlife + r.Next(-mlife/2, mlife/2);
            particletex = ptex;
            particlemodel = pmodel;
            spos = spawnpos;
            size = ((float)r.Next(1, 50)/10)*p_scale;

            wind_dir = winddir + new Vector3((float)r.Next(-250, 250) / 1000, (float)r.Next(-250, 250) / 1000, (float)r.Next(-250, 250) / 1000);
            wind_speed = winds;
            if (wind_speed != 0)
                wind_speed += r.Next(-1000, 1000)/100000;

            g = GameEngineData.game.GraphicsDevice;
            float x = (float)r.Next(700, 1300) / 1000;
            color = colour*=x;
        }
        public void update(Vector3 cpos, Vector3 camup)
        {
            life++;
            size -= size/deflation;
            position += wind_dir * wind_speed;
            world = Matrix.CreateScale(size) * Matrix.CreateBillboard(new Vector3(position.X, position.Y, position.Z) + Vector3.Normalize(position-spos)*((float)life/200), cpos, Vector3.Up, null);
            alpha = 0.4f - (float)((float)life / (float)maxlife);
        }
        public Matrix GetWorld(Vector3 cpos, Vector3 camup)
        {
            return Matrix.CreateScale(size) * Matrix.CreateBillboard(new Vector3(position.X, position.Y, position.Z) + Vector3.Normalize(position - spos) * ((float)life / 200), cpos, Vector3.Up, null);
        }
        public void draw(Matrix view, Matrix projection)
        {
            DrawModel(particlemodel, world, view, projection);
        }
        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world * mesh.ParentBone.Transform;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.Texture = particletex;
                    effect.TextureEnabled = true;
                    effect.Alpha = 0.8f - (float)((float)life / (float)maxlife);
                }
                mesh.Draw();
            }
        }
    }
}
