namespace WinCachebox.Views
{
  partial class MapView
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
        this.animationTimer = new System.Windows.Forms.Timer(this.components);
        this.zoomScaleTimer = new System.Windows.Forms.Timer(this.components);
        this.bZoomIn = new System.Windows.Forms.Button();
        this.bZoomOut = new System.Windows.Forms.Button();
        this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.tsiLayer = new System.Windows.Forms.ToolStripMenuItem();
        this.centerPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.tsiSetCenter = new System.Windows.Forms.ToolStripMenuItem();
        this.tsiRemoveCenter = new System.Windows.Forms.ToolStripMenuItem();
        this.loactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
        this.clipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.copyCoordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.contextMenuStrip1.SuspendLayout();
        this.SuspendLayout();
        // 
        // animationTimer
        // 
        this.animationTimer.Interval = 25;
        this.animationTimer.Tick += new System.EventHandler(this.animationTimer_Tick);
        // 
        // zoomScaleTimer
        // 
        this.zoomScaleTimer.Interval = 1000;
        this.zoomScaleTimer.Tick += new System.EventHandler(this.zoomScaleTimer_Tick);
        // 
        // bZoomIn
        // 
        this.bZoomIn.BackColor = System.Drawing.Color.Transparent;
        this.bZoomIn.Location = new System.Drawing.Point(10, 10);
        this.bZoomIn.Name = "bZoomIn";
        this.bZoomIn.Size = new System.Drawing.Size(32, 32);
        this.bZoomIn.TabIndex = 0;
        this.bZoomIn.Text = "+";
        this.bZoomIn.UseVisualStyleBackColor = false;
        this.bZoomIn.Click += new System.EventHandler(this.bZoomIn_Click);
        // 
        // bZoomOut
        // 
        this.bZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.bZoomOut.Location = new System.Drawing.Point(10, 308);
        this.bZoomOut.Name = "bZoomOut";
        this.bZoomOut.Size = new System.Drawing.Size(32, 32);
        this.bZoomOut.TabIndex = 1;
        this.bZoomOut.Text = "-";
        this.bZoomOut.UseVisualStyleBackColor = true;
        this.bZoomOut.Click += new System.EventHandler(this.bZoomOut_Click);
        // 
        // contextMenuStrip1
        // 
        this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiLayer,
            this.centerPointToolStripMenuItem,
            this.loactionToolStripMenuItem,
            this.toolStripMenuItem1,
            this.clipboardToolStripMenuItem});
        this.contextMenuStrip1.Name = "contextMenuStrip1";
        this.contextMenuStrip1.Size = new System.Drawing.Size(162, 120);
        this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
        // 
        // tsiLayer
        // 
        this.tsiLayer.Name = "tsiLayer";
        this.tsiLayer.Size = new System.Drawing.Size(161, 22);
        this.tsiLayer.Text = "&Layer";
        // 
        // centerPointToolStripMenuItem
        // 
        this.centerPointToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiSetCenter,
            this.tsiRemoveCenter});
        this.centerPointToolStripMenuItem.Name = "centerPointToolStripMenuItem";
        this.centerPointToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
        this.centerPointToolStripMenuItem.Text = "&Center Point";
        // 
        // tsiSetCenter
        // 
        this.tsiSetCenter.Name = "tsiSetCenter";
        this.tsiSetCenter.Size = new System.Drawing.Size(117, 22);
        this.tsiSetCenter.Text = "&Set";
        this.tsiSetCenter.Click += new System.EventHandler(this.tsiSetCenter_Click);
        // 
        // tsiRemoveCenter
        // 
        this.tsiRemoveCenter.Enabled = false;
        this.tsiRemoveCenter.Name = "tsiRemoveCenter";
        this.tsiRemoveCenter.Size = new System.Drawing.Size(117, 22);
        this.tsiRemoveCenter.Text = "&Remove";
        this.tsiRemoveCenter.Click += new System.EventHandler(this.tsiRemoveCenter_Click);
        // 
        // loactionToolStripMenuItem
        // 
        this.loactionToolStripMenuItem.Name = "loactionToolStripMenuItem";
        this.loactionToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
        this.loactionToolStripMenuItem.Text = "Save as Loaction";
        this.loactionToolStripMenuItem.Click += new System.EventHandler(this.loactionToolStripMenuItem_Click);
        // 
        // toolStripMenuItem1
        // 
        this.toolStripMenuItem1.Name = "toolStripMenuItem1";
        this.toolStripMenuItem1.Size = new System.Drawing.Size(158, 6);
        // 
        // clipboardToolStripMenuItem
        // 
        this.clipboardToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyCoordinatesToolStripMenuItem});
        this.clipboardToolStripMenuItem.Name = "clipboardToolStripMenuItem";
        this.clipboardToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
        this.clipboardToolStripMenuItem.Text = "&Clipboard";
        // 
        // copyCoordinatesToolStripMenuItem
        // 
        this.copyCoordinatesToolStripMenuItem.Name = "copyCoordinatesToolStripMenuItem";
        this.copyCoordinatesToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
        this.copyCoordinatesToolStripMenuItem.Text = "&Copy Coordinates";
        this.copyCoordinatesToolStripMenuItem.Click += new System.EventHandler(this.copyCoordinatesToolStripMenuItem_Click);
        // 
        // MapView
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        this.ClientSize = new System.Drawing.Size(407, 352);
        this.ContextMenuStrip = this.contextMenuStrip1;
        this.Controls.Add(this.bZoomIn);
        this.Controls.Add(this.bZoomOut);
        this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Name = "MapView";
        this.Load += new System.EventHandler(this.MapView_Load);
        this.DoubleClick += new System.EventHandler(this.MapView_DoubleClick);
        this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapView_MouseDown);
        this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapView_MouseMove);
        this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapView_MouseUp);
        this.Resize += new System.EventHandler(this.MapView_Resize);
        this.contextMenuStrip1.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Timer animationTimer;
    private System.Windows.Forms.Timer zoomScaleTimer;
    private System.Windows.Forms.Button bZoomIn;
    private System.Windows.Forms.Button bZoomOut;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem tsiLayer;
    private System.Windows.Forms.ToolStripMenuItem centerPointToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem tsiSetCenter;
    private System.Windows.Forms.ToolStripMenuItem tsiRemoveCenter;
    private System.Windows.Forms.ToolStripMenuItem loactionToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem clipboardToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyCoordinatesToolStripMenuItem;
  }
}
