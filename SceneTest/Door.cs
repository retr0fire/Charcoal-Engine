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
    class Door : CharcoalEngine.Object.UI_Object
    {
        bool opening = false;
        bool closing = false;

        AnimationRig rig;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        public Door(Scene scene)
        {
            rig = new AnimationRig();
            Children.Add(rig);
            OBJ_File obj = new OBJ_File();
            Name = "Door";
            obj.Load("C:\\Users\\Michael\\Documents\\XNA\\Hunt-Or-Gatherers\\Models\\Objects\\door.obj", Engine.g, new Vector3(0, 0, 0), Vector3.Zero, 1f, false, false, rig);
            rig.Children[0].Center = -rig.Children[0].Position;
            rig.Children.Add(new Transform() { Position = rig.Children[0].Position });
            rig.Children.Add(new Transform()
            {
                Position = rig.Children[0].Position,
                YawPitchRoll = new Vector3(MathHelper.ToRadians(90), 0, 0)
            }
            );
            rig.Update_Rig = false;
        }

        public void Open()
        {
            if (closing)
                return;
            opening = true;
            rig.Update_Rig = true;
        }
        public void Close()
        {
            if (opening)
                return;
            closing = true;
            rig.Update_Rig = true;
        }

        public override void Update()
        {
            if (opening)
            {
                if (rig.Joint == 1)
                {
                    rig.Update_Rig = false;
                    opening = false;
                }
            }
            if (closing)
            {
                if (rig.Joint == 0)
                {
                    rig.Update_Rig = false;
                    closing = false;
                }
            }
            base.Update();
        }
    }
}
