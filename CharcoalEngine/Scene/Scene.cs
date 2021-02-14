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
    public class Scene
    {
        public Transform Root = new Transform();
        GraphicsDevice g;
        SpriteBatch spriteBatch;

        RenderTarget2D NormalTarget;
        RenderTarget2D DepthTarget;
        RenderTarget2D TextureTarget;
        RenderTarget2D GeometryTarget;
        Effect NDT_Effect;
        Effect Light;
        Effect TextureEffect;
        RenderTarget2D LightTarget;

        public PhysicsManager physics;

        //test
        GizmoComponent _gizmo;

        public List<PointLight> Lights = new List<PointLight>();//hackish, but works for now

        public Scene()
        {

            foreach (Type t in AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes()))
            {
                if (t.IsSubclassOf(typeof(Transform)))
                    Console.WriteLine(t);
            }


            //Engine.Game.Window.AllowUserResizing = true;
            Engine.Game.IsMouseVisible = true;

            g = Engine.g;
            spriteBatch = new SpriteBatch(g);

            Engine.Game.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

            //load physics
            physics = new PhysicsManager();

            //load effects
            NDT_Effect = Engine.Content.Load<Effect>("Effects/NDT_Effect");
            NDT_Effect.Tag = (object)"NDT";
            Light = Engine.Content.Load<Effect>("Effects/PointLightEffect");
            TextureEffect = Engine.Content.Load<Effect>("Effects/TextureEffect");

            //load rendertargets
            NormalTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            DepthTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Single, DepthFormat.Depth24Stencil8);
            TextureTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            LightTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            GeometryTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            //Root.Children.Add(new PointLight());

            /*
            AnimationRig b = new AnimationRig();
            b.Children.Add(new PointLight());
            Root.Children.Add(b);

            AnimationRig c = new AnimationRig();
            c.Children.Add(new PointLight());
            Root.Children.Add(c);
            */

            _gizmo = new GizmoComponent(g, spriteBatch, Engine.Content.Load<SpriteFont>("Fonts/Font"));
            _gizmo.SetSelectionPool(Root.Children, Root);

            //_gizmo.TranslateEvent += _gizmo_TranslateEvent;
            _gizmo.RotateEvent += _gizmo_RotateEvent;
            //_gizmo.ScaleEvent += _gizmo_ScaleEvent;
            /*AnimationRig a = new AnimationRig();
            a.Children.Add(new PointLight());
            Root.Children.Add(a);*/
            Root.Update();
        }

        private void _gizmo_RotateEvent(Transform transformable, TransformationEventArgs e, TransformationEventArgs d)
        {
            _gizmo.RotationHelper(transformable, e, d);
        }

        /// <summary>
        /// not used yet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            //load rendertargets
            NormalTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            DepthTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Single, DepthFormat.Depth24Stencil8);
            TextureTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            LightTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            GeometryTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            Camera.Viewport.Bounds = g.PresentationParameters.Bounds;
        }

        private KeyboardState _previousKeys;
        private MouseState _previousMouse;
        private MouseState _currentMouse;
        private KeyboardState _currentKeys;

        public void Update(GameTime gameTime)
        {
            Engine.gameTime = gameTime;

            _currentMouse = Mouse.GetState();
            _currentKeys = Keyboard.GetState();
            // select entities with your cursor (add the desired keys for add-to / remove-from -selection)
            if (_currentMouse.RightButton == ButtonState.Pressed && _previousMouse.RightButton == ButtonState.Released)
                _gizmo.SelectEntities(new Vector2(_currentMouse.X, _currentMouse.Y),
                                      _currentKeys.IsKeyDown(Keys.LeftControl) || _currentKeys.IsKeyDown(Keys.RightControl),
                                      _currentKeys.IsKeyDown(Keys.LeftAlt) || _currentKeys.IsKeyDown(Keys.RightAlt));
                
            // set the active mode like translate or rotate
            if (IsNewButtonPress(Keys.D1))
                _gizmo.ActiveMode = GizmoMode.Translate;
            if (IsNewButtonPress(Keys.D2))
                _gizmo.ActiveMode = GizmoMode.Rotate;
            if (IsNewButtonPress(Keys.D3))
                _gizmo.ActiveMode = GizmoMode.NonUniformScale;
            if (IsNewButtonPress(Keys.D4))
                _gizmo.ActiveMode = GizmoMode.UniformScale;
            if (IsNewButtonPress(Keys.D5))
                _gizmo.ActiveMode = GizmoMode.CenterTranslate;

            if (IsNewButtonPress(Keys.Enter))
                _gizmo.LevelDown();
            if (IsNewButtonPress(Keys.Back))
                _gizmo.LevelUp();

            // toggle precision mode
            if (_currentKeys.IsKeyDown(Keys.LeftShift) || _currentKeys.IsKeyDown(Keys.RightShift))
                _gizmo.PrecisionModeEnabled = true;
            else
                _gizmo.PrecisionModeEnabled = false;
            
            // toggle snapping
            if (IsNewButtonPress(Keys.I))
                _gizmo.SnapEnabled = !_gizmo.SnapEnabled;
            

            // clear selection
            if (IsNewButtonPress(Keys.Escape))
                _gizmo.Clear();

            _gizmo.Update(gameTime);

            _previousKeys = _currentKeys;
            _previousMouse = _currentMouse;

            physics.UpdatePhysics();

            foreach (Transform o in Root.Children)
            {
                o.Update();
            }
        }

        private bool IsNewButtonPress(Keys key)
        {
            return _currentKeys.IsKeyDown(key) && _previousKeys.IsKeyUp(key);
        }

        public void Draw()
        {
            #region setup
            g.BlendState = BlendState.AlphaBlend;
            g.DepthStencilState = DepthStencilState.Default;
            g.SamplerStates[0] = SamplerState.LinearWrap;
            Engine.graphics.PreferMultiSampling = true;
            //g.DepthStencilState = DepthStencilState.Default;
            //RasterizerState r = new RasterizerState();
            //r.MultiSampleAntiAlias = true;
            //g.RasterizerState = r;

            #endregion

            //Set the active scene:
            Engine.ActiveScene = this;
            
            g.SetRenderTargets(NormalTarget, DepthTarget, TextureTarget);
            g.BlendState = BlendState.AlphaBlend;
            g.Clear(Color.Transparent);
            NDT_Effect.Parameters["FarPlane"].SetValue(Camera.Viewport.MaxDepth);
            for (int i = 0; i < Root.Children.Count; i++)
            {
                if (Root.Children[i].AbsoluteBoundingBox.Intersects(Camera.Frustum))
                    Root.Children[i].Draw(NDT_Effect);
            }
            NDT_Effect.Tag = (object)"ALPHANDT";

            //g.DepthStencilState = DepthStencilState.None;

            Lights.Clear();

            for (int i = 0; i < Root.Children.Count; i++)
            {
                Root.Children[i].Draw(NDT_Effect);
            }
            NDT_Effect.Tag = (object)"NDT";
            g.SetRenderTargets(null);

            //g.DepthStencilState = DepthStencilState.Default;

            //here we go
            #region setup
            //g.BlendState = BlendState.Opaque;
            g.DepthStencilState = DepthStencilState.Default;
            g.SamplerStates[0] = SamplerState.LinearWrap;
            //g.DepthStencilState = DepthStencilState.Default;
            RasterizerState r2 = new RasterizerState();
            r2 = RasterizerState.CullNone;
            g.RasterizerState = r2;
            #endregion
            g.SetRenderTarget(LightTarget);
            g.Clear(Color.Black);
            
            // Calculate the view * projection matrix  
            Matrix ViewProjection = Camera.View * Camera.Projection;

            Light.Parameters["DepthTexture"].SetValue(DepthTarget);
            Light.Parameters["NormalTexture"].SetValue(NormalTarget);
            Light.Parameters["BasicTexture"].SetValue(TextureTarget);
            Light.Parameters["viewportWidth"].SetValue((float)Camera.Viewport.Width);
            Light.Parameters["viewportHeight"].SetValue((float)Camera.Viewport.Height);
            // Set render states to additive (lights will add their influences)  
            g.BlendState = BlendState.Additive;
            g.DepthStencilState = DepthStencilState.None;
            
            

            foreach (PointLight l in Lights)
            {
                //l.Draw(null);

                Matrix World = (Matrix.CreateScale(l.Attenuation) * Matrix.CreateTranslation(Camera.Position));
                Matrix wvp = World * ViewProjection;
                Light.Parameters["WorldViewProjection"].SetValue(wvp);
                Light.Parameters["ViewProjection"].SetValue(ViewProjection);
                Light.Parameters["inverse_ViewProjection"].SetValue(Matrix.Invert(ViewProjection));
                Light.Parameters["inverse_view"].SetValue(Matrix.Invert(Camera.View));
                Light.Parameters["view"].SetValue(Camera.View);
                Light.Parameters["inverse_world"].SetValue(Matrix.Invert(World));
                Light.Parameters["world"].SetValue(World);
                Light.Parameters["inverse_projection"].SetValue(Matrix.Invert(Camera.Projection));
                Light.Parameters["projection"].SetValue(Camera.Projection);
                Light.Parameters["cam_pos"].SetValue(Camera.Position);
                Light.Parameters["LightColor"].SetValue(l.LightColor);
                Light.Parameters["LightPosition"].SetValue(l.AbsolutePosition);
                Light.Parameters["LightAttenuation"].SetValue(l.Attenuation);
                Light.Parameters["FarPlane"].SetValue(Camera.Viewport.MaxDepth);
                Light.Parameters["TanAspect"].SetValue(new Vector2((float)Math.Tan((double)Camera.FoV / 2) * Camera.Viewport.AspectRatio, -(float)Math.Tan((double)Camera.FoV / 2)));
                Light.Parameters["SpecularPower"].SetValue(l.SpecularPower);
                Light.Parameters["SpecularIntensity"].SetValue(l.SpecularIntensity);

                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, Light);
                Light.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(DepthTarget, NormalTarget.Bounds, Color.White);
                spriteBatch.End();

                /*
                // Calculate the world * view * projection matrix and set it to     
                // the effect    
               Light.Parameters["inverse_view"].SetValue(Matrix.Invert(Camera.View));
                Light.Parameters["view"].SetValue(Camera.View);
                Light.Parameters["inverse_world"].SetValue(Matrix.Invert(World));
                Light.Parameters["world"].SetValue(World);
                Light.Parameters["cam_pos"].SetValue(Camera.Position);
                Light.Parameters["LightColor"].SetValue(l.LightColor);
                Light.Parameters["LightPosition"].SetValue(l.Position);
                Light.Parameters["LightAttenuation"].SetValue(l.Attenuation);
                Light.Parameters["FarPlane"].SetValue(Camera.Viewport.MaxDepth);
                Light.Parameters["TanAspect"].SetValue(new Vector2((float)Math.Tan((double)Camera.FoV/2) * Camera.Viewport.AspectRatio, -(float)Math.Tan((double)Camera.FoV/2)));

                g.RasterizerState = RasterizerState.CullNone;

                LightMesh.Meshes[0].MeshParts[0].Effect = Light;
                LightMesh.Meshes[0].MeshParts[0].Effect.CurrentTechnique.Passes[0].Apply();
                LightMesh.Meshes[0].Draw();*/
                // Revert the cull mode   
                //g.RasterizerState = RasterizerState.CullCounterClockwise;
            }
            g.SetRenderTarget(null);
            g.SetRenderTarget(GeometryTarget);
            //TextureEffect.Parameters["BasicTexture"].SetValue(TextureTarget);
            TextureEffect.Parameters["LightTexture"].SetValue(LightTarget);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, TextureEffect);
            TextureEffect.CurrentTechnique.Passes[0].Apply();
            //spriteBatch.Begin();
            spriteBatch.Draw(LightTarget, LightTarget.Bounds, Color.White);
            spriteBatch.End();

            for (int i = 0; i < Root.Children.Count; i++)
            {
                Root.Children[i].Draw(null);
            }
            
            g.SetRenderTarget(null);

            g.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(GeometryTarget, GeometryTarget.Bounds, Color.White);
            spriteBatch.End();

            #region setup
            g.BlendState = BlendState.Opaque;
            g.DepthStencilState = DepthStencilState.Default;
            g.SamplerStates[0] = SamplerState.LinearWrap;
            //g.DepthStencilState = DepthStencilState.Default;
            #endregion
            _gizmo.Draw();
           /* #region setup
            g.BlendState = BlendState.Opaque;
            g.DepthStencilState = DepthStencilState.Default;
            g.SamplerStates[0] = SamplerState.LinearWrap;
            g.DepthStencilState = DepthStencilState.Default;
            #endregion*/

            spriteBatch.Begin();
            spriteBatch.Draw(LightTarget, new Rectangle(0, 0, 160, 120), Color.White);
            spriteBatch.Draw(TextureTarget, new Rectangle(160, 0, 160, 120), Color.White);
            spriteBatch.Draw(NormalTarget, new Rectangle(320, 0, 160, 120), Color.White);
            spriteBatch.Draw(DepthTarget, new Rectangle(480, 0, 160, 120), Color.White);
            spriteBatch.End();
        }


        List<Transform> AllObjects
        {
            get
            {
                return GetTransformsOfType(new Transform().GetType());
            }
        }

        List<Transform> transforms = new List<Transform>();
        /// <summary>
        /// Very wasteful, will need to be improved
        /// </summary>
        public List<Transform> GetTransformsOfType(Type T)
        {
            transforms.Clear();
            OfType(Root.Children, T);
            return transforms;
        }

        public void OfType(List<Transform> Children, Type T)
        {
            foreach (Transform transform in Children)
            {
                if (transform.GetType() == T)
                    transforms.Add(transform);
                OfType(transform.Children, T);
            }
        }
    }
}