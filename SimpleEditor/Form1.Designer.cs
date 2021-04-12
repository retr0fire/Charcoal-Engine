
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
            this.List_Remove = new System.Windows.Forms.Button();
            this.Refresh = new System.Windows.Forms.Button();
            this.AvailableObjectsTree = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // ObjectsProperties
            // 
            this.ObjectsProperties.HelpVisible = false;
            this.ObjectsProperties.Location = new System.Drawing.Point(16, 252);
            this.ObjectsProperties.Margin = new System.Windows.Forms.Padding(4);
            this.ObjectsProperties.Name = "ObjectsProperties";
            this.ObjectsProperties.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.ObjectsProperties.Size = new System.Drawing.Size(545, 277);
            this.ObjectsProperties.TabIndex = 4;
            // 
            // ObjectsTree
            // 
            this.ObjectsTree.Location = new System.Drawing.Point(16, 44);
            this.ObjectsTree.Margin = new System.Windows.Forms.Padding(4);
            this.ObjectsTree.Name = "ObjectsTree";
            this.ObjectsTree.Size = new System.Drawing.Size(217, 200);
            this.ObjectsTree.TabIndex = 3;
            this.ObjectsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ObjectsTree_AfterSelect);
            // 
            // List_Remove
            // 
            this.List_Remove.Location = new System.Drawing.Point(99, 8);
            this.List_Remove.Margin = new System.Windows.Forms.Padding(4);
            this.List_Remove.Name = "List_Remove";
            this.List_Remove.Size = new System.Drawing.Size(76, 28);
            this.List_Remove.TabIndex = 10;
            this.List_Remove.Text = "Remove";
            this.List_Remove.UseVisualStyleBackColor = true;
            this.List_Remove.Click += new System.EventHandler(this.List_Remove_Click);
            // 
            // Refresh
            // 
            this.Refresh.Location = new System.Drawing.Point(16, 8);
            this.Refresh.Margin = new System.Windows.Forms.Padding(4);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(75, 28);
            this.Refresh.TabIndex = 14;
            this.Refresh.Text = "Refresh";
            this.Refresh.UseVisualStyleBackColor = true;
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // AvailableObjectsTree
            // 
            this.AvailableObjectsTree.Location = new System.Drawing.Point(241, 44);
            this.AvailableObjectsTree.Margin = new System.Windows.Forms.Padding(4);
            this.AvailableObjectsTree.Name = "AvailableObjectsTree";
            this.AvailableObjectsTree.Size = new System.Drawing.Size(190, 200);
            this.AvailableObjectsTree.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 542);
            this.Controls.Add(this.AvailableObjectsTree);
            this.Controls.Add(this.Refresh);
            this.Controls.Add(this.List_Remove);
            this.Controls.Add(this.ObjectsProperties);
            this.Controls.Add(this.ObjectsTree);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PropertyGrid ObjectsProperties;
        public System.Windows.Forms.TreeView ObjectsTree;
        private System.Windows.Forms.Button List_Remove;
        private System.Windows.Forms.Button Refresh;
        public System.Windows.Forms.TreeView AvailableObjectsTree;
    }
}
