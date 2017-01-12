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
using MJLGameEngine.Scene;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;

namespace MJLGameEngine.Object
{
    public class Object
    {
        public string Name
        { 
            get;
            private set;
        }
        public Model model;
        
        public Matrix Orientation;
        public Matrix World; //this is the final compiled 'World' Matrix

        public Texture2D texture; //temporary, for one-texture models only

        //physics
        public RigidBody body;
        public bool _IsStatic
        {
            get { return body.IsStatic; }
            set { body.IsStatic = value; }
        }

        public Vector3 _Position
        {
            get { return Conversion.ToXNAVector(body.Position); }
            set { body.Position = Conversion.ToJitterVector(value); }
        }

        //effects
        public float SpecularPower { get; set; }

        public Texture2D normalmap;
        public bool normalmap_enabled { get; set; }
        public float bump_height { get; set; }
        public bool use_dds_normal { get; set; }
        public bool Has_TransparentGeometry { get; set; }

        BoundingBox b;

        public Object(ContentManager Content, bool Static, string name, Vector3 pos, Vector3 ypr, float scale, Texture2D tex, Texture2D NormalMap, bool NormalMap_Enabled, bool UseDDSNormal)
        {
            Name = name;
            Vector3 Position = pos;
            Vector3 Scale = new Vector3(scale, scale, scale);
            Vector3 YawPitchRoll = new Vector3(ypr.X, ypr.Y, ypr.Z);
            Orientation = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(YawPitchRoll.X), MathHelper.ToRadians(YawPitchRoll.Y), MathHelper.ToRadians(YawPitchRoll.Z));
            World = Orientation * Matrix.CreateTranslation(Position);
            Model m = Content.Load<Model>(name);
            model = m;
            texture = tex;

            normalmap = NormalMap;
            normalmap_enabled = NormalMap_Enabled;
            bump_height = 2.4f;
            SpecularPower = 20;
            use_dds_normal = UseDDSNormal;
            Has_TransparentGeometry = false;
            
            if (texture == null)
            {
                for (int i = 0; i < model.Meshes.Count; i++)
                    for (int j = 0; j < model.Meshes[i].MeshParts.Count; j++)
                    {
                        Console.WriteLine(model.Meshes[i].MeshParts[j].Tag);
                        if (model.Meshes[i].MeshParts[j].Tag == null)
                            model.Meshes[i].MeshParts[j].Tag = (BasicEffect)(model.Meshes[i].MeshParts[j].Effect as BasicEffect).Clone();
                    
                    }
            }
            else
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        BasicEffect e = new BasicEffect(GameEngineData.game.GraphicsDevice);
                        e.Texture = tex;
                        e.Alpha = 1;
                        e.TextureEnabled = true;
                        e.DiffuseColor = Vector3.One;

                        part.Tag = (e as BasicEffect);
                    }
                }
            }

            b = UpdateBoundingBox(model, Matrix.Identity);
            BoxShape bshape = new BoxShape(Conversion.ToJitterVector((b.Max - b.Min)*Scale));
            //bshape.geomCen = Conversion.ToJitterVector(b.Max-b.Min);
            Position += Conversion.ToXNAVector(new JBBox(Conversion.ToJitterVector(b.Min), Conversion.ToJitterVector(b.Max)).Center);
            body = new RigidBody(bshape);

            body.IsStatic = Static;
            body.Position = Conversion.ToJitterVector(Position);
            body.Orientation = Conversion.ToJitterMatrix(Orientation);
        }
        public void Update()
        {
            Orientation = Matrix.CreateTranslation(-Conversion.ToXNAVector(new JBBox(Conversion.ToJitterVector(b.Min), Conversion.ToJitterVector(b.Max)).Center)) * Conversion.ToXNAMatrix(body.Orientation);
            World = Orientation * Matrix.CreateTranslation(Conversion.ToXNAVector(body.Position));
            body.Update();
        }
        public BoundingBox UpdateBoundingBox(Model model, Matrix worldTransform)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            // Create and return bounding box
            return new BoundingBox(min, max);
        }
    }
    /*public class EnvironmentalProperties
    {
        Effect effect;
        Material mat;
    }
    public class Material
    {
        public bool UseNormal;
        public float Shininess;
        public Texture2D Texture;
        public Texture2D NormalMap;
        public float Roughness;
        public bool Glass;
        public bool UseTexture;
        public bool Visible;
    }
    public class MeshTag
    {
        public Texture2D Texture { get; private set; }
        public float Alpha { get; private set; }
        public bool TextureEnabled { get; private set; }
        public Vector3 DiffuseColor { get; private set; }

        public BasicEffect oEffect;

        public MeshTag(Texture2D texture, float alpha, bool textureenabled, Vector3 diffusecolor, BasicEffect original)
        {
            Texture = texture;
            Alpha = alpha;
            TextureEnabled = textureenabled;
            DiffuseColor = diffusecolor;
            oEffect = original;
        }
    }*/
}
