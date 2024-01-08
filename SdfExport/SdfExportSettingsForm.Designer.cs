namespace WinCachebox.SdfExport
{
  partial class SdfExportSettingsForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SdfExportSettingsForm));
        this.button2 = new System.Windows.Forms.Button();
        this.button1 = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.tbDescription = new System.Windows.Forms.TextBox();
        this.fsdExport = new System.Windows.Forms.SaveFileDialog();
        this.gbDatabase = new System.Windows.Forms.GroupBox();
        this.chkUpdate = new System.Windows.Forms.CheckBox();
        this.chkOwnRepository = new System.Windows.Forms.CheckBox();
        this.bSetDestination = new System.Windows.Forms.Button();
        this.tbExportPfad = new System.Windows.Forms.TextBox();
        this.gbExport = new System.Windows.Forms.GroupBox();
        this.lb0forall = new System.Windows.Forms.Label();
        this.lbMaxLogs = new System.Windows.Forms.Label();
        this.nudMaxLogs = new System.Windows.Forms.NumericUpDown();
        this.lstMapPacks = new System.Windows.Forms.CheckedListBox();
        this.chkMapPacks = new System.Windows.Forms.CheckBox();
        this.chkMaps = new System.Windows.Forms.CheckBox();
        this.chkSpoilers = new System.Windows.Forms.CheckBox();
        this.chkImages = new System.Windows.Forms.CheckBox();
        this.gbSelection = new System.Windows.Forms.GroupBox();
        this.label5 = new System.Windows.Forms.Label();
        this.cbLocation = new System.Windows.Forms.ComboBox();
        this.label3 = new System.Windows.Forms.Label();
        this.tbMaxDistance = new System.Windows.Forms.TextBox();
        this.bFilter = new System.Windows.Forms.Button();
        this.label2 = new System.Windows.Forms.Label();
        this.gbDatabase.SuspendLayout();
        this.gbExport.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudMaxLogs)).BeginInit();
        this.gbSelection.SuspendLayout();
        this.SuspendLayout();
        // 
        // button2
        // 
        this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.button2.Location = new System.Drawing.Point(344, 439);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(75, 23);
        this.button2.TabIndex = 6;
        this.button2.Text = "&Cancel";
        this.button2.UseVisualStyleBackColor = true;
        // 
        // button1
        // 
        this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.button1.Location = new System.Drawing.Point(425, 439);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(75, 23);
        this.button1.TabIndex = 5;
        this.button1.Text = "&OK";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(22, 10);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(63, 13);
        this.label1.TabIndex = 7;
        this.label1.Text = "Description:";
        // 
        // tbDescription
        // 
        this.tbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.tbDescription.Location = new System.Drawing.Point(127, 7);
        this.tbDescription.Name = "tbDescription";
        this.tbDescription.Size = new System.Drawing.Size(365, 20);
        this.tbDescription.TabIndex = 8;
        // 
        // fsdExport
        // 
        this.fsdExport.DefaultExt = "*.sdf";
        this.fsdExport.FileName = "cachebox.sdf";
        this.fsdExport.Filter = "CacheBox Database|*.sdf";
        this.fsdExport.OverwritePrompt = false;
        this.fsdExport.Title = "Export Database (SDF)";
        // 
        // gbDatabase
        // 
        this.gbDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.gbDatabase.Controls.Add(this.chkUpdate);
        this.gbDatabase.Controls.Add(this.chkOwnRepository);
        this.gbDatabase.Controls.Add(this.bSetDestination);
        this.gbDatabase.Controls.Add(this.tbExportPfad);
        this.gbDatabase.Location = new System.Drawing.Point(15, 124);
        this.gbDatabase.Name = "gbDatabase";
        this.gbDatabase.Size = new System.Drawing.Size(485, 97);
        this.gbDatabase.TabIndex = 34;
        this.gbDatabase.TabStop = false;
        this.gbDatabase.Text = "Database:";
        // 
        // chkUpdate
        // 
        this.chkUpdate.AutoSize = true;
        this.chkUpdate.Location = new System.Drawing.Point(10, 68);
        this.chkUpdate.Name = "chkUpdate";
        this.chkUpdate.Size = new System.Drawing.Size(61, 17);
        this.chkUpdate.TabIndex = 43;
        this.chkUpdate.Text = "Update";
        this.chkUpdate.UseVisualStyleBackColor = true;
        // 
        // chkOwnRepository
        // 
        this.chkOwnRepository.AutoSize = true;
        this.chkOwnRepository.Location = new System.Drawing.Point(10, 45);
        this.chkOwnRepository.Name = "chkOwnRepository";
        this.chkOwnRepository.Size = new System.Drawing.Size(349, 17);
        this.chkOwnRepository.TabIndex = 42;
        this.chkOwnRepository.Text = "Use own Repository for this Database for Spoilers, Images and Maps";
        this.chkOwnRepository.UseVisualStyleBackColor = true;
        // 
        // bSetDestination
        // 
        this.bSetDestination.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.bSetDestination.Location = new System.Drawing.Point(451, 19);
        this.bSetDestination.Name = "bSetDestination";
        this.bSetDestination.Size = new System.Drawing.Size(26, 23);
        this.bSetDestination.TabIndex = 40;
        this.bSetDestination.Text = "...";
        this.bSetDestination.UseVisualStyleBackColor = true;
        this.bSetDestination.Click += new System.EventHandler(this.bSetDestination_Click);
        // 
        // tbExportPfad
        // 
        this.tbExportPfad.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.tbExportPfad.Enabled = false;
        this.tbExportPfad.Location = new System.Drawing.Point(10, 19);
        this.tbExportPfad.Name = "tbExportPfad";
        this.tbExportPfad.Size = new System.Drawing.Size(435, 20);
        this.tbExportPfad.TabIndex = 39;
        // 
        // gbExport
        // 
        this.gbExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.gbExport.Controls.Add(this.lb0forall);
        this.gbExport.Controls.Add(this.lbMaxLogs);
        this.gbExport.Controls.Add(this.nudMaxLogs);
        this.gbExport.Controls.Add(this.lstMapPacks);
        this.gbExport.Controls.Add(this.chkMapPacks);
        this.gbExport.Controls.Add(this.chkMaps);
        this.gbExport.Controls.Add(this.chkSpoilers);
        this.gbExport.Controls.Add(this.chkImages);
        this.gbExport.Location = new System.Drawing.Point(15, 227);
        this.gbExport.Name = "gbExport";
        this.gbExport.Size = new System.Drawing.Size(485, 206);
        this.gbExport.TabIndex = 38;
        this.gbExport.TabStop = false;
        this.gbExport.Text = "Export:";
        // 
        // lb0forall
        // 
        this.lb0forall.Location = new System.Drawing.Point(174, 180);
        this.lb0forall.Name = "lb0forall";
        this.lb0forall.Size = new System.Drawing.Size(118, 13);
        this.lb0forall.TabIndex = 40;
        this.lb0forall.Text = "(0 for all)";
        this.lb0forall.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // lbMaxLogs
        // 
        this.lbMaxLogs.AutoSize = true;
        this.lbMaxLogs.Location = new System.Drawing.Point(7, 180);
        this.lbMaxLogs.Name = "lbMaxLogs";
        this.lbMaxLogs.Size = new System.Drawing.Size(56, 13);
        this.lbMaxLogs.TabIndex = 39;
        this.lbMaxLogs.Text = "Max. Logs";
        // 
        // nudMaxLogs
        // 
        this.nudMaxLogs.Location = new System.Drawing.Point(112, 178);
        this.nudMaxLogs.Name = "nudMaxLogs";
        this.nudMaxLogs.Size = new System.Drawing.Size(56, 20);
        this.nudMaxLogs.TabIndex = 38;
        // 
        // lstMapPacks
        // 
        this.lstMapPacks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.lstMapPacks.CheckOnClick = true;
        this.lstMapPacks.FormattingEnabled = true;
        this.lstMapPacks.Location = new System.Drawing.Point(10, 63);
        this.lstMapPacks.Name = "lstMapPacks";
        this.lstMapPacks.Size = new System.Drawing.Size(467, 109);
        this.lstMapPacks.TabIndex = 36;
        // 
        // chkMapPacks
        // 
        this.chkMapPacks.AutoSize = true;
        this.chkMapPacks.Location = new System.Drawing.Point(10, 42);
        this.chkMapPacks.Name = "chkMapPacks";
        this.chkMapPacks.Size = new System.Drawing.Size(80, 17);
        this.chkMapPacks.TabIndex = 32;
        this.chkMapPacks.Text = "Map Packs";
        this.chkMapPacks.UseVisualStyleBackColor = true;
        this.chkMapPacks.CheckedChanged += new System.EventHandler(this.chkMapPacks_CheckedChanged);
        // 
        // chkMaps
        // 
        this.chkMaps.AutoSize = true;
        this.chkMaps.Location = new System.Drawing.Point(290, 19);
        this.chkMaps.Name = "chkMaps";
        this.chkMaps.Size = new System.Drawing.Size(81, 17);
        this.chkMaps.TabIndex = 29;
        this.chkMaps.Text = "Map Cache";
        this.chkMaps.UseVisualStyleBackColor = true;
        // 
        // chkSpoilers
        // 
        this.chkSpoilers.AutoSize = true;
        this.chkSpoilers.Location = new System.Drawing.Point(150, 19);
        this.chkSpoilers.Name = "chkSpoilers";
        this.chkSpoilers.Size = new System.Drawing.Size(63, 17);
        this.chkSpoilers.TabIndex = 28;
        this.chkSpoilers.Text = "Spoilers";
        this.chkSpoilers.UseVisualStyleBackColor = true;
        // 
        // chkImages
        // 
        this.chkImages.AutoSize = true;
        this.chkImages.Location = new System.Drawing.Point(10, 19);
        this.chkImages.Name = "chkImages";
        this.chkImages.Size = new System.Drawing.Size(60, 17);
        this.chkImages.TabIndex = 27;
        this.chkImages.Text = "Images";
        this.chkImages.UseVisualStyleBackColor = true;
        // 
        // gbSelection
        // 
        this.gbSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.gbSelection.Controls.Add(this.label5);
        this.gbSelection.Controls.Add(this.cbLocation);
        this.gbSelection.Controls.Add(this.label3);
        this.gbSelection.Controls.Add(this.tbMaxDistance);
        this.gbSelection.Controls.Add(this.bFilter);
        this.gbSelection.Controls.Add(this.label2);
        this.gbSelection.Location = new System.Drawing.Point(15, 33);
        this.gbSelection.Name = "gbSelection";
        this.gbSelection.Size = new System.Drawing.Size(485, 85);
        this.gbSelection.TabIndex = 39;
        this.gbSelection.TabStop = false;
        this.gbSelection.Text = "Selection:";
        // 
        // label5
        // 
        this.label5.Location = new System.Drawing.Point(174, 57);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(118, 13);
        this.label5.TabIndex = 27;
        this.label5.Text = "km  (0 for all)     from";
        this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // cbLocation
        // 
        this.cbLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbLocation.FormattingEnabled = true;
        this.cbLocation.Location = new System.Drawing.Point(299, 54);
        this.cbLocation.Name = "cbLocation";
        this.cbLocation.Size = new System.Drawing.Size(178, 21);
        this.cbLocation.TabIndex = 25;
        this.cbLocation.SelectedIndexChanged += new System.EventHandler(this.cbLocation_SelectedIndexChanged);
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(7, 57);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(78, 13);
        this.label3.TabIndex = 24;
        this.label3.Text = "Max. Distance:";
        // 
        // tbMaxDistance
        // 
        this.tbMaxDistance.Location = new System.Drawing.Point(112, 54);
        this.tbMaxDistance.Name = "tbMaxDistance";
        this.tbMaxDistance.Size = new System.Drawing.Size(56, 20);
        this.tbMaxDistance.TabIndex = 23;
        this.tbMaxDistance.Text = "0";
        this.tbMaxDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        this.tbMaxDistance.TextChanged += new System.EventHandler(this.tbMaxDistance_TextChanged);
        // 
        // bFilter
        // 
        this.bFilter.Location = new System.Drawing.Point(112, 25);
        this.bFilter.Name = "bFilter";
        this.bFilter.Size = new System.Drawing.Size(180, 23);
        this.bFilter.TabIndex = 22;
        this.bFilter.Text = "Filter";
        this.bFilter.UseVisualStyleBackColor = true;
        this.bFilter.Click += new System.EventHandler(this.bFilter_Click);
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(7, 30);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(73, 13);
        this.label2.TabIndex = 21;
        this.label2.Text = "Filter Settings:";
        // 
        // SdfExportSettingsForm
        // 
        this.AcceptButton = this.button1;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.button2;
        this.ClientSize = new System.Drawing.Size(512, 470);
        this.Controls.Add(this.gbSelection);
        this.Controls.Add(this.gbExport);
        this.Controls.Add(this.gbDatabase);
        this.Controls.Add(this.tbDescription);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.button1);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "SdfExportSettingsForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Sdf-Export Settings";
        this.Load += new System.EventHandler(this.SdfExportSettingsForm_Load);
        this.gbDatabase.ResumeLayout(false);
        this.gbDatabase.PerformLayout();
        this.gbExport.ResumeLayout(false);
        this.gbExport.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.nudMaxLogs)).EndInit();
        this.gbSelection.ResumeLayout(false);
        this.gbSelection.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox tbDescription;
    private System.Windows.Forms.SaveFileDialog fsdExport;
    private System.Windows.Forms.GroupBox gbDatabase;
    private System.Windows.Forms.CheckBox chkUpdate;
    private System.Windows.Forms.CheckBox chkOwnRepository;
    private System.Windows.Forms.Button bSetDestination;
    private System.Windows.Forms.TextBox tbExportPfad;
    private System.Windows.Forms.GroupBox gbExport;
    private System.Windows.Forms.Label lb0forall;
    private System.Windows.Forms.Label lbMaxLogs;
    private System.Windows.Forms.NumericUpDown nudMaxLogs;
    private System.Windows.Forms.CheckedListBox lstMapPacks;
    private System.Windows.Forms.CheckBox chkMapPacks;
    private System.Windows.Forms.CheckBox chkMaps;
    private System.Windows.Forms.CheckBox chkSpoilers;
    private System.Windows.Forms.CheckBox chkImages;
    private System.Windows.Forms.GroupBox gbSelection;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.ComboBox cbLocation;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox tbMaxDistance;
    private System.Windows.Forms.Button bFilter;
    private System.Windows.Forms.Label label2;
  }
}
