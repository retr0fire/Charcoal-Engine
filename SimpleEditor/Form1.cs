using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using CharcoalEngine;
using CharcoalEngine.Scene;
using CharcoalEngine.Object;
using System.IO;

namespace SimpleEditor
{
    public partial class Form1 : Form
    {
        CharcoalEngine.Scene.Scene scene;

        public Form1()
        {
            InitializeComponent();
        }

        public void RunAsEditor(CharcoalEngine.Scene.Scene s)
        {
            Application.EnableVisualStyles();
            this.Show();
            scene = s;
            UpdateNodes();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            //this.Activated
            System.Windows.Forms.OpenFileDialog d = new System.Windows.Forms.OpenFileDialog();
            d.ValidateNames = true;
            d.CheckFileExists = true;
            d.Filter = "OBJ files (*.obj)|*.obj|All files (*.*)|*.*";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            // ObjectSelectedEvent

            OBJ_File obj = new OBJ_File();
            Transform T = new Transform();


            scene.Root.Children.Add(T);
            scene.Root.Update();
            obj.Load(d.FileName, Engine.g, new Vector3(0, 0, 0), Vector3.Zero, 1f, false, false, T);
            scene.Root.Update();

            UpdateNodes();
        }

        private void ObjectsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            /*int parents = 0;

            TreeNode n = e.Node;

            while (true)
            {
                n = n.Parent;
                parents++;
                if (n == null)
                    break;
            }

            List<CharcoalEngine.Object.Transform> objects = scene.Objects;
            TreeNodeCollection nodes = ObjectsTree.Nodes;


            for (int i = 0; i < parents; i++)
            {
                Root.Children = ObjectsTree.
            }

            Console.WriteLine(parents-1);*/
            ObjectsProperties.SelectedObject = e.Node.Tag;
        }

        public void UpdateNodes()
        {
            ObjectsTree.Nodes.Clear();
            CharcoalEngine.Object.Transform root = new CharcoalEngine.Object.Transform();
            root.Children = scene.Root.Children;
            UpdateChildren(root, ObjectsTree.Nodes);
        }
        public void UpdateChildren(CharcoalEngine.Object.Transform parent, TreeNodeCollection Nodes)
        {
            foreach (CharcoalEngine.Object.Transform obj in parent.Children)
            {
                Nodes.Add(obj.Name);
                Nodes[Nodes.Count - 1].Tag = obj;
                UpdateChildren(obj, Nodes[Nodes.Count - 1].Nodes);
            }
        }

        private void XML_Load_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog d = new System.Windows.Forms.OpenFileDialog();
            d.ValidateNames = true;
            d.CheckFileExists = true;
            d.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            scene.Root.Children.Add(DeSerialize(d.FileName));

            UpdateNodes();
        }
        private void XML_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.AddExtension = true;
            d.DefaultExt = ".xml";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            Serialize(scene.Root.Children[ObjectsTree.SelectedNode.Index], d.FileName);
        }

        public void Serialize(Transform transform, string filename)
        {
            List<Type> types = new List<Type>();
            types.Add(transform.GetType());
            Add_To_Type_List(transform, types);
            
            System.IO.StreamWriter typewriter = new System.IO.StreamWriter(filename + ".types");
            foreach (Type t in types)
            {
                typewriter.WriteLine(t.AssemblyQualifiedName);
            }
            typewriter.Close();

            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(transform.GetType(), types.ToArray());
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
            s.Serialize(sw.BaseStream, transform);
            sw.Close();
        }
        public Transform DeSerialize(string filename)
        {
            Type[] types = GetTypeList(filename + ".types");

            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(types[0], types);

            System.IO.StreamReader sr = new System.IO.StreamReader(filename);
            Transform t = (Transform)s.Deserialize(sr.BaseStream);
            sr.Close();
            return t;
        }

        public void Add_To_Type_List(Transform parent, List<Type> Types)
        {
            foreach (CharcoalEngine.Object.Transform obj in parent.Children)
            {
                Types.Add(obj.GetType());
                Add_To_Type_List(obj, Types);
            }
        }
        public Type[] GetTypeList(string filename)
        {
            List<Type> Types = new List<Type>();

            System.IO.StreamReader sr = new System.IO.StreamReader(filename);
            while(true)
            {
                Types.Add(Type.GetType(sr.ReadLine(), true));
                Console.WriteLine(Types[Types.Count - 1].ToString());
                if (sr.EndOfStream)
                    break;
            }
            return Types.ToArray();
        }

        private void XML_Load_Scene_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog d = new System.Windows.Forms.OpenFileDialog();
            d.ValidateNames = true;
            d.CheckFileExists = true;
            d.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            /*StreamReader sr = new StreamReader(d.FileName);
            scene.Root.Children.Clear();
            while(true)
            {
                scene.Root.Children.Add(DeSerialize(sr.ReadLine()));
                if (sr.EndOfStream)
                    break;
            }
            sr.Close();*/

            scene.Root = DeSerialize(d.FileName);

            UpdateNodes();
        }
        private void XML_Save_Scene_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog d = new SaveFileDialog();
            d.AddExtension = true;
            d.DefaultExt = ".xml";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
/*
            StreamWriter sw = new StreamWriter(d.FileName);
            string dir = Get_Directory(d.FileName);
            int n = 0;
            foreach (Transform t in scene.Root.Children)
            {
                sw.WriteLine(dir + t.Name + "_" + n + ".xml");
                Serialize(t, dir + t.Name + "_" + n + ".xml");
                n++;
            }
            sw.Close();*/
            
            Serialize(scene.Root, d.FileName);

        }

        public string Get_Directory(string filename)
        {
             while (true)
            {
                Console.WriteLine(filename);
                if (filename[filename.Length - 1] == '\\') break;
                filename = filename.Remove(filename.Length - 1, 1);
            }
            return filename;
        }

        private void Add_Light_Click(object sender, EventArgs e)
        {
            scene.Root.Children.Add(new PointLight());
            UpdateNodes();
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            UpdateNodes();
        }

        private void List_Up_Click(object sender, EventArgs e)
        {
            try
            {
                Transform T = (Transform)ObjectsTree.SelectedNode.Tag;
                T.Parent.Parent.Children.Add(T);
                T.Parent.Children.Remove(T);
                T.Update();
                T.Parent.Update();
                try
                {
                    T.Parent.Parent.Update();
                }
                catch
                {

                }
                UpdateNodes();
            }
            catch
            {
                UpdateNodes();
            }
        }

        private void List_Down_Click(object sender, EventArgs e)
        {

        }

        private void List_Left_Click(object sender, EventArgs e)
        {

        }

        private void List_Right_Click(object sender, EventArgs e)
        {

        }

        private void List_Remove_Click(object sender, EventArgs e)
        {
            Transform T = (Transform)ObjectsTree.SelectedNode.Tag;
            T.Parent.Children.Remove(T);
            T.Parent.Update();
            UpdateNodes();
        }
    }
}