namespace WinCachebox.Components
{
  partial class CoordCacheWaypoint
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
      this.bCache = new System.Windows.Forms.Button();
      this.bCoord = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // bCache
      // 
      this.bCache.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.bCache.Location = new System.Drawing.Point(0, 0);
      this.bCache.Name = "bCache";
      this.bCache.Size = new System.Drawing.Size(388, 31);
      this.bCache.TabIndex = 0;
      this.bCache.Text = "other Waypoint...";
      this.bCache.UseVisualStyleBackColor = true;
      this.bCache.Click += new System.EventHandler(this.bCache_Click);
      // 
      // bCoord
      // 
      this.bCoord.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.bCoord.Location = new System.Drawing.Point(0, 30);
      this.bCoord.Name = "bCoord";
      this.bCoord.Size = new System.Drawing.Size(388, 43);
      this.bCoord.TabIndex = 1;
      this.bCoord.Text = "Coord";
      this.bCoord.UseVisualStyleBackColor = true;
      this.bCoord.Click += new System.EventHandler(this.bCoord_Click);
      // 
      // CoordCacheWaypoint
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.bCoord);
      this.Controls.Add(this.bCache);
      this.Name = "CoordCacheWaypoint";
      this.Size = new System.Drawing.Size(388, 73);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button bCache;
    private System.Windows.Forms.Button bCoord;
  }
}
