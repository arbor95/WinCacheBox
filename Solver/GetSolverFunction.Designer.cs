namespace WinCachebox.CBSolver
{
  partial class GetSolverFunction
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
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this.lvGroups = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.lvFuctions = new System.Windows.Forms.ListView();
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.splitContainer3 = new System.Windows.Forms.SplitContainer();
      this.lvOtherNames = new System.Windows.Forms.ListView();
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.tbDescription = new System.Windows.Forms.TextBox();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.splitContainer3.Panel1.SuspendLayout();
      this.splitContainer3.Panel2.SuspendLayout();
      this.splitContainer3.SuspendLayout();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.button1.Location = new System.Drawing.Point(589, 324);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 0;
      this.button1.Text = "&OK";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // button2
      // 
      this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.button2.Location = new System.Drawing.Point(589, 353);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 1;
      this.button2.Text = "&Cancel";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // splitContainer1
      // 
      this.splitContainer1.Location = new System.Drawing.Point(12, 12);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
      this.splitContainer1.Size = new System.Drawing.Size(571, 364);
      this.splitContainer1.SplitterDistance = 239;
      this.splitContainer1.TabIndex = 2;
      // 
      // splitContainer2
      // 
      this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer2.Location = new System.Drawing.Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.Controls.Add(this.lvGroups);
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.Controls.Add(this.lvFuctions);
      this.splitContainer2.Size = new System.Drawing.Size(571, 239);
      this.splitContainer2.SplitterDistance = 226;
      this.splitContainer2.TabIndex = 0;
      // 
      // lvGroups
      // 
      this.lvGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
      this.lvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lvGroups.FullRowSelect = true;
      this.lvGroups.HideSelection = false;
      this.lvGroups.Location = new System.Drawing.Point(0, 0);
      this.lvGroups.MultiSelect = false;
      this.lvGroups.Name = "lvGroups";
      this.lvGroups.Size = new System.Drawing.Size(226, 239);
      this.lvGroups.TabIndex = 0;
      this.lvGroups.UseCompatibleStateImageBehavior = false;
      this.lvGroups.View = System.Windows.Forms.View.Details;
      this.lvGroups.SelectedIndexChanged += new System.EventHandler(this.lvGroups_SelectedIndexChanged);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Group";
      this.columnHeader1.Width = 196;
      // 
      // lvFuctions
      // 
      this.lvFuctions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
      this.lvFuctions.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lvFuctions.FullRowSelect = true;
      this.lvFuctions.HideSelection = false;
      this.lvFuctions.Location = new System.Drawing.Point(0, 0);
      this.lvFuctions.MultiSelect = false;
      this.lvFuctions.Name = "lvFuctions";
      this.lvFuctions.Size = new System.Drawing.Size(341, 239);
      this.lvFuctions.TabIndex = 1;
      this.lvFuctions.UseCompatibleStateImageBehavior = false;
      this.lvFuctions.View = System.Windows.Forms.View.Details;
      this.lvFuctions.SelectedIndexChanged += new System.EventHandler(this.lvFuctions_SelectedIndexChanged);
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Functions";
      this.columnHeader2.Width = 309;
      // 
      // splitContainer3
      // 
      this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer3.Location = new System.Drawing.Point(0, 0);
      this.splitContainer3.Name = "splitContainer3";
      // 
      // splitContainer3.Panel1
      // 
      this.splitContainer3.Panel1.Controls.Add(this.lvOtherNames);
      // 
      // splitContainer3.Panel2
      // 
      this.splitContainer3.Panel2.Controls.Add(this.tbDescription);
      this.splitContainer3.Size = new System.Drawing.Size(571, 121);
      this.splitContainer3.SplitterDistance = 139;
      this.splitContainer3.TabIndex = 0;
      // 
      // lvOtherNames
      // 
      this.lvOtherNames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
      this.lvOtherNames.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lvOtherNames.FullRowSelect = true;
      this.lvOtherNames.HideSelection = false;
      this.lvOtherNames.Location = new System.Drawing.Point(0, 0);
      this.lvOtherNames.MultiSelect = false;
      this.lvOtherNames.Name = "lvOtherNames";
      this.lvOtherNames.Size = new System.Drawing.Size(139, 121);
      this.lvOtherNames.TabIndex = 2;
      this.lvOtherNames.UseCompatibleStateImageBehavior = false;
      this.lvOtherNames.View = System.Windows.Forms.View.Details;
      this.lvOtherNames.SelectedIndexChanged += new System.EventHandler(this.lvOtherNames_SelectedIndexChanged);
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Other Names";
      this.columnHeader3.Width = 309;
      // 
      // tbDescription
      // 
      this.tbDescription.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tbDescription.Location = new System.Drawing.Point(0, 0);
      this.tbDescription.Multiline = true;
      this.tbDescription.Name = "tbDescription";
      this.tbDescription.ReadOnly = true;
      this.tbDescription.Size = new System.Drawing.Size(428, 121);
      this.tbDescription.TabIndex = 1;
      // 
      // GetSolverFunction
      // 
      this.AcceptButton = this.button1;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.button2;
      this.ClientSize = new System.Drawing.Size(676, 388);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Name = "GetSolverFunction";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Get Solver Function";
      this.Load += new System.EventHandler(this.GetSolverFunction_Load);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.ResumeLayout(false);
      this.splitContainer3.Panel1.ResumeLayout(false);
      this.splitContainer3.Panel2.ResumeLayout(false);
      this.splitContainer3.Panel2.PerformLayout();
      this.splitContainer3.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.ListView lvGroups;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ListView lvFuctions;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.SplitContainer splitContainer3;
    private System.Windows.Forms.ListView lvOtherNames;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.TextBox tbDescription;
  }
}