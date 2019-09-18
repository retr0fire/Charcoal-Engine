using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.IO;
using CharcoalEngine.Scene;
using CharcoalEngine;
using CharcoalEngine.Utilities;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Windows;
using System.Design;

namespace CharcoalEngine.Object
{
    public class OBJ_File
    {
        //Whether you want a nice verbose log dump on every load call:
        const bool LOG = false;

        //sharing is caring
        public List<Vertex> Vertices = new List<Vertex>();
        public List<TexCoord> TexCoords = new List<TexCoord>();
        public List<Normal> Normals = new List<Normal>();
        
        //only one?
        public MTLFile MTLfile;
                
        //always got his nose in a file...
        private StreamReader reader;
        private string foldername;
        private string location;
        private int Line_Number;
                     
        //yeah, stay in there!
        BoundingBox b;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_location"></param>
        /// <param name="GraphicsDevice"></param>
        /// <param name="__Position"></param>
        /// <param name="__YawPitchRoll"></param>
        /// <param name="__Scale"></param>
        /// <param name="__FlipAxis"></param>
        /// <param name="__IsStatic"></param>
        /// <param name="Root">Node to attach the meshes to</param>
        public void Load(string _location, GraphicsDevice GraphicsDevice, Vector3 __Position, Vector3 __YawPitchRoll, float __Scale, bool __FlipAxis, bool __IsStatic, Transform Root)
        {
            //Application.EnableVisualStyles();
            //DialogResult FlipResult = MessageBox.Show("Flip Y & Z Axis?", "Flip Axis?", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, 0);



            location = _location;
            reader = new System.IO.StreamReader(location);
            foldername = location;
            int num_removed = 0;
            for (int i = foldername.Length - 1; i > 0; i--)
            {
                if (foldername[i] != '\\')
                {
                    foldername = foldername.Remove(i, 1);
                    num_removed++;
                }
                else
                {
                    break;
                }
            }
            if (LOG)
            {
                Console.WriteLine("foldername : " + foldername);
                Console.WriteLine("file name : " + location);
            }
            Root.Name = location.Remove(0, location.Length - num_removed);

            string line;
            while (true)
            {
                if (reader.EndOfStream == true)
                    break;
                line = reader.ReadLine();
                if (line.StartsWith("#"))
                    if (LOG) Console.WriteLine("Comment: " + line.Remove(0, 1));
                if (line.StartsWith("v "))
                    ReadVertex(line);
                if (line.StartsWith("vt "))
                    ReadTexCoord(line);
                if (line.StartsWith("vn "))
                    ReadNormal(line);
                if (line.StartsWith("f "))
                {
                    if (Root.Children.Count == 0)
                    {
                        MessageBox.Show("Error, no groups");
                        return;
                    }
                    if (Root.Children[Root.Children.Count - 1].Children.Count == 0)
                    {
                        //no material has been specified yet, borrow the material from the last mesh
                        if (Root.Children.Count > 1)
                        {
                            Mesh mesh = new Mesh();
                            mesh.Load("error");
                            Root.Children[Root.Children.Count - 1].Children.Add(mesh);

                            mesh.Material = ((Mesh)Root.Children[Root.Children.Count - 2].Children[Root.Children[Root.Children.Count - 1].Children.Count - 1]).Material.Clone();
                            mesh.Name = ((Mesh)Root.Children[Root.Children.Count - 2].Children[Root.Children[Root.Children.Count - 1].Children.Count - 1]).Name;
                        }
                        
                    }
                    ReadFace(line, Root);
                }
                if (line.StartsWith("g "))
                {
                    if (LOG) Console.WriteLine("Group!");
                    Transform T = new Transform();
                    T.Name = line.Remove(0, 2);

                    Root.Children.Add(T);

                    /*Material mat = new Material();
                    mat.Load("null");

                    ((Mesh)Root.Children[Root.Children.Count - 1]).Material = mat;
                    ((Mesh)Root.Children[Root.Children.Count - 1]).Material.TextureEnabled = false;

                    if (Root.Children.Count > 1)
                    {
                        ((Mesh)Root.Children[Root.Children.Count - 1]).Material = ((Mesh)Root.Children[Root.Children.Count - 2]).Material.Clone();
                    }*/
                }
                if (line.StartsWith("usemtl "))
                {
                    //if a new group has just been added, don't add another...
                    //try
                    //{
                    //    if (((Mesh)Root.Children[Root.Children.Count - 1]).Faces.Count != 0)
                    //    {
                    if (Root.Children.Count == 0)
                    {
                        Transform T = new Transform();
                        T.Name = line.Remove(0, 7);

                        Root.Children.Add(T);
                    }
                    if (LOG) Console.WriteLine("Group!");
                    Mesh ms = new Mesh();
                    ms.Load(line.Remove(0, 7));
                    Root.Children[Root.Children.Count - 1].Children.Add(ms);
                    //Root.Children[Root.Children.Count - 1].Name = line.Remove(0, 7);
                    Material mat = new Material();
                    mat.Load("null");

                    ((Mesh)Root.Children[Root.Children.Count - 1].Children[Root.Children[Root.Children.Count - 1].Children.Count - 1]).Material = mat;
                    ((Mesh)Root.Children[Root.Children.Count - 1].Children[Root.Children[Root.Children.Count - 1].Children.Count - 1]).Material.TextureEnabled = false;
                    /*    }
                    }
                    catch
                    {
                        try
                        {
                            if (((Mesh)Root.Children[Root.Children.Count - 1]).Faces.Count == 0)
                                Root.Children.RemoveAt(Root.Children.Count - 1);
                            if (LOG) Console.WriteLine("Group!");
                            Mesh m = new Mesh();
                            m.Load(line.Remove(0, 2));
                            Root.Children.Add(m);

                            Material mat = new Material();
                            mat.Load("null");

                            ((Mesh)Root.Children[Root.Children.Count - 1]).Material = mat;
                            ((Mesh)Root.Children[Root.Children.Count - 1]).Material.TextureEnabled = false;
                        }
                        catch
                        {
                            Mesh m = new Mesh();
                            m.Load(line.Remove(0, 2));
                            Root.Children.Add(m);

                            Material mat = new Material();
                            mat.Load("null");

                            ((Mesh)Root.Children[Root.Children.Count - 1]).Material = mat;
                            ((Mesh)Root.Children[Root.Children.Count - 1]).Material.TextureEnabled = false;
                        }
                    }*/

                    if (LOG) Console.WriteLine("usemtl.....");
                    string materialname = line.Remove(0, 7);
                    foreach (Material m in MTLfile.Materials)
                    {
                        if (materialname == m.name)
                        {
                            if (LOG) Console.WriteLine("material found");
                            /*if (Root.Children.Count == 0)
                            {
                                Mesh mesh = new Mesh();
                                mesh.Load(materialname);
                                Root.Children.Add(mesh);
                                Material material = new Material();
                                material.Load("null");
                                ((Mesh)Root.Children[Root.Children.Count - 1]).Material = material;
                                ((Mesh)Root.Children[Root.Children.Count - 1]).Material.TextureEnabled = false;
                            }*/
                            ((Mesh)Root.Children[Root.Children.Count - 1].Children[Root.Children[Root.Children.Count - 1].Children.Count - 1]).Material = m.Clone();
                            break;
                        }
                    }
                }
                if (line.StartsWith("mtllib "))
                {
                    if (LOG) Console.WriteLine("mtllib!");
                    line = line.Remove(0, 7);
                    MTLFile mtl = new MTLFile();
                    if (mtl.Load(foldername + line, foldername, GraphicsDevice) == false) return;
                    MTLfile = mtl;
                }

                Line_Number++;

            }
            reader.Close();

            for (int group = 0; group < Root.Children.Count; group++)
            {
                Root.Children[group].Parent = Root;

                for (int sub = 0; sub < Root.Children[group].Children.Count; sub++)
                {
                    ((Mesh)Root.Children[group].Children[sub]).Parent = Root.Children[group];
                    ((Mesh)Root.Children[group].Children[sub]).Obj_File = this;
                    ((Mesh)Root.Children[group].Children[sub]).UpdateMesh();
                    ((Mesh)Root.Children[group].Children[sub]).UpdateBoundingBox();
                    ((Mesh)Root.Children[group].Children[sub]).UpdateMatrix();
                }

            }

            //now go through each group and center the position so that it is easy to select sections
            //Root.boundingBox = new BoundingBox(Vector3.Zero, Vector3.Zero);
            for (int group = 0; group < Root.Children.Count; group++)
            {
                //Root.Children[group].boundingBox = new BoundingBox(Vector3.Zero, Vector3.Zero);
                BoundingBox b = Root.GetBBox(Root.Children[group].Children[0].boundingBox, ((Mesh)(Root.Children[group].Children[0])).WBPosition);

                for (int sub = 0; sub < Root.Children[group].Children.Count; sub++)
                {
                    Root.Children[group].Children[sub].boundingBox = new BoundingBox(Vector3.Zero, Vector3.Zero);
                    b = BoundingBox.CreateMerged(b, Root.GetBBox(Root.Children[group].Children[sub].boundingBox, ((Mesh)(Root.Children[group].Children[sub])).WBPosition));
                    
                }

                for (int sub = 0; sub < Root.Children[group].Children.Count; sub++)
                {
                    Root.Children[group].Children[sub].Position = ((Mesh)(Root.Children[group].Children[sub])).WBPosition - (b.Max + b.Min) / 2;
                }
                //Root.Children[group].__boundingbox__.Min -= Root.Children[group].Position;
                //Root.Children[group].__boundingbox__.Max -= Root.Children[group].Position;
                Root.Children[group].Position = (b.Max + b.Min) / 2;

                //Root.Children[group].UpdateBoundingBox();
                //for (int sub = 0; sub < Root.Children[group].Children.Count; sub++)
                //{
                //    Root.Children[group].Children[sub].UpdateBoundingBox();
                //}

            }
            
            for (int group = 0; group < Root.Children.Count; group++)
            {
                Root.Children[group].Parent = Root;

                for (int sub = 0; sub < Root.Children[group].Children.Count; sub++)
                {
                    ((Mesh)Root.Children[group].Children[sub]).Parent = Root.Children[group];
                    ((Mesh)Root.Children[group].Children[sub]).Obj_File = this;
                    //((Mesh)Root.Children[group].Children[sub]).UpdateMesh();
                    ((Mesh)Root.Children[group].Children[sub]).UpdateBoundingBox();
                    //((Mesh)Root.Children[group].Children[sub]).UpdateMatrix();
                }

            }
            Root.Update();
            Root.UpdateBoundingBox();
            /*for (int group = 0; group < Root.Children.Count; group++)
            {
                Root.Children[group].Parent = Root;

                for (int sub = 0; sub < Root.Children[group].Children.Count; sub++)
                {
                    ((Mesh)Root.Children[group].Children[sub]).Parent = Root.Children[group];
                    ((Mesh)Root.Children[group].Children[sub]).Obj_File = this;
                    ((Mesh)Root.Children[group].Children[sub]).UpdateMesh();
                    ((Mesh)Root.Children[group].Children[sub]).UpdateBoundingBox();
                    ((Mesh)Root.Children[group].Children[sub]).UpdateMatrix();
                }

            }*/


            //if (FlipResult == DialogResult.Yes)
            //{
            //    YawPitchRoll.Y = -MathHelper.PiOver2;
            //}

            Root.Position = __Position;
            //YawPitchRoll = __YawPitchRoll;
            //Scale = new Vector3(__Scale);
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

            for (; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                y += l[i];
            }

            for (; i < l.Length; i++)
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

            for (; i < l.Length; i++)
            {
                if (l[i] == ' ')
                {
                    i++;
                    break;
                }
                y += l[i];
            }

            for (; i < l.Length; i++)
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

            for (; i < l.Length; i++)
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
        private void ReadFace(string l, Transform Root)
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

            for (; i < l.Length; i++)
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

            for (; i < l.Length; i++)
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
            for (; i < l.Length; i++)
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

            for (; i < l.Length; i++)
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

            for (; i < l.Length; i++)
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
            for (; i < l.Length; i++)
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

            for (; i < l.Length; i++)
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

            for (; i < l.Length; i++)
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
            for (; i < l.Length; i++)
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

            for (; i < l.Length; i++)
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

            for (; i < l.Length; i++)
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
                ((Mesh)(Root.Children[Root.Children.Count - 1]).Children[Root.Children[Root.Children.Count - 1].Children.Count-1]).Faces.Add(f2);

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
            ((Mesh)Root.Children[Root.Children.Count - 1].Children[Root.Children[Root.Children.Count - 1].Children.Count - 1]).Faces.Add(f);
        }                
    }
    /// <summary>
    /// The draw function requires these 6 parameters exist. The first target is the light target. 
    /// generally speaking draw white to your section if you are doing anything special
    /// the second is texture. Here draw what you want it to look like. If you are drawing flames, make the flames here. 
    /// The normal target should not need to be written to if you are using a custom effect.
    /// e.Parameters["World"].SetValue(AbsoluteWorld);
    /// e.Parameters["View"].SetValue(Camera.View);
    /// e.Parameters["Projection"].SetValue(Camera.Projection);
    /// e.Parameters["BasicTexture"].SetValue(Material.Texture);
    /// e.Parameters["TextureEnabled"].SetValue(Material._TextureEnabled);
    /// e.Parameters["DiffuseColor"].SetValue(Material.DiffuseColor);
    /// </summary>
    public class Mesh : Transform
    {
        //reference to OBJ file object
        public OBJ_File Obj_File;

        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public List<Material> Material_Edit
        {
            get
            {
                List<Material> T = new List<Material>();
                T.Add(Material);
                return T;
            }
            set
            {
                Material = value[0];
            }
        }


