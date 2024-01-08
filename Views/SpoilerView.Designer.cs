namespace WinCachebox.Views
{
  partial class SpoilerView
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
      this.components = new System.ComponentModel.Container();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.gSpoiler = new SourceGrid.Grid();
      this.label1 = new System.Windows.Forms.Label();
      this.button2 = new System.Windows.Forms.Button();
      this.button1 = new System.Windows.Forms.Button();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.downloadSpoilersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.importSpoilerFromURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // pictureBox1
      // 
      this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox1.Location = new System.Drawing.Point(0, 16);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(400, 151);
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.SpoilerView_Paint);
      this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SpoilerView_MouseDown);
      this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SpoilerView_MouseMove);
      this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SpoilerView_MouseUp);
      this.pictureBox1.Resize += new System.EventHandler(this.SpoilerView_Resize);
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.splitContainer1.Location = new System.Drawing.Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.gSpoiler);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.label1);
      this.splitContainer1.Panel2.Controls.Add(this.button2);
      this.splitContainer1.Panel2.Controls.Add(this.button1);
      this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
      this.splitContainer1.Size = new System.Drawing.Size(400, 261);
      this.splitContainer1.SplitterDistance = 90;
      this.splitContainer1.TabIndex = 2;
      // 
      // gSpoiler
      // 
      this.gSpoiler.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gSpoiler.EnableSort = true;
      this.gSpoiler.Location = new System.Drawing.Point(0, 0);
      this.gSpoiler.Name = "gSpoiler";
      this.gSpoiler.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
      this.gSpoiler.SelectionMode = SourceGrid.GridSelectionMode.Cell;
      this.gSpoiler.Size = new System.Drawing.Size(400, 90);
      this.gSpoiler.TabIndex = 1;
      this.gSpoiler.TabStop = true;
      this.gSpoiler.ToolTipText = "";
      this.gSpoiler.Resize += new System.EventHandler(this.gSpoiler_Resize);
      // 
      // label1
      // 
      this.label1.BackColor = System.Drawing.Color.DarkKhaki;
      this.label1.Dock = System.Windows.Forms.DockStyle.Top;
      this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(400, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "label1";
      this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(3, 16);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(23, 23);
      this.button2.TabIndex = 2;
      this.button2.Text = "+";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.button1.Location = new System.Drawing.Point(3, 141);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(23, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "-";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadSpoilersToolStripMenuItem,
            this.importSpoilerFromURLToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(268, 70);
      // 
      // downloadSpoilersToolStripMenuItem
      // 
      this.downloadSpoilersToolStripMenuItem.Name = "downloadSpoilersToolStripMenuItem";
      this.downloadSpoilersToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
      this.downloadSpoilersToolStripMenuItem.Text = "&Download Spoilers";
      this.downloadSpoilersToolStripMenuItem.Click += new System.EventHandler(this.downloadSpoilersToolStripMenuItem_Click);
      // 
      // importSpoilerFromURLToolStripMenuItem
      // 
      this.importSpoilerFromURLToolStripMenuItem.Name = "importSpoilerFromURLToolStripMenuItem";
      this.importSpoilerFromURLToolStripMenuItem.Size = new System.Drawing.Size(267, 22);
      this.importSpoilerFromURLToolStripMenuItem.Text = "Import Spoiler From &URL (Clipboard)";
      this.importSpoilerFromURLToolStripMenuItem.Click += new System.EventHandler(this.importSpoilerFromURLToolStripMenuItem_Click);
      // 
      // SpoilerView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(400, 261);
      this.ContextMenuStrip = this.contextMenuStrip1;
      this.Controls.Add(this.splitContainer1);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "SpoilerView";
      this.Load += new System.EventHandler(this.SpoilerView_Load);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpoilerView_KeyDown);
      this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpoilerView_KeyPress);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBox1;
    private SourceGrid.Grid gSpoiler;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem downloadSpoilersToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem importSpoilerFromURLToolStripMenuItem;

  }
}
