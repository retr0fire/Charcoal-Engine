namespace MapEditor
{
    partial class Editor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.EditorView = new System.Windows.Forms.TabControl();
            this.Objects = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Update = new System.Windows.Forms.Button();
            this.ObjectsTree = new System.Windows.Forms.TreeView();
            this.ObjectsProperties = new System.Windows.Forms.PropertyGrid();
            this.Lights = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.AddLight = new System.Windows.Forms.Button();
            this.LightsTree = new System.Windows.Forms.TreeView();
            this.LightsProperties = new System.Windows.Forms.PropertyGrid();
            this.ParticleGenerators = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.AddNewGenerator = new System.Windows.Forms.Button();
            this.GeneratorsTreeView = new System.Windows.Forms.TreeView();
            this.GeneratorsProperties = new System.Windows.Forms.PropertyGrid();
            this.Settings = new System.Windows.Forms.TabPage();
            this.Enable_Physics = new System.Windows.Forms.CheckBox();
            this.DebugView = new System.Windows.Forms.CheckBox();
            this.PostScreenBlur = new System.Windows.Forms.CheckBox();
            this.Alpha_Mapping = new System.Windows.Forms.CheckBox();
            this.Shadow_Mapping = new System.Windows.Forms.CheckBox();
            this.Arc_Ball = new System.Windows.Forms.CheckBox();
            this.ModifyMeshes = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.EditorView.SuspendLayout();
            this.Objects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.Lights.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.ParticleGenerators.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.Settings.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.EditorView);
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(289, 475);
            this.panel1.TabIndex = 1;
            // 
            // EditorView
            // 
            this.EditorView.Controls.Add(this.Objects);
            this.EditorView.Controls.Add(this.Lights);
            this.EditorView.Controls.Add(this.ParticleGenerators);
            this.EditorView.Controls.Add(this.Settings);
            this.EditorView.Location = new System.Drawing.Point(3, 25);
            this.EditorView.Name = "EditorView";
            this.EditorView.SelectedIndex = 0;
            this.EditorView.Size = new System.Drawing.Size(283, 447);
            this.EditorView.TabIndex = 0;
            // 
            // Objects
            // 
            this.Objects.Controls.Add(this.splitContainer1);
            this.Objects.Location = new System.Drawing.Point(4, 22);
            this.Objects.Name = "Objects";
            this.Objects.Padding = new System.Windows.Forms.Padding(3);
            this.Objects.Size = new System.Drawing.Size(275, 421);
            this.Objects.TabIndex = 0;
            this.Objects.Text = "Objects";
            this.Objects.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ModifyMeshes);
            this.splitContainer1.Panel1.Controls.Add(this.Update);
            this.splitContainer1.Panel1.Controls.Add(this.ObjectsTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ObjectsProperties);
            this.splitContainer1.Size = new System.Drawing.Size(275, 415);
            this.splitContainer1.SplitterDistance = 207;
            this.splitContainer1.TabIndex = 3;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // Update
            // 
            this.Update.Location = new System.Drawing.Point(6, 2);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(132, 23);
            this.Update.TabIndex = 2;
            this.Update.Text = "Update";
            this.Update.UseVisualStyleBackColor = true;
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // ObjectsTree
            // 
            this.ObjectsTree.Location = new System.Drawing.Point(6, 31);
            this.ObjectsTree.Name = "ObjectsTree";
            this.ObjectsTree.Size = new System.Drawing.Size(263, 176);
            this.ObjectsTree.TabIndex = 0;
            this.ObjectsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ObjectsTree_AfterSelect);
            this.ObjectsTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ObjectsTree_NodeMouseDoubleClick);
            // 
            // ObjectsProperties
            // 
            this.ObjectsProperties.Location = new System.Drawing.Point(6, 3);
            this.ObjectsProperties.Name = "ObjectsProperties";
            this.ObjectsProperties.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.ObjectsProperties.SelectedObject = this.ObjectsProperties;
            this.ObjectsProperties.Size = new System.Drawing.Size(263, 196);
            this.ObjectsProperties.TabIndex = 2;
            // 
            // Lights
            // 
            this.Lights.Controls.Add(this.splitContainer2);
            this.Lights.Location = new System.Drawing.Point(4, 22);
            this.Lights.Name = "Lights";
            this.Lights.Padding = new System.Windows.Forms.Padding(3);
            this.Lights.Size = new System.Drawing.Size(275, 421);
            this.Lights.TabIndex = 1;
            this.Lights.Text = "Lights";
            this.Lights.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Location = new System.Drawing.Point(0, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.AddLight);
            this.splitContainer2.Panel1.Controls.Add(this.LightsTree);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.LightsProperties);
            this.splitContainer2.Size = new System.Drawing.Size(275, 415);
            this.splitContainer2.SplitterDistance = 207;
            this.splitContainer2.TabIndex = 4;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
            // 
            // AddLight
            // 
            this.AddLight.Location = new System.Drawing.Point(6, 3);
            this.AddLight.Name = "AddLight";
            this.AddLight.Size = new System.Drawing.Size(263, 22);
            this.AddLight.TabIndex = 1;
            this.AddLight.Text = "Add New Light";
            this.AddLight.UseVisualStyleBackColor = true;
            this.AddLight.Click += new System.EventHandler(this.AddLight_Click);
            // 
            // LightsTree
            // 
            this.LightsTree.Location = new System.Drawing.Point(6, 31);
            this.LightsTree.Name = "LightsTree";
            this.LightsTree.Size = new System.Drawing.Size(263, 176);
            this.LightsTree.TabIndex = 0;
            this.LightsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LightsTree_AfterSelect);
            // 
            // LightsProperties
            // 
            this.LightsProperties.Location = new System.Drawing.Point(6, 3);
            this.LightsProperties.Name = "LightsProperties";
            this.LightsProperties.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.LightsProperties.Size = new System.Drawing.Size(263, 196);
            this.LightsProperties.TabIndex = 2;
            // 
            // ParticleGenerators
            // 
            this.ParticleGenerators.Controls.Add(this.splitContainer3);
            this.ParticleGenerators.Location = new System.Drawing.Point(4, 22);
            this.ParticleGenerators.Name = "ParticleGenerators";
            this.ParticleGenerators.Padding = new System.Windows.Forms.Padding(3);
            this.ParticleGenerators.Size = new System.Drawing.Size(275, 421);
            this.ParticleGenerators.TabIndex = 2;
            this.ParticleGenerators.Text = "Particle Generators";
            this.ParticleGenerators.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Location = new System.Drawing.Point(0, 3);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.AddNewGenerator);
            this.splitContainer3.Panel1.Controls.Add(this.GeneratorsTreeView);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.GeneratorsProperties);
            this.splitContainer3.Size = new System.Drawing.Size(275, 415);
            this.splitContainer3.SplitterDistance = 207;
            this.splitContainer3.TabIndex = 4;
            // 
            // AddNewGenerator
            // 
            this.AddNewGenerator.Location = new System.Drawing.Point(6, 3);
            this.AddNewGenerator.Name = "AddNewGenerator";
            this.AddNewGenerator.Size = new System.Drawing.Size(263, 22);
            this.AddNewGenerator.TabIndex = 1;
            this.AddNewGenerator.Text = "Add New Generator";
            this.AddNewGenerator.UseVisualStyleBackColor = true;
            // 
            // GeneratorsTreeView
            // 
            this.GeneratorsTreeView.Location = new System.Drawing.Point(6, 31);
            this.GeneratorsTreeView.Name = "GeneratorsTreeView";
            this.GeneratorsTreeView.Size = new System.Drawing.Size(263, 176);
            this.GeneratorsTreeView.TabIndex = 0;
            this.GeneratorsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.GeneratorsTreeView_AfterSelect);
            // 
            // GeneratorsProperties
            // 
            this.GeneratorsProperties.Location = new System.Drawing.Point(6, 3);
            this.GeneratorsProperties.Name = "GeneratorsProperties";
            this.GeneratorsProperties.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.GeneratorsProperties.Size = new System.Drawing.Size(263, 196);
            this.GeneratorsProperties.TabIndex = 2;
            // 
            // Settings
            // 
            this.Settings.Controls.Add(this.Enable_Physics);
            this.Settings.Controls.Add(this.DebugView);
            this.Settings.Controls.Add(this.PostScreenBlur);
            this.Settings.Controls.Add(this.Alpha_Mapping);
            this.Settings.Controls.Add(this.Shadow_Mapping);
            this.Settings.Controls.Add(this.Arc_Ball);
            this.Settings.Location = new System.Drawing.Point(4, 22);
            this.Settings.Name = "Settings";
            this.Settings.Padding = new System.Windows.Forms.Padding(3);
            this.Settings.Size = new System.Drawing.Size(275, 421);
            this.Settings.TabIndex = 5;
            this.Settings.Text = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            // 
            // Enable_Physics
            // 
            this.Enable_Physics.AutoSize = true;
            this.Enable_Physics.Location = new System.Drawing.Point(7, 127);
            this.Enable_Physics.Name = "Enable_Physics";
            this.Enable_Physics.Size = new System.Drawing.Size(62, 17);
            this.Enable_Physics.TabIndex = 5;
            this.Enable_Physics.Text = "Physics";
            this.Enable_Physics.UseVisualStyleBackColor = true;
            // 
            // DebugView
            // 
            this.DebugView.AutoSize = true;
            this.DebugView.Location = new System.Drawing.Point(7, 103);
            this.DebugView.Name = "DebugView";
            this.DebugView.Size = new System.Drawing.Size(88, 17);
            this.DebugView.TabIndex = 4;
            this.DebugView.Text = "Debug Mode";
            this.DebugView.UseVisualStyleBackColor = true;
            // 
            // PostScreenBlur
            // 
            this.PostScreenBlur.AutoSize = true;
            this.PostScreenBlur.Location = new System.Drawing.Point(7, 79);
            this.PostScreenBlur.Name = "PostScreenBlur";
            this.PostScreenBlur.Size = new System.Drawing.Size(138, 17);
            this.PostScreenBlur.TabIndex = 3;
            this.PostScreenBlur.Text = "Post-Screen Radial Blur";
            this.PostScreenBlur.UseVisualStyleBackColor = true;
            // 
            // Alpha_Mapping
            // 
            this.Alpha_Mapping.AutoSize = true;
            this.Alpha_Mapping.Checked = true;
            this.Alpha_Mapping.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Alpha_Mapping.Location = new System.Drawing.Point(7, 55);
            this.Alpha_Mapping.Name = "Alpha_Mapping";
            this.Alpha_Mapping.Size = new System.Drawing.Size(97, 17);
            this.Alpha_Mapping.TabIndex = 2;
            this.Alpha_Mapping.Text = "Alpha Mapping";
            this.Alpha_Mapping.UseVisualStyleBackColor = true;
            // 
            // Shadow_Mapping
            // 
            this.Shadow_Mapping.AutoSize = true;
            this.Shadow_Mapping.Checked = true;
            this.Shadow_Mapping.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Shadow_Mapping.Location = new System.Drawing.Point(7, 31);
            this.Shadow_Mapping.Name = "Shadow_Mapping";
            this.Shadow_Mapping.Size = new System.Drawing.Size(109, 17);
            this.Shadow_Mapping.TabIndex = 1;
            this.Shadow_Mapping.Text = "Shadow Mapping";
            this.Shadow_Mapping.UseVisualStyleBackColor = true;
            // 
            // Arc_Ball
            // 
            this.Arc_Ball.AutoSize = true;
            this.Arc_Ball.Checked = true;
            this.Arc_Ball.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Arc_Ball.Location = new System.Drawing.Point(7, 7);
            this.Arc_Ball.Name = "Arc_Ball";
            this.Arc_Ball.Size = new System.Drawing.Size(101, 17);
            this.Arc_Ball.TabIndex = 0;
            this.Arc_Ball.Text = "Arc Ball Camera";
            this.Arc_Ball.UseVisualStyleBackColor = true;
            // 
            // ModifyMeshes
            // 
            this.ModifyMeshes.Location = new System.Drawing.Point(144, 2);
            this.ModifyMeshes.Name = "ModifyMeshes";
            this.ModifyMeshes.Size = new System.Drawing.Size(125, 23);
            this.ModifyMeshes.TabIndex = 3;
            this.ModifyMeshes.Text = "Modify Meshes";
            this.ModifyMeshes.UseVisualStyleBackColor = true;
            this.ModifyMeshes.Click += new System.EventHandler(this.ModifyMeshes_Click);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 480);
            this.Controls.Add(this.panel1);
            this.Name = "Editor";
            this.Text = "Editor";
            this.panel1.ResumeLayout(false);
            this.EditorView.ResumeLayout(false);
            this.Objects.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.Lights.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ParticleGenerators.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.Settings.ResumeLayout(false);
            this.Settings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.TabControl EditorView;
        public System.Windows.Forms.TabPage Objects;
        public System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.TreeView ObjectsTree;
        public System.Windows.Forms.PropertyGrid ObjectsProperties;
        public System.Windows.Forms.TabPage Lights;
        public System.Windows.Forms.SplitContainer splitContainer2;
        public System.Windows.Forms.Button AddLight;
        public System.Windows.Forms.TreeView LightsTree;
        public System.Windows.Forms.PropertyGrid LightsProperties;
        public System.Windows.Forms.TabPage ParticleGenerators;
        public System.Windows.Forms.SplitContainer splitContainer3;
        public System.Windows.Forms.Button AddNewGenerator;
        public System.Windows.Forms.TreeView GeneratorsTreeView;
        public System.Windows.Forms.PropertyGrid GeneratorsProperties;
        public System.Windows.Forms.TabPage Settings;
        public System.Windows.Forms.CheckBox PostScreenBlur;
        public System.Windows.Forms.CheckBox Alpha_Mapping;
        public System.Windows.Forms.CheckBox Shadow_Mapping;
        public System.Windows.Forms.CheckBox Arc_Ball;
        public System.Windows.Forms.CheckBox DebugView;
        public System.Windows.Forms.Button Update;
        public System.Windows.Forms.CheckBox Enable_Physics;
        private System.Windows.Forms.Button ModifyMeshes;
    }
}

