namespace WinCachebox.Views.Forms
{
  partial class RemoveArchivedForm
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
      this.label1 = new System.Windows.Forms.Label();
      this.progressBar1 = new System.Windows.Forms.ProgressBar();
      this.labelProgress = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(150, 96);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 0;
      this.button1.Text = "&Cancel";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(95, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Remove Caches...";
      // 
      // progressBar1
      // 
      this.progressBar1.Location = new System.Drawing.Point(12, 57);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size(213, 23);
      this.progressBar1.TabIndex = 2;
      // 
      // labelProgress
      // 
      this.labelProgress.AutoSize = true;
      this.labelProgress.Location = new System.Drawing.Point(12, 32);
      this.labelProgress.Name = "labelProgress";
      this.labelProgress.Size = new System.Drawing.Size(0, 13);
      this.labelProgress.TabIndex = 3;
      // 
      // RemoveArchivedForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(237, 133);
      this.Controls.Add(this.labelProgress);
      this.Controls.Add(this.progressBar1);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.button1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "RemoveArchivedForm";
      this.Text = "DB Maintenance";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Windows.Forms.Label labelProgress;
  }
}