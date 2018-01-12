
namespace SimpleEditor
{
    partial class Form1
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
            this.ObjectsProperties = new System.Windows.Forms.PropertyGrid();
            this.ObjectsTree = new System.Windows.Forms.TreeView();
            this.Add = new System.Windows.Forms.Button();
            this.XML_Load = new System.Windows.Forms.Button();
            this.XML_Save = new System.Windows.Forms.Button();
            this.List_Remove = new System.Windows.Forms.Button();
            this.List_Down = new System.Windows.Forms.Button();
            this.List_Up = new System.Windows.Forms.Button();
            this.XML_Load_Scene = new System.Windows.Forms.Button();
            this.XML_Save_Scene = new System.Windows.Forms.Button();
            this.Add_Light = new System.Windows.Forms.Button();
            this.Refresh = new System.Windows.Forms.Button();
            this.List_Right = new System.Windows.Forms.Button();
            this.List_Left = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ObjectsProperties
            // 
            this.ObjectsProperties.HelpVisible = false;
            this.ObjectsProperties.Location = new System.Drawing.Point(12, 205);
            this.ObjectsProperties.Name = "ObjectsProperties";
            this.ObjectsProperties.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.ObjectsProperties.Size = new System.Drawing.Size(409, 225);
            this.ObjectsProperties.TabIndex = 4;
            // 
            // ObjectsTree
            // 
            this.ObjectsTree.Location = new System.Drawing.Point(12, 36);
            this.ObjectsTree.Name = "ObjectsTree";
            this.ObjectsTree.Size = new System.Drawing.Size(353, 163);
            this.ObjectsTree.TabIndex = 3;
            this.ObjectsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ObjectsTree_AfterSelect);
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(12, 7);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(198, 23);
            this.Add.TabIndex = 5;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // XML_Load
            // 
            this.XML_Load.Location = new System.Drawing.Point(12, 436);
            this.XML_Load.Name = "XML_Load";
            this.XML_Load.Size = new System.Drawing.Size(143, 23);
            this.XML_Load.TabIndex = 6;
            this.XML_Load.Text = "Load From File";
            this.XML_Load.UseVisualStyleBackColor = true;
            this.XML_Load.Click += new System.EventHandler(this.XML_Load_Click);
            // 
            // XML_Save
            // 
            this.XML_Save.Location = new System.Drawing.Point(12, 465);
            this.XML_Save.Name = "XML_Save";
            this.XML_Save.Size = new System.Drawing.Size(144, 23);
            this.XML_Save.TabIndex = 7;
            this.XML_Save.Text = "Save To File";
            this.XML_Save.UseVisualStyleBackColor = true;
            this.XML_Save.Click += new System.EventHandler(this.XML_Save_Click);
            // 
            // List_Remove
            // 
            this.List_Remove.Location = new System.Drawing.Point(371, 152);
            this.List_Remove.Name = "List_Remove";
            this.List_Remove.Size = new System.Drawing.Size(57, 23);
            this.List_Remove.TabIndex = 10;
            this.List_Remove.Text = "Remove";
            this.List_Remove.UseVisualStyleBackColor = true;
            this.List_Remove.Click += new System.EventHandler(this.List_Remove_Click);
            // 
            // List_Down
            // 
            this.List_Down.BackgroundImage = global::SimpleEditor.Properties.Resources.Arrow2;
            this.List_Down.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.List_Down.Location = new System.Drawing.Point(371, 65);
            this.List_Down.Name = "List_Down";
            this.List_Down.Size = new System.Drawing.Size(57, 23);
            this.List_Down.TabIndex = 9;
            this.List_Down.UseVisualStyleBackColor = true;
            this.List_Down.Click += new System.EventHandler(this.List_Down_Click);
            // 
            // List_Up
            // 
            this.List_Up.BackgroundImage = global::SimpleEditor.Properties.Resources.Arrow;
            this.List_Up.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.List_Up.Location = new System.Drawing.Point(371, 36);
            this.List_Up.Name = "List_Up";
            this.List_Up.Size = new System.Drawing.Size(57, 23);
            this.List_Up.TabIndex = 8;
            this.List_Up.UseVisualStyleBackColor = true;
            this.List_Up.Click += new System.EventHandler(this.List_Up_Click);
            // 
            // XML_Load_Scene
            // 
            this.XML_Load_Scene.Location = new System.Drawing.Point(161, 436);
            this.XML_Load_Scene.Name = "XML_Load_Scene";
            this.XML_Load_Scene.Size = new System.Drawing.Size(145, 23);
            this.XML_Load_Scene.TabIndex = 11;
            this.XML_Load_Scene.Text = "Load Scene From File";
            this.XML_Load_Scene.UseVisualStyleBackColor = true;
            this.XML_Load_Scene.Click += new System.EventHandler(this.XML_Load_Scene_Click);
            // 
            // XML_Save_Scene
            // 
            this.XML_Save_Scene.Location = new System.Drawing.Point(162, 465);
            this.XML_Save_Scene.Name = "XML_Save_Scene";
            this.XML_Save_Scene.Size = new System.Drawing.Size(145, 23);
            this.XML_Save_Scene.TabIndex = 12;
            this.XML_Save_Scene.Text = "Save Scene To File";
            this.XML_Save_Scene.UseVisualStyleBackColor = true;
            this.XML_Save_Scene.Click += new System.EventHandler(this.XML_Save_Scene_Click);
            // 
            // Add_Light
            // 
            this.Add_Light.Location = new System.Drawing.Point(217, 7);
            this.Add_Light.Name = "Add_Light";
            this.Add_Light.Size = new System.Drawing.Size(211, 23);
            this.Add_Light.TabIndex = 13;
            this.Add_Light.Text = "Add Light";
            this.Add_Light.UseVisualStyleBackColor = true;
            this.Add_Light.Click += new System.EventHandler(this.Add_Light_Click);
            // 
            // Refresh
            // 
            this.Refresh.Location = new System.Drawing.Point(371, 181);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(56, 23);
            this.Refresh.TabIndex = 14;
            this.Refresh.Text = "Refresh";
            this.Refresh.UseVisualStyleBackColor = true;
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // List_Right
            // 
            this.List_Right.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.List_Right.Location = new System.Drawing.Point(371, 123);
            this.List_Right.Name = "List_Right";
            this.List_Right.Size = new System.Drawing.Size(57, 23);
            this.List_Right.TabIndex = 16;
            this.List_Right.Text = "->";
            this.List_Right.UseVisualStyleBackColor = true;
            this.List_Right.Click += new System.EventHandler(this.List_Right_Click);
            // 
            // List_Left
            // 
            this.List_Left.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.List_Left.Location = new System.Drawing.Point(371, 94);
            this.List_Left.Name = "List_Left";
            this.List_Left.Size = new System.Drawing.Size(57, 23);
            this.List_Left.TabIndex = 15;
            this.List_Left.Text = "<-";
            this.List_Left.UseVisualStyleBackColor = true;
            this.List_Left.Click += new System.EventHandler(this.List_Left_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 500);
            this.Controls.Add(this.List_Right);
            this.Controls.Add(this.List_Left);
            this.Controls.Add(this.Refresh);
            this.Controls.Add(this.Add_Light);
            this.Controls.Add(this.XML_Save_Scene);
            this.Controls.Add(this.XML_Load_Scene);
            this.Controls.Add(this.List_Remove);
            this.Controls.Add(this.List_Down);
            this.Controls.Add(this.List_Up);
            this.Controls.Add(this.XML_Save);
            this.Controls.Add(this.XML_Load);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.ObjectsProperties);
            this.Controls.Add(this.ObjectsTree);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PropertyGrid ObjectsProperties;
        public System.Windows.Forms.TreeView ObjectsTree;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button XML_Load;
        private System.Windows.Forms.Button XML_Save;
        private System.Windows.Forms.Button List_Up;
        private System.Windows.Forms.Button List_Down;
        private System.Windows.Forms.Button List_Remove;
        private System.Windows.Forms.Button XML_Load_Scene;
        private System.Windows.Forms.Button XML_Save_Scene;
        private System.Windows.Forms.Button Add_Light;
        private System.Windows.Forms.Button Refresh;
        private System.Windows.Forms.Button List_Right;
        private System.Windows.Forms.Button List_Left;
    }
}
