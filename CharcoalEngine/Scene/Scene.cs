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

        public List<DrawingSystem> DrawingSystems = new List<DrawingSystem>();



        GraphicsDevice g;
        SpriteBatch spriteBatch;

        GizmoComponent _gizmo;

        public Scene()
        {
            //Engine.Game.Window.AllowUserResizing = true;
            Engine.Game.IsMouseVisible = true;

            g = Engine.g;
            spriteBatch = new SpriteBatch(g);

            Engine.Game.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

            _gizmo = new GizmoComponent(g, spriteBatch, Engine.Content.Load<SpriteFont>("Fonts/Font"));
            _gizmo.SetSelectionPool(Root.Children, Root);

            //_gizmo.TranslateEvent += _gizmo_TranslateEvent;
            _gizmo.RotateEvent += _gizmo_RotateEvent;
            //_gizmo.ScaleEvent += _gizmo_ScaleEvent;


            Root.Update();       
            DrawingSystems.Add(new RayMarching());


            Root.Children.Add(new Sphere());     
            ((RayMarching)DrawingSystems[0]).RegisterItem(Root.Children[0]);
            Root.Children.Add(new Sphere());
            ((RayMarching)DrawingSystems[0]).RegisterItem(Root.Children[1]);
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

            g.Clear(Color.Black);

            /*for (int i = 0; i < Root.Children.Count; i++)
            {
                Root.Children[i].Draw();
            }*/

            for (int i = 0; i < DrawingSystems.Count; i++)
            {
                DrawingSystems[i].Draw();
            }

            #region setup
            g.BlendState = BlendState.Opaque;
            g.DepthStencilState = DepthStencilState.Default;
            g.SamplerStates[0] = SamplerState.LinearWrap;
            //g.DepthStencilState = DepthStencilState.Default;
            #endregion

            _gizmo.Draw();
        }

        List<Transform> transforms = new List<Transform>();
    }
}