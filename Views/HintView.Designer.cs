namespace WinCachebox.Views
{
  partial class HintView
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
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.button1 = new System.Windows.Forms.Button();
        this.panel1 = new System.Windows.Forms.Panel();
        this.panel2 = new System.Windows.Forms.Panel();
        this.panel1.SuspendLayout();
        this.panel2.SuspendLayout();
        this.SuspendLayout();
        // 
        // textBox1
        // 
        this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.textBox1.Location = new System.Drawing.Point(0, 0);
        this.textBox1.Multiline = true;
        this.textBox1.Name = "textBox1";
        this.textBox1.ReadOnly = true;
        this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        this.textBox1.Size = new System.Drawing.Size(868, 549);
        this.textBox1.TabIndex = 2;
        this.textBox1.WordWrap = false;
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(12, 3);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(90, 23);
        this.button1.TabIndex = 3;
        this.button1.Text = "&Decode";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // panel1
        // 
        this.panel1.Controls.Add(this.button1);
        this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panel1.Location = new System.Drawing.Point(0, 549);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(868, 30);
        this.panel1.TabIndex = 4;
        // 
        // panel2
        // 
        this.panel2.Controls.Add(this.textBox1);
        this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel2.Location = new System.Drawing.Point(0, 0);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(868, 549);
        this.panel2.TabIndex = 5;
        // 
        // HintView
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(868, 579);
        this.Controls.Add(this.panel2);
        this.Controls.Add(this.panel1);
        this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Name = "HintView";
        this.panel1.ResumeLayout(false);
        this.panel2.ResumeLayout(false);
        this.panel2.PerformLayout();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Panel panel2;
  }
}
