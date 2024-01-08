namespace WinCachebox.Views
{
  partial class LogView
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
      this.grid1 = new SourceGrid.Grid();
      this.SuspendLayout();
      // 
      // grid1
      // 
      this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.grid1.EnableSort = true;
      this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.grid1.Location = new System.Drawing.Point(0, 0);
      this.grid1.Name = "grid1";
      this.grid1.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
      this.grid1.SelectionMode = SourceGrid.GridSelectionMode.Row;
      this.grid1.Size = new System.Drawing.Size(284, 262);
      this.grid1.TabIndex = 1;
      this.grid1.TabStop = true;
      this.grid1.ToolTipText = "";
      this.grid1.Resize += new System.EventHandler(this.grid1_Resize);
      // 
      // LogView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 262);
      this.Controls.Add(this.grid1);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "LogView";
      this.ResumeLayout(false);

    }

    #endregion

    private SourceGrid.Grid grid1;
  }
}
