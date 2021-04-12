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
using CharcoalEngine.Utilities.MapGeneration;

namespace CharcoalEngine.Object
{
    class DebugDrawTester : Transform
    {
        public new string Name = "3DLineTester";

        float[,,] potential_field;
        Vector3[,,] gradient_field;
        int w = 8;

        public DebugDrawTester()
        {
            potential_field = _3DPerlinMap.Create_3D_Perlin_Map_W_Octaves(w, new Random(DateTime.UtcNow.Second), 3);
            gradient_field = new Vector3[w, w, w];
        }

        public override void Draw()
        {
            if (DebugDraw)
            {
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < w; y++)
                    {
                        for (int z = 0; z < w; z++)
                        {
                            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.White, AbsolutePosition + new Vector3(x, y, z), AbsolutePosition + new Vector3(x, y, z) + potential_field[x, y, z] * Vector3.Up / 10);
                            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.White, AbsolutePosition + new Vector3(x, y, z), AbsolutePosition + new Vector3(x, y, z) + potential_field[x, y, z] * Vector3.Right / 10);
                            LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.White, AbsolutePosition + new Vector3(x, y, z), AbsolutePosition + new Vector3(x, y, z) + potential_field[x, y, z] * Vector3.Forward / 10);
                        }
                    }
                }
            }

            base.Draw();
        }
    }
}
