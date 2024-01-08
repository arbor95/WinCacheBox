namespace WinCachebox.Views.Forms
{
  partial class EditWaypoint
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
        this.components = new System.ComponentModel.Container();
        this.button2 = new System.Windows.Forms.Button();
        this.button1 = new System.Windows.Forms.Button();
        this.bCoord = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
        this.tbTitel = new System.Windows.Forms.TextBox();
        this.label2 = new System.Windows.Forms.Label();
        this.tbDescription = new System.Windows.Forms.TextBox();
        this.label3 = new System.Windows.Forms.Label();
        this.tbClue = new System.Windows.Forms.TextBox();
        this.label4 = new System.Windows.Forms.Label();
        this.label5 = new System.Windows.Forms.Label();
        this.cbTyp = new System.Windows.Forms.ComboBox();
        this.tbGC = new System.Windows.Forms.MaskedTextBox();
        this.lCacheName = new System.Windows.Forms.Label();
        this.button3 = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // button2
        // 
        this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.button2.Location = new System.Drawing.Point(329, 290);
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
        this.button1.Location = new System.Drawing.Point(329, 261);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(75, 23);
        this.button1.TabIndex = 2;
        this.button1.Text = "&OK";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // bCoord
        // 
        this.bCoord.Location = new System.Drawing.Point(12, 43);
        this.bCoord.Name = "bCoord";
        this.bCoord.Size = new System.Drawing.Size(283, 38);
        this.bCoord.TabIndex = 4;
        this.bCoord.Text = "button3";
        this.bCoord.UseVisualStyleBackColor = true;
        this.bCoord.Click += new System.EventHandler(this.bCoord_Click);
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(12, 126);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(27, 13);
        this.label1.TabIndex = 5;
        this.label1.Text = "Title";
        this.label1.Click += new System.EventHandler(this.label1_Click);
        // 
        // tbTitel
        // 
        this.tbTitel.Location = new System.Drawing.Point(12, 140);
        this.tbTitel.Name = "tbTitel";
        this.tbTitel.Size = new System.Drawing.Size(283, 20);
        this.tbTitel.TabIndex = 6;
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(12, 165);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(60, 13);
        this.label2.TabIndex = 7;
        this.label2.Text = "Description";
        this.label2.Click += new System.EventHandler(this.label2_Click);
        // 
        // tbDescription
        // 
        this.tbDescription.Location = new System.Drawing.Point(12, 179);
        this.tbDescription.Multiline = true;
        this.tbDescription.Name = "tbDescription";
        this.tbDescription.Size = new System.Drawing.Size(283, 92);
        this.tbDescription.TabIndex = 8;
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(304, 44);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(46, 13);
        this.label3.TabIndex = 9;
        this.label3.Text = "GcCode";
        // 
        // tbClue
        // 
        this.tbClue.Location = new System.Drawing.Point(12, 290);
        this.tbClue.Name = "tbClue";
        this.tbClue.Size = new System.Drawing.Size(283, 20);
        this.tbClue.TabIndex = 12;
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Location = new System.Drawing.Point(12, 276);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(28, 13);
        this.label4.TabIndex = 11;
        this.label4.Text = "Clue";
        this.label4.Click += new System.EventHandler(this.label4_Click);
        // 
        // label5
        // 
        this.label5.AutoSize = true;
        this.label5.Location = new System.Drawing.Point(12, 86);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(25, 13);
        this.label5.TabIndex = 13;
        this.label5.Text = "Typ";
        this.label5.Click += new System.EventHandler(this.label5_Click);
        // 
        // cbTyp
        // 
        this.cbTyp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbTyp.FormattingEnabled = true;
        this.cbTyp.Items.AddRange(new object[] {
            "Reference",
            "Stage of a Multicache",
            "Question to answer",
            "Trailhead",
            "Parking Area",
            "Final"});
        this.cbTyp.Location = new System.Drawing.Point(15, 100);
        this.cbTyp.Name = "cbTyp";
        this.cbTyp.Size = new System.Drawing.Size(280, 21);
        this.cbTyp.TabIndex = 14;
        this.cbTyp.SelectedIndexChanged += new System.EventHandler(this.cbTyp_SelectedIndexChanged);
        // 
        // tbGC
        // 
        this.tbGC.Location = new System.Drawing.Point(307, 61);
        this.tbGC.Mask = "??ADDS";
        this.tbGC.Name = "tbGC";
        this.tbGC.Size = new System.Drawing.Size(97, 20);
        this.tbGC.TabIndex = 15;
        // 
        // lCacheName
        // 
        this.lCacheName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lCacheName.Location = new System.Drawing.Point(0, 9);
        this.lCacheName.Name = "lCacheName";
        this.lCacheName.Size = new System.Drawing.Size(417, 22);
        this.lCacheName.TabIndex = 16;
        this.lCacheName.Text = "label6";
        this.lCacheName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // button3
        // 
        this.button3.Location = new System.Drawing.Point(329, 177);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(75, 23);
        this.button3.TabIndex = 17;
        this.button3.Text = "Projection";
        this.button3.UseVisualStyleBackColor = true;
        this.button3.Click += new System.EventHandler(this.button3_Click);
        // 
        // EditWaypoint
        // 
        this.AcceptButton = this.button1;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.button2;
        this.ClientSize = new System.Drawing.Size(416, 325);
        this.Controls.Add(this.button3);
        this.Controls.Add(this.lCacheName);
        this.Controls.Add(this.tbGC);
        this.Controls.Add(this.cbTyp);
        this.Controls.Add(this.label5);
        this.Controls.Add(this.tbClue);
        this.Controls.Add(this.label4);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.tbDescription);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.tbTitel);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.bCoord);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.button1);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        this.Name = "EditWaypoint";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Edit Waypoint";
        this.Load += new System.EventHandler(this.EditWaypoint_Load);
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button bCoord;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.TextBox tbTitel;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox tbDescription;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox tbClue;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.ComboBox cbTyp;
    private System.Windows.Forms.MaskedTextBox tbGC;
    private System.Windows.Forms.Label lCacheName;
    private System.Windows.Forms.Button button3;
  }
}