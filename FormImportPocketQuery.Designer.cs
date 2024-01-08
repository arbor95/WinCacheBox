namespace WinCachebox
{
  partial class FormImportPocketQuery
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
        this.labelAction = new System.Windows.Forms.Label();
        this.progressBar1 = new System.Windows.Forms.ProgressBar();
        this.labelError = new System.Windows.Forms.Label();
        this.buttonCancel = new System.Windows.Forms.Button();
        this.progressBar2 = new System.Windows.Forms.ProgressBar();
        this.SuspendLayout();
        // 
        // labelAction
        // 
        this.labelAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.labelAction.Location = new System.Drawing.Point(12, 9);
        this.labelAction.Name = "labelAction";
        this.labelAction.Size = new System.Drawing.Size(260, 59);
        this.labelAction.TabIndex = 0;
        // 
        // progressBar1
        // 
        this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.progressBar1.Location = new System.Drawing.Point(12, 97);
        this.progressBar1.Name = "progressBar1";
        this.progressBar1.Size = new System.Drawing.Size(260, 23);
        this.progressBar1.TabIndex = 1;
        // 
        // labelError
        // 
        this.labelError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.labelError.Location = new System.Drawing.Point(12, 123);
        this.labelError.Name = "labelError";
        this.labelError.Size = new System.Drawing.Size(260, 85);
        this.labelError.TabIndex = 2;
        // 
        // buttonCancel
        // 
        this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.buttonCancel.Location = new System.Drawing.Point(197, 211);
        this.buttonCancel.Name = "buttonCancel";
        this.buttonCancel.Size = new System.Drawing.Size(75, 23);
        this.buttonCancel.TabIndex = 3;
        this.buttonCancel.Text = "&Cancel";
        this.buttonCancel.UseVisualStyleBackColor = true;
        this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        // 
        // progressBar2
        // 
        this.progressBar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.progressBar2.Location = new System.Drawing.Point(12, 71);
        this.progressBar2.Name = "progressBar2";
        this.progressBar2.Size = new System.Drawing.Size(260, 23);
        this.progressBar2.TabIndex = 4;
        // 
        // FormImportPocketQuery
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.buttonCancel;
        this.ClientSize = new System.Drawing.Size(284, 244);
        this.Controls.Add(this.progressBar2);
        this.Controls.Add(this.buttonCancel);
        this.Controls.Add(this.labelError);
        this.Controls.Add(this.progressBar1);
        this.Controls.Add(this.labelAction);
        this.Name = "FormImportPocketQuery";
        this.Text = "FormImportPocketQuery";
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label labelAction;
    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Windows.Forms.Label labelError;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ProgressBar progressBar2;
  }
}