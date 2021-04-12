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
using System.Reflection;
using System.Diagnostics;
using System.IO;
using CharcoalEngine.Object;
using CharcoalEngine.Editing;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;

namespace CharcoalEngine.Scene
{
    public class DrawingSystem
    {
        //This is the only guaranteed global list
        public List<Transform> Items = new List<Transform>();

        public DrawingSystem()
        {

        }

        public virtual void RegisterItem(Transform t)
        {
            Items.Add(t);
        }

        public virtual void Draw()
        {

        }
    }
}
