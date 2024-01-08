namespace WinCachebox.Views
{
  partial class CacheListView
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
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CacheListView));
        this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.setAsCenterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.markAsFavoriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.dgCaches = new System.Windows.Forms.DataGridView();
        this.cTyp = new System.Windows.Forms.DataGridViewImageColumn();
        this.cFound = new System.Windows.Forms.DataGridViewImageColumn();
        this.cGcCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cCoordinate = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cFirstImported = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cLastImported = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.Country = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.timer1 = new System.Windows.Forms.Timer(this.components);
        this.contextMenuStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgCaches)).BeginInit();
        this.SuspendLayout();
        // 
        // contextMenuStrip1
        // 
        this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setAsCenterToolStripMenuItem,
            this.markAsFavoriteToolStripMenuItem});
        this.contextMenuStrip1.Name = "contextMenuStrip1";
        this.contextMenuStrip1.Size = new System.Drawing.Size(145, 48);
        // 
        // setAsCenterToolStripMenuItem
        // 
        this.setAsCenterToolStripMenuItem.Name = "setAsCenterToolStripMenuItem";
        this.setAsCenterToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
        this.setAsCenterToolStripMenuItem.Text = "Set As Center";
        this.setAsCenterToolStripMenuItem.Click += new System.EventHandler(this.setAsCenterToolStripMenuItem_Click);
        // 
        // markAsFavoriteToolStripMenuItem
        // 
        this.markAsFavoriteToolStripMenuItem.Name = "markAsFavoriteToolStripMenuItem";
        this.markAsFavoriteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
        this.markAsFavoriteToolStripMenuItem.Text = "Favorite?";
        this.markAsFavoriteToolStripMenuItem.Click += new System.EventHandler(this.markAsFavoriteToolStripMenuItem_Click);
        // 
        // dgCaches
        // 
        this.dgCaches.AllowUserToAddRows = false;
        this.dgCaches.AllowUserToDeleteRows = false;
        this.dgCaches.AllowUserToOrderColumns = true;
        this.dgCaches.AllowUserToResizeRows = false;
        this.dgCaches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgCaches.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cTyp,
            this.cFound,
            this.cGcCode,
            this.cName,
            this.cDistance,
            this.cCoordinate,
            this.cCategory,
            this.cFirstImported,
            this.cLastImported,
            this.Country,
            this.State});
        this.dgCaches.ContextMenuStrip = this.contextMenuStrip1;
        dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
        dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
        dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
        this.dgCaches.DefaultCellStyle = dataGridViewCellStyle1;
        this.dgCaches.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dgCaches.Location = new System.Drawing.Point(0, 0);
        this.dgCaches.Name = "dgCaches";
        this.dgCaches.ReadOnly = true;
        this.dgCaches.RowHeadersVisible = false;
        this.dgCaches.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgCaches.Size = new System.Drawing.Size(480, 430);
        this.dgCaches.TabIndex = 9;
        this.dgCaches.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgCaches_CellContentClick);
        this.dgCaches.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgCaches_CellPainting);
        this.dgCaches.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dgCaches_RowStateChanged);
        this.dgCaches.SelectionChanged += new System.EventHandler(this.dgCaches_SelectionChanged);
        this.dgCaches.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dgCaches_SortCompare);
        this.dgCaches.Sorted += new System.EventHandler(this.dgCaches_Sorted);
        this.dgCaches.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgCaches_KeyPress);
        // 
        // cTyp
        // 
        this.cTyp.HeaderText = "";
        this.cTyp.Name = "cTyp";
        this.cTyp.ReadOnly = true;
        this.cTyp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
        this.cTyp.Width = 23;
        // 
        // cFound
        // 
        this.cFound.HeaderText = "";
        this.cFound.Name = "cFound";
        this.cFound.ReadOnly = true;
        this.cFound.Width = 23;
        // 
        // cGcCode
        // 
        this.cGcCode.HeaderText = "GcCode";
        this.cGcCode.Name = "cGcCode";
        this.cGcCode.ReadOnly = true;
        // 
        // cName
        // 
        this.cName.HeaderText = "Name";
        this.cName.Name = "cName";
        this.cName.ReadOnly = true;
        // 
        // cDistance
        // 
        this.cDistance.HeaderText = "Distance";
        this.cDistance.Name = "cDistance";
        this.cDistance.ReadOnly = true;
        // 
        // cCoordinate
        // 
        this.cCoordinate.HeaderText = "Coordinate";
        this.cCoordinate.Name = "cCoordinate";
        this.cCoordinate.ReadOnly = true;
        // 
        // cCategory
        // 
        this.cCategory.HeaderText = "Category";
        this.cCategory.Name = "cCategory";
        this.cCategory.ReadOnly = true;
        // 
        // cFirstImported
        // 
        this.cFirstImported.HeaderText = "FirstImported";
        this.cFirstImported.Name = "cFirstImported";
        this.cFirstImported.ReadOnly = true;
        // 
        // cLastImported
        // 
        this.cLastImported.HeaderText = "LastImported";
        this.cLastImported.Name = "cLastImported";
        this.cLastImported.ReadOnly = true;
        // 
        // Country
        // 
        this.Country.HeaderText = "Country";
        this.Country.Name = "Country";
        this.Country.ReadOnly = true;
        // 
        // State
        // 
        this.State.HeaderText = "State";
        this.State.Name = "State";
        this.State.ReadOnly = true;
        // 
        // timer1
        // 
        this.timer1.Interval = 2000;
        this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        // 
        // CacheListView
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(480, 430);
        this.Controls.Add(this.dgCaches);
        this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "CacheListView";
        this.Load += new System.EventHandler(this.CacheListView_Load);
        this.contextMenuStrip1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.dgCaches)).EndInit();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem setAsCenterToolStripMenuItem;
    private System.Windows.Forms.DataGridView dgCaches;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.DataGridViewImageColumn cTyp;
    private System.Windows.Forms.DataGridViewImageColumn cFound;
    private System.Windows.Forms.DataGridViewTextBoxColumn cGcCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn cName;
    private System.Windows.Forms.DataGridViewTextBoxColumn cDistance;
    private System.Windows.Forms.DataGridViewTextBoxColumn cCoordinate;
    private System.Windows.Forms.DataGridViewTextBoxColumn cCategory;
    private System.Windows.Forms.DataGridViewTextBoxColumn cFirstImported;
    private System.Windows.Forms.DataGridViewTextBoxColumn cLastImported;
    private System.Windows.Forms.DataGridViewTextBoxColumn Country;
    private System.Windows.Forms.DataGridViewTextBoxColumn State;
    private System.Windows.Forms.ToolStripMenuItem markAsFavoriteToolStripMenuItem;
  }
}