        [Editor(typeof(WindowsFormsComponentEditor), typeof(UITypeEditor))]
        [Browsable(true)]
        public Material Material
        {
            get
            {
                return __material__;
            }
            set
            {
                __material__ = value;
            }
        }
        Material __material__;
        public List<Face> Faces = new List<Face>();

        public VertexPositionNormalTexture[] V;

        public void Load(string n)
        {
            Name = n;
        }
        public Vector3 WBPosition;
        /*
        public void Update_Matrix()
        {
           MeshWorld = Matrix.CreateTranslation(-Center) * Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(YawPitchRoll.X, YawPitchRoll.Y, YawPitchRoll.Z) * Matrix.CreateTranslation(Center) * Matrix.CreateTranslation(Position);
        }
        */
        public void UpdateMesh()
        {
            boundingBox = new BoundingBox();
            //boundingSphere = 
            V = new VertexPositionNormalTexture[Faces.Count * 3/* * 2 */];

            List<Vector3> Points = new List<Vector3>();

            for (int i = 0; i < Faces.Count; i++)
            {

                Points.Add(Obj_File.Vertices[((Face)Faces[i]).fv[0].v1 - 1]._Vertex);
                Points.Add(Obj_File.Vertices[((Face)Faces[i]).fv[1].v1 - 1]._Vertex);
                Points.Add(Obj_File.Vertices[((Face)Faces[i]).fv[2].v1 - 1]._Vertex);
            }
            boundingBox = BoundingBox.CreateFromPoints(Points);
            __localboundingbox__ = new BoundingBox(boundingBox.Min - (boundingBox.Max + boundingBox.Min) / 2, boundingBox.Max - (boundingBox.Max + boundingBox.Min) / 2);
            WBPosition = (boundingBox.Max + boundingBox.Min) / 2;
            //Console.WriteLine(boundingBox);
            UpdateBoundingBox();

            for (int i = 0; i < Faces.Count; i++)
            {

                V[i * 3 + 2] = new VertexPositionNormalTexture(Obj_File.Vertices[Faces[i].fv[0].v1 - 1]._Vertex - (AbsolutePosition + WBPosition), Obj_File.Normals[Faces[i].fv[0].n1 - 1]._Normal, Obj_File.TexCoords[Faces[i].fv[0].t1 - 1]._TexCoord);
                V[i * 3 + 1] = new VertexPositionNormalTexture(Obj_File.Vertices[Faces[i].fv[1].v1 - 1]._Vertex - (AbsolutePosition + WBPosition), Obj_File.Normals[Faces[i].fv[1].n1 - 1]._Normal, Obj_File.TexCoords[Faces[i].fv[1].t1 - 1]._TexCoord);
                V[i * 3 + 0] = new VertexPositionNormalTexture(Obj_File.Vertices[Faces[i].fv[2].v1 - 1]._Vertex - (AbsolutePosition + WBPosition), Obj_File.Normals[Faces[i].fv[2].n1 - 1]._Normal, Obj_File.TexCoords[Faces[i].fv[2].t1 - 1]._TexCoord);

            }
            //Parent.Position = Position;
            //Position = Vector3.Zero;
            

        }
        public override void Draw(Effect e)
        {
            if (Faces.Count != 0)
            {
                if (Material.Visible)
                {
                    //if (Camera.Viewport.frAbsoluteBoundingBox)
                    //fill basic parameters
                    if (e != null)
                    {
                        if ((string)e.Tag == "NDT" && Material.AlphaEnabled == true)
                        {
                            base.Draw(e);
                            return;
                        }

                        if ((string)e.Tag == "ALPHANDT" && Material.AlphaEnabled == false)
                        {
                            base.Draw(e);
                            return;
                        }

                        e.Parameters["World"].SetValue(AbsoluteWorld);
                        e.Parameters["View"].SetValue(Camera.View);
                        e.Parameters["Projection"].SetValue(Camera.Projection);
                        e.Parameters["BasicTexture"].SetValue(Material.Texture);
                        e.Parameters["TextureEnabled"].SetValue(Material._TextureEnabled);
                        e.Parameters["NormalMap"].SetValue(Material.NormalMap);
                        e.Parameters["NormalMapEnabled"].SetValue(Material.NormalMapEnabled);
                        e.Parameters["DiffuseColor"].SetValue(Material.DiffuseColor);
                        e.Parameters["Alpha"].SetValue(Material.Alpha);
                        e.Parameters["AlphaEnabled"].SetValue(Material.AlphaEnabled);
                        
                        //...
                        e.CurrentTechnique.Passes[0].Apply();
                        e.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                        Engine.g.DrawUserPrimitives(PrimitiveType.TriangleList, V, 0, Faces.Count/* * 2 */);
                    }
                }
            }
            base.Draw(e);
        }
    }

