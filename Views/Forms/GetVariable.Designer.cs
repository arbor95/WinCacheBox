namespace WinCachebox.Views.Forms
{
  partial class GetVariable
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
      this.button2 = new System.Windows.Forms.Button();
      this.button1 = new System.Windows.Forms.Button();
      this.lvVariable = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.SuspendLayout();
      // 
      // button2
      // 
      this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.button2.Location = new System.Drawing.Point(413, 273);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 3;
      this.button2.Text = "&Cancel";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.button1.Location = new System.Drawing.Point(413, 244);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 2;
      this.button1.Text = "&OK";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // lvVariable
      // 
      this.lvVariable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
      this.lvVariable.FullRowSelect = true;
      this.lvVariable.HideSelection = false;
      this.lvVariable.Location = new System.Drawing.Point(12, 12);
      this.lvVariable.MultiSelect = false;
      this.lvVariable.Name = "lvVariable";
      this.lvVariable.Size = new System.Drawing.Size(395, 284);
      this.lvVariable.TabIndex = 4;
      this.lvVariable.UseCompatibleStateImageBehavior = false;
      this.lvVariable.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Variable";
      this.columnHeader1.Width = 150;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Value";
      this.columnHeader2.Width = 150;
      // 
      // GetVariable
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(500, 308);
      this.Controls.Add(this.lvVariable);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Name = "GetVariable";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "GetVariable";
      this.Load += new System.EventHandler(this.GetVariable_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.ListView lvVariable;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
  }
}