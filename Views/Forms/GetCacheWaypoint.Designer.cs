namespace WinCachebox.Views.Forms
{
  partial class GetCacheWaypoint
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
      this.button2 = new System.Windows.Forms.Button();
      this.button1 = new System.Windows.Forms.Button();
      this.cacheListView1 = new WinCachebox.Views.CacheListView();
      this.waypointView1 = new WinCachebox.Views.WaypointView();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // button2
      // 
      this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.button2.Location = new System.Drawing.Point(526, 363);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 5;
      this.button2.Text = "&Cancel";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.button1.Location = new System.Drawing.Point(526, 334);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 4;
      this.button1.Text = "&OK";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // cacheListView1
      // 
      this.cacheListView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cacheListView1.Location = new System.Drawing.Point(0, 0);
      this.cacheListView1.Name = "cacheListView1";
      this.cacheListView1.Size = new System.Drawing.Size(508, 236);
      this.cacheListView1.TabIndex = 10;
      this.cacheListView1.WaypointView = this.waypointView1;
      // 
      // waypointView1
      // 
      this.waypointView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.waypointView1.Local = true;
      this.waypointView1.Location = new System.Drawing.Point(0, 0);
      this.waypointView1.Name = "waypointView1";
      this.waypointView1.Size = new System.Drawing.Size(508, 134);
      this.waypointView1.TabIndex = 9;
      // 
      // splitContainer1
      // 
      this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.splitContainer1.Location = new System.Drawing.Point(12, 12);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.cacheListView1);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.waypointView1);
      this.splitContainer1.Size = new System.Drawing.Size(508, 374);
      this.splitContainer1.SplitterDistance = 236;
      this.splitContainer1.TabIndex = 11;
      // 
      // GetCacheWaypoint
      // 
      this.AcceptButton = this.button1;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.button2;
      this.ClientSize = new System.Drawing.Size(613, 398);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Name = "GetCacheWaypoint";
      this.Text = "Get Waypoint";
      this.Load += new System.EventHandler(this.GetCacheWaypoint_Load);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button1;
    private WaypointView waypointView1;
    private CacheListView cacheListView1;
    private System.Windows.Forms.SplitContainer splitContainer1;
  }
}