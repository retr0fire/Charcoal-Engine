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
using CharcoalEngine.Object;

namespace CharcoalEngine.Scene
{
    public class PhysicsManager
    {
        public List<PhysicsObject> Objects = new List<PhysicsObject>();

        public void UpdatePhysics()
        {
            /*if (Engine.PhysicsEnabled)
            {
                int index_1 = 0;
                foreach (PhysicsObject p1 in Objects)
                {
                    int index_2 = 0;
                    foreach (PhysicsObject p2 in Objects)
                    {                        
                        if (index_1 != index_2)
                        {
                            //Console.WriteLine("1: " + index_1 + " 2: " + index_2);
                            if (p1.AbsoluteBoundingBox.Intersects(p2.AbsoluteBoundingBox) ||
                                p2.AbsoluteBoundingBox.Intersects(p1.AbsoluteBoundingBox))
                            {
                                //Console.WriteLine("collision");
                                PhysicsObject.Collide(p1, p2);

                                if (!p2.IsStatic) p2.Position += p2.Velocity / 40;
                                if (!p1.IsStatic) p1.Position += p1.Velocity / 40;
                            }
                        }
                        index_2++;
                    }
                    index_1++;
                }
            }*/
        }
    }
}
