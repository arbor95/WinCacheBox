namespace WinCachebox.Views.Forms
{
  partial class ProjectionForm
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
      WinCachebox.Coordinate coordinate1 = new WinCachebox.Coordinate();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.tbDistance = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.tbBearing = new System.Windows.Forms.TextBox();
      this.button4 = new System.Windows.Forms.Button();
      this.button5 = new System.Windows.Forms.Button();
      this.label5 = new System.Windows.Forms.Label();
      this.bResult = new System.Windows.Forms.Button();
      this.bCoord = new WinCachebox.Components.CoordCacheWaypoint();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.button1.Location = new System.Drawing.Point(318, 156);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 0;
      this.button1.Text = "&OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.button2.Location = new System.Drawing.Point(318, 185);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 1;
      this.button2.Text = "&Cancel";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // tbDistance
      // 
      this.tbDistance.Location = new System.Drawing.Point(89, 98);
      this.tbDistance.Name = "tbDistance";
      this.tbDistance.Size = new System.Drawing.Size(100, 20);
      this.tbDistance.TabIndex = 3;
      this.tbDistance.Text = "0";
      this.tbDistance.TextChanged += new System.EventHandler(this.tbDistance_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 101);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(52, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Distance:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(195, 101);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(15, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "m";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(195, 127);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(11, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "°";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(12, 127);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(46, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Bearing:";
      // 
      // tbBearing
      // 
      this.tbBearing.Location = new System.Drawing.Point(89, 124);
      this.tbBearing.Name = "tbBearing";
      this.tbBearing.Size = new System.Drawing.Size(100, 20);
      this.tbBearing.TabIndex = 6;
      this.tbBearing.Text = "0";
      this.tbBearing.TextChanged += new System.EventHandler(this.tbDistance_TextChanged);
      // 
      // button4
      // 
      this.button4.Location = new System.Drawing.Point(216, 98);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(88, 23);
      this.button4.TabIndex = 9;
      this.button4.Text = "From Solver...";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new System.EventHandler(this.button4_Click);
      // 
      // button5
      // 
      this.button5.Location = new System.Drawing.Point(216, 124);
      this.button5.Name = "button5";
      this.button5.Size = new System.Drawing.Size(88, 23);
      this.button5.TabIndex = 10;
      this.button5.Text = "From Solver...";
      this.button5.UseVisualStyleBackColor = true;
      this.button5.Click += new System.EventHandler(this.button5_Click);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(12, 154);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(40, 13);
      this.label5.TabIndex = 11;
      this.label5.Text = "Result:";
      // 
      // bResult
      // 
      this.bResult.Location = new System.Drawing.Point(12, 170);
      this.bResult.Name = "bResult";
      this.bResult.Size = new System.Drawing.Size(292, 38);
      this.bResult.TabIndex = 12;
      this.bResult.Text = "button6";
      this.bResult.UseVisualStyleBackColor = true;
      this.bResult.Click += new System.EventHandler(this.bResult_Click);
      // 
      // bCoord
      // 
      this.bCoord.Cache = null;
      coordinate1.Elevation = 0D;
      coordinate1.Latitude = 0D;
      coordinate1.Longitude = 0D;
      this.bCoord.Coordinate = coordinate1;
      this.bCoord.Location = new System.Drawing.Point(12, 12);
      this.bCoord.Name = "bCoord";
      this.bCoord.Size = new System.Drawing.Size(292, 65);
      this.bCoord.TabIndex = 14;
      this.bCoord.Waypoint = null;
      // 
      // ProjectionForm
      // 
      this.AcceptButton = this.button1;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.button2;
      this.ClientSize = new System.Drawing.Size(405, 220);
      this.Controls.Add(this.bCoord);
      this.Controls.Add(this.bResult);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.button5);
      this.Controls.Add(this.button4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.tbBearing);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.tbDistance);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "ProjectionForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Projection";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.TextBox tbDistance;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox tbBearing;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.Button button5;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Button bResult;
    private Components.CoordCacheWaypoint bCoord;
  }
}