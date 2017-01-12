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
    public class SpotLight
    {
        public Vector3 Light_Position
        {
            get { return LightPosition; }
            set { LightPosition = value; }
        }
        public Vector3 LightPosition;

        public Vector3 Light_Direction
        {
            get { return LightDirection; }
            set { LightDirection = value; }
        }
        public Vector3 LightDirection;

        public Vector3 Ambient_LightColor
        {
            get { return AmbientLightColor; }
            set { AmbientLightColor = value; }
        }
        public Vector3 AmbientLightColor;

        public float ConeAngle { get; set; }

        public Vector3 Light_Color
        {
            get { return LightColor; }
            set { LightColor = value; }
        }
        public Vector3 LightColor;

        public float LightFalloff { get; set; }

        public RenderTarget2D lighttarget;
        public RenderTarget2D shadowmap;
        public RenderTarget2D alphamap;

        public float shadowoffset { get; set; }

        public float FarPlane = 60.0f;
        public float NearPlane = 2.0f;

        public Matrix Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 2.0f, 1.0f, 2.0f, 50.0f);
        /// <summary>
        /// Create a new SpotLight
        /// </summary>
        /// <param name="Ambientcolor">Ambient light value for this spotlight - Note that only one light, Spot or Directional should be given a nonzero ambient value</param>
        /// <param name="Lightposition"></param>
        /// <param name="Lightdirection"></param>
        /// <param name="Coneangle"></param>
        /// <param name="Lightcolor"></param>
        /// <param name="Lightfalloff"></param>
        /// <param name="g"></param>
        public SpotLight(Vector3 Ambientcolor, Vector3 Lightposition, Vector3 Lightdirection, float Coneangle, Vector3 Lightcolor, float Lightfalloff)
        {
            GraphicsDevice g = GameEngineData.game.GraphicsDevice;
            AmbientLightColor = Ambientcolor;
            LightPosition = Lightposition;
            LightDirection = Lightdirection;
            ConeAngle = Coneangle;
            LightColor = Lightcolor;
            LightFalloff = Lightfalloff;
            lighttarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            shadowmap = new RenderTarget2D(g, 4096/2, 4096/2, true, SurfaceFormat.Single, DepthFormat.Depth24);
            alphamap = new RenderTarget2D(g, 4096/2, 4096/2, true, SurfaceFormat.Color, DepthFormat.Depth24);
            shadowoffset = 0.006f;
        }
        /// <summary>
        /// Creates a new SpotLight with default settings
        /// </summary>
        /// <param name="g"></param>
        public SpotLight()
        {
            GraphicsDevice g = GameEngineData.game.GraphicsDevice;
            lighttarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            shadowmap = new RenderTarget2D(g, 4096/2, 4096/2, false, SurfaceFormat.Single, DepthFormat.Depth24);
            alphamap = new RenderTarget2D(g, 4096/2, 4096/2, false, SurfaceFormat.Color, DepthFormat.Depth24);
            shadowoffset = 0.0006f; 
            AmbientLightColor = Vector3.Zero;
            LightPosition = Vector3.Up*4;
            LightDirection = Vector3.Down;
            ConeAngle = 0.6f;
            LightColor = Vector3.One;
            LightFalloff = 1.0f;
        }
        public Matrix View()
        {
            return Matrix.CreateLookAt(LightPosition, LightPosition + LightDirection, Vector3.Cross(LightDirection, Vector3.Left));
        }
    }
}
