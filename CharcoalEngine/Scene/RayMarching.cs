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
using CharcoalEngine.Object;

namespace CharcoalEngine.Scene
{
    class RayMarching : DrawingSystem
    {
        Effect effect;
        VertexPositionColor[] V;

        public RayMarching()
        {
            effect = Engine.Content.Load<Effect>("Effects/RayMarching");
            V = new VertexPositionColor[6];

            Random r = new Random();

            V[0] = new VertexPositionColor(new Vector3(-1, -1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[1] = new VertexPositionColor(new Vector3(-1, 1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[2] = new VertexPositionColor(new Vector3(1, 1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[3] = new VertexPositionColor(new Vector3(-1, -1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[4] = new VertexPositionColor(new Vector3(1, 1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[5] = new VertexPositionColor(new Vector3(1, -1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            
        }

        public override void Draw()
        {
            effect.Parameters["w"].SetValue((float)Camera.Viewport.Width);
            effect.Parameters["h"].SetValue((float)Camera.Viewport.Height);
            effect.Parameters["Position"].SetValue(Vector3.Zero);

            Matrix[] worlds = new Matrix[15];
            float[] radii = new float[15];

            for (int i = 0; i < 15; i++)
            {
                worlds[i] = Matrix.Identity;
                radii[i] = 0.0f;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                worlds[i] = Items[i].AbsoluteWorld;
                radii[i] = ((Object.Sphere)Items[i]).Radius;
            }

            effect.Parameters["WorldArray"].SetValue(worlds);
            effect.Parameters["ViewProjection"].SetValue(Camera.View * Camera.Projection);
            effect.Parameters["InverseViewProjection"].SetValue(Matrix.Invert(Camera.View * Camera.Projection));
            effect.Parameters["NearClip"].SetValue(Camera.Viewport.MinDepth);
            effect.Parameters["FarClip"].SetValue(Camera.Viewport.MaxDepth);
            effect.Parameters["CameraPosition"].SetValue(Camera.Position);
            effect.Parameters["RadiusArray"].SetValue(radii);
            effect.Parameters["ActiveEntities"].SetValue(Items.Count);
            effect.CurrentTechnique.Passes[0].Apply();

            Engine.g.DrawUserPrimitives(PrimitiveType.TriangleList, V, 0, 2);

            base.Draw();
        }
    }
}
