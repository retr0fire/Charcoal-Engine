namespace MapEditor
{
    partial class MeshEditor
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
            this.MeshTreeView = new System.Windows.Forms.TreeView();
            this.MeshEditorView = new System.Windows.Forms.PropertyGrid();
            this.Close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MeshTreeView
            // 
            this.MeshTreeView.Location = new System.Drawing.Point(12, 12);
            this.MeshTreeView.Name = "MeshTreeView";
            this.MeshTreeView.Size = new System.Drawing.Size(246, 424);
            this.MeshTreeView.TabIndex = 0;
            this.MeshTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MeshTreeView_AfterSelect);
            // 
            // MeshEditorView
            // 
            this.MeshEditorView.Location = new System.Drawing.Point(264, 12);
            this.MeshEditorView.Name = "MeshEditorView";
            this.MeshEditorView.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.MeshEditorView.Size = new System.Drawing.Size(325, 395);
            this.MeshEditorView.TabIndex = 1;
            this.MeshEditorView.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.MeshEditorView_PropertyValueChanged);
            // 
            // Close
            // 
            this.Close.Location = new System.Drawing.Point(264, 413);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(324, 22);
            this.Close.TabIndex = 2;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // MeshEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 448);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.MeshEditorView);
            this.Controls.Add(this.MeshTreeView);
            this.Name = "MeshEditor";
            this.Text = "MeshEditor";
            this.Load += new System.EventHandler(this.MeshEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TreeView MeshTreeView;
        public System.Windows.Forms.PropertyGrid MeshEditorView;
        private System.Windows.Forms.Button Close;
    }
}