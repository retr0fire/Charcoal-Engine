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
    class Weapon : CharcoalEngine.Object.UI_Object
    {
        Transform Model;

        public Weapon(string ModelName, string WeaponName)
        {
            /*Model = new Transform();
            Children.Add(Model);
            OBJ_File obj = new OBJ_File();
            Name = WeaponName;
            obj.Load(ModelName, Engine.g, new Vector3(0, 0, 0), Vector3.Zero, 1f, false, false, this);*/

        }

    }
}