namespace WinCachebox.Views
{
  partial class Coordinates
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Coordinates));
        this.tbText = new System.Windows.Forms.TextBox();
        this.pParse = new System.Windows.Forms.Button();
        this.tabControl1 = new System.Windows.Forms.TabControl();
        this.tabPage1 = new System.Windows.Forms.TabPage();
        this.label5 = new System.Windows.Forms.Label();
        this.label6 = new System.Windows.Forms.Label();
        this.cbDLon = new System.Windows.Forms.ComboBox();
        this.cbDLat = new System.Windows.Forms.ComboBox();
        this.tbDLon = new System.Windows.Forms.TextBox();
        this.tbDLat = new System.Windows.Forms.TextBox();
        this.tabPage2 = new System.Windows.Forms.TabPage();
        this.label3 = new System.Windows.Forms.Label();
        this.label4 = new System.Windows.Forms.Label();
        this.tbMLonMin = new System.Windows.Forms.TextBox();
        this.tbMLatMin = new System.Windows.Forms.TextBox();
        this.label2 = new System.Windows.Forms.Label();
        this.label1 = new System.Windows.Forms.Label();
        this.cbMLon = new System.Windows.Forms.ComboBox();
        this.cbMLat = new System.Windows.Forms.ComboBox();
        this.tbMLonDeg = new System.Windows.Forms.TextBox();
        this.tbMLatDeg = new System.Windows.Forms.TextBox();
        this.tabPage3 = new System.Windows.Forms.TabPage();
        this.label11 = new System.Windows.Forms.Label();
        this.label12 = new System.Windows.Forms.Label();
        this.tbSLonSec = new System.Windows.Forms.TextBox();
        this.tbSLatSec = new System.Windows.Forms.TextBox();
        this.label7 = new System.Windows.Forms.Label();
        this.label8 = new System.Windows.Forms.Label();
        this.tbSLonMin = new System.Windows.Forms.TextBox();
        this.tbSLatMin = new System.Windows.Forms.TextBox();
        this.label9 = new System.Windows.Forms.Label();
        this.label10 = new System.Windows.Forms.Label();
        this.cbSLon = new System.Windows.Forms.ComboBox();
        this.cbSLat = new System.Windows.Forms.ComboBox();
        this.tbSLonDeg = new System.Windows.Forms.TextBox();
        this.tbSLatDeg = new System.Windows.Forms.TextBox();
        this.tabPage4 = new System.Windows.Forms.TabPage();
        this.tbZone = new System.Windows.Forms.TextBox();
        this.tbEasting = new System.Windows.Forms.TextBox();
        this.tbNording = new System.Windows.Forms.TextBox();
        this.cbULon = new System.Windows.Forms.ComboBox();
        this.cbULat = new System.Windows.Forms.ComboBox();
        this.button1 = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.tabControl1.SuspendLayout();
        this.tabPage1.SuspendLayout();
        this.tabPage2.SuspendLayout();
        this.tabPage3.SuspendLayout();
        this.tabPage4.SuspendLayout();
        this.SuspendLayout();
        // 
        // tbText
        // 
        this.tbText.Location = new System.Drawing.Point(12, 123);
        this.tbText.Name = "tbText";
        this.tbText.Size = new System.Drawing.Size(189, 20);
        this.tbText.TabIndex = 1;
        this.tbText.TextChanged += new System.EventHandler(this.tbText_TextChanged);
        // 
        // pParse
        // 
        this.pParse.Location = new System.Drawing.Point(201, 122);
        this.pParse.Name = "pParse";
        this.pParse.Size = new System.Drawing.Size(75, 23);
        this.pParse.TabIndex = 2;
        this.pParse.Text = "Parse";
        this.pParse.UseVisualStyleBackColor = true;
        this.pParse.Click += new System.EventHandler(this.pParse_Click);
        // 
        // tabControl1
        // 
        this.tabControl1.Controls.Add(this.tabPage1);
        this.tabControl1.Controls.Add(this.tabPage2);
        this.tabControl1.Controls.Add(this.tabPage3);
        this.tabControl1.Controls.Add(this.tabPage4);
        this.tabControl1.Location = new System.Drawing.Point(12, 12);
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Size = new System.Drawing.Size(268, 105);
        this.tabControl1.TabIndex = 0;
        this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
        // 
        // tabPage1
        // 
        this.tabPage1.Controls.Add(this.label5);
        this.tabPage1.Controls.Add(this.label6);
        this.tabPage1.Controls.Add(this.cbDLon);
        this.tabPage1.Controls.Add(this.cbDLat);
        this.tabPage1.Controls.Add(this.tbDLon);
        this.tabPage1.Controls.Add(this.tbDLat);
        this.tabPage1.Location = new System.Drawing.Point(4, 22);
        this.tabPage1.Name = "tabPage1";
        this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage1.Size = new System.Drawing.Size(260, 79);
        this.tabPage1.TabIndex = 0;
        this.tabPage1.Text = "Deg.";
        this.tabPage1.UseVisualStyleBackColor = true;
        // 
        // label5
        // 
        this.label5.AutoSize = true;
        this.label5.Location = new System.Drawing.Point(206, 46);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(11, 13);
        this.label5.TabIndex = 12;
        this.label5.Text = "°";
        this.label5.Click += new System.EventHandler(this.label5_Click);
        // 
        // label6
        // 
        this.label6.AutoSize = true;
        this.label6.Location = new System.Drawing.Point(206, 19);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(11, 13);
        this.label6.TabIndex = 11;
        this.label6.Text = "°";
        this.label6.Click += new System.EventHandler(this.label6_Click);
        // 
        // cbDLon
        // 
        this.cbDLon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbDLon.FormattingEnabled = true;
        this.cbDLon.Items.AddRange(new object[] {
            "E",
            "W"});
        this.cbDLon.Location = new System.Drawing.Point(16, 43);
        this.cbDLon.Name = "cbDLon";
        this.cbDLon.Size = new System.Drawing.Size(38, 21);
        this.cbDLon.TabIndex = 3;
        this.cbDLon.SelectedIndexChanged += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // cbDLat
        // 
        this.cbDLat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbDLat.FormattingEnabled = true;
        this.cbDLat.Items.AddRange(new object[] {
            "N",
            "S"});
        this.cbDLat.Location = new System.Drawing.Point(16, 16);
        this.cbDLat.Name = "cbDLat";
        this.cbDLat.Size = new System.Drawing.Size(38, 21);
        this.cbDLat.TabIndex = 1;
        this.cbDLat.SelectedIndexChanged += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tbDLon
        // 
        this.tbDLon.Location = new System.Drawing.Point(60, 44);
        this.tbDLon.Name = "tbDLon";
        this.tbDLon.Size = new System.Drawing.Size(146, 20);
        this.tbDLon.TabIndex = 2;
        this.tbDLon.Text = "0";
        this.tbDLon.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tbDLat
        // 
        this.tbDLat.Location = new System.Drawing.Point(60, 17);
        this.tbDLat.Name = "tbDLat";
        this.tbDLat.Size = new System.Drawing.Size(146, 20);
        this.tbDLat.TabIndex = 0;
        this.tbDLat.Text = "0";
        this.tbDLat.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tabPage2
        // 
        this.tabPage2.Controls.Add(this.label3);
        this.tabPage2.Controls.Add(this.label4);
        this.tabPage2.Controls.Add(this.tbMLonMin);
        this.tabPage2.Controls.Add(this.tbMLatMin);
        this.tabPage2.Controls.Add(this.label2);
        this.tabPage2.Controls.Add(this.label1);
        this.tabPage2.Controls.Add(this.cbMLon);
        this.tabPage2.Controls.Add(this.cbMLat);
        this.tabPage2.Controls.Add(this.tbMLonDeg);
        this.tabPage2.Controls.Add(this.tbMLatDeg);
        this.tabPage2.Location = new System.Drawing.Point(4, 22);
        this.tabPage2.Name = "tabPage2";
        this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage2.Size = new System.Drawing.Size(260, 79);
        this.tabPage2.TabIndex = 1;
        this.tabPage2.Text = "Deg. Min.";
        this.tabPage2.UseVisualStyleBackColor = true;
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(181, 47);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(9, 13);
        this.label3.TabIndex = 14;
        this.label3.Text = "\'";
        this.label3.Click += new System.EventHandler(this.label3_Click);
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Location = new System.Drawing.Point(181, 20);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(9, 13);
        this.label4.TabIndex = 13;
        this.label4.Text = "\'";
        this.label4.Click += new System.EventHandler(this.label4_Click);
        // 
        // tbMLonMin
        // 
        this.tbMLonMin.Location = new System.Drawing.Point(120, 44);
        this.tbMLonMin.Name = "tbMLonMin";
        this.tbMLonMin.Size = new System.Drawing.Size(61, 20);
        this.tbMLonMin.TabIndex = 4;
        this.tbMLonMin.Text = "0";
        this.tbMLonMin.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tbMLatMin
        // 
        this.tbMLatMin.Location = new System.Drawing.Point(120, 17);
        this.tbMLatMin.Name = "tbMLatMin";
        this.tbMLatMin.Size = new System.Drawing.Size(61, 20);
        this.tbMLatMin.TabIndex = 1;
        this.tbMLatMin.Text = "0";
        this.tbMLatMin.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(105, 47);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(11, 13);
        this.label2.TabIndex = 10;
        this.label2.Text = "°";
        this.label2.Click += new System.EventHandler(this.label2_Click);
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(105, 20);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(11, 13);
        this.label1.TabIndex = 9;
        this.label1.Text = "°";
        this.label1.Click += new System.EventHandler(this.label1_Click);
        // 
        // cbMLon
        // 
        this.cbMLon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbMLon.FormattingEnabled = true;
        this.cbMLon.Items.AddRange(new object[] {
            "E",
            "W"});
        this.cbMLon.Location = new System.Drawing.Point(16, 43);
        this.cbMLon.Name = "cbMLon";
        this.cbMLon.Size = new System.Drawing.Size(38, 21);
        this.cbMLon.TabIndex = 5;
        this.cbMLon.SelectedIndexChanged += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // cbMLat
        // 
        this.cbMLat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbMLat.FormattingEnabled = true;
        this.cbMLat.Items.AddRange(new object[] {
            "N",
            "S"});
        this.cbMLat.Location = new System.Drawing.Point(16, 16);
        this.cbMLat.Name = "cbMLat";
        this.cbMLat.Size = new System.Drawing.Size(38, 21);
        this.cbMLat.TabIndex = 2;
        this.cbMLat.SelectedIndexChanged += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tbMLonDeg
        // 
        this.tbMLonDeg.Location = new System.Drawing.Point(60, 44);
        this.tbMLonDeg.Name = "tbMLonDeg";
        this.tbMLonDeg.Size = new System.Drawing.Size(45, 20);
        this.tbMLonDeg.TabIndex = 3;
        this.tbMLonDeg.Text = "0";
        this.tbMLonDeg.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tbMLatDeg
        // 
        this.tbMLatDeg.Location = new System.Drawing.Point(60, 17);
        this.tbMLatDeg.Name = "tbMLatDeg";
        this.tbMLatDeg.Size = new System.Drawing.Size(45, 20);
        this.tbMLatDeg.TabIndex = 0;
        this.tbMLatDeg.Text = "0";
        this.tbMLatDeg.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tabPage3
        // 
        this.tabPage3.Controls.Add(this.label11);
        this.tabPage3.Controls.Add(this.label12);
        this.tabPage3.Controls.Add(this.tbSLonSec);
        this.tabPage3.Controls.Add(this.tbSLatSec);
        this.tabPage3.Controls.Add(this.label7);
        this.tabPage3.Controls.Add(this.label8);
        this.tabPage3.Controls.Add(this.tbSLonMin);
        this.tabPage3.Controls.Add(this.tbSLatMin);
        this.tabPage3.Controls.Add(this.label9);
        this.tabPage3.Controls.Add(this.label10);
        this.tabPage3.Controls.Add(this.cbSLon);
        this.tabPage3.Controls.Add(this.cbSLat);
        this.tabPage3.Controls.Add(this.tbSLonDeg);
        this.tabPage3.Controls.Add(this.tbSLatDeg);
        this.tabPage3.Location = new System.Drawing.Point(4, 22);
        this.tabPage3.Name = "tabPage3";
        this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage3.Size = new System.Drawing.Size(260, 79);
        this.tabPage3.TabIndex = 2;
        this.tabPage3.Text = "Deg. Min. Sec.";
        this.tabPage3.UseVisualStyleBackColor = true;
        // 
        // label11
        // 
        this.label11.AutoSize = true;
        this.label11.Location = new System.Drawing.Point(235, 47);
        this.label11.Name = "label11";
        this.label11.Size = new System.Drawing.Size(11, 13);
        this.label11.TabIndex = 28;
        this.label11.Text = "\'\'";
        // 
        // label12
        // 
        this.label12.AutoSize = true;
        this.label12.Location = new System.Drawing.Point(235, 20);
        this.label12.Name = "label12";
        this.label12.Size = new System.Drawing.Size(11, 13);
        this.label12.TabIndex = 27;
        this.label12.Text = "\'\'";
        // 
        // tbSLonSec
        // 
        this.tbSLonSec.Location = new System.Drawing.Point(174, 44);
        this.tbSLonSec.Name = "tbSLonSec";
        this.tbSLonSec.Size = new System.Drawing.Size(61, 20);
        this.tbSLonSec.TabIndex = 6;
        this.tbSLonSec.Text = "0";
        this.tbSLonSec.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tbSLatSec
        // 
        this.tbSLatSec.Location = new System.Drawing.Point(174, 17);
        this.tbSLatSec.Name = "tbSLatSec";
        this.tbSLatSec.Size = new System.Drawing.Size(61, 20);
        this.tbSLatSec.TabIndex = 2;
        this.tbSLatSec.Text = "0";
        this.tbSLatSec.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // label7
        // 
        this.label7.AutoSize = true;
        this.label7.Location = new System.Drawing.Point(159, 47);
        this.label7.Name = "label7";
        this.label7.Size = new System.Drawing.Size(9, 13);
        this.label7.TabIndex = 24;
        this.label7.Text = "\'";
        // 
        // label8
        // 
        this.label8.AutoSize = true;
        this.label8.Location = new System.Drawing.Point(159, 20);
        this.label8.Name = "label8";
        this.label8.Size = new System.Drawing.Size(9, 13);
        this.label8.TabIndex = 23;
        this.label8.Text = "\'";
        // 
        // tbSLonMin
        // 
        this.tbSLonMin.Location = new System.Drawing.Point(120, 44);
        this.tbSLonMin.Name = "tbSLonMin";
        this.tbSLonMin.Size = new System.Drawing.Size(39, 20);
        this.tbSLonMin.TabIndex = 5;
        this.tbSLonMin.Text = "0";
        this.tbSLonMin.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tbSLatMin
        // 
        this.tbSLatMin.Location = new System.Drawing.Point(120, 17);
        this.tbSLatMin.Name = "tbSLatMin";
        this.tbSLatMin.Size = new System.Drawing.Size(39, 20);
        this.tbSLatMin.TabIndex = 1;
        this.tbSLatMin.Text = "0";
        this.tbSLatMin.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // label9
        // 
        this.label9.AutoSize = true;
        this.label9.Location = new System.Drawing.Point(105, 47);
        this.label9.Name = "label9";
        this.label9.Size = new System.Drawing.Size(11, 13);
        this.label9.TabIndex = 20;
        this.label9.Text = "°";
        // 
        // label10
        // 
        this.label10.AutoSize = true;
        this.label10.Location = new System.Drawing.Point(105, 20);
        this.label10.Name = "label10";
        this.label10.Size = new System.Drawing.Size(11, 13);
        this.label10.TabIndex = 19;
        this.label10.Text = "°";
        // 
        // cbSLon
        // 
        this.cbSLon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbSLon.FormattingEnabled = true;
        this.cbSLon.Items.AddRange(new object[] {
            "E",
            "W"});
        this.cbSLon.Location = new System.Drawing.Point(16, 43);
        this.cbSLon.Name = "cbSLon";
        this.cbSLon.Size = new System.Drawing.Size(38, 21);
        this.cbSLon.TabIndex = 7;
        this.cbSLon.SelectedIndexChanged += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // cbSLat
        // 
        this.cbSLat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbSLat.FormattingEnabled = true;
        this.cbSLat.Items.AddRange(new object[] {
            "N",
            "S"});
        this.cbSLat.Location = new System.Drawing.Point(16, 16);
        this.cbSLat.Name = "cbSLat";
        this.cbSLat.Size = new System.Drawing.Size(38, 21);
        this.cbSLat.TabIndex = 3;
        this.cbSLat.SelectedIndexChanged += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tbSLonDeg
        // 
        this.tbSLonDeg.Location = new System.Drawing.Point(60, 44);
        this.tbSLonDeg.Name = "tbSLonDeg";
        this.tbSLonDeg.Size = new System.Drawing.Size(45, 20);
        this.tbSLonDeg.TabIndex = 4;
        this.tbSLonDeg.Text = "0";
        this.tbSLonDeg.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tbSLatDeg
        // 
        this.tbSLatDeg.Location = new System.Drawing.Point(60, 17);
        this.tbSLatDeg.Name = "tbSLatDeg";
        this.tbSLatDeg.Size = new System.Drawing.Size(45, 20);
        this.tbSLatDeg.TabIndex = 0;
        this.tbSLatDeg.Text = "0";
        this.tbSLatDeg.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tabPage4
        // 
        this.tabPage4.Controls.Add(this.tbZone);
        this.tabPage4.Controls.Add(this.tbEasting);
        this.tabPage4.Controls.Add(this.tbNording);
        this.tabPage4.Controls.Add(this.cbULon);
        this.tabPage4.Controls.Add(this.cbULat);
        this.tabPage4.Location = new System.Drawing.Point(4, 22);
        this.tabPage4.Name = "tabPage4";
        this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage4.Size = new System.Drawing.Size(260, 79);
        this.tabPage4.TabIndex = 3;
        this.tabPage4.Text = "UTM";
        this.tabPage4.UseVisualStyleBackColor = true;
        // 
        // tbZone
        // 
        this.tbZone.Location = new System.Drawing.Point(185, 44);
        this.tbZone.Name = "tbZone";
        this.tbZone.Size = new System.Drawing.Size(52, 20);
        this.tbZone.TabIndex = 8;
        this.tbZone.Text = "0";
        this.tbZone.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tbEasting
        // 
        this.tbEasting.Location = new System.Drawing.Point(56, 18);
        this.tbEasting.Name = "tbEasting";
        this.tbEasting.Size = new System.Drawing.Size(112, 20);
        this.tbEasting.TabIndex = 7;
        this.tbEasting.Text = "0";
        this.tbEasting.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // tbNording
        // 
        this.tbNording.Location = new System.Drawing.Point(56, 44);
        this.tbNording.Name = "tbNording";
        this.tbNording.Size = new System.Drawing.Size(112, 20);
        this.tbNording.TabIndex = 6;
        this.tbNording.Text = "0";
        this.tbNording.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // cbULon
        // 
        this.cbULon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbULon.Enabled = false;
        this.cbULon.FormattingEnabled = true;
        this.cbULon.Items.AddRange(new object[] {
            "E",
            "W"});
        this.cbULon.Location = new System.Drawing.Point(13, 17);
        this.cbULon.Name = "cbULon";
        this.cbULon.Size = new System.Drawing.Size(38, 21);
        this.cbULon.TabIndex = 5;
        this.cbULon.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // cbULat
        // 
        this.cbULat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbULat.Enabled = false;
        this.cbULat.FormattingEnabled = true;
        this.cbULat.Items.AddRange(new object[] {
            "N",
            "S"});
        this.cbULat.Location = new System.Drawing.Point(13, 43);
        this.cbULat.Name = "cbULat";
        this.cbULat.Size = new System.Drawing.Size(38, 21);
        this.cbULat.TabIndex = 4;
        this.cbULat.Leave += new System.EventHandler(this.tbMLatMin_Leave);
        // 
        // button1
        // 
        this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.button1.Location = new System.Drawing.Point(288, 93);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(75, 23);
        this.button1.TabIndex = 3;
        this.button1.Text = "&OK";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // button2
        // 
        this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.button2.Location = new System.Drawing.Point(288, 121);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(75, 23);
        this.button2.TabIndex = 4;
        this.button2.Text = "&Cancel";
        this.button2.UseVisualStyleBackColor = true;
        // 
        // Coordinates
        // 
        this.AcceptButton = this.button1;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.button2;
        this.ClientSize = new System.Drawing.Size(375, 160);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.tabControl1);
        this.Controls.Add(this.pParse);
        this.Controls.Add(this.tbText);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "Coordinates";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Coordinates";
        this.Load += new System.EventHandler(this.Coordinates_Load);
        this.tabControl1.ResumeLayout(false);
        this.tabPage1.ResumeLayout(false);
        this.tabPage1.PerformLayout();
        this.tabPage2.ResumeLayout(false);
        this.tabPage2.PerformLayout();
        this.tabPage3.ResumeLayout(false);
        this.tabPage3.PerformLayout();
        this.tabPage4.ResumeLayout(false);
        this.tabPage4.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox tbText;
    private System.Windows.Forms.Button pParse;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TextBox tbDLat;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.TabPage tabPage3;
    private System.Windows.Forms.TextBox tbDLon;
    private System.Windows.Forms.ComboBox cbDLon;
    private System.Windows.Forms.ComboBox cbDLat;
    private System.Windows.Forms.TextBox tbMLonMin;
    private System.Windows.Forms.TextBox tbMLatMin;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbMLon;
    private System.Windows.Forms.ComboBox cbMLat;
    private System.Windows.Forms.TextBox tbMLonDeg;
    private System.Windows.Forms.TextBox tbMLatDeg;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.TextBox tbSLonSec;
    private System.Windows.Forms.TextBox tbSLatSec;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox tbSLonMin;
    private System.Windows.Forms.TextBox tbSLatMin;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.ComboBox cbSLon;
    private System.Windows.Forms.ComboBox cbSLat;
    private System.Windows.Forms.TextBox tbSLonDeg;
    private System.Windows.Forms.TextBox tbSLatDeg;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.TabPage tabPage4;
    private System.Windows.Forms.TextBox tbZone;
    private System.Windows.Forms.TextBox tbEasting;
    private System.Windows.Forms.TextBox tbNording;
    private System.Windows.Forms.ComboBox cbULon;
    private System.Windows.Forms.ComboBox cbULat;
  }
}