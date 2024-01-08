namespace WinCachebox.Views.Forms
{
  partial class LocationsForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationsForm));
        this.button1 = new System.Windows.Forms.Button();
        this.gLocations = new SourceGrid.Grid();
        this.bNew = new System.Windows.Forms.Button();
        this.bEdit = new System.Windows.Forms.Button();
        this.bDelete = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // button1
        // 
        this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.button1.Location = new System.Drawing.Point(487, 251);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(75, 23);
        this.button1.TabIndex = 6;
        this.button1.Text = "&OK";
        this.button1.UseVisualStyleBackColor = true;
        // 
        // gLocations
        // 
        this.gLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.gLocations.EnableSort = true;
        this.gLocations.Location = new System.Drawing.Point(12, 12);
        this.gLocations.Name = "gLocations";
        this.gLocations.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
        this.gLocations.SelectionMode = SourceGrid.GridSelectionMode.Cell;
        this.gLocations.Size = new System.Drawing.Size(469, 262);
        this.gLocations.TabIndex = 0;
        this.gLocations.TabStop = true;
        this.gLocations.ToolTipText = "";
        // 
        // bNew
        // 
        this.bNew.Location = new System.Drawing.Point(487, 12);
        this.bNew.Name = "bNew";
        this.bNew.Size = new System.Drawing.Size(75, 23);
        this.bNew.TabIndex = 9;
        this.bNew.Text = "&New";
        this.bNew.UseVisualStyleBackColor = true;
        this.bNew.Click += new System.EventHandler(this.bNew_Click);
        // 
        // bEdit
        // 
        this.bEdit.Location = new System.Drawing.Point(487, 41);
        this.bEdit.Name = "bEdit";
        this.bEdit.Size = new System.Drawing.Size(75, 23);
        this.bEdit.TabIndex = 10;
        this.bEdit.Text = "&Edit";
        this.bEdit.UseVisualStyleBackColor = true;
        this.bEdit.Click += new System.EventHandler(this.bEdit_Click);
        // 
        // bDelete
        // 
        this.bDelete.Location = new System.Drawing.Point(487, 70);
        this.bDelete.Name = "bDelete";
        this.bDelete.Size = new System.Drawing.Size(75, 23);
        this.bDelete.TabIndex = 11;
        this.bDelete.Text = "&Delete";
        this.bDelete.UseVisualStyleBackColor = true;
        this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
        // 
        // LocationsForm
        // 
        this.AcceptButton = this.button1;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(574, 286);
        this.Controls.Add(this.bDelete);
        this.Controls.Add(this.bEdit);
        this.Controls.Add(this.bNew);
        this.Controls.Add(this.gLocations);
        this.Controls.Add(this.button1);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "LocationsForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Locations";
        this.Load += new System.EventHandler(this.LocationsForm_Load);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private SourceGrid.Grid gLocations;
    private System.Windows.Forms.Button bNew;
    private System.Windows.Forms.Button bEdit;
    private System.Windows.Forms.Button bDelete;
  }
}
