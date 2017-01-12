using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MapEditor
{
    public partial class MeshEditor : Form
    {
        public List<Transform> Transforms = new List<Transform>();
        public MJLGameEngine.Scene.Scene scene;

        public MeshEditor()
        {
            InitializeComponent();
        }

        public void RunAsMeshEditor(MJLGameEngine.Scene.Scene s, Model m)
        {
            scene = s;
            foreach (ModelMesh mesh in m.Meshes)
            {
                
                Transforms.Add(new Transform(mesh.ParentBone.Transform));
            }
            Application.EnableVisualStyles();
            this.ShowDialog();
        }

        private void MeshEditor_Load(object sender, EventArgs e)
        {
            int meshnumber = 0;
            foreach (Transform m in Transforms)
            {
                MeshTreeView.Nodes.Add("Mesh" + " " + meshnumber);
                meshnumber++;
            }
        }

        private void MeshTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MeshEditorView.SelectedObject = Transforms[e.Node.Index];
            scene.SelectedMesh = e.Node.Index;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            //this.
        }

        private void MeshEditorView_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Transforms[MeshTreeView.SelectedNode.Index].Update();
        }
    }
    public class Transform
    {
        public Vector3 _YawPitchRoll
        {
            get { return YPR; }
            set { YPR = value; }
        }
        public Vector3 YPR = Vector3.Zero;

        public Vector3 _Position
        {
            get { return Position; }
            set { Position = value; }
        }
        public Vector3 Position = Vector3.Zero;

        public Vector3 _Scale
        {
            get { return Scale; }
            set { Scale = value; }
        }
        public Vector3 Scale = Vector3.One;

        public Matrix OutTransform = Matrix.Identity;

        public Transform(Matrix t)
        {
            Position = t.Translation;
        }

        public void Update()
        {
            OutTransform = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(YPR.X), MathHelper.ToRadians(YPR.Y), MathHelper.ToRadians(YPR.Z)) * Matrix.CreateTranslation(Position);
        }
    }
}