    public struct Vertex
    {
        public Vector3 _Vertex;
        public int LineNumber;
    }
    public struct Normal
    {
        public Vector3 _Normal;
        public int LineNumber;
    }
    public struct TexCoord
    {
        public Vector2 _TexCoord;
        public int LineNumber;
    }
    public struct FaceVertex
    {
        public int v1;
        public int t1;
        public int n1;
    }

    public struct Face
    {
        public FaceVertex[] fv;// = new FaceVertex[3];
    }
    public class MTLFile
    {
        public List<Material> Materials = new List<Material>();

        public bool Load(string Path, string LocalFolder, GraphicsDevice g)
        {
            StreamReader reader;
            try
            {
                reader = new System.IO.StreamReader(Path);
            }
            catch
            {
                MessageBox.Show("This model has no MTL File, & is unusable");
                return false;
            }
            string line = "";
            while (true)
            {
                if (reader.EndOfStream == true)
                    break;
                line = reader.ReadLine();
                while (true)
                {
                    if (line.StartsWith(" "))
                        line = line.Remove(0);
                    else
                        break;
                }
                if (line.StartsWith("newmtl "))
                {
                    Material newmtl = new Material();
                    newmtl.Load(line.Remove(0, 7));
                    Materials.Add(newmtl);
                    Materials[Materials.Count - 1].TextureEnabled = false;
                }
                #region load_texture
                if (line.StartsWith("map_Kd "))
                {
                    Materials[Materials.Count - 1].TextureEnabled = true;

                    string texturename = LocalFolder + line.Remove(0, 7);
                    Materials[Materials.Count - 1].Texture = TextureImporter.LoadTextureFromFile(texturename);
                    Materials[Materials.Count - 1].TextureFileName = texturename;
                    if (Materials[Materials.Count - 1].Texture == null)
                        Materials[Materials.Count - 1].TextureEnabled = false;
                }
                if (line.StartsWith("map_bump "))
                {
                    Materials[Materials.Count - 1].NormalMapEnabled = true;

                    line = line.Remove(0, 9);

                    if (line.Length > 0)
                    {
                        while (line[0] == ' ')
                            line = line.Remove(0, 1);

                        string texturename = LocalFolder + line;

                        Materials[Materials.Count - 1].NormalMap = TextureImporter.LoadTextureFromFile(texturename);
                        //Materials[Materials.Count - 1].TextureFileName = texturename;
                    }
                    if (Materials[Materials.Count - 1].NormalMap == null)
                        Materials[Materials.Count - 1].NormalMapEnabled = false;
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

                    for (; c < dc.Length; c++)
                    {
                        if (dc[c] != ' ')
                            x += dc[c];
                        else
                        {
                            c++;
                            break;
                        }
                    }
                    for (; c < dc.Length; c++)
                    {
                        if (dc[c] != ' ')
                            y += dc[c];
                        else
                        {
                            c++;
                            break;
                        }
                    }
                    for (; c < dc.Length; c++)
                    {
                        if (dc[c] != ' ')
                            z += dc[c];
                        else
                        {
                            c++;
                            break;
                        }
                    }

                    //Console.WriteLine("Diffuse Color: " + x + " " + y + " " + z);
                    Materials[Materials.Count - 1].DiffuseColor = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
                }
                if (line.StartsWith("d "))//alpha
                {
                    // Ka 0.0470588
                    string a = line.Remove(0, 2);

                    while (a[0] == ' ')
                        a = a.Remove(0, 1);

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
                    if (Materials[Materials.Count - 1].Alpha < 1.0f) Materials[Materials.Count - 1].AlphaEnabled = true;
                }

            }
            reader.Close();
            return true;
        }
    }

