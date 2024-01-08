namespace WinCachebox.Views
{
  partial class CacheInfoView
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CacheInfoView));
        this.lName = new System.Windows.Forms.Label();
        this.pictureBoxType = new System.Windows.Forms.PictureBox();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.pictureBoxDifficulty = new System.Windows.Forms.PictureBox();
        this.pictureBoxTerrain = new System.Windows.Forms.PictureBox();
        this.label3 = new System.Windows.Forms.Label();
        this.pictureBoxContainerSize = new System.Windows.Forms.PictureBox();
        this.label4 = new System.Windows.Forms.Label();
        this.labelSize = new System.Windows.Forms.Label();
        this.pictureBoxTb = new System.Windows.Forms.PictureBox();
        this.labelTbMultiply = new System.Windows.Forms.Label();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBoxType)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDifficulty)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTerrain)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBoxContainerSize)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTb)).BeginInit();
        this.SuspendLayout();
        // 
        // lName
        // 
        this.lName.AutoSize = true;
        this.lName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lName.Location = new System.Drawing.Point(3, 0);
        this.lName.Name = "lName";
        this.lName.Size = new System.Drawing.Size(157, 20);
        this.lName.TabIndex = 0;
        this.lName.Text = "No cache choosen";
        this.lName.Click += new System.EventHandler(this.lName_Click);
        this.lName.MouseEnter += new System.EventHandler(this.lName_MouseEnter);
        this.lName.MouseLeave += new System.EventHandler(this.lName_MouseLeave);
        // 
        // pictureBoxType
        // 
        this.pictureBoxType.Location = new System.Drawing.Point(0, 23);
        this.pictureBoxType.Name = "pictureBoxType";
        this.pictureBoxType.Size = new System.Drawing.Size(32, 32);
        this.pictureBoxType.TabIndex = 1;
        this.pictureBoxType.TabStop = false;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label1.Location = new System.Drawing.Point(38, 23);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(286, 16);
        this.label1.TabIndex = 2;
        this.label1.Text = "Please choose a geocache in the list on the left";
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label2.Location = new System.Drawing.Point(37, 42);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(93, 16);
        this.label2.TabIndex = 3;
        this.label2.Text = "Schwierigkeit: ";
        // 
        // pictureBoxDifficulty
        // 
        this.pictureBoxDifficulty.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxDifficulty.Image")));
        this.pictureBoxDifficulty.Location = new System.Drawing.Point(132, 43);
        this.pictureBoxDifficulty.Name = "pictureBoxDifficulty";
        this.pictureBoxDifficulty.Size = new System.Drawing.Size(66, 13);
        this.pictureBoxDifficulty.TabIndex = 4;
        this.pictureBoxDifficulty.TabStop = false;
        // 
        // pictureBoxTerrain
        // 
        this.pictureBoxTerrain.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxTerrain.Image")));
        this.pictureBoxTerrain.Location = new System.Drawing.Point(273, 43);
        this.pictureBoxTerrain.Name = "pictureBoxTerrain";
        this.pictureBoxTerrain.Size = new System.Drawing.Size(66, 13);
        this.pictureBoxTerrain.TabIndex = 6;
        this.pictureBoxTerrain.TabStop = false;
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label3.Location = new System.Drawing.Point(204, 42);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(66, 16);
        this.label3.TabIndex = 5;
        this.label3.Text = "Gelände: ";
        // 
        // pictureBoxContainerSize
        // 
        this.pictureBoxContainerSize.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxContainerSize.Image")));
        this.pictureBoxContainerSize.Location = new System.Drawing.Point(385, 45);
        this.pictureBoxContainerSize.Name = "pictureBoxContainerSize";
        this.pictureBoxContainerSize.Size = new System.Drawing.Size(50, 12);
        this.pictureBoxContainerSize.TabIndex = 8;
        this.pictureBoxContainerSize.TabStop = false;
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label4.Location = new System.Drawing.Point(340, 42);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(34, 16);
        this.label4.TabIndex = 7;
        this.label4.Text = "Size";
        // 
        // labelSize
        // 
        this.labelSize.AutoSize = true;
        this.labelSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.labelSize.Location = new System.Drawing.Point(436, 42);
        this.labelSize.Name = "labelSize";
        this.labelSize.Size = new System.Drawing.Size(68, 16);
        this.labelSize.TabIndex = 9;
        this.labelSize.Text = "(unknown)";
        // 
        // pictureBoxTb
        // 
        this.pictureBoxTb.Location = new System.Drawing.Point(533, 42);
        this.pictureBoxTb.Name = "pictureBoxTb";
        this.pictureBoxTb.Size = new System.Drawing.Size(16, 16);
        this.pictureBoxTb.TabIndex = 10;
        this.pictureBoxTb.TabStop = false;
        // 
        // labelTbMultiply
        // 
        this.labelTbMultiply.AutoSize = true;
        this.labelTbMultiply.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.labelTbMultiply.Location = new System.Drawing.Point(510, 43);
        this.labelTbMultiply.Name = "labelTbMultiply";
        this.labelTbMultiply.Size = new System.Drawing.Size(17, 16);
        this.labelTbMultiply.TabIndex = 11;
        this.labelTbMultiply.Text = "T";
        // 
        // CacheInfoView
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Controls.Add(this.labelTbMultiply);
        this.Controls.Add(this.pictureBoxTb);
        this.Controls.Add(this.labelSize);
        this.Controls.Add(this.pictureBoxContainerSize);
        this.Controls.Add(this.label4);
        this.Controls.Add(this.pictureBoxTerrain);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.pictureBoxDifficulty);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.pictureBoxType);
        this.Controls.Add(this.lName);
        this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Name = "CacheInfoView";
        this.Size = new System.Drawing.Size(601, 73);
        ((System.ComponentModel.ISupportInitialize)(this.pictureBoxType)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDifficulty)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTerrain)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBoxContainerSize)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTb)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lName;
    private System.Windows.Forms.PictureBox pictureBoxType;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.PictureBox pictureBoxDifficulty;
    private System.Windows.Forms.PictureBox pictureBoxTerrain;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.PictureBox pictureBoxContainerSize;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label labelSize;
    private System.Windows.Forms.PictureBox pictureBoxTb;
    private System.Windows.Forms.Label labelTbMultiply;
  }
}
