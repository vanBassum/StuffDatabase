namespace StuffDatabase
{
    partial class CTRL_Component
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_New = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btn_Edit = new System.Windows.Forms.Button();
            this.btn_Print = new System.Windows.Forms.Button();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AllowDrop = true;
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackgroundImage = global::StuffDatabase.Properties.Resources.pdf_icon_datasheet;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(372, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(50, 46);
            this.panel2.TabIndex = 4;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(0, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(327, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(3, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(324, 148);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btn_New
            // 
            this.btn_New.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_New.Location = new System.Drawing.Point(3, 511);
            this.btn_New.Name = "btn_New";
            this.btn_New.Size = new System.Drawing.Size(67, 34);
            this.btn_New.TabIndex = 10;
            this.btn_New.Text = "New";
            this.btn_New.UseVisualStyleBackColor = true;
            this.btn_New.Click += new System.EventHandler(this.btn_New_Click);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.Location = new System.Drawing.Point(3, -3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(359, 505);
            this.treeView1.TabIndex = 15;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btn_Edit);
            this.splitContainer1.Panel1.Controls.Add(this.btn_New);
            this.splitContainer1.Panel1.Controls.Add(this.btn_Print);
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1.Controls.Add(this.btn_Delete);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btn_Cancel);
            this.splitContainer1.Panel2.Controls.Add(this.btn_Save);
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(702, 548);
            this.splitContainer1.SplitterDistance = 365;
            this.splitContainer1.TabIndex = 16;
            // 
            // btn_Edit
            // 
            this.btn_Edit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Edit.Location = new System.Drawing.Point(76, 511);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(67, 34);
            this.btn_Edit.TabIndex = 16;
            this.btn_Edit.Text = "Edit";
            this.btn_Edit.UseVisualStyleBackColor = true;
            this.btn_Edit.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_Print
            // 
            this.btn_Print.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Print.Location = new System.Drawing.Point(222, 511);
            this.btn_Print.Name = "btn_Print";
            this.btn_Print.Size = new System.Drawing.Size(67, 34);
            this.btn_Print.TabIndex = 12;
            this.btn_Print.Text = "Print";
            this.btn_Print.UseVisualStyleBackColor = true;
            this.btn_Print.Click += new System.EventHandler(this.btn_Print_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Delete.Location = new System.Drawing.Point(149, 511);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(67, 34);
            this.btn_Delete.TabIndex = 13;
            this.btn_Delete.Text = "Delete";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Cancel.Location = new System.Drawing.Point(76, 511);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(67, 34);
            this.btn_Cancel.TabIndex = 15;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Save.Location = new System.Drawing.Point(3, 511);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(67, 34);
            this.btn_Save.TabIndex = 16;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.button3_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.propertyGrid1);
            this.splitContainer2.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer2.Panel2.Controls.Add(this.comboBox1);
            this.splitContainer2.Size = new System.Drawing.Size(330, 502);
            this.splitContainer2.SplitterDistance = 317;
            this.splitContainer2.TabIndex = 14;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(327, 314);
            this.propertyGrid1.TabIndex = 13;
            // 
            // CTRL_Component
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CTRL_Component";
            this.Size = new System.Drawing.Size(702, 548);
            this.Load += new System.EventHandler(this.CTRL_Components_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_New;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btn_Edit;
        private System.Windows.Forms.Button btn_Print;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_Save;
    }
}
