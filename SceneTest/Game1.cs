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
//using SceneTest.Objects;
namespace SceneTest
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState _currentKeys;
        KeyboardState _previousKeys;
        //Door door;
        Scene scene;
        //Objects.Landscape Landscape;
        //Panel panel;

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
            Camera.Position = new Vector3(64, 128, 64);
            Camera.Update();
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

            /*int size = 128;
            float[,,] heights = CharcoalEngine.Utilities.MapGeneration._3DPerlinMap.Create_3D_Perlin_Map_W_Octaves(size, new Random(), 4);

            CubeType[,,] cubes = new CubeType[size, size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        cubes[x, y, z] = CubeType.Nothing;
                        if (heights[x,y,z] > (y/250.0f + heights[x/4,99,z/4]/2.5f))
                        {
                            cubes[x, y, z] = CubeType.Dirt;
                        }
                    }
                }
            }

            scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(0,0,0)));*/
            /*
            float[,,] heights2 = CharcoalEngine.Utilities.MapGeneration._3DPerlinMap.Create_3D_Perlin_Map_W_Octaves(size, new Random(), 5);

            cubes = new CubeType[size, size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        cubes[x, y, z] = CubeType.Nothing;
                        if (heights[x, y, z] > (y / 250.0f + heights[size/4 + x / 4, 99, z / 4] / 2.5f))
                        {
                            cubes[x, y, z] = CubeType.Dirt;
                        }
                    }
                }
            }

            scene.Root.Children.Add(new CubeSection(cubes, size, new Vector3(size, 0, 0)));
            */
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

            //

            //scene.Root.Children.Add(new Section(CharcoalEngine.Utilities.MapGeneration._2DPerlinMap.Create_2D_Perlin_Map_W_Octaves(512, new Random(), 8), 512, 512, new Vector3(1, 200, 1)));

            /*float[,] heights = new float[2, 2];
            heights[0, 0] = 0.0f;
            heights[1, 0] = 0.0f;
            heights[0, 1] = 0.0f;
            heights[1, 1] = 0.0f;

            scene.Root.Children.Add(new Section(heights, 2, 2, Vector3.One));*/
            /*
            float[,] heights = new float[2, 2];
            heights[0, 0] = 0.0f;
            heights[1, 0] = 0.0f;
            heights[0, 1] = 0.0f;
            heights[1, 1] = 0.0f;
ParticleGenerator p = new ParticleGenerator();
            scene.Root.Children.Add(p);
            p.Init();*/

            /*PhysicsObject obj1 = new PhysicsObject();
            PhysicsObject obj2 = new PhysicsObject();
            PhysicsObject obj3 = new PhysicsObject();

            OBJ_File obj = new OBJ_File();
            //obj.Load("C:\\Users\\Michael\\OneDrive\\3D Models\\sphere\\sphere.obj", Engine.g, new Vector3(0, 0, 0), Vector3.Zero, 1f, false, false, obj1);
            //obj.Load("C:\\Users\\Michael\\OneDrive\\3D Models\\sphere\\sphere.obj", Engine.g, new Vector3(0, 0, 0), Vector3.Zero, 1f, false, false, obj2);

            scene.Root.Children.Add(obj1);
            scene.Root.Children.Add(obj2);
            scene.Root.Children.Add(obj3);

            obj1.CanCollide = false;
            obj1.Position = Vector3.Zero;
            obj1.Velocity = Vector3.Zero;
            obj1.Mass = 1;

            obj2.CanCollide = false;
            obj2.Position = Vector3.Down * 5;
            obj2.Velocity = Vector3.Zero;
            obj2.Mass = 1;
            obj2.AddForce(new Force(Vector3.Down * 9.81f, float.PositiveInfinity));

            obj3.CanCollide = false;
            obj3.Position = Vector3.Down * 10;
            obj3.Velocity = Vector3.Zero;
            obj3.Mass = 1;
            obj3.AddForce(new Force(Vector3.Down * 9.81f, float.PositiveInfinity));

            obj1.IsStatic = true;            

            scene.physics.Objects.Add(obj1);
            scene.physics.Objects.Add(obj2);
            scene.physics.Objects.Add(obj3);

            Spring s1 = new Spring(obj1, obj2, 10, 4);
            Spring s2 = new Spring(obj2, obj3, 10, 4);

            scene.physics.Objects.Add(s1);
            scene.physics.Objects.Add(s2);
            scene.Root.Children.Add(s1);
            scene.Root.Children.Add(s2);*/

            /*int max_x = 4;
            int max_y = 4;
            int max_z = 4;

            PhysicsObject[,,] objs = new PhysicsObject[max_x, max_y, max_z];

            for (int x = 0; x < max_x; x++)
            {
                for (int y = 0; y < max_y; y++)
                {
                    for (int z = 0; z < max_z; z++)
                    {
                        objs[x, y, z] = new PhysicsObject();
                        objs[x, y, z].CanCollide = false;
                        objs[x, y, z].Position = new Vector3(x, y, z) * 10;
                        objs[x, y, z].Velocity = Vector3.Zero;
                        objs[x, y, z].Mass = 1;
                        if (y == 0)
                            objs[x, y, z].IsStatic = true;
                        scene.physics.Objects.Add(objs[x, y, z]);
                        scene.Root.Children.Add(objs[x, y, z]);
                    }
                }
            }
            for (int x = 0; x < max_x; x++)
            {
                for (int y = 0; y < max_y; y++)
                {
                    for (int z = 0; z < max_z; z++)
                    {
                        if (x > 0)
                        {
                            Spring s = new Spring(objs[x, y, z], objs[x - 1, y, z], 10, 10);
                            scene.physics.Objects.Add(s);
                            scene.Root.Children.Add(s);
                        }
                        if (x < max_x - 1)
                        {
                            Spring s = new Spring(objs[x, y, z], objs[x + 1, y, z], 10, 10);
                            scene.physics.Objects.Add(s);
                            scene.Root.Children.Add(s);
                        }

                        if (y > 0)
                        {
                            Spring s = new Spring(objs[x, y, z], objs[x, y - 1, z], 10, 10);
                            scene.physics.Objects.Add(s);
                            scene.Root.Children.Add(s);
                        }
                        if (y < max_y - 1)
                        {
                            Spring s = new Spring(objs[x, y, z], objs[x, y + 1, z], 10, 10);
                            scene.physics.Objects.Add(s);
                            scene.Root.Children.Add(s);
                        }

                        if (z > 0)
                        {
                            Spring s = new Spring(objs[x, y, z], objs[x, y, z - 1], 10, 10);
                            scene.physics.Objects.Add(s);
                            scene.Root.Children.Add(s);
                        }
                        if (z < max_z - 1)
                        {
                            Spring s = new Spring(objs[x, y, z], objs[x, y, z + 1], 10, 10);
                            scene.physics.Objects.Add(s);
                            scene.Root.Children.Add(s);
                        }
                    }
                }
            } */

            /*
            PhysicsObject[] wall = new PhysicsObject[6];

            for (int i = 0; i < wall.Length; i++) wall[i] = new PhysicsObject();
            
            wall[0].Position = new Vector3(-12, 0, 0);
            wall[1].Position = new Vector3(12, 0, 0);
            wall[2].Position = new Vector3(0, 0, -12);
            wall[3].Position = new Vector3(0, 0, 12);
            wall[4].Position = new Vector3(0, -12, 0);
            wall[5].Position = new Vector3(0, 12, 0);

            wall[0].LocalBoundingBox = new BoundingBox(new Vector3(-2, -10, -10), new Vector3(2, 10, 10));
            wall[1].LocalBoundingBox = new BoundingBox(new Vector3(-2, -10, -10), new Vector3(2, 10, 10));

            wall[2].LocalBoundingBox = new BoundingBox(new Vector3(-10, -10, -2), new Vector3(10, 10, 2));
            wall[3].LocalBoundingBox = new BoundingBox(new Vector3(-10, -10, -2), new Vector3(10, 10, 2));
            
            wall[4].LocalBoundingBox = new BoundingBox(new Vector3(-10, -2, -10), new Vector3(10, 2, 10));
            wall[5].LocalBoundingBox = new BoundingBox(new Vector3(-10, -2, -10), new Vector3(10, 2, 10));

            for (int i = 0; i < wall.Length; i++)
            {
                wall[i].Mass = 10000;
                wall[i].IsStatic = true;
                wall[i].Velocity = Vector3.Zero;
                scene.physics.Objects.Add(wall[i]);
                scene.Root.Children.Add(wall[i]);
            }
            */
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
        MouseState lms;
        MouseState ms;
        int check = 0;
        protected override void Update(GameTime gameTime)
        {
            scene.Update(gameTime);
            /*_currentKeys = Keyboard.GetState();

            Vector3 Velocity = new Vector3(0, -0.5f, 0);

            if (_currentKeys.IsKeyDown(Keys.W))
                Velocity += Vector3.Forward * .521f;
            if (_currentKeys.IsKeyDown(Keys.S))
                Velocity += -Vector3.Forward * .521f;
            if (_currentKeys.IsKeyDown(Keys.A))
                Velocity += Vector3.Right * .521f;
            if (_currentKeys.IsKeyDown(Keys.D))
                Velocity += -Vector3.Right * .521f;

            if (_currentKeys.IsKeyDown(Keys.Space))
                Velocity += Vector3.Up;


            float vel_length = Math.Abs(Velocity.Length());

            Ray velocity_ray = new Ray(Camera.Position+new Vector3(0, -2, 0), Velocity);
            float closest = float.PositiveInfinity;
            int cx = -1, cy = -1, cz = -1;

            int Size = 128;
            check++;
            if (check > 15/*IsNewButtonPress(Keys.K)*//*)
            {
                check = 0;
                for (int x = 0; x < Size; x++)
                {
                    for (int y = 0; y < Size; y++)
                    {
                        for (int z = 0; z < Size; z++)
                        {
                            if (((CubeSection)scene.Root.Children[1]).Cubes[x, y, z] != CubeType.Nothing)
                            {
                                BoundingBox b = new BoundingBox(new Vector3(x, y, z) - Vector3.One / 2, new Vector3(x, y, z) + Vector3.One / 2);
                                if (velocity_ray.Intersects(b) != null)
                                    if (velocity_ray.Intersects(b) >= 0)
                                        if (velocity_ray.Intersects(b) < closest)
                                        {
                                            if (velocity_ray.Intersects(b)/10.0f < vel_length)
                                            {
                                                closest = (float)velocity_ray.Intersects(b);
                                                cx = x;
                                                cy = y;
                                                cz = z;
                                              //  Console.WriteLine("Collided with block " + x + " " + y + " " + z + " " + (float)velocity_ray.Intersects(b));
                                            }
                                            else
                                            {
                                                //Console.WriteLine("Collided with block " + x + " " + y + " " + z + " " + (float)velocity_ray.Intersects(b));
                                            }
                                        }
                            }
                        }
                    }
                }
                
                BoundingBox vb = new BoundingBox(new Vector3(cx, cy, cz) - Vector3.One / 2, new Vector3(cx, cy, cz) + Vector3.One / 2);
                float diff = vel_length;
                //Console.WriteLine("Collided with block " + cx + " " + cy + " " + cz + " " + velocity_ray.Intersects(vb));

                if (cx != -1)
                    diff = (float)velocity_ray.Intersects(vb)/10;

                //Camera.Translate_AxisAligned(Velocity * (diff/vel_length));
            }*/
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

            //Camera.Update_WASD(0.03f, .21f);
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
            /*
            ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed && lms.LeftButton == ButtonState.Released)
            {
                Ray r = Utilities3D.ConvertMouseToRay(Mouse.GetState().Position.ToVector2());
                float closest = float.PositiveInfinity;
                int cx = 0, cy = 0, cz = 0;

                int Size = 128;
                for (int x = 0; x < Size; x++)
                {
                    for (int y = 0; y < Size; y++)
                    {
                        for (int z = 0; z < Size; z++)
                        {
                            if (((CubeSection)scene.Root.Children[1]).Cubes[x, y, z] != CubeType.Nothing)
                            {
                                BoundingBox b = new BoundingBox(new Vector3(x, y, z) - Vector3.One / 2, new Vector3(x, y, z) + Vector3.One / 2);
                                if (r.Intersects(b) != null)
                                    if (r.Intersects(b) > 0)
                                        if (r.Intersects(b) < closest)
                                        {
                                            closest = (float)r.Intersects(b);
                                            cx = x;
                                            cy = y;
                                            cz = z;
                                        }
                            }
                        }
                    }
                }


                ((CubeSection)scene.Root.Children[1]).Remove_Block(cx, cy, cz);
            }

            _previousKeys = _currentKeys;

            lms = ms;
            */
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
    }
}
