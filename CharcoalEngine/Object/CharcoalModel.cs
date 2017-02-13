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
using CharcoalEngine.Scene;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;

using System.IO;
using System.Windows.Forms;

using System.Drawing;

namespace CharcoalEngine.Object
{
    public class CharcoalModel
    {
        public List<Vertex> Vertices = new List<Vertex>();
        public List<TexCoord> TexCoords = new List<TexCoord>();
        public List<Normal> Normals = new List<Normal>();
        public List<Mesh> Meshes = new List<Mesh>();
        
        public MTLFile MTLfile;
        private StreamReader reader;
        private string foldername;
        private string location;
        private int Line_Number;

        public string FileName;
        //public RigidBody body;
        /*public bool _IsStatic
        {
            get { return body.IsStatic; }
            set { body.IsStatic = value; }
        }*/

        public Vector3 _Position
        {
            get { return Position; }
            set { Position = value; }
        }
        public Vector3 Position;

        public Vector3 _Center
        {
            get { return Center; }
            set { Center = value; }
        }
        public Vector3 Center;

        public Vector3 _YawPitchRoll
        {
            get { return YawPitchRoll; }
            set { YawPitchRoll = value; }
        }
        public Vector3 YawPitchRoll;
        public float _Scale
        {
            get { return Scale; }
            set { Scale = value; }
        }
        public float Scale = 1.0f;

        //public Matrix Orientation;
        public Matrix World; //this is the final compiled 'World' Matrix

        public void Update()
        {
            World = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z) * Matrix.CreateTranslation(Position);
            foreach (Mesh m in Meshes)
            {
                m.UpdateMatrix();
            }
        
        }

        BasicEffect effect;

        BoundingBox b;
        
