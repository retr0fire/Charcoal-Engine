using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using MJLGameEngine;
using MJLGameEngine.Scene;
using MJLGameEngine._2D_Graphics;
using MJLGameEngine._2D_Graphics.Objects;
using MapEditor;

using Jitter;
using Jitter.Dynamics;
using Jitter.Dynamics.Constraints;
using Jitter.Dynamics.Joints;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;
using System.Diagnostics;
using Jitter.DataStructures;

namespace SceneTest
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Scene scene;
        //_2DFrame frame;
        Microsoft.Xna.Framework.Input.KeyboardState lk;
        Microsoft.Xna.Framework.Input.MouseState mouseState;
        Microsoft.Xna.Framework.Input.MouseState mousePreviousState;
        
        Editor e = new Editor();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = false;

            Engine.StartGameEngine(this);
            scene = new Scene();
            MJLGameEngine.Scene.Camera.Intitialize_WithDefaults();

            

            //frame = new _2DFrame("Editor",null,Color.White,new Rectangle(900, 0, 100, 600),Content.Load<SpriteFont>("Font"));
            //frame.SetColorsToDefault();
            //frame.Collapsed = false;

            //_2DShapes.Init(Content.Load<SpriteFont>("Font"));

            base.Initialize();
        }
        protected override void LoadContent()
        {
            //uncomment to run FAST!!
            //this.TargetElapsedTime = new TimeSpan(100000);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            scene.LoadScene(Color.CornflowerBlue);
            scene.DirectionalLights.Add(new MJLGameEngine.Scene.DirectionalLight());
            scene.SpotLights.Add(new SpotLight(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0, 14, 0), new Vector3(0, -1f, 0), 0.7f, Color.White.ToVector3(), 2));
            
            //scene.ParticleGenerators.Add(new MJLGameEngine.Object.ParticleGenerator.Particle_Generator(new Vector3(0, 20, 0), 3000, Content.Load<Texture2D>("smoke"), Content.Load<Model>("particleplane"), Vector3.Down, 0.012f, true, Vector3.One, 100000));
            //scene.ParticleGenerators.Add(new MJLGameEngine.Object.ParticleGenerator.Particle_Generator(new Vector3(0, 0, 0), 100, Content.Load<Texture2D>("smoke"), Content.Load<Model>("particleplane"), Vector3.Up, 0.009f, true, Color.Orange.ToVector3(), 100));
                        
            scene.AddObject(true, "ground", Vector3.Down, Vector3.Zero, 1, Content.Load<Texture2D>("Road"), Content.Load<Texture2D>("RoadNormal"), true, true);
            /*
            for (int i = 0; i < 5; i++)
            {

                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        scene.AddObject(false, "barrel", new Vector3(i * 1, 0 + k*1, j * 1), new Vector3(0, 90, 0), 1, null, Content.Load<Texture2D>("MedBarrelNormal"), true);
                    }
                }
            }
            */
            //scene.AddObject(true, "x", new Vector3(20, -0.8f, -30), Vector3.Zero, 1, Content.Load<Texture2D>("Road"), Content.Load<Texture2D>("RoadNormal"), true);
            //scene.AddObject(true, "chapelx", new Vector3(-27, -0.8f, 10), new Vector3(180, 0, 0), 1, Content.Load<Texture2D>("Chapel_Mat_"), Content.Load<Texture2D>("chapel_normal"), true);
            //scene.AddObject(true, "cottage_3ds", new Vector3(25, -0.8f, -5), Vector3.Zero, 1, null, null, false);
            //scene.AddObject(true, "house_obj", new Vector3(-20, -0.8f, 30), Vector3.Zero, 1, null, Content.Load<Texture2D>("house_normal"), true);

            scene.AddObject(false, "car", new Vector3(3, 3, 3), new Vector3(0, 90, 0), 1, null, null, false, false);
            //scene.AddObject(false, "f22- 3x", new Vector3(10, 10, 10), new Vector3(0, 0, 0), 1, null, Content.Load<Texture2D>("FA-22_Raptor_N"), true);
            //scene.AddObject(false, "f5", new Vector3(-10, 10, 10), new Vector3(0, 0, 0), 1, null, null, false);
            //scene.AddObject(false, "mig29-2", new Vector3(-20, 10, 10), new Vector3(0, 0, 0), 1, null, Content.Load<Texture2D>("Mig-29_Fulcrum_N"), true);
            
            e.RunAsEditor(scene);
        }
        protected override void UnloadContent()
        {

        }

        #region varsforupdate
        // Store information for drag and drop
        JVector hitPoint, hitNormal;
        Jitter.Dynamics.Constraints.SingleBody.PointOnPoint grabConstraint;
        RigidBody grabBody;
        float hitDistance = 0.0f;
        int scrollWheel = 0;
        private bool RaycastCallback(RigidBody body, JVector normal, float fraction)
        {
            if (body.IsStatic) return false;
            else return true;
        }

        RigidBody lastBody = null;
        #endregion

        protected override void Update(GameTime gameTime)
        {
            #region init
            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            Microsoft.Xna.Framework.Input.KeyboardState k = Microsoft.Xna.Framework.Input.Keyboard.GetState();


            //JoystickState j = new JoystickState();
            //j = joystick.GetCurrentState();

            #endregion
            #region save_screenshots
            if (k.IsKeyDown(Keys.PrintScreen))
                if (lk.IsKeyUp(Keys.PrintScreen))
                    MJLGameEngine.Utilities.ScreenShot.Save_Image_To_OneDrive(scene.SceneTarget);

            #endregion
            #region shoot_barrels
            if (k.IsKeyDown(Keys.Space))
            {
                if (lk.IsKeyUp(Keys.Space))
                {
                    MJLGameEngine.Object.Object o = scene.AddObject(false, "barrel", Camera.Position, new Vector3(0, 90, 0), 1, null, Content.Load<Texture2D>("MedBarrelNormal"), true, false);
                    o.body.LinearVelocity = new JVector(Camera.look.X * 30, Camera.look.Y * 30, Camera.look.Z * 30);
                    //Console.WriteLine(Camera.look);
                }
            }
            #endregion
            #region update_camera
            float rotspeed = 0.02f;
            float transpeed = 0.08f;


            if (e.Arc_Ball.Checked)
                Camera.Update_Arc_Ball(Vector3.Zero);
            else
                Camera.Update_WASD(rotspeed, transpeed);
            
            #endregion
            #region update_settings_from_editor
            scene.ShadowMappingEnabled = e.Shadow_Mapping.Checked;
            scene.AlphaMappingEnabled = e.Alpha_Mapping.Checked;
            scene.RadialBlurEnabled = e.PostScreenBlur.Checked;
            scene.DebugDraw = e.DebugView.Checked;
            scene.PhysicsEnabled = e.Enable_Physics.Checked;
            #endregion
            #region grab_stuff

            mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            World World;
            World = scene.world;

            if (mouseState.LeftButton == ButtonState.Pressed &&
                mousePreviousState.LeftButton == ButtonState.Released)
            {
                JVector ray = Conversion.ToJitterVector(RayTo(mouseState.X, mouseState.Y));
                JVector camp = Conversion.ToJitterVector(Camera.Position);

                ray = JVector.Normalize(ray) * 100;

                float fraction;
                
                bool result = World.CollisionSystem.Raycast(camp, ray, RaycastCallback, out grabBody, out hitNormal, out fraction);

                if (result)
                {
                    hitPoint = camp + fraction * ray;

                    if (grabConstraint != null) World.RemoveConstraint(grabConstraint);

                    JVector lanchor = hitPoint - grabBody.Position;
                    lanchor = JVector.Transform(lanchor, JMatrix.Transpose(grabBody.Orientation));

                    grabConstraint = new Jitter.Dynamics.Constraints.SingleBody.PointOnPoint(grabBody, lanchor);
                    grabConstraint.Softness = 0.01f;
                    grabConstraint.BiasFactor = 0.1f;

                    World.AddConstraint(grabConstraint);
                    hitDistance = (Conversion.ToXNAVector(hitPoint) - Camera.Position).Length();
                    scrollWheel = mouseState.ScrollWheelValue;
                    grabConstraint.Anchor = hitPoint;
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                hitDistance += (mouseState.ScrollWheelValue - scrollWheel) * 0.01f;
                scrollWheel = mouseState.ScrollWheelValue;

                if (grabBody != null)
                {
                    Vector3 ray = RayTo(mouseState.X, mouseState.Y); ray.Normalize();
                    grabConstraint.Anchor = Conversion.ToJitterVector(Camera.Position + ray * hitDistance);
                    grabBody.IsActive = true;
                    if (!grabBody.IsStatic)
                    {
                        grabBody.LinearVelocity *= 0.98f;
                        grabBody.AngularVelocity *= 0.98f;
                    }
                }
            }
            else
            {
                if (grabConstraint != null) World.RemoveConstraint(grabConstraint);
                grabBody = null;
                grabConstraint = null;
            }

            #endregion
            #region update_our_scene
            
            scene.Update(gameTime);
            
            #endregion
            #region cleanup
            lk = k;
            mousePreviousState = mouseState;
            #endregion
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            scene.Draw(spriteBatch);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            spriteBatch.Draw(scene.SpotLights[0].shadowmap, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, new Vector2(0.02f, 0.02f), SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);

        }

        //extra stuff

        private Vector3 RayTo(int x, int y)
        {
            Vector3 nearSource = new Vector3(x, y, 0);
            Vector3 farSource = new Vector3(x, y, 1);

            Matrix world = Matrix.Identity;

            Vector3 nearPoint = graphics.GraphicsDevice.Viewport.Unproject(nearSource, Camera.Projection, Camera.View, world);
            Vector3 farPoint = graphics.GraphicsDevice.Viewport.Unproject(farSource, Camera.Projection, Camera.View, world);

            Vector3 direction = farPoint - nearPoint;
            return direction;
        }
    }
}
