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
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows;

namespace SceneTest
{
    class Torch : CharcoalEngine.Object.UI_Object
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        public Torch(Scene scene)
        {
            Children.Add(new Transform());
            OBJ_File obj = new OBJ_File();
            Name = "Torch";
            obj.Load("C:\\Users\\Michael\\Documents\\XNA\\3D Models\\MedievalBarrel (1)\\MedievalBarrel\\MedievalBarrel_OBJ.obj", Engine.g, new Vector3(0, 0, 0), Vector3.Zero, 1f, false, false, Children[0]);
            
        }

        public override void Draw(Effect e)
        {
            if (e != null && (string)e.Tag == "NDT")
            {
                Effect effect = Engine.Content.Load<Effect>("NDT_Effect");
                VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[6];
                effect.Parameters["World"].SetValue(Matrix.CreateScale(10) * AbsoluteWorld);
                effect.Parameters["View"].SetValue(Camera.View);
                effect.Parameters["Projection"].SetValue(Camera.Projection);
                effect.Parameters["FarPlane"].SetValue(Camera.Viewport.MaxDepth);

                effect.CurrentTechnique.Passes[0].Apply();
                vertices[0] = new VertexPositionNormalTexture(Vector3.Zero, Vector3.Forward, Vector2.Zero);
                vertices[1] = new VertexPositionNormalTexture(Vector3.Right, Vector3.Forward, Vector2.UnitX);
                vertices[2] = new VertexPositionNormalTexture(Vector3.Right + Vector3.Up, Vector3.Forward, Vector2.One);

                vertices[4] = new VertexPositionNormalTexture(Vector3.Right + Vector3.Up, Vector3.Forward, Vector2.One);
                vertices[3] = new VertexPositionNormalTexture(Vector3.Zero, Vector3.Forward, Vector2.Zero);
                vertices[5] = new VertexPositionNormalTexture(Vector3.Up, Vector3.Forward, Vector2.UnitY);

                Engine.g.BlendState = BlendState.AlphaBlend;
                Engine.g.SamplerStates[0] = SamplerState.PointWrap;
                Engine.g.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 2);
            }
            base.Draw(e);
        }
    }
}
