namespace Interp2D
{
    partial class Live2DForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.addLayerButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.modelingPage = new System.Windows.Forms.TabPage();
            this.vertexColor3 = new System.Windows.Forms.ComboBox();
            this.vertexColor2 = new System.Windows.Forms.ComboBox();
            this.vertexColor1 = new System.Windows.Forms.ComboBox();
            this.modelingControl = new Interp2D.ModelingControl();
            this.riggingPage = new System.Windows.Forms.TabPage();
            this.animatonPage = new System.Windows.Forms.TabPage();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spinningTriangleControl = new Interp2D.SpinningTriangleControl();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.layerList = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.modelingPage.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1056, 587);
            this.splitContainer1.SplitterDistance = 229;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.addLayerButton);
            this.splitContainer2.Panel1.Controls.Add(this.layerList);
            this.splitContainer2.Size = new System.Drawing.Size(229, 587);
            this.splitContainer2.SplitterDistance = 373;
            this.splitContainer2.TabIndex = 0;
            // 
            // addLayerButton
            // 
            this.addLayerButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.addLayerButton.Location = new System.Drawing.Point(0, 348);
            this.addLayerButton.Name = "addLayerButton";
            this.addLayerButton.Size = new System.Drawing.Size(229, 25);
            this.addLayerButton.TabIndex = 0;
            this.addLayerButton.Text = "+";
            this.addLayerButton.UseVisualStyleBackColor = true;
            this.addLayerButton.Click += new System.EventHandler(this.addLayerButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.modelingPage);
            this.tabControl1.Controls.Add(this.riggingPage);
            this.tabControl1.Controls.Add(this.animatonPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(822, 587);
            this.tabControl1.TabIndex = 0;
            // 
            // modelingPage
            // 
            this.modelingPage.Controls.Add(this.vertexColor3);
            this.modelingPage.Controls.Add(this.vertexColor2);
            this.modelingPage.Controls.Add(this.vertexColor1);
            this.modelingPage.Controls.Add(this.modelingControl);
            this.modelingPage.Location = new System.Drawing.Point(4, 25);
            this.modelingPage.Margin = new System.Windows.Forms.Padding(4);
            this.modelingPage.Name = "modelingPage";
            this.modelingPage.Padding = new System.Windows.Forms.Padding(4);
            this.modelingPage.Size = new System.Drawing.Size(814, 558);
            this.modelingPage.TabIndex = 0;
            this.modelingPage.Text = "Modeling";
            this.modelingPage.UseVisualStyleBackColor = true;
            // 
            // vertexColor3
            // 
            this.vertexColor3.DropDownHeight = 500;
            this.vertexColor3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vertexColor3.FormattingEnabled = true;
            this.vertexColor3.IntegralHeight = false;
            this.vertexColor3.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue"});
            this.vertexColor3.Location = new System.Drawing.Point(299, 15);
            this.vertexColor3.Margin = new System.Windows.Forms.Padding(4);
            this.vertexColor3.Name = "vertexColor3";
            this.vertexColor3.Size = new System.Drawing.Size(136, 24);
            this.vertexColor3.TabIndex = 3;
            this.vertexColor3.SelectedIndexChanged += new System.EventHandler(this.vertexColor_SelectedIndexChanged);
            // 
            // vertexColor2
            // 
            this.vertexColor2.DropDownHeight = 500;
            this.vertexColor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vertexColor2.FormattingEnabled = true;
            this.vertexColor2.IntegralHeight = false;
            this.vertexColor2.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue"});
            this.vertexColor2.Location = new System.Drawing.Point(153, 15);
            this.vertexColor2.Margin = new System.Windows.Forms.Padding(4);
            this.vertexColor2.Name = "vertexColor2";
            this.vertexColor2.Size = new System.Drawing.Size(136, 24);
            this.vertexColor2.TabIndex = 2;
            this.vertexColor2.SelectedIndexChanged += new System.EventHandler(this.vertexColor_SelectedIndexChanged);
            // 
            // vertexColor1
            // 
            this.vertexColor1.DropDownHeight = 500;
            this.vertexColor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vertexColor1.FormattingEnabled = true;
            this.vertexColor1.IntegralHeight = false;
            this.vertexColor1.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue"});
            this.vertexColor1.Location = new System.Drawing.Point(8, 15);
            this.vertexColor1.Margin = new System.Windows.Forms.Padding(4);
            this.vertexColor1.Name = "vertexColor1";
            this.vertexColor1.Size = new System.Drawing.Size(136, 24);
            this.vertexColor1.TabIndex = 1;
            this.vertexColor1.SelectedIndexChanged += new System.EventHandler(this.vertexColor_SelectedIndexChanged);
            //
            // LayerList
            //
            this.layerList.Dock = System.Windows.Forms.DockStyle.Fill;
            
            
            // 
            // modelingControl
            // 
            this.modelingControl.CurrentLayer = null;
            this.modelingControl.Location = new System.Drawing.Point(4, 4);
            this.modelingControl.Margin = new System.Windows.Forms.Padding(4);
            this.modelingControl.Name = "modelingControl";
            this.modelingControl.Size = new System.Drawing.Size(683, 630);
            this.modelingControl.TabIndex = 0;
            // 
            // riggingPage
            // 
            this.riggingPage.Location = new System.Drawing.Point(4, 25);
            this.riggingPage.Margin = new System.Windows.Forms.Padding(4);
            this.riggingPage.Name = "riggingPage";
            this.riggingPage.Padding = new System.Windows.Forms.Padding(4);
            this.riggingPage.Size = new System.Drawing.Size(814, 558);
            this.riggingPage.TabIndex = 1;
            this.riggingPage.Text = "Main";
            // 
            // animatonPage
            // 
            this.animatonPage.Location = new System.Drawing.Point(4, 25);
            this.animatonPage.Margin = new System.Windows.Forms.Padding(4);
            this.animatonPage.Name = "animatonPage";
            this.animatonPage.Padding = new System.Windows.Forms.Padding(4);
            this.animatonPage.Size = new System.Drawing.Size(814, 558);
            this.animatonPage.TabIndex = 2;
            this.animatonPage.Text = "Animator";
            // 
            // menuStrip2
            // 
            this.menuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip2.Size = new System.Drawing.Size(1056, 28);
            this.menuStrip2.TabIndex = 1;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(133, 26);
            this.newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(133, 26);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(133, 26);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(133, 26);
            this.saveAsToolStripMenuItem.Text = "Save as";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(120, 26);
            this.undoToolStripMenuItem.Text = "Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(120, 26);
            this.redoToolStripMenuItem.Text = "Redo";
            // 
            // spinningTriangleControl
            // 
            this.spinningTriangleControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spinningTriangleControl.Location = new System.Drawing.Point(3, 3);
            this.spinningTriangleControl.Name = "spinningTriangleControl";
            this.spinningTriangleControl.Size = new System.Drawing.Size(602, 444);
            this.spinningTriangleControl.TabIndex = 0;
            this.spinningTriangleControl.Text = "spinningTriangleControl";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            // 
            // Live2DForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 615);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Live2DForm";
            this.Text = "Form ext";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.modelingPage.ResumeLayout(false);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.SplitContainer splitContainer1;
        
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage modelingPage;
        private System.Windows.Forms.TabPage riggingPage;
        private System.Windows.Forms.TabPage animatonPage;
        private System.Windows.Forms.Button addLayerButton;
        private System.Windows.Forms.ListBox layerList;
        //private SpriteFontControl spriteFontControl;
        private SpinningTriangleControl spinningTriangleControl;
        private ModelingControl modelingControl;
        private System.Windows.Forms.ComboBox vertexColor1;
        private System.Windows.Forms.ComboBox vertexColor2;
        private System.Windows.Forms.ComboBox vertexColor3;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button1;
    }
}

