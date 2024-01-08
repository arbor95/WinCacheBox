namespace WinCachebox
{
  partial class ImportSettingsForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportSettingsForm));
        this.checkImportPQfromGC = new System.Windows.Forms.CheckBox();
        this.checkBoxImportGPX = new System.Windows.Forms.CheckBox();
        this.checkBoxImportGpxFromMail = new System.Windows.Forms.CheckBox();
        this.checkBoxGcVote = new System.Windows.Forms.CheckBox();
        this.checkBoxPreloadImages = new System.Windows.Forms.CheckBox();
        this.checkBoxImportMaps = new System.Windows.Forms.CheckBox();
        this.checkBoxImportCellIds = new System.Windows.Forms.CheckBox();
        this.button1 = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.checkDeleteGPX = new System.Windows.Forms.CheckBox();
        this.SuspendLayout();
        // 
        // checkImportPQfromGC
        // 
        this.checkImportPQfromGC.AutoSize = true;
        this.checkImportPQfromGC.Location = new System.Drawing.Point(12, 12);
        this.checkImportPQfromGC.Name = "checkImportPQfromGC";
        this.checkImportPQfromGC.Size = new System.Drawing.Size(143, 17);
        this.checkImportPQfromGC.TabIndex = 0;
        this.checkImportPQfromGC.Text = "Pocket Queries (gc.com)";
        this.checkImportPQfromGC.UseVisualStyleBackColor = true;
        this.checkImportPQfromGC.CheckStateChanged += new System.EventHandler(this.checkImportPQfromGC_CheckStateChanged);
        // 
        // checkBoxImportGPX
        // 
        this.checkBoxImportGPX.AutoSize = true;
        this.checkBoxImportGPX.Location = new System.Drawing.Point(12, 35);
        this.checkBoxImportGPX.Name = "checkBoxImportGPX";
        this.checkBoxImportGPX.Size = new System.Drawing.Size(48, 17);
        this.checkBoxImportGPX.TabIndex = 1;
        this.checkBoxImportGPX.Text = "GPX";
        this.checkBoxImportGPX.UseVisualStyleBackColor = true;
        this.checkBoxImportGPX.CheckStateChanged += new System.EventHandler(this.checkBoxImportGPX_CheckStateChanged);
        // 
        // checkBoxImportGpxFromMail
        // 
        this.checkBoxImportGpxFromMail.AutoSize = true;
        this.checkBoxImportGpxFromMail.Location = new System.Drawing.Point(189, 21);
        this.checkBoxImportGpxFromMail.Name = "checkBoxImportGpxFromMail";
        this.checkBoxImportGpxFromMail.Size = new System.Drawing.Size(83, 17);
        this.checkBoxImportGpxFromMail.TabIndex = 2;
        this.checkBoxImportGpxFromMail.Text = "check Mails";
        this.checkBoxImportGpxFromMail.UseVisualStyleBackColor = true;
        // 
        // checkBoxGcVote
        // 
        this.checkBoxGcVote.AutoSize = true;
        this.checkBoxGcVote.Location = new System.Drawing.Point(12, 78);
        this.checkBoxGcVote.Name = "checkBoxGcVote";
        this.checkBoxGcVote.Size = new System.Drawing.Size(213, 17);
        this.checkBoxGcVote.TabIndex = 3;
        this.checkBoxGcVote.Text = "GcVote Cache Ratings (Filter Selection)";
        this.checkBoxGcVote.UseVisualStyleBackColor = true;
        // 
        // checkBoxPreloadImages
        // 
        this.checkBoxPreloadImages.AutoSize = true;
        this.checkBoxPreloadImages.Location = new System.Drawing.Point(12, 101);
        this.checkBoxPreloadImages.Name = "checkBoxPreloadImages";
        this.checkBoxPreloadImages.Size = new System.Drawing.Size(194, 17);
        this.checkBoxPreloadImages.TabIndex = 4;
        this.checkBoxPreloadImages.Text = "Description Images (Filter Selection)";
        this.checkBoxPreloadImages.UseVisualStyleBackColor = true;
        // 
        // checkBoxImportMaps
        // 
        this.checkBoxImportMaps.AutoSize = true;
        this.checkBoxImportMaps.Location = new System.Drawing.Point(12, 124);
        this.checkBoxImportMaps.Name = "checkBoxImportMaps";
        this.checkBoxImportMaps.Size = new System.Drawing.Size(52, 17);
        this.checkBoxImportMaps.TabIndex = 6;
        this.checkBoxImportMaps.Text = "Maps";
        this.checkBoxImportMaps.UseVisualStyleBackColor = true;
        // 
        // checkBoxImportCellIds
        // 
        this.checkBoxImportCellIds.AutoSize = true;
        this.checkBoxImportCellIds.Location = new System.Drawing.Point(12, 147);
        this.checkBoxImportCellIds.Name = "checkBoxImportCellIds";
        this.checkBoxImportCellIds.Size = new System.Drawing.Size(60, 17);
        this.checkBoxImportCellIds.TabIndex = 7;
        this.checkBoxImportCellIds.Text = "Cell Ids";
        this.checkBoxImportCellIds.UseVisualStyleBackColor = true;
        this.checkBoxImportCellIds.Visible = false;
        this.checkBoxImportCellIds.CheckedChanged += new System.EventHandler(this.checkBoxImportCellIds_CheckedChanged);
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(259, 155);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(78, 34);
        this.button1.TabIndex = 8;
        this.button1.Text = "Import";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // button2
        // 
        this.button2.Location = new System.Drawing.Point(175, 155);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(78, 34);
        this.button2.TabIndex = 9;
        this.button2.Text = "Cancel";
        this.button2.UseVisualStyleBackColor = true;
        this.button2.Click += new System.EventHandler(this.button2_Click);
        // 
        // checkDeleteGPX
        // 
        this.checkDeleteGPX.AutoSize = true;
        this.checkDeleteGPX.Location = new System.Drawing.Point(32, 55);
        this.checkDeleteGPX.Name = "checkDeleteGPX";
        this.checkDeleteGPX.Size = new System.Drawing.Size(82, 17);
        this.checkDeleteGPX.TabIndex = 10;
        this.checkDeleteGPX.Text = "Delete GPX";
        this.checkDeleteGPX.UseVisualStyleBackColor = true;
        // 
        // ImportSettingsForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(349, 201);
        this.Controls.Add(this.checkDeleteGPX);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.checkBoxImportCellIds);
        this.Controls.Add(this.checkBoxImportMaps);
        this.Controls.Add(this.checkBoxPreloadImages);
        this.Controls.Add(this.checkBoxGcVote);
        this.Controls.Add(this.checkBoxImportGpxFromMail);
        this.Controls.Add(this.checkBoxImportGPX);
        this.Controls.Add(this.checkImportPQfromGC);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "ImportSettingsForm";
        this.Text = "Synchronize";
        this.Load += new System.EventHandler(this.ImportSettingsForm_Load);
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox checkImportPQfromGC;
    private System.Windows.Forms.CheckBox checkBoxImportGPX;
    private System.Windows.Forms.CheckBox checkBoxImportGpxFromMail;
    private System.Windows.Forms.CheckBox checkBoxGcVote;
    private System.Windows.Forms.CheckBox checkBoxPreloadImages;
    private System.Windows.Forms.CheckBox checkBoxImportMaps;
    private System.Windows.Forms.CheckBox checkBoxImportCellIds;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.CheckBox checkDeleteGPX;
  }
}