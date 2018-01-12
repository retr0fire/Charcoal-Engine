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

namespace CharcoalEngine.Object
{
    public class PointLight : UI_Object
    {
        public Vector3 AmbientLightColor
        {
            get
            {
                return __ambientlightcolor__;
            }
            set
            {
                __ambientlightcolor__ = value;
            }
        }
        Vector3 __ambientlightcolor__;


        public float Attenuation
        {
            get
            {
                return __attenuation__;
            }
            set
            {
                __attenuation__ = value;
            }
        }
        float __attenuation__ = 500;

        public Vector3 LightColor
        {
            get
            {
                return __lightcolor__;
            }
            set
            {
                __lightcolor__ = value;
            }
        }
        Vector3 __lightcolor__ = Color.White.ToVector3();
        public float Falloff = 1;

        public PointLight()
        {
            Name = "PointLight";
            Position = new Vector3(0, 10, 0);
        }

        public override void Draw(Effect e)
        {
            //register as a light in the light list
            if (Engine.ActiveScene != null)
                Engine.ActiveScene.Lights.Add(this);

            base.Draw(e);
        }
    }
}
