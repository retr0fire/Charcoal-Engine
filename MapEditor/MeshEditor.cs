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
        public CharcoalEngine.Scene.Scene scene;
        public CharcoalEngine.Object.CharcoalModel model;

        public MeshEditor()
        {
            InitializeComponent();
        }

        public void RunAsMeshEditor(CharcoalEngine.Scene.Scene s, CharcoalEngine.Object.CharcoalModel m)
        {
            scene = s;
            Application.EnableVisualStyles();
            model = m;
            this.ShowDialog();
        }

        private void MeshEditor_Load(object sender, EventArgs e)
        {
            foreach (CharcoalEngine.Object.Mesh m in model.Meshes)
            {
                MeshTreeView.Nodes.Add(m.name);
            }
        }

        private void MeshTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MeshEditorView.SelectedObject = model.Meshes[e.Node.Index];
            scene.SelectedMesh = e.Node.Index;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            //this.
        }

        private void MeshEditorView_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //Transforms[MeshTreeView.SelectedNode.Index].Update();
        }
    }
    /*public class Transform
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
    }*/
}
