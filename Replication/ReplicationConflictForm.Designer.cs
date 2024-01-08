﻿namespace WinCachebox
{
  partial class ReplicationConflictForm
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
        this.panel1 = new System.Windows.Forms.Panel();
        this.splitContainer1 = new System.Windows.Forms.SplitContainer();
        this.panel3 = new System.Windows.Forms.Panel();
        this.tbOriginal = new System.Windows.Forms.TextBox();
        this.panel2 = new System.Windows.Forms.Panel();
        this.bOriginal = new System.Windows.Forms.Button();
        this.panel5 = new System.Windows.Forms.Panel();
        this.tbCopy = new System.Windows.Forms.TextBox();
        this.panel4 = new System.Windows.Forms.Panel();
        this.bCopy = new System.Windows.Forms.Button();
        this.bCancel = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.lCacheName = new System.Windows.Forms.Label();
        this.panel1.SuspendLayout();
        this.splitContainer1.Panel1.SuspendLayout();
        this.splitContainer1.Panel2.SuspendLayout();
        this.splitContainer1.SuspendLayout();
        this.panel3.SuspendLayout();
        this.panel2.SuspendLayout();
        this.panel5.SuspendLayout();
        this.panel4.SuspendLayout();
        this.SuspendLayout();
        // 
        // panel1
        // 
        this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.panel1.Controls.Add(this.splitContainer1);
        this.panel1.Location = new System.Drawing.Point(12, 107);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(474, 237);
        this.panel1.TabIndex = 0;
        // 
        // splitContainer1
        // 
        this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitContainer1.Location = new System.Drawing.Point(0, 0);
        this.splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        this.splitContainer1.Panel1.Controls.Add(this.panel3);
        this.splitContainer1.Panel1.Controls.Add(this.panel2);
        // 
        // splitContainer1.Panel2
        // 
        this.splitContainer1.Panel2.Controls.Add(this.panel5);
        this.splitContainer1.Panel2.Controls.Add(this.panel4);
        this.splitContainer1.Size = new System.Drawing.Size(474, 237);
        this.splitContainer1.SplitterDistance = 228;
        this.splitContainer1.TabIndex = 0;
        // 
        // panel3
        // 
        this.panel3.Controls.Add(this.tbOriginal);
        this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel3.Location = new System.Drawing.Point(0, 0);
        this.panel3.Name = "panel3";
        this.panel3.Size = new System.Drawing.Size(228, 199);
        this.panel3.TabIndex = 4;
        // 
        // tbOriginal
        // 
        this.tbOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tbOriginal.Location = new System.Drawing.Point(0, 0);
        this.tbOriginal.Multiline = true;
        this.tbOriginal.Name = "tbOriginal";
        this.tbOriginal.ReadOnly = true;
        this.tbOriginal.Size = new System.Drawing.Size(228, 199);
        this.tbOriginal.TabIndex = 0;
        // 
        // panel2
        // 
        this.panel2.Controls.Add(this.bOriginal);
        this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panel2.Location = new System.Drawing.Point(0, 199);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(228, 38);
        this.panel2.TabIndex = 3;
        // 
        // bOriginal
        // 
        this.bOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
        this.bOriginal.Location = new System.Drawing.Point(0, 0);
        this.bOriginal.Name = "bOriginal";
        this.bOriginal.Size = new System.Drawing.Size(228, 38);
        this.bOriginal.TabIndex = 2;
        this.bOriginal.Text = "Use Original Value";
        this.bOriginal.UseVisualStyleBackColor = true;
        this.bOriginal.Click += new System.EventHandler(this.bOriginal_Click);
        // 
        // panel5
        // 
        this.panel5.Controls.Add(this.tbCopy);
        this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel5.Location = new System.Drawing.Point(0, 0);
        this.panel5.Name = "panel5";
        this.panel5.Size = new System.Drawing.Size(242, 199);
        this.panel5.TabIndex = 1;
        // 
        // tbCopy
        // 
        this.tbCopy.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tbCopy.Location = new System.Drawing.Point(0, 0);
        this.tbCopy.Multiline = true;
        this.tbCopy.Name = "tbCopy";
        this.tbCopy.ReadOnly = true;
        this.tbCopy.Size = new System.Drawing.Size(242, 199);
        this.tbCopy.TabIndex = 1;
        // 
        // panel4
        // 
        this.panel4.Controls.Add(this.bCopy);
        this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panel4.Location = new System.Drawing.Point(0, 199);
        this.panel4.Name = "panel4";
        this.panel4.Size = new System.Drawing.Size(242, 38);
        this.panel4.TabIndex = 0;
        // 
        // bCopy
        // 
        this.bCopy.Dock = System.Windows.Forms.DockStyle.Fill;
        this.bCopy.Location = new System.Drawing.Point(0, 0);
        this.bCopy.Name = "bCopy";
        this.bCopy.Size = new System.Drawing.Size(242, 38);
        this.bCopy.TabIndex = 3;
        this.bCopy.Text = "Use Value of Copy";
        this.bCopy.UseVisualStyleBackColor = true;
        this.bCopy.Click += new System.EventHandler(this.bCopy_Click);
        // 
        // bCancel
        // 
        this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.bCancel.Location = new System.Drawing.Point(244, 353);
        this.bCancel.Name = "bCancel";
        this.bCancel.Size = new System.Drawing.Size(242, 33);
        this.bCancel.TabIndex = 2;
        this.bCancel.Text = "Do not solve this conflict now";
        this.bCancel.UseVisualStyleBackColor = true;
        // 
        // label1
        // 
        this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label1.Location = new System.Drawing.Point(12, 50);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(474, 54);
        this.label1.TabIndex = 3;
        this.label1.Text = "Since the last Synchronisation the Solver Text was changed in both databases.";
        this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // lCacheName
        // 
        this.lCacheName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lCacheName.Location = new System.Drawing.Point(12, 9);
        this.lCacheName.Name = "lCacheName";
        this.lCacheName.Size = new System.Drawing.Size(474, 27);
        this.lCacheName.TabIndex = 4;
        this.lCacheName.Text = "Cache";
        this.lCacheName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // ReplicationConflictForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.bCancel;
        this.ClientSize = new System.Drawing.Size(501, 396);
        this.Controls.Add(this.lCacheName);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.bCancel);
        this.Controls.Add(this.panel1);
        this.Name = "ReplicationConflictForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Replication Conflict";
        this.panel1.ResumeLayout(false);
        this.splitContainer1.Panel1.ResumeLayout(false);
        this.splitContainer1.Panel2.ResumeLayout(false);
        this.splitContainer1.ResumeLayout(false);
        this.panel3.ResumeLayout(false);
        this.panel3.PerformLayout();
        this.panel2.ResumeLayout(false);
        this.panel5.ResumeLayout(false);
        this.panel5.PerformLayout();
        this.panel4.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.TextBox tbOriginal;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button bOriginal;
    private System.Windows.Forms.Panel panel5;
    private System.Windows.Forms.TextBox tbCopy;
    private System.Windows.Forms.Panel panel4;
    private System.Windows.Forms.Button bCopy;
    private System.Windows.Forms.Button bCancel;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lCacheName;

  }
}