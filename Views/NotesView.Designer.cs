namespace WinCachebox.Views
{
  partial class NotesView
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
      this.textBox121 = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // textBox121
      // 
      this.textBox121.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBox121.Location = new System.Drawing.Point(0, 0);
      this.textBox121.Multiline = true;
      this.textBox121.Name = "textBox121";
      this.textBox121.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.textBox121.Size = new System.Drawing.Size(284, 262);
      this.textBox121.TabIndex = 1;
      this.textBox121.WordWrap = false;
      this.textBox121.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
      // 
      // NotesView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 262);
      this.Controls.Add(this.textBox121);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "NotesView";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBox121;
  }
}
