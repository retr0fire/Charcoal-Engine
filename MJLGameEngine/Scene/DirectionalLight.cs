using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MJLGameEngine.Scene
{
    public class DirectionalLight
    {
        public Vector3 Ambient_LightColor
        {
            get { return AmbientLightColor; }
            set { AmbientLightColor = value; }
        }
        public Vector3 AmbientLightColor;
        public Vector3 Light_Position
        {
            get { return LightPosition; }
            set { LightPosition = value; }
        }
        public Vector3 LightPosition;
        public Vector3 Light_Color
        {
            get { return LightColor; }
            set { LightColor = value; }
        }
        public Vector3 LightColor;

        public float LightAttenuation { get; set; }
        public float LightFalloff  { get; set; }

        public RenderTarget2D lighttarget;
        public RenderTarget2D shadowmap;
        public DirectionalLight(Vector3 Ambientcolor, Vector3 Lightposition, Vector3 Lightcolor, float Lightatt, float Lightfalloff)
        {
            GraphicsDevice g = GameEngineData.game.GraphicsDevice;
            AmbientLightColor = Ambientcolor;
            LightPosition = Lightposition;
            LightColor = Lightcolor;
            LightAttenuation = Lightatt;
            LightFalloff = Lightfalloff;
            lighttarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            shadowmap   = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
        }
        public DirectionalLight()
        {
            GraphicsDevice g = GameEngineData.game.GraphicsDevice;
            AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);
            LightPosition = new Vector3(0, 8, 0);
            LightColor = new Vector3(1, 1, 1);
            LightAttenuation = 60;
            LightFalloff = 0.2f;
            lighttarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            shadowmap   = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
        }
    }
}
