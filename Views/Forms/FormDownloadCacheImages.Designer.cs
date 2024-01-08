namespace WinCachebox.Views.Forms
{
  partial class FormDownloadCacheImages
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
        this.buttonCancel = new System.Windows.Forms.Button();
        this.labelError = new System.Windows.Forms.Label();
        this.progressBar1 = new System.Windows.Forms.ProgressBar();
        this.labelAction = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // buttonCancel
        // 
        this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.buttonCancel.Location = new System.Drawing.Point(197, 211);
        this.buttonCancel.Name = "buttonCancel";
        this.buttonCancel.Size = new System.Drawing.Size(75, 23);
        this.buttonCancel.TabIndex = 7;
        this.buttonCancel.Text = "&Cancel";
        this.buttonCancel.UseVisualStyleBackColor = true;
        this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        // 
        // labelError
        // 
        this.labelError.Location = new System.Drawing.Point(12, 123);
        this.labelError.Name = "labelError";
        this.labelError.Size = new System.Drawing.Size(260, 85);
        this.labelError.TabIndex = 6;
        // 
        // progressBar1
        // 
        this.progressBar1.Location = new System.Drawing.Point(12, 97);
        this.progressBar1.Name = "progressBar1";
        this.progressBar1.Size = new System.Drawing.Size(260, 23);
        this.progressBar1.TabIndex = 5;
        // 
        // labelAction
        // 
        this.labelAction.Location = new System.Drawing.Point(12, 9);
        this.labelAction.Name = "labelAction";
        this.labelAction.Size = new System.Drawing.Size(260, 85);
        this.labelAction.TabIndex = 4;
        // 
        // FormDownloadCacheImages
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(283, 245);
        this.Controls.Add(this.buttonCancel);
        this.Controls.Add(this.labelError);
        this.Controls.Add(this.progressBar1);
        this.Controls.Add(this.labelAction);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        this.Name = "FormDownloadCacheImages";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Download Cache-Images";
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Label labelError;
    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Windows.Forms.Label labelAction;

  }
}