        public CharcoalModel(string _location, GraphicsDevice GraphicsDevice, Vector3 __Position, Vector3 __YawPitchRoll, float __Scale, bool __FlipAxis, bool __IsStatic)
        {
            //Application.EnableVisualStyles();
            //DialogResult FlipResult = MessageBox.Show("Flip Y & Z Axis?", "Flip Axis?", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, 0);



            location = _location;
            reader = new System.IO.StreamReader(location);
            foldername = location;
            for (int i = foldername.Length - 1; i > 0; i--)
            {
                if (foldername[i] != '\\')
                {
                    foldername = foldername.Remove(i, 1);
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("foldername : " + foldername);

            Console.WriteLine("file name : " + location);

            FileName = location;

            string line;
            while (true)
            {
                if (reader.EndOfStream == true)
                    break;
                line = reader.ReadLine();
                if (line.StartsWith("#"))
                    Console.WriteLine("Comment: " + line.Remove(0, 1));
                if (line.StartsWith("v "))
                    ReadVertex(line);
                if (line.StartsWith("vt "))
                    ReadTexCoord(line);
                if (line.StartsWith("vn "))
                    ReadNormal(line);
                if (line.StartsWith("f "))
                {
                    if (Meshes.Count == 0)
                    {
                        MessageBox.Show("Error, no g");
                        return;
                    }
                    ReadFace(line);
                }
                if (line.StartsWith("g "))
                {
                    Console.WriteLine("Group!");
                    Meshes.Add(new Mesh(line.Remove(0, 2)));
                    Meshes[Meshes.Count - 1].mat = new Material("null");
                    Meshes[Meshes.Count - 1].mat.TextureEnabled = false;
                }
                if (line.StartsWith("usemtl "))
                {
                    Console.WriteLine("usemtl.....");
                    string materialname = line.Remove(0, 7);
                    foreach (Material m in MTLfile.Materials)
                    {
                        if (materialname == m.name)
                        {
                            Console.WriteLine("material found");
                            if (Meshes.Count == 0)
                            {
                                Meshes.Add(new Mesh(materialname));
                                Meshes[Meshes.Count - 1].mat = new Material("null");
                                Meshes[Meshes.Count - 1].mat.TextureEnabled = false;
                            }
                            Meshes[Meshes.Count - 1].mat = m.Clone();
                            break;
                        }
                    }
                }
                if (line.StartsWith("mtllib "))
                {
                    Console.WriteLine("mtllib!");
                    line = line.Remove(0, 7);
                    MTLfile = new MTLFile(foldername + line, foldername, GraphicsDevice);
                }

                Line_Number++;

            }
            reader.Close();

            for (int group = 0; group < Meshes.Count; group++)
            {
                Meshes[group].UpdateMesh(Vertices, TexCoords, Normals);
                b = BoundingBox.CreateMerged(b, Meshes[group].boundingBox);
            }
            //if (FlipResult == DialogResult.Yes)
            //{
            //    YawPitchRoll.Y = -MathHelper.PiOver2;
            //}

            Position = __Position;
            YawPitchRoll = __YawPitchRoll;
            Scale = __Scale;
        }

        private void ReadVertex(string l)
        {
            Vertex v = new Vertex();
            l = l.Remove(0, 2);

            while (l[0] == ' ')
                l = l.Remove(0, 1);

            string x = "";
            string y = "";
            string z = "";

            int i = 0;

            for (i = 0; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                x += l[i];
            }

            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                y += l[i];
            }

            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                z += l[i];
            }
            /*Console.WriteLine("------------");
            Console.Write("Vertex ");
            Console.Write(x + " ");
            Console.Write(y + " ");
            Console.Write(z);
            Console.WriteLine(" ");*/
            v.LineNumber = Line_Number;
            v._Vertex = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
            Vertices.Add(v);
        }
        private void ReadNormal(string l)
        {
            Normal n = new Normal();
            l = l.Remove(0, 3);

            while (l[0] == ' ')
                l = l.Remove(0, 1);

            string x = "";
            string y = "";
            string z = "";

            int i = 0;

            for (i = 0; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                x += l[i];
            }

            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                y += l[i];
            }

            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                z += l[i];
            }
            /*Console.WriteLine("------------");
            Console.Write("Normal ");
            Console.Write(x + " ");
            Console.Write(y + " ");
            Console.Write(z);
            Console.WriteLine(" ");*/
            n.LineNumber = Line_Number;
            n._Normal = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
            Normals.Add(n);
        }
        private void ReadTexCoord(string l)
        {
            TexCoord v = new TexCoord();
            l = l.Remove(0, 3);

            while (l[0] == ' ')
                l = l.Remove(0, 1);

            string x = "";
            string y = "";

            int i = 0;

            for (i = 0; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                x += l[i];
            }

            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                y += l[i];
            }

            /*Console.WriteLine("------------");
            Console.Write("TexCoord: ");
            Console.Write(x + " ");
            Console.Write(y);
            Console.WriteLine(" ");*/
            v.LineNumber = Line_Number;

            float tx, ty;
            tx = 0;
            ty = 0;

            try
            {
                tx = float.Parse(x);
                ty = float.Parse(y);
            }
            catch
            {

            }

            v._TexCoord = new Vector2(tx, 1 - ty);
            TexCoords.Add(v);
        }
        private void ReadFace(string l)
        {
            l = l.Remove(0, 2);
            while (l.StartsWith(" "))
            {
                l = l.Remove(0, 1);
            }
            string x = "";
            string y = "";
            string z = "";

            FaceVertex[] fv = new FaceVertex[3];
            fv[0] = new FaceVertex();
            fv[1] = new FaceVertex();
            fv[2] = new FaceVertex();
            #region fv1
            x = y = z = "";

            int i = 0;

            for (i = 0; i < l.Length; i++)
            {
                if (l[i] == '/')
                {
                    i++;
                    break;
                }
                if (l[i] == ' ')
                {
                    break;
                }
                x += l[i];
            }

            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == '/')
                {
                    i++;
                    break;
                }
                if (l[i] == ' ')
                {
                    break;
                }
                y += l[i];
            }

            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                z += l[i];
            }
            //
            if (x == "") { x = "0"; }
            fv[0].v1 = int.Parse(x);
            if (y == "")
            {
                TexCoord n = new TexCoord();
                n._TexCoord = Vector2.Zero;
                if (TexCoords.Count == 0)
                    TexCoords.Add(n);
                y = "1";
            }
            fv[0].t1 = int.Parse(y);
            if (z == "")
            {
                Normal n = new Normal();
                n._Normal = Vector3.Zero;
                if (Normals.Count == 0)
                    Normals.Add(n);
                z = "1";
            }
            fv[0].n1 = int.Parse(z);
            #endregion
            #region fv2
            x = y = z = "";
            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == '/')
                {
                    i++;
                    break;
                }
                if (l[i] == ' ')
                {
                    break;
                }
                x += l[i];
            }

            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == '/')
                {
                    i++;
                    break;
                }
                if (l[i] == ' ')
                {
                    break;
                }
                y += l[i];
            }

            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                z += l[i];
            }
            //
            if (x == "") { x = "0"; }
            fv[1].v1 = int.Parse(x);
            if (y == "")
            {
                TexCoord n = new TexCoord();
                n._TexCoord = Vector2.Zero;
                if (TexCoords.Count == 0)
                    TexCoords.Add(n);
                y = "1";
            }
            fv[1].t1 = int.Parse(y);
            if (z == "")
            {
                Normal n = new Normal();
                n._Normal = Vector3.Zero;
                if (Normals.Count == 0)
                    Normals.Add(n);
                z = "1";
            }
            fv[1].n1 = int.Parse(z);
            #endregion
            #region fv3
            x = y = z = "";
            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == '/')
                {
                    i++;
                    break;
                }
                if (l[i] == ' ')
                {
                    break;
                }
                x += l[i];
            }

            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == '/')
                {
                    i++;
                    break;
                }
                if (l[i] == ' ')
                {
                    break;
                }
                y += l[i];
            }

            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                z += l[i];
            }
            //
            if (x == "") { x = "0"; }
            fv[2].v1 = int.Parse(x);
            if (y == "")
            {
                TexCoord n = new TexCoord();
                n._TexCoord = Vector2.Zero;
                if (TexCoords.Count == 0)
                    TexCoords.Add(n);
                y = "1";
            }
            fv[2].t1 = int.Parse(y);
            if (z == "")
            {
                Normal n = new Normal();
                n._Normal = Vector3.Zero;
                if (Normals.Count == 0)
                    Normals.Add(n);
                z = "1";
            }
            fv[2].n1 = int.Parse(z);
            #endregion
            #region fv4?
            x = y = z = "";
            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == '/')
                {
                    i++;
                    break;
                }
                if (l[i] == ' ')
                {
                    break;
                }
                x += l[i];
            }

            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == '/')
                {
                    i++;
                    break;
                }
                if (l[i] == ' ')
                {
                    break;
                }
                y += l[i];
            }

            for (int useless = 0; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                z += l[i];
            }
            //
            if (x != "")
            {
                int v1 = int.Parse(x);
                if (y == "")
                {
                    TexCoord n = new TexCoord();
                    n._TexCoord = Vector2.Zero;
                    if (TexCoords.Count == 0)
                        TexCoords.Add(n);
                    y = "1";
                }
                int t1 = int.Parse(y);
                if (z == "")
                {
                    Normal n = new Normal();
                    n._Normal = Vector3.Zero;
                    if (Normals.Count == 0)
                        Normals.Add(n);
                    z = "1";
                }
                int n1 = int.Parse(z);



                FaceVertex[] fv2 = new FaceVertex[3];
                fv2[0] = fv[0];
                fv2[1] = fv[2];
                fv2[2] = new FaceVertex();
                fv2[2].n1 = n1;
                fv2[2].v1 = v1;
                fv2[2].t1 = t1;

                Face f2 = new Face();
                f2.fv = fv2;
                Meshes[Meshes.Count - 1].Faces.Add(f2);

            }
            else
            {
                //Console.WriteLine("did not add 4th face!!!");
            }
            #endregion


            /*Console.WriteLine("------------");
            Console.Write("Face: ");
            Console.Write(fv[0].v1 + "/" + fv[0].t1 + "/" + fv[0].n1 + " ");
            Console.Write(fv[1].v1 + "/" + fv[1].t1 + "/" + fv[1].n1 + " ");
            Console.Write(fv[2].v1 + "/" + fv[2].t1 + "/" + fv[2].n1 + " ");
            Console.WriteLine(" ");*/
            Face f = new Face();
            f.fv = fv;
            Meshes[Meshes.Count - 1].Faces.Add(f);
        }
        public void DrawModel(GraphicsDevice GraphicsDevice, Matrix World, Matrix View, Matrix Projection)
        {
            Matrix w = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z) * Matrix.CreateTranslation(Position);

            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
            for (int g = 0; g < Meshes.Count; g++)
            {
                Matrix gw = Matrix.CreateTranslation(-Meshes[g].Center) * Matrix.CreateScale(Meshes[g].Scale) * Matrix.CreateFromYawPitchRoll(Meshes[g].YawPitchRoll.X, Meshes[g].YawPitchRoll.Y, Meshes[g].YawPitchRoll.Z) * Matrix.CreateTranslation(Meshes[g].Center) * Matrix.CreateTranslation(Meshes[g].Position);

                if (effect == null)
                    effect = new BasicEffect(GraphicsDevice);
                effect.World = gw * w * World;
                effect.View = View;
                effect.Projection = Projection;
                effect.DiffuseColor = Microsoft.Xna.Framework.Color.White.ToVector3();
                //
                effect.DiffuseColor = Meshes[g].mat.DiffuseColor;
                effect.Alpha = Meshes[g].mat.Alpha;
                GraphicsDevice.BlendState = BlendState.AlphaBlend;
                effect.EnableDefaultLighting();
                effect.PreferPerPixelLighting = true;
                effect.Texture = Meshes[g].mat.Texture;
                effect.TextureEnabled = Meshes[g].mat.TextureEnabled;

                effect.CurrentTechnique.Passes[0].Apply();

                //VertexPositionNormalTexture[] V = new VertexPositionNormalTexture[Meshes[g].Faces.Count * 3/* * 2 */];
                /*for (int i = 0; i < Meshes[g].Faces.Count; i++)
                {

                    //Face f = Faces[462];
                    V[i * 3 + 2] = new VertexPositionNormalTexture(Vertices[Meshes[g].Faces[i].fv[0].v1 - 1]._Vertex, Normals[Meshes[g].Faces[i].fv[0].n1 - 1]._Normal, TexCoords[Meshes[g].Faces[i].fv[0].t1 - 1]._TexCoord);
                    V[i * 3 + 1] = new VertexPositionNormalTexture(Vertices[Meshes[g].Faces[i].fv[1].v1 - 1]._Vertex, Normals[Meshes[g].Faces[i].fv[1].n1 - 1]._Normal, TexCoords[Meshes[g].Faces[i].fv[1].t1 - 1]._TexCoord);
                    V[i * 3 + 0] = new VertexPositionNormalTexture(Vertices[Meshes[g].Faces[i].fv[2].v1 - 1]._Vertex, Normals[Meshes[g].Faces[i].fv[2].n1 - 1]._Normal, TexCoords[Meshes[g].Faces[i].fv[2].t1 - 1]._TexCoord);

                }
                for (int i = 0; i < Meshes[g].Faces.Count; i++)
                {
                    V[Meshes[g].Faces.Count * 3 + i * 3 + 0] = new VertexPositionNormalTexture(Vertices[Meshes[g].Faces[i].fv[0].v1 - 1]._Vertex, Normals[Meshes[g].Faces[i].fv[0].n1 - 1]._Normal, TexCoords[Meshes[g].Faces[i].fv[0].t1 - 1]._TexCoord);
                    V[Meshes[g].Faces.Count * 3 + i * 3 + 1] = new VertexPositionNormalTexture(Vertices[Meshes[g].Faces[i].fv[1].v1 - 1]._Vertex, Normals[Meshes[g].Faces[i].fv[1].n1 - 1]._Normal, TexCoords[Meshes[g].Faces[i].fv[1].t1 - 1]._TexCoord);
                    V[Meshes[g].Faces.Count * 3 + i * 3 + 2] = new VertexPositionNormalTexture(Vertices[Meshes[g].Faces[i].fv[2].v1 - 1]._Vertex, Normals[Meshes[g].Faces[i].fv[2].n1 - 1]._Normal, TexCoords[Meshes[g].Faces[i].fv[2].t1 - 1]._TexCoord);

                }*/
                if (Meshes[g].Faces.Count != 0)
                {
                    if (Meshes[g].mat.Visible)
                    {
                        GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Meshes[g].V, 0, Meshes[g].Faces.Count/* * 2 */);
                    }
                }

            }
        }
   

    }
    public class Mesh
    {
        public string name;
        public Material mat;
        public Mesh ParentMesh;
        public BoundingBox boundingBox = new BoundingBox();

        public Vector3 _Position
        {
            get { return Position; }
            set
            {
                Position = value;
                UpdateMatrix();
            }
        }
        public Vector3 Position;

        public Vector3 _Center
        {
            get { return Center; }
            set
            {
                Center = value;
                UpdateMatrix();
            }
        }
        public Vector3 Center;

        public Vector3 _YawPitchRoll
        {
            get { return YawPitchRoll; }
            set
            {
                YawPitchRoll = value;
                UpdateMatrix();
            }
        }
        public Vector3 YawPitchRoll;
        public float _Scale
        {
            get { return Scale; }
            set
            {
                Scale = value;
                UpdateMatrix();
            }
        }
        public float Scale = 1.0f;

        public Matrix MeshWorld;

        public List<Face> Faces = new List<Face>();

        public VertexPositionNormalTexture[] V;

        public Mesh(string n)
        {
            name = n;
        }

        public void UpdateMatrix()
        {
            MeshWorld = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(-Center) * Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z) * Matrix.CreateTranslation(Center) * Matrix.CreateTranslation(Position);
        }
        public void UpdateMesh(List<Vertex> Vertices, List<TexCoord> TexCoords, List<Normal> Normals)
        {
            boundingBox = new BoundingBox();
            //boundingSphere = 
            V = new VertexPositionNormalTexture[Faces.Count * 3/* * 2 */];

            List<Vector3> Points = new List<Vector3>();

            for (int i = 0; i < Faces.Count; i++)
            {

                Points.Add(Vertices[Faces[i].fv[0].v1 - 1]._Vertex);
                //Face f = Faces[462];
                V[i * 3 + 2] = new VertexPositionNormalTexture(Vertices[Faces[i].fv[0].v1 - 1]._Vertex, Normals[Faces[i].fv[0].n1 - 1]._Normal, TexCoords[Faces[i].fv[0].t1 - 1]._TexCoord);
                V[i * 3 + 1] = new VertexPositionNormalTexture(Vertices[Faces[i].fv[1].v1 - 1]._Vertex, Normals[Faces[i].fv[1].n1 - 1]._Normal, TexCoords[Faces[i].fv[1].t1 - 1]._TexCoord);
                V[i * 3 + 0] = new VertexPositionNormalTexture(Vertices[Faces[i].fv[2].v1 - 1]._Vertex, Normals[Faces[i].fv[2].n1 - 1]._Normal, TexCoords[Faces[i].fv[2].t1 - 1]._TexCoord);

            }

            boundingBox = BoundingBox.CreateFromPoints(Points);
            _Center = (boundingBox.Max + boundingBox.Min) / 2;
        }

    }

    public class Vertex
    {
        public Vector3 _Vertex;
        public int LineNumber;
    }
    public class Normal
    {
        public Vector3 _Normal;
        public int LineNumber;
    }
    public class TexCoord
    {
        public Vector2 _TexCoord;
        public int LineNumber;
    }
    public class FaceVertex
    {
        public int v1;
        public int t1;
        public int n1;
    }

    public class Face
    {
        public FaceVertex[] fv = new FaceVertex[3];
    }
    public class MTLFile
    {
        public List<Material> Materials = new List<Material>();

        public MTLFile(string Path, string LocalFolder, GraphicsDevice g)
        {
            StreamReader reader = new System.IO.StreamReader(Path);

            string line = "";
            while (true)
            {
                if (reader.EndOfStream == true)
                    break;
                line = reader.ReadLine();
                if (line.StartsWith("newmtl "))
                {
                    Materials.Add(new Material(line.Remove(0, 7)));
                    Materials[Materials.Count - 1].TextureEnabled = false;
                }
                #region load_texture
                if (line.StartsWith("map_Kd "))
                {
                    Materials[Materials.Count - 1].TextureEnabled = true;
                    string texturename = LocalFolder + line.Remove(0, 7);
                    try
                    {
                        Materials[Materials.Count - 1].Texture = Texture2D.FromStream(g, new System.IO.StreamReader(texturename).BaseStream);
                    }
                    catch
                    {
                        Image image;
                        try
                        {
                            image = Image.FromFile(texturename);


                            for (int i = texturename.Length - 1; i > 0; i--)
                            {
                                if (texturename[i] != '.')
                                    texturename = texturename.Remove(texturename.Length - 1, 1);
                                else
                                {
                                    texturename = texturename.Remove(texturename.Length - 1, 1);
                                    break;
                                }
                            }
                            try
                            {
                                texturename += ".png";
                                try
                                {
                                    image.Save(texturename, System.Drawing.Imaging.ImageFormat.Png);
                                }
                                catch
                                {
                                    Console.WriteLine("Image save failed, possible file already exists...");
                                }
                                Materials[Materials.Count - 1].TextureEnabled = true;
                                Materials[Materials.Count - 1].Texture = Texture2D.FromStream(g, new System.IO.StreamReader(texturename).BaseStream);
                                Console.WriteLine("__________________" + texturename);
                            }
                            catch
                            {
                                Materials[Materials.Count - 1].TextureEnabled = false;
                                Console.WriteLine("FAIL!!!!! ++++++  " + texturename);
                            }


                            Materials[Materials.Count - 1].TextureFileName = texturename;
                        }
                        catch
                        {
                            Materials[Materials.Count - 1].TextureEnabled = false;
                            Console.WriteLine("Fatal Error, specified texture does not exist: " + texturename);
                        }
                    }
                }
                #endregion
                if (line.StartsWith("Kd "))//diffuse color
                {
                    // Kd 0.0470588 0.447059 0.133333
                    string dc = line.Remove(0, 3);

                    while (dc[0] == ' ')
                        dc = dc.Remove(0, 1);

                    string x = "", y = "", z = "";

                    int c = 0;

                    for (int useless = 0; c < dc.Length; c++)
                    {
                        if (dc[c] != ' ')
                            x += dc[c];
                        else
                        {
                            c++;
                            break;
                        }
                    }
                    for (int useless = 0; c < dc.Length; c++)
                    {
                        if (dc[c] != ' ')
                            y += dc[c];
                        else
                        {
                            c++;
                            break;
                        }
                    }
                    for (int useless = 0; c < dc.Length; c++)
                    {
                        if (dc[c] != ' ')
                            z += dc[c];
                        else
                        {
                            c++;
                            break;
                        }
                    }

                    Console.WriteLine("Diffuse Color: " + x + " " + y + " " + z);
                    Materials[Materials.Count - 1].DiffuseColor = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
                }
                /*if (line.StartsWith("Ka "))//alpha
                {
                    // Ka 0.0470588
                    string a = line.Remove(0, 3);

                    string alpha = "";

                    for (int c = 0; c < a.Length; c++)
                    {
                        if (a[c] != ' ')
                            alpha += a[c];
                        else
                        {
                            c++;
                            break;
                        }
                    }
                    Console.WriteLine("Alpha: " + alpha);
                    Materials[Materials.Count - 1].Alpha = float.Parse(alpha);
                }*/

            }
            reader.Close();
        }
    }
    public class Material
    {
        public string name;


        public string TextureFileName;
        public Texture2D Texture;

        public bool _TextureEnabled
        {
            get { return TextureEnabled; }
            set { TextureEnabled = value; }
        }
        public bool TextureEnabled;

        public bool _Visible
        {
            get { return Visible; }
            set { Visible = value; }
        }
        public bool Visible = true;

        public Vector3 _DiffuseColor
        {
            get { return DiffuseColor; }
            set { DiffuseColor = value; }
        }
        public Vector3 DiffuseColor;
        public Vector3 Ambient;

        public float _Alpha
        {
            get { return Alpha; }
            set { Alpha = value; }
        }
        public float Alpha = 1;
        /*Vector3 SpecularColor;
        Texture2D SpecularMap;
        bool SpecularMapEnabled;
        Texture2D BumpMap;
        bool BumpMapEnabled;*/

        public Material(string n)
        {
            name = n;
        }
        public Material Clone()
        {
            Material mat = new Material(name);

            mat.TextureFileName = TextureFileName;
            mat.Texture = Texture;
            mat.TextureEnabled = TextureEnabled;
            mat.Visible = Visible;
            mat.DiffuseColor = DiffuseColor;
            mat.Ambient = Ambient;
            mat.Alpha = Alpha;

            return mat;
        }
    }
}
