namespace WinCachebox.Views.Forms
{
    partial class OpenDB
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
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.DBList = new System.Windows.Forms.ListView();
            this.DBType = new System.Windows.Forms.ComboBox();
            this.OwnRepository = new System.Windows.Forms.ComboBox();
            this.DBName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(464, 313);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 1;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(383, 313);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // DBList
            // 
            this.DBList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DBList.Location = new System.Drawing.Point(12, 12);
            this.DBList.Name = "DBList";
            this.DBList.Size = new System.Drawing.Size(527, 259);
            this.DBList.TabIndex = 3;
            this.DBList.UseCompatibleStateImageBehavior = false;
            this.DBList.SelectedIndexChanged += new System.EventHandler(this.DBList_SelectedIndexChanged);
            // 
            // DBType
            // 
            this.DBType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DBType.FormattingEnabled = true;
            this.DBType.Items.AddRange(new object[] {
            "sdf",
            "db3"});
            this.DBType.Location = new System.Drawing.Point(412, 277);
            this.DBType.Name = "DBType";
            this.DBType.Size = new System.Drawing.Size(60, 21);
            this.DBType.TabIndex = 4;
            // 
            // OwnRepository
            // 
            this.OwnRepository.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OwnRepository.FormattingEnabled = true;
            this.OwnRepository.Items.AddRange(new object[] {
            "false",
            "true"});
            this.OwnRepository.Location = new System.Drawing.Point(478, 277);
            this.OwnRepository.Name = "OwnRepository";
            this.OwnRepository.Size = new System.Drawing.Size(60, 21);
            this.OwnRepository.TabIndex = 5;
            // 
            // DBName
            // 
            this.DBName.Location = new System.Drawing.Point(12, 277);
            this.DBName.Name = "DBName";
            this.DBName.Size = new System.Drawing.Size(394, 20);
            this.DBName.TabIndex = 6;
            // 
            // OpenDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(551, 348);
            this.Controls.Add(this.DBName);
            this.Controls.Add(this.OwnRepository);
            this.Controls.Add(this.DBType);
            this.Controls.Add(this.DBList);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Name = "OpenDB";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "OpenDB";
            this.Load += new System.EventHandler(this.OpenDB_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.ListView DBList;
        private System.Windows.Forms.ComboBox DBType;
        private System.Windows.Forms.ComboBox OwnRepository;
        private System.Windows.Forms.TextBox DBName;
    }
}