    public class Material
    {
        public string name;

        public string TextureFileName;

        [BrowsableAttribute(true)]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string _Texture
        {
            get { return TextureFileName; }
            set {
                TextureEnabled = true;
                Texture = TextureImporter.LoadTextureFromFile(value);
                if (Texture == null)
                    TextureEnabled = false;
                TextureFileName = value;
            }
        }

        public Texture2D Texture;

        public bool _TextureEnabled
        {
            get { return TextureEnabled; }
            set { TextureEnabled = value; }
        }
        public bool TextureEnabled;


        public string NormalMapFileName;

        [BrowsableAttribute(true)]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string _NormalMap
        {
            get { return NormalMapFileName; }
            set
            {
                NormalMapEnabled = true;
                NormalMap = TextureImporter.LoadTextureFromFile(value);
                if (NormalMap == null)
                    NormalMapEnabled = false;
                NormalMapFileName = value;
            }
        }

        public Texture2D NormalMap;

        public bool _NormalMapEnabled
        {
            get { return NormalMapEnabled; }
            set { NormalMapEnabled = value; }
        }
        public bool NormalMapEnabled;

        public bool _Visible
        {
            get { return Visible; }
            set { Visible = value; }
        }
        public bool Visible = true;

        public bool AlphaEnabled { get; set; }

        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public System.Drawing.Color _DiffuseColor
        {
            get
            {
                return System.Drawing.Color.FromArgb((int)(DiffuseColor.X * 255), (int)(DiffuseColor.Z * 255), (int)(DiffuseColor.Z * 255));
            }
            set { DiffuseColor = new Vector3((float)value.R / 255.0f, (float)value.G / 255.0f, (float)value.B / 255.0f); }
        }
        public Vector3 DiffuseColor = Vector3.One;
        public Vector3 Ambient;

