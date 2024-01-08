namespace WinCachebox.Views
{
  partial class SdfExport
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
      this.tbExportPfad = new System.Windows.Forms.TextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.progressBar1 = new System.Windows.Forms.ProgressBar();
      this.lCache = new System.Windows.Forms.Label();
      this.lKategorie = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.tbMaxDistance = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.fsdExport = new System.Windows.Forms.SaveFileDialog();
      this.rbUpdate = new System.Windows.Forms.RadioButton();
      this.rbNew = new System.Windows.Forms.RadioButton();
      this.SuspendLayout();
      // 
      // tbExportPfad
      // 
      this.tbExportPfad.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tbExportPfad.Enabled = false;
      this.tbExportPfad.Location = new System.Drawing.Point(12, 27);
      this.tbExportPfad.Name = "tbExportPfad";
      this.tbExportPfad.Size = new System.Drawing.Size(433, 20);
      this.tbExportPfad.TabIndex = 0;
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.button1.Location = new System.Drawing.Point(451, 24);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(26, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "...";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.button2.Location = new System.Drawing.Point(403, 200);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 2;
      this.button2.Text = "&Export";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // progressBar1
      // 
      this.progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.progressBar1.Location = new System.Drawing.Point(12, 199);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size(385, 23);
      this.progressBar1.Step = 1;
      this.progressBar1.TabIndex = 3;
      // 
      // lCache
      // 
      this.lCache.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.lCache.Location = new System.Drawing.Point(12, 177);
      this.lCache.Name = "lCache";
      this.lCache.Size = new System.Drawing.Size(385, 19);
      this.lCache.TabIndex = 4;
      this.lCache.Text = "Cache:";
      // 
      // lKategorie
      // 
      this.lKategorie.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.lKategorie.Location = new System.Drawing.Point(12, 158);
      this.lKategorie.Name = "lKategorie";
      this.lKategorie.Size = new System.Drawing.Size(385, 19);
      this.lKategorie.TabIndex = 5;
      this.lKategorie.Text = "Kategorie:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(63, 13);
      this.label1.TabIndex = 6;
      this.label1.Text = "Destination:";
      // 
      // tbMaxDistance
      // 
      this.tbMaxDistance.Location = new System.Drawing.Point(242, 57);
      this.tbMaxDistance.Name = "tbMaxDistance";
      this.tbMaxDistance.Size = new System.Drawing.Size(56, 20);
      this.tbMaxDistance.TabIndex = 7;
      this.tbMaxDistance.Text = "0";
      this.tbMaxDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(304, 60);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(21, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "km";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 60);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(167, 13);
      this.label3.TabIndex = 9;
      this.label3.Text = "Max. Distance to Center: (0 for all)";
      // 
      // fsdExport
      // 
      this.fsdExport.DefaultExt = "*.sdf";
      this.fsdExport.FileName = "cachebox.sdf";
      this.fsdExport.Filter = "CacheBox Database|*.sdf";
      this.fsdExport.OverwritePrompt = false;
      this.fsdExport.Title = "Export Database";
      // 
      // rbUpdate
      // 
      this.rbUpdate.AutoSize = true;
      this.rbUpdate.Checked = true;
      this.rbUpdate.Location = new System.Drawing.Point(12, 98);
      this.rbUpdate.Name = "rbUpdate";
      this.rbUpdate.Size = new System.Drawing.Size(161, 17);
      this.rbUpdate.TabIndex = 10;
      this.rbUpdate.TabStop = true;
      this.rbUpdate.Text = "Update destination database";
      this.rbUpdate.UseVisualStyleBackColor = true;
      // 
      // rbNew
      // 
      this.rbNew.AutoSize = true;
      this.rbNew.Location = new System.Drawing.Point(12, 121);
      this.rbNew.Name = "rbNew";
      this.rbNew.Size = new System.Drawing.Size(178, 17);
      this.rbNew.TabIndex = 11;
      this.rbNew.Text = "Create new database, delete old";
      this.rbNew.UseVisualStyleBackColor = true;
      // 
      // SdfExport
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(490, 235);
      this.Controls.Add(this.rbNew);
      this.Controls.Add(this.rbUpdate);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.tbMaxDistance);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.lKategorie);
      this.Controls.Add(this.lCache);
      this.Controls.Add(this.progressBar1);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.tbExportPfad);
      this.Name = "SdfExport";
      this.Text = "SdfExport";
      this.Load += new System.EventHandler(this.SdfExport_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox tbExportPfad;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Windows.Forms.Label lCache;
    private System.Windows.Forms.Label lKategorie;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox tbMaxDistance;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.SaveFileDialog fsdExport;
    private System.Windows.Forms.RadioButton rbUpdate;
    private System.Windows.Forms.RadioButton rbNew;
  }
}