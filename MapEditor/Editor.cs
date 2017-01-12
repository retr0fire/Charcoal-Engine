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
    public partial class Editor : Form
    {
        MJLGameEngine.Scene.Scene scene;
        public Editor()
        {
            InitializeComponent();
        }

        public void RunAsEditor(MJLGameEngine.Scene.Scene s)
        {
            Application.EnableVisualStyles();
            this.Show();
            scene = s;
            foreach (MJLGameEngine.Object.Object obj in scene.Objects)
            {
                ObjectsTree.Nodes.Add(obj.Name);
            }
            foreach (MJLGameEngine.Scene.DirectionalLight obj in scene.DirectionalLights)
            {
                LightsTree.Nodes.Add("Directional Light");
            }
            foreach (MJLGameEngine.Scene.SpotLight obj in scene.SpotLights)
            {
                LightsTree.Nodes.Add("SpotLight");
            }
            foreach (MJLGameEngine.Object.ParticleGenerator.Particle_Generator obj in scene.ParticleGenerators)
            {
                GeneratorsTreeView.Nodes.Add("Particle Generators");
            }
        }

        private void ObjectsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ObjectsProperties.SelectedObject = scene.Objects[e.Node.Index];
            scene.SelectedModel = e.Node.Index;
            scene.SelectedMesh = -1;
        }

        private void AddNewObject_Click(object sender, EventArgs e)
        {
            //ObjectsTree.Nodes.Add("new node");
            //MJLGameEngine.Object.Object o = scene.Objects[1];
            //scene.Objects.Add(new MJLGameEngine.Object.Object(o.Name, o.model, o.Position, o.YawPitchRoll, o.Scale.X, o.texture, o.normalmap, o.normalmap_enabled));
        }

        private void ObjectsTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ObjectsProperties.Height = splitContainer1.Panel2.Height - 2;
            ObjectsTree.Height = splitContainer1.Panel1.Height - 2;
        }

        private void LightsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text == "Directional Light")
                LightsProperties.SelectedObject = scene.DirectionalLights[e.Node.Index];
            if (e.Node.Text == "SpotLight")
                LightsProperties.SelectedObject = scene.SpotLights[e.Node.Index - (scene.DirectionalLights.Count)];
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            LightsProperties.Height = splitContainer2.Panel2.Height - 2;
            LightsTree.Height = splitContainer2.Panel1.Height - 2;
        }

        private void AddLight_Click(object sender, System.EventArgs e)
        {
            scene.SpotLights.Add(new MJLGameEngine.Scene.SpotLight());
            LightsTree.Nodes.Add("SpotLight");
        }

        private void GeneratorsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            GeneratorsProperties.SelectedObject = scene.ParticleGenerators[e.Node.Index];
        }

        private void Update_Click(object sender, EventArgs e)
        {
            ObjectsTree.Nodes.Clear();
            LightsTree.Nodes.Clear();
            GeneratorsTreeView.Nodes.Clear();
            foreach (MJLGameEngine.Object.Object obj in scene.Objects)
            {
                ObjectsTree.Nodes.Add(obj.Name);
            }
            foreach (MJLGameEngine.Scene.DirectionalLight obj in scene.DirectionalLights)
            {
                LightsTree.Nodes.Add("Directional Light");
            }
            foreach (MJLGameEngine.Scene.SpotLight obj in scene.SpotLights)
            {
                LightsTree.Nodes.Add("SpotLight");
            }
            foreach (MJLGameEngine.Object.ParticleGenerator.Particle_Generator obj in scene.ParticleGenerators)
            {
                GeneratorsTreeView.Nodes.Add("Particle Generators");
            }
        }

        private void ModifyMeshes_Click(object sender, EventArgs e)
        {
            MeshEditor m = new MeshEditor();
            try
            {
                m.RunAsMeshEditor(scene, scene.Objects[ObjectsTree.SelectedNode.Index].model);
                //scene.Objects[ObjectsTree.SelectedNode.Index].model = 
                int meshnumber = 0;
                foreach (ModelMesh mesh in scene.Objects[ObjectsTree.SelectedNode.Index].model.Meshes)
                {
                    mesh.ParentBone.Transform = m.Transforms[meshnumber].OutTransform;
                    meshnumber++;
                }
            }
            catch
            {
                MessageBox.Show("You must select an object to modify the meshes");
            }
        }
    }
}