        public float _Alpha
        {
            get { return Alpha; }
            set { Alpha = value; }
        }
        public float Alpha = 1;
        
        //public Texture2D SpecularMap;
        //public bool SpecularMapEnabled = false;
        //public Texture2D BumpMap { get; set; }
        //public bool BumpMapEnabled { get; set; } = false;

        public Material()
        {
            
        }
        public void Load(string n)
        {
            name = n;
        }
        public Material Clone()
        {
            Material mat = new Material();
            mat.Load(name);
            mat.TextureFileName = TextureFileName;
            mat.Texture = Texture;
            mat.TextureEnabled = TextureEnabled;
            mat.Visible = Visible;
            mat.DiffuseColor = DiffuseColor;
            mat.Ambient = Ambient;
            mat.Alpha = Alpha;
            mat.AlphaEnabled = AlphaEnabled;
            mat.NormalMap = NormalMap;
            mat.NormalMapEnabled = NormalMapEnabled;

            return mat;
        }
    }
    /*public class VertexPositionNormalTangentTexture : IVertexType
    {
        public VertexDeclaration VertexDeclaration { get; }
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinate;
        public Vector3 Tangent;

        public VertexPositionNormalTangentTexture(Vector3 position, Vector3 normal, Vector2 textureCoordinate, Vector3 tangent)
        {
            Position = position;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
            Tangent = tangent;

            VertexDeclaration = new VertexDeclaration()
        }
    }*/
}
