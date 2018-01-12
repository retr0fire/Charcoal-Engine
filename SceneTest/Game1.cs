using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using CharcoalEngine;
using CharcoalEngine.Scene;
using CharcoalEngine.Object;
using CharcoalEngine._2D;
using CharcoalEngine.Utilities;

using Jitter;
using Jitter.Dynamics;
using Jitter.Dynamics.Constraints;
using Jitter.Dynamics.Joints;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;
using System.Diagnostics;
using Jitter.DataStructures;
using SceneTest.Objects;
namespace SceneTest
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState _currentKeys;
        KeyboardState _previousKeys;
        Door door;
        Scene scene;
        Objects.Landscape Landscape;
        Panel panel;

        SimpleEditor.Form1 e = new SimpleEditor.Form1();

        public Game1()
        {
            this.Window.AllowUserResizing = true;
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            //GraphicsDevice.PresentationParameters.BackBufferHeight += 200;
            //GraphicsDevice.PresentationParameters.BackBufferWidth += 400;

            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = false;

            Engine.StartGameEngine(this, Content, graphics);
            scene = new Scene();
            CharcoalEngine.Scene.Camera.Initialize_WithDefaults();
            /*
            for (int x = 0; x < size; x++)
            {
                Transform a = new Transform();
                a.Position = new Vector3(x, 0, 0);
                scene.Root.Children.Add(a);

                for (int y = 0; y < size; y++)
                {
                    Transform b = new Transform();
                    b.Position = new Vector3(0, y, 0);
                    a.Children.Add(b);

                    for (int z = 0; z < size; z++)
                    {
                        if (heights[x, y, z] < 0.0f)
                        {
                            Cube cube = new Cube();
                            cube.Position = new Vector3(0, 0, z);
                            b.Children.Add(cube);
                        }
                    }
                }
            }
            */

            int size = 128;
            float[,,] heights = CharcoalEngine.Utilities.MapGeneration._3DPerlinMap.Create_3D_Perlin_Map_W_Octaves(size, new Random(), 6);

            CubeType[,,] cubes = new CubeType[size, size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        cubes[x, y, z] = CubeType.Nothing;
                        if (heights[x,y,z] > (y/250.0f + heights[x,99,z]/2.5f))
                        {
                            cubes[x, y, z] = CubeType.Dirt;
                        }
                    }
                }
            }

            scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(0,0,0)));
            /*scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(size, 0, 0)));
            scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(size, 0, size)));
            scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(0, 0, size)));

            scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(0, size, 0)));
            scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(size, size, 0)));
            scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(size, size, size)));
            scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(0, size, size)));

            scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(0 + size, 0, 0)));
            scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(size + size, 0, 0)));
            scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(size + size, 0, size)));
            scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(0 + size, 0, size)));*/

            //scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(0 + size, size, 0)));
            //scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(size + size, size, 0)));
            //scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(size + size, size, size)));
            //scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(0 + size, size, size)));

            scene.Root.Update();
            e.RunAsEditor(scene);

            base.Initialize();
        }

        public float[,] getpixeldata(Texture2D tex, int Width, int Height)
        {
            Microsoft.Xna.Framework.Color[] pixeldata;

            pixeldata = new Microsoft.Xna.Framework.Color[tex.Bounds.Width * tex.Bounds.Height];
            tex.GetData(pixeldata, 0, Height * Width);
            //HMTexture.Dispose();

            float[,] retheights = new float[Width, Height];

            for (int i = 0; i < Height * Width; i++)
            {
                int row = i / Width;
                int column = i - (int)(i / Width) * Width;

                retheights[column, row] = 1.0f - (float)pixeldata[i].G / 255.0f;
            }
            pixeldata = null;

            return retheights;
        }

        protected override void LoadContent()
        {
            //uncomment to run FAST!!
            this.TargetElapsedTime = new TimeSpan(100000);
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        protected override void UnloadContent()
        {

        }
        Vector3 Target;
        protected override void Update(GameTime gameTime)
        {
            scene.Update(gameTime);
            _currentKeys = Keyboard.GetState();
                        
            /*if (_currentKeys.IsKeyDown(Keys.W))
            {
                Transform t = scene.Root.Children[scene.Root.Children.Count - 1];
                t.Position += Landscape.MoveCharacter(t.boundingBox, t.AbsolutePosition, t.AbsoluteYawPitchRoll, t.Forward, 0.05f);
            }

            if (_currentKeys.IsKeyDown(Keys.Left))
            {
                Transform t = scene.Root.Children[scene.Root.Children.Count - 1];
                t.YawPitchRoll = new Vector3(t.YawPitchRoll.X - 0.01f, t.YawPitchRoll.Y, t.YawPitchRoll.Z);
            }
            if (_currentKeys.IsKeyDown(Keys.Right))
            {
                Transform t = scene.Root.Children[scene.Root.Children.Count - 1];
                t.YawPitchRoll = new Vector3(t.YawPitchRoll.X + 0.01f, t.YawPitchRoll.Y, t.YawPitchRoll.Z);
            }*/

            #region update_camera
            Camera.Update_Arc_Ball(Target);
            /*
            if (IsNewButtonPress(Keys.Q))
                door.Open();
            if (IsNewButtonPress(Keys.W))
                door.Close();*/

            //Camera.Update_WASD(0.03f, 1f);
            /*
            if (IsNewButtonPress(Keys.K))
            {
                //fire an arrow;
                OBJ_File obj = new OBJ_File();
                
                Arrow Arrow = new Arrow();
                Arrow.Name = "Arrow";
                scene.Root.Children.Add(Arrow);
                obj.Load("C:\\Users\\Michael\\Documents\\XNA\\Hunt-Or-Gatherers\\Models\\Objects\\Arrow.obj", Engine.g, new Vector3(0, 0, 0), Vector3.Zero, 1f, false, false, Arrow);
                Arrow.Up = Camera.Up;
                Arrow.Forward = Camera.look;
                Arrow.YawPitchRoll = DirectiontoYPR(Arrow.Up, Arrow.Forward);
                Arrow.Position = Camera.Position;
                scene.Root.Update();
                Arrow.Fire(scene.Root);
            }

            */
            _previousKeys = _currentKeys;

            //panel.Update();

            #endregion
            base.Update(gameTime);
        }

        private bool IsNewButtonPress(Keys key)
        {
            return _currentKeys.IsKeyDown(key) && _previousKeys.IsKeyUp(key);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            scene.Draw();
            
            base.Draw(gameTime);
        }

        public Vector3 DirectiontoYPR(Vector3 Up, Vector3 Forward)
        {
            Vector3 yawvec = new Vector3(Forward.X, Forward.Y, Forward.Z);
            yawvec.Y = 0;
            float yawlen = yawvec.Length();
            float yaw = -(float)Math.Atan2((double)Forward.X / yawlen, -(double)Forward.Z / yawlen);
            
            Vector3 pitchvec = Vector3.Transform(Forward, Matrix.CreateFromAxisAngle(Vector3.Up, -yaw));

            yaw += MathHelper.ToRadians(90);
            Console.WriteLine(pitchvec);

            float pitch = -(float)Math.Atan2((double)pitchvec.Y / yawlen, (double)pitchvec.Z / yawlen);
            pitch += MathHelper.ToRadians(180);
            return new Vector3(yaw, 0, pitch);
        }
        
        public void AddModel(Vector3 Position, Vector3 YawPitchRoll, Transform AttachmentPoint, string file, string name)
        {
            OBJ_File obj = new OBJ_File();

            AttachmentPoint.Children.Add(new Transform());
            AttachmentPoint.Children[AttachmentPoint.Children.Count - 1].Name = name;
            obj.Load(file, Engine.g,Position, YawPitchRoll, 1f, false, false, AttachmentPoint.Children[AttachmentPoint.Children.Count-1]);
        }
    }
}
