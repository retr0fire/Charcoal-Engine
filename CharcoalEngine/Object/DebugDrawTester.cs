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
    class DebugDrawTester : Transform
    {
        Effect effect;
        VertexPositionColor[] V;

        Vector2[] Gradients = new Vector2[2*2];

        public DebugDrawTester()
        {
            effect = Engine.Content.Load<Effect>("Effects/TextureEffect");
            V = new VertexPositionColor[6];

            Random r = new Random();

            V[0] = new VertexPositionColor(new Vector3(-1, -1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[1] = new VertexPositionColor(new Vector3(-1, 1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[2] = new VertexPositionColor(new Vector3(1, 1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[3] = new VertexPositionColor(new Vector3(-1, -1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[4] = new VertexPositionColor(new Vector3(1, 1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));
            V[5] = new VertexPositionColor(new Vector3(1, -1, 0.0f), new Color(1.0f, 1.0f, 1.0f, 0));

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Gradients[i + j*2] = new Vector2((float)r.NextDouble() * 2 - 1.0f, (float)r.NextDouble() * 2 - 1.0f);
                    Gradients[i + j*2].Normalize();
                }
            }

        }

        public override void Draw()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Gradients[i + j * 2] = Vector2.Transform(Gradients[i + j * 2], Matrix.CreateRotationZ(0.01f));
                }
            }

            effect.Parameters["w"].SetValue((float)Camera.Viewport.Width);
            effect.Parameters["h"].SetValue((float)Camera.Viewport.Height);
            effect.Parameters["position"].SetValue(Vector3.Zero);
            effect.Parameters["WorldViewProjection"].SetValue(AbsoluteWorld * Camera.View * Camera.Projection);
            effect.Parameters["gradients"].SetValue(Gradients);
            //effect.Parameters["WVPInverse"].SetValue(Matrix.Invert(AbsoluteWorld * Camera.View * Camera.Projection));
            effect.CurrentTechnique.Passes[0].Apply();

            Engine.g.DrawUserPrimitives(PrimitiveType.TriangleList, V, 0, 2);
                                                        
            base.Draw();                                
        }                                               
    }
}
