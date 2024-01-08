namespace WinCachebox.Views
{
  partial class FilterForm
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
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterForm));
        this.button1 = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.tbName = new System.Windows.Forms.TextBox();
        this.label2 = new System.Windows.Forms.Label();
        this.cmsGpxFilenames = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.unselectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
        this.checkSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.uncheckSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.tabControl1 = new System.Windows.Forms.TabControl();
        this.tabPage7 = new System.Windows.Forms.TabPage();
        this.dgPresets = new System.Windows.Forms.DataGridView();
        this.hIcon = new System.Windows.Forms.DataGridViewImageColumn();
        this.hPreset = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.tabPage1 = new System.Windows.Forms.TabPage();
        this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
        this.label5 = new System.Windows.Forms.Label();
        this.splitContainer1 = new System.Windows.Forms.SplitContainer();
        this.gCategory = new SourceGrid.Grid();
        this.cmsCategory = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.miPinned = new System.Windows.Forms.ToolStripMenuItem();
        this.gGPXFilenames = new SourceGrid.Grid();
        this.tabPage2 = new System.Windows.Forms.TabPage();
        this.gGeneral = new SourceGrid.Grid();
        this.tabPage3 = new System.Windows.Forms.TabPage();
        this.gDT = new SourceGrid.Grid();
        this.tabPage4 = new System.Windows.Forms.TabPage();
        this.gCacheType = new SourceGrid.Grid();
        this.tabPage5 = new System.Windows.Forms.TabPage();
        this.gAttributes = new SourceGrid.Grid();
        this.tabPage6 = new System.Windows.Forms.TabPage();
        this.lstStates = new System.Windows.Forms.CheckedListBox();
        this.lstCountries = new System.Windows.Forms.CheckedListBox();
        this.label4 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.cmsGpxFilenames.SuspendLayout();
        this.tabControl1.SuspendLayout();
        this.tabPage7.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgPresets)).BeginInit();
        this.tabPage1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
        this.splitContainer1.Panel1.SuspendLayout();
        this.splitContainer1.Panel2.SuspendLayout();
        this.splitContainer1.SuspendLayout();
        this.cmsCategory.SuspendLayout();
        this.tabPage2.SuspendLayout();
        this.tabPage3.SuspendLayout();
        this.tabPage4.SuspendLayout();
        this.tabPage5.SuspendLayout();
        this.tabPage6.SuspendLayout();
        this.SuspendLayout();
        // 
        // button1
        // 
        this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.button1.Location = new System.Drawing.Point(436, 383);
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
        this.button2.Location = new System.Drawing.Point(436, 412);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(75, 23);
        this.button2.TabIndex = 1;
        this.button2.Text = "&Cancel";
        this.button2.UseVisualStyleBackColor = true;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(6, 8);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(94, 13);
        this.label1.TabIndex = 3;
        this.label1.Text = "Filter Cache-Name";
        // 
        // tbName
        // 
        this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.tbName.Location = new System.Drawing.Point(6, 24);
        this.tbName.Name = "tbName";
        this.tbName.Size = new System.Drawing.Size(398, 20);
        this.tbName.TabIndex = 4;
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(6, 74);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(104, 13);
        this.label2.TabIndex = 5;
        this.label2.Text = "Filter GPX-Filenames";
        // 
        // cmsGpxFilenames
        // 
        this.cmsGpxFilenames.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.unselectAllToolStripMenuItem,
            this.toolStripMenuItem1,
            this.checkSelectionToolStripMenuItem,
            this.uncheckSelectionToolStripMenuItem});
        this.cmsGpxFilenames.Name = "cmsGpxFilenames";
        this.cmsGpxFilenames.Size = new System.Drawing.Size(172, 98);
        // 
        // selectAllToolStripMenuItem
        // 
        this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
        this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
        this.selectAllToolStripMenuItem.Text = "Checkt All";
        this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
        // 
        // unselectAllToolStripMenuItem
        // 
        this.unselectAllToolStripMenuItem.Name = "unselectAllToolStripMenuItem";
        this.unselectAllToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
        this.unselectAllToolStripMenuItem.Text = "Uncheck All";
        this.unselectAllToolStripMenuItem.Click += new System.EventHandler(this.unselectAllToolStripMenuItem_Click);
        // 
        // toolStripMenuItem1
        // 
        this.toolStripMenuItem1.Name = "toolStripMenuItem1";
        this.toolStripMenuItem1.Size = new System.Drawing.Size(168, 6);
        // 
        // checkSelectionToolStripMenuItem
        // 
        this.checkSelectionToolStripMenuItem.Name = "checkSelectionToolStripMenuItem";
        this.checkSelectionToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
        this.checkSelectionToolStripMenuItem.Text = "Check Selection";
        this.checkSelectionToolStripMenuItem.Click += new System.EventHandler(this.checkSelectionToolStripMenuItem_Click);
        // 
        // uncheckSelectionToolStripMenuItem
        // 
        this.uncheckSelectionToolStripMenuItem.Name = "uncheckSelectionToolStripMenuItem";
        this.uncheckSelectionToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
        this.uncheckSelectionToolStripMenuItem.Text = "Uncheck Selection";
        this.uncheckSelectionToolStripMenuItem.Click += new System.EventHandler(this.uncheckSelectionToolStripMenuItem_Click);
        // 
        // tabControl1
        // 
        this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.tabControl1.Controls.Add(this.tabPage7);
        this.tabControl1.Controls.Add(this.tabPage1);
        this.tabControl1.Controls.Add(this.tabPage2);
        this.tabControl1.Controls.Add(this.tabPage3);
        this.tabControl1.Controls.Add(this.tabPage4);
        this.tabControl1.Controls.Add(this.tabPage5);
        this.tabControl1.Controls.Add(this.tabPage6);
        this.tabControl1.Location = new System.Drawing.Point(12, 12);
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Size = new System.Drawing.Size(418, 423);
        this.tabControl1.TabIndex = 6;
        // 
        // tabPage7
        // 
        this.tabPage7.Controls.Add(this.dgPresets);
        this.tabPage7.Location = new System.Drawing.Point(4, 22);
        this.tabPage7.Name = "tabPage7";
        this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage7.Size = new System.Drawing.Size(410, 397);
        this.tabPage7.TabIndex = 6;
        this.tabPage7.Text = "Presets";
        this.tabPage7.UseVisualStyleBackColor = true;
        // 
        // dgPresets
        // 
        this.dgPresets.AllowUserToAddRows = false;
        this.dgPresets.AllowUserToDeleteRows = false;
        this.dgPresets.AllowUserToOrderColumns = true;
        this.dgPresets.AllowUserToResizeColumns = false;
        this.dgPresets.AllowUserToResizeRows = false;
        this.dgPresets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgPresets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.hIcon,
            this.hPreset});
        this.dgPresets.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dgPresets.Location = new System.Drawing.Point(3, 3);
        this.dgPresets.MultiSelect = false;
        this.dgPresets.Name = "dgPresets";
        this.dgPresets.ReadOnly = true;
        this.dgPresets.RowHeadersVisible = false;
        this.dgPresets.RowTemplate.Height = 30;
        this.dgPresets.ShowEditingIcon = false;
        this.dgPresets.ShowRowErrors = false;
        this.dgPresets.Size = new System.Drawing.Size(404, 391);
        this.dgPresets.TabIndex = 0;
        // 
        // hIcon
        // 
        this.hIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
        this.hIcon.HeaderText = "";
        this.hIcon.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
        this.hIcon.Name = "hIcon";
        this.hIcon.ReadOnly = true;
        this.hIcon.Resizable = System.Windows.Forms.DataGridViewTriState.True;
        this.hIcon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
        this.hIcon.Width = 19;
        // 
        // hPreset
        // 
        this.hPreset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        this.hPreset.HeaderText = "Preset";
        this.hPreset.Name = "hPreset";
        this.hPreset.ReadOnly = true;
        // 
        // tabPage1
        // 
        this.tabPage1.Controls.Add(this.numericUpDown1);
        this.tabPage1.Controls.Add(this.label5);
        this.tabPage1.Controls.Add(this.splitContainer1);
        this.tabPage1.Controls.Add(this.tbName);
        this.tabPage1.Controls.Add(this.label2);
        this.tabPage1.Controls.Add(this.label1);
        this.tabPage1.Location = new System.Drawing.Point(4, 22);
        this.tabPage1.Name = "tabPage1";
        this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage1.Size = new System.Drawing.Size(410, 397);
        this.tabPage1.TabIndex = 0;
        this.tabPage1.Text = "Name";
        this.tabPage1.UseVisualStyleBackColor = true;
        // 
        // numericUpDown1
        // 
        this.numericUpDown1.Location = new System.Drawing.Point(146, 48);
        this.numericUpDown1.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
        this.numericUpDown1.Name = "numericUpDown1";
        this.numericUpDown1.Size = new System.Drawing.Size(116, 20);
        this.numericUpDown1.TabIndex = 9;
        // 
        // label5
        // 
        this.label5.AutoSize = true;
        this.label5.Location = new System.Drawing.Point(5, 50);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(135, 13);
        this.label5.TabIndex = 8;
        this.label5.Text = "Filter GPX older than (days)";
        // 
        // splitContainer1
        // 
        this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.splitContainer1.Location = new System.Drawing.Point(6, 90);
        this.splitContainer1.Name = "splitContainer1";
        this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
        // 
        // splitContainer1.Panel1
        // 
        this.splitContainer1.Panel1.Controls.Add(this.gCategory);
        // 
        // splitContainer1.Panel2
        // 
        this.splitContainer1.Panel2.Controls.Add(this.gGPXFilenames);
        this.splitContainer1.Size = new System.Drawing.Size(396, 300);
        this.splitContainer1.SplitterDistance = 179;
        this.splitContainer1.TabIndex = 7;
        // 
        // gCategory
        // 
        this.gCategory.ContextMenuStrip = this.cmsCategory;
        this.gCategory.Dock = System.Windows.Forms.DockStyle.Fill;
        this.gCategory.EnableSort = true;
        this.gCategory.Location = new System.Drawing.Point(0, 0);
        this.gCategory.Name = "gCategory";
        this.gCategory.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
        this.gCategory.SelectionMode = SourceGrid.GridSelectionMode.Cell;
        this.gCategory.Size = new System.Drawing.Size(396, 179);
        this.gCategory.TabIndex = 6;
        this.gCategory.TabStop = true;
        this.gCategory.ToolTipText = "";
        this.gCategory.Resize += new System.EventHandler(this.gCategory_Resize);
        // 
        // cmsCategory
        // 
        this.cmsCategory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miPinned});
        this.cmsCategory.Name = "cmsCategory";
        this.cmsCategory.Size = new System.Drawing.Size(112, 26);
        // 
        // miPinned
        // 
        this.miPinned.Name = "miPinned";
        this.miPinned.Size = new System.Drawing.Size(111, 22);
        this.miPinned.Text = "Pinned";
        this.miPinned.Click += new System.EventHandler(this.miPinned_Click);
        // 
        // gGPXFilenames
        // 
        this.gGPXFilenames.ContextMenuStrip = this.cmsGpxFilenames;
        this.gGPXFilenames.Dock = System.Windows.Forms.DockStyle.Fill;
        this.gGPXFilenames.EnableSort = true;
        this.gGPXFilenames.Location = new System.Drawing.Point(0, 0);
        this.gGPXFilenames.Name = "gGPXFilenames";
        this.gGPXFilenames.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
        this.gGPXFilenames.SelectionMode = SourceGrid.GridSelectionMode.Cell;
        this.gGPXFilenames.Size = new System.Drawing.Size(396, 117);
        this.gGPXFilenames.TabIndex = 2;
        this.gGPXFilenames.TabStop = true;
        this.gGPXFilenames.ToolTipText = "";
        this.gGPXFilenames.Resize += new System.EventHandler(this.gGPXFilenames_Resize);
        // 
        // tabPage2
        // 
        this.tabPage2.Controls.Add(this.gGeneral);
        this.tabPage2.Location = new System.Drawing.Point(4, 22);
        this.tabPage2.Name = "tabPage2";
        this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage2.Size = new System.Drawing.Size(410, 397);
        this.tabPage2.TabIndex = 1;
        this.tabPage2.Text = "General";
        this.tabPage2.UseVisualStyleBackColor = true;
        // 
        // gGeneral
        // 
        this.gGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.gGeneral.EnableSort = true;
        this.gGeneral.Location = new System.Drawing.Point(6, 6);
        this.gGeneral.Name = "gGeneral";
        this.gGeneral.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
        this.gGeneral.SelectionMode = SourceGrid.GridSelectionMode.Cell;
        this.gGeneral.Size = new System.Drawing.Size(396, 362);
        this.gGeneral.TabIndex = 0;
        this.gGeneral.TabStop = true;
        this.gGeneral.ToolTipText = "";
        this.gGeneral.Resize += new System.EventHandler(this.gGeneral_Resize);
        // 
        // tabPage3
        // 
        this.tabPage3.Controls.Add(this.gDT);
        this.tabPage3.Location = new System.Drawing.Point(4, 22);
        this.tabPage3.Name = "tabPage3";
        this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage3.Size = new System.Drawing.Size(410, 397);
        this.tabPage3.TabIndex = 2;
        this.tabPage3.Text = "D / T";
        this.tabPage3.UseVisualStyleBackColor = true;
        // 
        // gDT
        // 
        this.gDT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.gDT.EnableSort = true;
        this.gDT.Location = new System.Drawing.Point(6, 6);
        this.gDT.Name = "gDT";
        this.gDT.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
        this.gDT.SelectionMode = SourceGrid.GridSelectionMode.Cell;
        this.gDT.Size = new System.Drawing.Size(396, 362);
        this.gDT.TabIndex = 2;
        this.gDT.TabStop = true;
        this.gDT.ToolTipText = "";
        this.gDT.Resize += new System.EventHandler(this.gDT_Resize);
        // 
        // tabPage4
        // 
        this.tabPage4.Controls.Add(this.gCacheType);
        this.tabPage4.Location = new System.Drawing.Point(4, 22);
        this.tabPage4.Name = "tabPage4";
        this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage4.Size = new System.Drawing.Size(410, 397);
        this.tabPage4.TabIndex = 3;
        this.tabPage4.Text = "Cache Types";
        this.tabPage4.UseVisualStyleBackColor = true;
        // 
        // gCacheType
        // 
        this.gCacheType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.gCacheType.EnableSort = true;
        this.gCacheType.Location = new System.Drawing.Point(6, 6);
        this.gCacheType.Name = "gCacheType";
        this.gCacheType.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
        this.gCacheType.SelectionMode = SourceGrid.GridSelectionMode.Cell;
        this.gCacheType.Size = new System.Drawing.Size(396, 362);
        this.gCacheType.TabIndex = 1;
        this.gCacheType.TabStop = true;
        this.gCacheType.ToolTipText = "";
        this.gCacheType.Resize += new System.EventHandler(this.gCacheType_Resize);
        // 
        // tabPage5
        // 
        this.tabPage5.Controls.Add(this.gAttributes);
        this.tabPage5.Location = new System.Drawing.Point(4, 22);
        this.tabPage5.Name = "tabPage5";
        this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage5.Size = new System.Drawing.Size(410, 397);
        this.tabPage5.TabIndex = 4;
        this.tabPage5.Text = "Attributes";
        this.tabPage5.UseVisualStyleBackColor = true;
        // 
        // gAttributes
        // 
        this.gAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.gAttributes.EnableSort = true;
        this.gAttributes.Location = new System.Drawing.Point(6, 6);
        this.gAttributes.Name = "gAttributes";
        this.gAttributes.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
        this.gAttributes.SelectionMode = SourceGrid.GridSelectionMode.Cell;
        this.gAttributes.Size = new System.Drawing.Size(396, 362);
        this.gAttributes.TabIndex = 2;
        this.gAttributes.TabStop = true;
        this.gAttributes.ToolTipText = "";
        this.gAttributes.Resize += new System.EventHandler(this.grid1_Resize);
        // 
        // tabPage6
        // 
        this.tabPage6.Controls.Add(this.lstStates);
        this.tabPage6.Controls.Add(this.lstCountries);
        this.tabPage6.Controls.Add(this.label4);
        this.tabPage6.Controls.Add(this.label3);
        this.tabPage6.Location = new System.Drawing.Point(4, 22);
        this.tabPage6.Name = "tabPage6";
        this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage6.Size = new System.Drawing.Size(410, 397);
        this.tabPage6.TabIndex = 5;
        this.tabPage6.Text = "Country/State";
        this.tabPage6.UseVisualStyleBackColor = true;
        // 
        // lstStates
        // 
        this.lstStates.CheckOnClick = true;
        this.lstStates.FormattingEnabled = true;
        this.lstStates.Location = new System.Drawing.Point(9, 186);
        this.lstStates.Name = "lstStates";
        this.lstStates.Size = new System.Drawing.Size(398, 199);
        this.lstStates.TabIndex = 11;
        // 
        // lstCountries
        // 
        this.lstCountries.CheckOnClick = true;
        this.lstCountries.FormattingEnabled = true;
        this.lstCountries.Location = new System.Drawing.Point(9, 19);
        this.lstCountries.Name = "lstCountries";
        this.lstCountries.Size = new System.Drawing.Size(395, 139);
        this.lstCountries.TabIndex = 10;
        this.lstCountries.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstCountries_ItemCheck);
        this.lstCountries.SelectedIndexChanged += new System.EventHandler(this.lstCountries_SelectedIndexChanged);
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Location = new System.Drawing.Point(6, 3);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(68, 13);
        this.label4.TabIndex = 7;
        this.label4.Text = "Filter Country";
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(6, 166);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(57, 13);
        this.label3.TabIndex = 5;
        this.label3.Text = "Filter State";
        // 
        // FilterForm
        // 
        this.AcceptButton = this.button1;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.button2;
        this.ClientSize = new System.Drawing.Size(523, 447);
        this.Controls.Add(this.tabControl1);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.button1);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "FilterForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Filter";
        this.Load += new System.EventHandler(this.FilterForm_Load);
        this.cmsGpxFilenames.ResumeLayout(false);
        this.tabControl1.ResumeLayout(false);
        this.tabPage7.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.dgPresets)).EndInit();
        this.tabPage1.ResumeLayout(false);
        this.tabPage1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
        this.splitContainer1.Panel1.ResumeLayout(false);
        this.splitContainer1.Panel2.ResumeLayout(false);
        this.splitContainer1.ResumeLayout(false);
        this.cmsCategory.ResumeLayout(false);
        this.tabPage2.ResumeLayout(false);
        this.tabPage3.ResumeLayout(false);
        this.tabPage4.ResumeLayout(false);
        this.tabPage5.ResumeLayout(false);
        this.tabPage6.ResumeLayout(false);
        this.tabPage6.PerformLayout();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private SourceGrid.Grid gGPXFilenames;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox tbName;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private SourceGrid.Grid gGeneral;
    private System.Windows.Forms.TabPage tabPage3;
    private System.Windows.Forms.TabPage tabPage4;
    private System.Windows.Forms.TabPage tabPage5;
    private SourceGrid.Grid gCacheType;
    private SourceGrid.Grid gAttributes;
    private SourceGrid.Grid gDT;
    private System.Windows.Forms.ContextMenuStrip cmsGpxFilenames;
    private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem unselectAllToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem checkSelectionToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem uncheckSelectionToolStripMenuItem;
    private SourceGrid.Grid gCategory;
    private System.Windows.Forms.ContextMenuStrip cmsCategory;
    private System.Windows.Forms.ToolStripMenuItem miPinned;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.TabPage tabPage6;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.CheckedListBox lstCountries;
    private System.Windows.Forms.CheckedListBox lstStates;
    private System.Windows.Forms.TabPage tabPage7;
    private System.Windows.Forms.DataGridView dgPresets;
    private System.Windows.Forms.DataGridViewImageColumn hIcon;
    private System.Windows.Forms.DataGridViewTextBoxColumn hPreset;
    private System.Windows.Forms.NumericUpDown numericUpDown1;
    private System.Windows.Forms.Label label5;
  }
}