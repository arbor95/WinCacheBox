namespace WinCachebox.Views
{
  partial class SolverView
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tbLösung = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bAddFinal = new System.Windows.Forms.Button();
            this.bSetCenter = new System.Windows.Forms.Button();
            this.fFunction = new System.Windows.Forms.Button();
            this.bInsertWaypoint = new System.Windows.Forms.Button();
            this.ttInsertWaypoint = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(422, 388);
            this.textBox1.TabIndex = 0;
            this.textBox1.WordWrap = false;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(3, 397);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 27);
            this.button1.TabIndex = 1;
            this.button1.Text = "Solve";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbLösung
            // 
            this.tbLösung.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLösung.Location = new System.Drawing.Point(0, 0);
            this.tbLösung.Multiline = true;
            this.tbLösung.Name = "tbLösung";
            this.tbLösung.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLösung.Size = new System.Drawing.Size(206, 388);
            this.tbLösung.TabIndex = 2;
            this.tbLösung.WordWrap = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbLösung);
            this.splitContainer1.Size = new System.Drawing.Size(633, 388);
            this.splitContainer1.SplitterDistance = 422;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 3;
            // 
            // bAddFinal
            // 
            this.bAddFinal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bAddFinal.Location = new System.Drawing.Point(536, 397);
            this.bAddFinal.Name = "bAddFinal";
            this.bAddFinal.Size = new System.Drawing.Size(87, 27);
            this.bAddFinal.TabIndex = 4;
            this.bAddFinal.Text = "Add Final";
            this.bAddFinal.UseVisualStyleBackColor = true;
            this.bAddFinal.Click += new System.EventHandler(this.bAddFinal_Click);
            // 
            // bSetCenter
            // 
            this.bSetCenter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bSetCenter.Location = new System.Drawing.Point(442, 397);
            this.bSetCenter.Name = "bSetCenter";
            this.bSetCenter.Size = new System.Drawing.Size(88, 27);
            this.bSetCenter.TabIndex = 5;
            this.bSetCenter.Text = "As Center";
            this.bSetCenter.UseVisualStyleBackColor = true;
            this.bSetCenter.Click += new System.EventHandler(this.bSetCenter_Click);
            // 
            // fFunction
            // 
            this.fFunction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.fFunction.Location = new System.Drawing.Point(96, 397);
            this.fFunction.Name = "fFunction";
            this.fFunction.Size = new System.Drawing.Size(115, 27);
            this.fFunction.TabIndex = 6;
            this.fFunction.Text = "&Insert Function";
            this.fFunction.UseVisualStyleBackColor = true;
            this.fFunction.Click += new System.EventHandler(this.fFunction_Click);
            // 
            // bInsertWaypoint
            // 
            this.bInsertWaypoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bInsertWaypoint.Location = new System.Drawing.Point(217, 397);
            this.bInsertWaypoint.Name = "bInsertWaypoint";
            this.bInsertWaypoint.Size = new System.Drawing.Size(115, 27);
            this.bInsertWaypoint.TabIndex = 7;
            this.bInsertWaypoint.Text = "Insert &Waypoint";
            this.ttInsertWaypoint.SetToolTip(this.bInsertWaypoint, "Fügt einen zum Cachetyp passenden Wegpunkt und Zuweisung im Solver ein");
            this.bInsertWaypoint.UseVisualStyleBackColor = true;
            this.bInsertWaypoint.Click += new System.EventHandler(this.bInsertWaypoint_Click);
            // 
            // SolverView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 429);
            this.Controls.Add(this.bInsertWaypoint);
            this.Controls.Add(this.fFunction);
            this.Controls.Add(this.bSetCenter);
            this.Controls.Add(this.bAddFinal);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SolverView";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.TextBox tbLösung;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.Button bAddFinal;
    private System.Windows.Forms.Button bSetCenter;
    private System.Windows.Forms.Button fFunction;
    private System.Windows.Forms.Button bInsertWaypoint;
    private System.Windows.Forms.ToolTip ttInsertWaypoint;
  }
}
