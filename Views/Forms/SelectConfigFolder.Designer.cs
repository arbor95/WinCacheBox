namespace WinCachebox.Views.Forms
{
  partial class SelectConfigFolder
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
      this.button2 = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.rbDefault = new System.Windows.Forms.RadioButton();
      this.rbUserDefined = new System.Windows.Forms.RadioButton();
      this.tbConfigPath = new System.Windows.Forms.TextBox();
      this.bSelectConfigPath = new System.Windows.Forms.Button();
      this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
      this.rbLocal = new System.Windows.Forms.RadioButton();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.button1.Location = new System.Drawing.Point(435, 202);
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
      this.button2.Location = new System.Drawing.Point(435, 231);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 1;
      this.button2.Text = "&Cancel";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(12, 69);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(399, 15);
      this.label1.TabIndex = 2;
      this.label1.Text = "Please select the folder where the database should be stored";
      // 
      // rbDefault
      // 
      this.rbDefault.AutoSize = true;
      this.rbDefault.Checked = true;
      this.rbDefault.Location = new System.Drawing.Point(15, 94);
      this.rbDefault.Name = "rbDefault";
      this.rbDefault.Size = new System.Drawing.Size(260, 17);
      this.rbDefault.TabIndex = 3;
      this.rbDefault.Text = "Default folder - in the users AppData (recommend)";
      this.rbDefault.UseVisualStyleBackColor = true;
      this.rbDefault.CheckedChanged += new System.EventHandler(this.rbDefault_CheckedChanged);
      // 
      // rbUserDefined
      // 
      this.rbUserDefined.AutoSize = true;
      this.rbUserDefined.Location = new System.Drawing.Point(15, 128);
      this.rbUserDefined.Name = "rbUserDefined";
      this.rbUserDefined.Size = new System.Drawing.Size(111, 17);
      this.rbUserDefined.TabIndex = 4;
      this.rbUserDefined.Text = "Userdefined folder";
      this.rbUserDefined.UseVisualStyleBackColor = true;
      this.rbUserDefined.CheckedChanged += new System.EventHandler(this.rbUserDefined_CheckedChanged);
      // 
      // tbConfigPath
      // 
      this.tbConfigPath.Location = new System.Drawing.Point(35, 151);
      this.tbConfigPath.Name = "tbConfigPath";
      this.tbConfigPath.Size = new System.Drawing.Size(344, 20);
      this.tbConfigPath.TabIndex = 5;
      this.tbConfigPath.Visible = false;
      // 
      // bSelectConfigPath
      // 
      this.bSelectConfigPath.Location = new System.Drawing.Point(383, 149);
      this.bSelectConfigPath.Name = "bSelectConfigPath";
      this.bSelectConfigPath.Size = new System.Drawing.Size(27, 23);
      this.bSelectConfigPath.TabIndex = 6;
      this.bSelectConfigPath.Text = "...";
      this.bSelectConfigPath.UseVisualStyleBackColor = true;
      this.bSelectConfigPath.Visible = false;
      this.bSelectConfigPath.Click += new System.EventHandler(this.button3_Click);
      // 
      // rbLocal
      // 
      this.rbLocal.AutoSize = true;
      this.rbLocal.Location = new System.Drawing.Point(15, 181);
      this.rbLocal.Name = "rbLocal";
      this.rbLocal.Size = new System.Drawing.Size(298, 17);
      this.rbLocal.TabIndex = 7;
      this.rbLocal.Text = "Portable mode (in the same folder than the executable file)";
      this.rbLocal.UseVisualStyleBackColor = true;
      this.rbLocal.CheckedChanged += new System.EventHandler(this.rbLocal_CheckedChanged);
      // 
      // label2
      // 
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(15, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(396, 23);
      this.label2.TabIndex = 8;
      this.label2.Text = "WinCachebox";
      this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // label3
      // 
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(16, 32);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(394, 23);
      this.label3.TabIndex = 9;
      this.label3.Text = "First Start";
      this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // label4
      // 
      this.label4.Location = new System.Drawing.Point(32, 202);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(378, 52);
      this.label4.TabIndex = 10;
      this.label4.Text = "Please be careful with this. Under some circumstances it is not allowed to save u" +
          "ser data into the [program files] folder!\r\n";
      // 
      // SelectConfigFolder
      // 
      this.AcceptButton = this.button1;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.button2;
      this.ClientSize = new System.Drawing.Size(522, 266);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.rbLocal);
      this.Controls.Add(this.bSelectConfigPath);
      this.Controls.Add(this.tbConfigPath);
      this.Controls.Add(this.rbUserDefined);
      this.Controls.Add(this.rbDefault);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Name = "SelectConfigFolder";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Select Data Folder";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.RadioButton rbDefault;
    private System.Windows.Forms.RadioButton rbUserDefined;
    private System.Windows.Forms.TextBox tbConfigPath;
    private System.Windows.Forms.Button bSelectConfigPath;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    private System.Windows.Forms.RadioButton rbLocal;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
  }
}