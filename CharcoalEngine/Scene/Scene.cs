using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using CharcoalEngine.Object;
using CharcoalEngine.Object.ParticleGenerator;
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
        public List<DirectionalLight> DirectionalLights = new List<DirectionalLight>();
        public List<SpotLight> SpotLights = new List<SpotLight>();
        public List<Object.Object> Objects = new List<Object.Object>();
        public List<Particle_Generator> ParticleGenerators = new List<Particle_Generator>();

        public int SelectedModel = -1;
        public int SelectedMesh = -1;

        CollisionSystem collision;
        public World world;

        public Color BackgroundColor;
        Effect AMEffect;
        Effect DLEffect;
        Effect SLEffect;
        Effect SMEffect;
        Effect FBEffect;
        Effect BTEffect;

        Texture2D Pixel;
        Texture2D Pixel2;

        //unused
        public Vector3 Wind;
        public DateTime TimeOfDay;


        //Post Screen Shaders
        Effect RadialBlurEffect;

        public bool ShadowMappingEnabled = true;
        public bool AlphaMappingEnabled = true;
        public bool RadialBlurEnabled = false;
        public bool DebugDraw = false;
        public bool PhysicsEnabled = true;

        public GraphicsDevice g;

        public RenderTarget2D LightTarget;
        public RenderTarget2D SceneTarget;
        public RenderTarget2D BlurTarget;

        
        public Scene()
        {
            if (GameEngineData.game == null)
                throw new NullReferenceException("You have not started the Game Engine. Call MJLGameEgine.StartGameEngine(Game game) to start the Engine");
            GameEngineData.game.Window.AllowUserResizing = true;
            GameEngineData.game.IsMouseVisible = true;

            collision = new CollisionSystemSAP();
            world = new World(collision);

        }
        public void LoadScene(Color backgroundcolor)
        {
            g = GameEngineData.game.GraphicsDevice;
            AMEffect = Utilities.EngineContent.Content.Load<Effect>("Effects/AlphaMapEffect");
            DLEffect = Utilities.EngineContent.Content.Load<Effect>("Effects/DirectionalLightEffect");
            SLEffect = Utilities.EngineContent.Content.Load<Effect>("Effects/SpotLightEffect");
            SMEffect = Utilities.EngineContent.Content.Load<Effect>("Effects/ShadowMapEffect");
            FBEffect = Utilities.EngineContent.Content.Load<Effect>("Effects/FinalBlendEffect");
            BTEffect = Utilities.EngineContent.Content.Load<Effect>("Effects/BasicTextureEffect");

            RadialBlurEffect = Utilities.EngineContent.Content.Load<Effect>("Effects/RadialBlurEffect");

            LightTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, true, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            SceneTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, true, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            BlurTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, true, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            BackgroundColor = backgroundcolor;

            GameEngineData.game.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

            Pixel = new Texture2D(g, 1, 1, false, SurfaceFormat.Color);
            Color[] pix = new Color[1];
            pix[0] = new Color((float)1, (float)1, (float)1, (float)1);
            Pixel.SetData(pix, 0, 1);

            Pixel2 = new Texture2D(g, 1, 1, false, SurfaceFormat.Color);
            Color[] pix2 = new Color[1];
            pix2[0] = new Color((float)0, (float)0, (float)0, (float)1);
            Pixel2.SetData(pix2, 0, 1);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            LightTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, true, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            SceneTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, true, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            BlurTarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, true, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            foreach (DirectionalLight d in DirectionalLights)
            {
                d.lighttarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            }
            foreach (SpotLight s in SpotLights)
            {
                s.lighttarget = new RenderTarget2D(g, g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            }
        }
        public Object.Object AddObject(bool Static, string name, Vector3 pos, Vector3 ypr,
                                       float scale, Texture2D texture, Texture2D NormalMap, bool NormalMap_Enabled, bool UseDDSNormal)
        {
            CharcoalEngine.Object.Object o = new CharcoalEngine.Object.Object(GameEngineData.game.Content, Static, name, pos, ypr, scale, texture, NormalMap, NormalMap_Enabled, UseDDSNormal);
            Objects.Add(o);
            world.AddBody(o.body);

            return o;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Object.Object obj in Objects)
            {
                obj.Update();
            }
            foreach (Particle_Generator p_g in ParticleGenerators)
            {
                p_g.Update(gameTime, Camera.Position, Camera.Rotation);
            }
            if (PhysicsEnabled)
            {
                world.Step(1.0f / 100.0f, true);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            #region setup
            g.BlendState = BlendState.Opaque;
            g.DepthStencilState = DepthStencilState.Default;
            g.SamplerStates[0] = SamplerState.LinearWrap;
            g.DepthStencilState = DepthStencilState.Default;
            #endregion
            RenderShadowMaps();
            RenderAlphaMaps();
            RenderScene(spriteBatch);
            RenderPostScreen(spriteBatch);
        }
        private void RenderShadowMaps()
        {
            #region spot_lights
            if (ShadowMappingEnabled)
            {

                for (int i = 0; i < SpotLights.Count; i++)
                {
                    g.SetRenderTarget(SpotLights[i].shadowmap);
                    g.Clear(Color.Black);
                    foreach (Object.Object obj in Objects)
                    {
                        if (obj.Has_TransparentGeometry == false)
                        {
                            foreach (ModelMesh mesh in obj.model.Meshes)
                            {
                                foreach (ModelMeshPart part in mesh.MeshParts)
                                {
                                    part.Effect = SMEffect;
                                    SMEffect.Parameters["World"].SetValue(mesh.ParentBone.Transform * obj.World);
                                    SMEffect.Parameters["View"].SetValue(SpotLights[i].View());
                                    SMEffect.Parameters["Projection"].SetValue(SpotLights[i].Projection);
                                    SMEffect.Parameters["NearPlane"].SetValue(SpotLights[i].NearPlane);
                                    SMEffect.Parameters["FarPlane"].SetValue(SpotLights[i].FarPlane);


                                    SMEffect.Parameters["TextureEnabled"].SetValue(true);
                                    if (part.Tag != null)
                                        SMEffect.Parameters["BasicTexture"].SetValue(((BasicEffect)part.Tag).Texture);
                                    else
                                        SMEffect.Parameters["TextureEnabled"].SetValue(false);

                                    SMEffect.Parameters["Alpha"].SetValue(1);
                                }
                                mesh.Draw();
                            }
                        }
                    }

                    g.DepthStencilState = DepthStencilState.DepthRead;

                    foreach (Object.Object obj in Objects)
                    {
                        if (obj.Has_TransparentGeometry == true)
                        {
                            foreach (ModelMesh mesh in obj.model.Meshes)
                            {
                                foreach (ModelMeshPart part in mesh.MeshParts)
                                {
                                    part.Effect = SMEffect;
                                    SMEffect.Parameters["World"].SetValue(mesh.ParentBone.Transform * obj.World);
                                    SMEffect.Parameters["View"].SetValue(SpotLights[i].View());
                                    SMEffect.Parameters["Projection"].SetValue(SpotLights[i].Projection);
                                    SMEffect.Parameters["NearPlane"].SetValue(SpotLights[i].NearPlane);
                                    SMEffect.Parameters["FarPlane"].SetValue(SpotLights[i].FarPlane);


                                    SMEffect.Parameters["TextureEnabled"].SetValue(true);
                                    if (part.Tag != null)
                                        SMEffect.Parameters["BasicTexture"].SetValue(((BasicEffect)part.Tag).Texture);
                                    else
                                        SMEffect.Parameters["TextureEnabled"].SetValue(false);

                                    SMEffect.Parameters["Alpha"].SetValue(1);
                                }
                                mesh.Draw();
                            }
                        }
                    }
                    g.BlendState = BlendState.Opaque;
                    g.DepthStencilState = DepthStencilState.Default;
                    g.SetRenderTarget(null);

                }

            }
            #endregion
        }
        private void RenderAlphaMaps()
        {
            if (AlphaMappingEnabled)
            {
                #region spot_lights
                for (int i = 0; i < SpotLights.Count; i++)
                {
                    g.SetRenderTarget(SpotLights[i].alphamap);
                    g.Clear(Color.Black);
                    g.BlendState = BlendState.AlphaBlend;


                    g.DepthStencilState = DepthStencilState.Default;

                    foreach (Object.Object obj in Objects)
                    {
                        foreach (ModelMesh mesh in obj.model.Meshes)
                        {
                            foreach (ModelMeshPart part in mesh.MeshParts)
                            {
                                part.Effect = AMEffect;
                                AMEffect.Parameters["World"].SetValue(mesh.ParentBone.Transform * obj.World);
                                AMEffect.Parameters["View"].SetValue(SpotLights[i].View());
                                AMEffect.Parameters["Projection"].SetValue(SpotLights[i].Projection);

                                AMEffect.Parameters["TextureEnabled"].SetValue(true);
                                if (part.Tag != null)
                                    AMEffect.Parameters["BasicTexture"].SetValue(((BasicEffect)part.Tag).Texture);
                                else
                                    AMEffect.Parameters["TextureEnabled"].SetValue(false);

                                AMEffect.Parameters["Alpha"].SetValue(1);
                            }
                            mesh.Draw();
                        }
                    }

                    g.DepthStencilState = DepthStencilState.DepthRead;

                    for (int j = 0; j < ParticleGenerators.Count; j++)
                    {
                        for (int k = 0; k < ParticleGenerators[j].Particles.Count; k++)
                        {
                            foreach (ModelMesh mesh in ParticleGenerators[j].Particles[k].particlemodel.Meshes)
                            {
                                foreach (ModelMeshPart part in mesh.MeshParts)
                                {
                                    part.Effect = AMEffect;
                                    AMEffect.Parameters["World"].SetValue(ParticleGenerators[j].Particles[k].GetWorld(SpotLights[i].LightPosition, Vector3.Cross(SpotLights[i].LightDirection, Vector3.Left)));
                                    AMEffect.Parameters["View"].SetValue(SpotLights[i].View());
                                    AMEffect.Parameters["Projection"].SetValue(SpotLights[i].Projection);
                                    AMEffect.Parameters["BasicTexture"].SetValue(ParticleGenerators[j].Particles[k].particletex);
                                    AMEffect.Parameters["TextureEnabled"].SetValue(true);
                                    AMEffect.Parameters["Alpha"].SetValue(ParticleGenerators[j].Particles[k].alpha);
                                }
                                mesh.Draw();
                            }
                        }
                    }

                    g.BlendState = BlendState.Opaque;
                    g.DepthStencilState = DepthStencilState.Default;

                    g.SetRenderTarget(null);
                }
                #endregion
            }
        }
        private void RenderScene(SpriteBatch spriteBatch)
        {
            #region directional_lights
            for (int i = 0; i < DirectionalLights.Count; i++)
            {
                g.SetRenderTarget(DirectionalLights[i].lighttarget);
                g.Clear(Color.Transparent);
                g.BlendState = BlendState.AlphaBlend;
                foreach (Object.Object obj in Objects)
                {
                    foreach (ModelMesh mesh in obj.model.Meshes)
                    {
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            part.Effect = DLEffect;
                            DLEffect.Parameters["World"].SetValue(mesh.ParentBone.Transform * obj.World);
                            DLEffect.Parameters["View"].SetValue(Camera.View);
                            DLEffect.Parameters["Projection"].SetValue(Camera.Projection);
                            DLEffect.Parameters["CameraPosition"].SetValue(Camera.Position);

                            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(obj.World));
                            DLEffect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);

                            DLEffect.Parameters["LightPosition"].SetValue(DirectionalLights[i].LightPosition);
                            DLEffect.Parameters["LightColor"].SetValue(DirectionalLights[i].LightColor);
                            DLEffect.Parameters["LightAttenuation"].SetValue(DirectionalLights[i].LightAttenuation);
                            DLEffect.Parameters["LightFalloff"].SetValue(DirectionalLights[i].LightFalloff);
                            DLEffect.Parameters["AmbientLightColor"].SetValue(DirectionalLights[i].AmbientLightColor);
                            DLEffect.Parameters["SpecularPower"].SetValue(obj.SpecularPower);
                            DLEffect.Parameters["bump_Height"].SetValue(obj.bump_height);

                            DLEffect.Parameters["TextureEnabled"].SetValue(true);
                            if (part.Tag != null)
                                DLEffect.Parameters["BasicTexture"].SetValue(((BasicEffect)part.Tag).Texture);
                            else
                                DLEffect.Parameters["TextureEnabled"].SetValue(false);

                            DLEffect.Parameters["NormalMapEnabled"].SetValue(obj.normalmap_enabled);
                            DLEffect.Parameters["dds_Normal"].SetValue(obj.use_dds_normal);
                            DLEffect.Parameters["NormalMap"].SetValue(obj.normalmap);
                            DLEffect.Parameters["Alpha"].SetValue(1);
                        }
                        mesh.Draw();
                    }
                }
                g.SetRenderTarget(null);
            }

            #endregion
            #region spot_lights
            for (int i = 0; i < SpotLights.Count; i++)
            {
                g.SetRenderTarget(SpotLights[i].lighttarget);
                g.Clear(Color.Transparent);
                g.BlendState = BlendState.AlphaBlend;

                foreach (Object.Object obj in Objects)
                {
                    foreach (ModelMesh mesh in obj.model.Meshes)
                    {
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            part.Effect = SLEffect;
                            SLEffect.Parameters["World"].SetValue(mesh.ParentBone.Transform * obj.World);
                            SLEffect.Parameters["View"].SetValue(Camera.View);
                            SLEffect.Parameters["Projection"].SetValue(Camera.Projection);

                            SLEffect.Parameters["CameraPosition"].SetValue(Camera.Position);

                            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(obj.World));
                            SLEffect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                            if (ShadowMappingEnabled == false)
                                SLEffect.Parameters["ShadowMap"].SetValue(Pixel);
                            else
                                SLEffect.Parameters["ShadowMap"].SetValue(SpotLights[i].shadowmap);

                            if (AlphaMappingEnabled == false)
                                SLEffect.Parameters["AlphaMap"].SetValue(Pixel2);
                            else
                                SLEffect.Parameters["AlphaMap"].SetValue(SpotLights[i].alphamap);
                            SLEffect.Parameters["LightViewProjection"].SetValue(obj.World * SpotLights[i].View() * SpotLights[i].Projection);
                            SLEffect.Parameters["LightPosition"].SetValue(SpotLights[i].LightPosition);
                            SLEffect.Parameters["LightDirection"].SetValue(SpotLights[i].LightDirection);
                            SLEffect.Parameters["ConeAngle"].SetValue(SpotLights[i].ConeAngle);
                            SLEffect.Parameters["LightColor"].SetValue(SpotLights[i].LightColor);
                            SLEffect.Parameters["LightFalloff"].SetValue(SpotLights[i].LightFalloff);
                            SLEffect.Parameters["SpecularPower"].SetValue(obj.SpecularPower);
                            SLEffect.Parameters["dds_Normal"].SetValue(obj.use_dds_normal);
                            SLEffect.Parameters["bump_Height"].SetValue(obj.bump_height);
                            SLEffect.Parameters["NearPlane"].SetValue(SpotLights[i].NearPlane);
                            SLEffect.Parameters["FarPlane"].SetValue(SpotLights[i].FarPlane);
                            SLEffect.Parameters["offset"].SetValue(SpotLights[i].shadowoffset);
                            SLEffect.Parameters["AmbientLightColor"].SetValue(SpotLights[i].AmbientLightColor);

                            SLEffect.Parameters["TextureEnabled"].SetValue(true);
                            if (part.Tag != null)
                                SLEffect.Parameters["BasicTexture"].SetValue(((BasicEffect)part.Tag).Texture);
                            else
                                SLEffect.Parameters["TextureEnabled"].SetValue(false);

                            SLEffect.Parameters["NormalMapEnabled"].SetValue(obj.normalmap_enabled);
                            SLEffect.Parameters["NormalMap"].SetValue(obj.normalmap);

                        }
                        mesh.Draw();
                    }
                }
                g.BlendState = BlendState.Opaque;

                g.SetRenderTarget(null);
            }
            #endregion
            #region blend lights together

            g.SetRenderTarget(LightTarget);
            g.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            for (int i = 0; i < DirectionalLights.Count; i++)
            {
                spriteBatch.Draw(DirectionalLights[i].lighttarget, new Rectangle(0, 0, DirectionalLights[i].lighttarget.Bounds.Width, DirectionalLights[i].lighttarget.Bounds.Height), Color.White);
            }

            for (int i = 0; i < SpotLights.Count; i++)
            {
                spriteBatch.Draw(SpotLights[i].lighttarget, new Rectangle(0, 0, SpotLights[i].lighttarget.Bounds.Width, SpotLights[i].lighttarget.Bounds.Height), Color.White);
            }

            spriteBatch.End();

            g.SetRenderTarget(null);
            #endregion
            #region setup

            g.DepthStencilState = DepthStencilState.Default;
            g.SamplerStates[0] = SamplerState.LinearWrap;
            #endregion
            #region blend final scene
            g.SetRenderTarget(SceneTarget);
            g.Clear(BackgroundColor);
            g.BlendState = BlendState.AlphaBlend;

            foreach (Object.Object obj in Objects)
            {
                foreach (ModelMesh mesh in obj.model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = FBEffect;
                        FBEffect.Parameters["World"].SetValue(mesh.ParentBone.Transform * obj.World);
                        FBEffect.Parameters["View"].SetValue(Camera.View);
                        FBEffect.Parameters["Projection"].SetValue(Camera.Projection);
                        FBEffect.Parameters["LightMap"].SetValue(LightTarget);
                        FBEffect.Parameters["EnableLightMap"].SetValue(true);
                        FBEffect.Parameters["TintColor"].SetValue(Vector3.One);

                        FBEffect.Parameters["NumberOfLights"].SetValue(DirectionalLights.Count + SpotLights.Count);

                        FBEffect.Parameters["Alpha"].SetValue(1);
                        FBEffect.Parameters["TextureEnabled"].SetValue(true);
                        if (part.Tag != null)
                        {
                            FBEffect.Parameters["BasicTexture"].SetValue(((BasicEffect)part.Tag).Texture);
                            FBEffect.Parameters["Alpha"].SetValue(((BasicEffect)part.Tag).Alpha);
                        }
                        else
                            FBEffect.Parameters["TextureEnabled"].SetValue(false);
                    }
                    mesh.Draw();
                }
            }

            g.BlendState = BlendState.AlphaBlend;
            g.DepthStencilState = DepthStencilState.DepthRead;

            for (int j = 0; j < ParticleGenerators.Count; j++)
            {
                for (int k = 0; k < ParticleGenerators[j].Particles.Count; k++)
                {
                    foreach (ModelMesh mesh in ParticleGenerators[j].Particles[k].particlemodel.Meshes)
                    {
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            part.Effect = FBEffect;
                            FBEffect.Parameters["World"].SetValue(ParticleGenerators[j].Particles[k].world);
                            FBEffect.Parameters["View"].SetValue(Camera.View);
                            FBEffect.Parameters["Projection"].SetValue(Camera.Projection);
                            FBEffect.Parameters["BasicTexture"].SetValue(ParticleGenerators[j].Particles[k].particletex);
                            FBEffect.Parameters["TextureEnabled"].SetValue(true);
                            FBEffect.Parameters["LightMap"].SetValue(LightTarget);
                            FBEffect.Parameters["Alpha"].SetValue(ParticleGenerators[j].Particles[k].alpha);
                            FBEffect.Parameters["EnableLightMap"].SetValue(false);
                            FBEffect.Parameters["TintColor"].SetValue(ParticleGenerators[j].Particles[k].color);
                            FBEffect.Parameters["NumberOfLights"].SetValue(DirectionalLights.Count + SpotLights.Count);
                        }
                        mesh.Draw();
                    }
                }
            }
            g.BlendState = BlendState.Opaque;
            g.DepthStencilState = DepthStencilState.Default;
            if (DebugDraw) { RenderDebug(); }
            RenderEdit();
            g.SetRenderTarget(null);
            #endregion
        }
        private void RenderPostScreen(SpriteBatch spriteBatch)
        {
            g.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (RadialBlurEnabled == true)
                RadialBlurEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(SceneTarget, g.PresentationParameters.Bounds, Color.White);
            spriteBatch.End();



        }
        private void RenderDebug()
        {
            //fill this with debug drawing...
            foreach (CharcoalEngine.Object.Object obj in Objects)
            {
                BoundingBox b = obj.UpdateBoundingBox(obj.model, obj.World);

                Vector3[] c = b.GetCorners();
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[0], c[1]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[1], c[2]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[2], c[3]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[3], c[0]);

                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[4], c[5]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[5], c[6]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[6], c[7]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[7], c[4]);

                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[0], c[4]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[1], c[5]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[2], c[6]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[3], c[7]);
            }
        }
        private void RenderEdit()
        {
            /*const int n = 360/2;
            float[,] h = new float[n, n];
            for (int y = 0; y < n; y++)
            {
                for (int x = 0; x < n; x++)
                {
                    h[x, y] = (float)Math.Sin((double)MathHelper.ToRadians(x*16))*10;
                }
            }
            LineUtility3D.DrawPlane(g, Camera.View, Camera.Projection, n-1, h, 0.1f);
                */
            if (SelectedModel != -1)
            {

                BoundingBox b = Objects[SelectedModel].UpdateBoundingBox(Objects[SelectedModel].model, Objects[SelectedModel].World);
                
                Vector3[] c = b.GetCorners();
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[0], c[1]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[1], c[2]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[2], c[3]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[3], c[0]);

                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[4], c[5]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[5], c[6]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[6], c[7]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[7], c[4]);

                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[0], c[4]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[1], c[5]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[2], c[6]);
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Red, c[3], c[7]);
                //
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Orange, Vector3.Transform(Vector3.Zero, Objects[SelectedModel].World), Vector3.Transform(Vector3.Up * 4, Objects[SelectedModel].World));
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Orange, Vector3.Transform(Vector3.Zero, Objects[SelectedModel].World), Vector3.Transform(Vector3.Forward * 4, Objects[SelectedModel].World));
                LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Orange, Vector3.Transform(Vector3.Zero, Objects[SelectedModel].World), Vector3.Transform(Vector3.Right * 4, Objects[SelectedModel].World));
            
            }
            if (SelectedMesh != -1)
            {
                BoundingSphere bs = Objects[SelectedModel].model.Meshes[SelectedMesh].BoundingSphere;
                Vector3 Center = bs.Center;
                Center = Vector3.Transform(Center, Objects[SelectedModel].model.Meshes[SelectedMesh].ParentBone.Transform * Objects[SelectedModel].World);

                for (int i = 0; i < 360; i++)
                {
                    LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Green, Center + new Vector3((float)Math.Cos((double)MathHelper.ToRadians(i)) * bs.Radius, (float)Math.Sin((double)MathHelper.ToRadians(i)) * bs.Radius, 0), Center + new Vector3((float)Math.Cos((double)MathHelper.ToRadians(i)) * bs.Radius, (float)Math.Sin((double)MathHelper.ToRadians(i+1)) * bs.Radius, 0));
                    LineUtility3D.Draw3DLine(g, Camera.View, Camera.Projection, Color.Green, Center + new Vector3(0, (float)Math.Sin((double)MathHelper.ToRadians(i)) * bs.Radius, (float)Math.Cos((double)MathHelper.ToRadians(i)) * bs.Radius), Center + new Vector3(0, (float)Math.Sin((double)MathHelper.ToRadians(i + 1)) * bs.Radius, (float)Math.Cos((double)MathHelper.ToRadians(i+1)) * bs.Radius));
                
                }

            }

        }
    }   
}
