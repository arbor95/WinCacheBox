namespace WinCachebox.Views
{
  partial class WaypointView
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
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.miNew = new System.Windows.Forms.ToolStripMenuItem();
      this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
      this.setAsCenterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
      this.projectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
      this.copyGcCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.copyCoordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.gWaypoints = new SourceGrid.Grid();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEdit,
            this.toolStripMenuItem1,
            this.miNew,
            this.miDelete,
            this.toolStripMenuItem2,
            this.setAsCenterToolStripMenuItem,
            this.toolStripMenuItem3,
            this.projectionToolStripMenuItem,
            this.toolStripMenuItem4,
            this.copyGcCodeToolStripMenuItem,
            this.copyCoordinatesToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(170, 204);
      this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
      // 
      // miEdit
      // 
      this.miEdit.Name = "miEdit";
      this.miEdit.Size = new System.Drawing.Size(169, 22);
      this.miEdit.Text = "&Edit Waypoint";
      this.miEdit.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(166, 6);
      // 
      // miNew
      // 
      this.miNew.Name = "miNew";
      this.miNew.Size = new System.Drawing.Size(169, 22);
      this.miNew.Text = "&New Waypoint";
      this.miNew.Click += new System.EventHandler(this.newWaypointToolStripMenuItem_Click);
      // 
      // miDelete
      // 
      this.miDelete.Name = "miDelete";
      this.miDelete.Size = new System.Drawing.Size(169, 22);
      this.miDelete.Text = "&Delete Waypoint";
      this.miDelete.Click += new System.EventHandler(this.löschenToolStripMenuItem_Click);
      // 
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new System.Drawing.Size(166, 6);
      // 
      // setAsCenterToolStripMenuItem
      // 
      this.setAsCenterToolStripMenuItem.Name = "setAsCenterToolStripMenuItem";
      this.setAsCenterToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.setAsCenterToolStripMenuItem.Text = "Set as &Center";
      this.setAsCenterToolStripMenuItem.Click += new System.EventHandler(this.setAsCenterToolStripMenuItem_Click);
      // 
      // toolStripMenuItem3
      // 
      this.toolStripMenuItem3.Name = "toolStripMenuItem3";
      this.toolStripMenuItem3.Size = new System.Drawing.Size(166, 6);
      // 
      // projectionToolStripMenuItem
      // 
      this.projectionToolStripMenuItem.Name = "projectionToolStripMenuItem";
      this.projectionToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.projectionToolStripMenuItem.Text = "&Projection";
      this.projectionToolStripMenuItem.Click += new System.EventHandler(this.projectionToolStripMenuItem_Click);
      // 
      // toolStripMenuItem4
      // 
      this.toolStripMenuItem4.Name = "toolStripMenuItem4";
      this.toolStripMenuItem4.Size = new System.Drawing.Size(166, 6);
      // 
      // copyGcCodeToolStripMenuItem
      // 
      this.copyGcCodeToolStripMenuItem.Name = "copyGcCodeToolStripMenuItem";
      this.copyGcCodeToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.copyGcCodeToolStripMenuItem.Text = "Copy &GcCode";
      this.copyGcCodeToolStripMenuItem.Click += new System.EventHandler(this.copyGcCodeToolStripMenuItem_Click);
      // 
      // copyCoordinatesToolStripMenuItem
      // 
      this.copyCoordinatesToolStripMenuItem.Name = "copyCoordinatesToolStripMenuItem";
      this.copyCoordinatesToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.copyCoordinatesToolStripMenuItem.Text = "Copy &Coordinates";
      this.copyCoordinatesToolStripMenuItem.Click += new System.EventHandler(this.copyCoordinatesToolStripMenuItem_Click);
      // 
      // gWaypoints
      // 
      this.gWaypoints.ContextMenuStrip = this.contextMenuStrip1;
      this.gWaypoints.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gWaypoints.EnableSort = true;
      this.gWaypoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.gWaypoints.Location = new System.Drawing.Point(0, 0);
      this.gWaypoints.Name = "gWaypoints";
      this.gWaypoints.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
      this.gWaypoints.SelectionMode = SourceGrid.GridSelectionMode.Row;
      this.gWaypoints.Size = new System.Drawing.Size(436, 295);
      this.gWaypoints.TabIndex = 9;
      this.gWaypoints.TabStop = true;
      this.gWaypoints.ToolTipText = "";
      this.gWaypoints.DoubleClick += new System.EventHandler(this.gWaypoints_DoubleClick);
      this.gWaypoints.Resize += new System.EventHandler(this.gWaypoints_Resize);
      // 
      // WaypointView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(436, 295);
      this.Controls.Add(this.gWaypoints);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "WaypointView";
      this.Load += new System.EventHandler(this.WaypointView_Load);
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private SourceGrid.Grid gWaypoints;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem miEdit;
    private System.Windows.Forms.ToolStripMenuItem miNew;
    private System.Windows.Forms.ToolStripMenuItem miDelete;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    private System.Windows.Forms.ToolStripMenuItem setAsCenterToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
    private System.Windows.Forms.ToolStripMenuItem projectionToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
    private System.Windows.Forms.ToolStripMenuItem copyGcCodeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyCoordinatesToolStripMenuItem;
  }
}
