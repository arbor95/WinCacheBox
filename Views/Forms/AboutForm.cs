using System;
using System.Windows.Forms;

namespace WinCachebox.Views
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            this.button1.Text = "&" + Global.Translations.Get("ok");
            this.textBox1.Text = "Uses portions of " +
                "SharpZipLib, SourceGrid, CacheBox, WeifenLuo.WinFormsUI.Docking" +
                "\r\nGeocaching.com Cache Icons Copyright 2009" +
                "\r\nGroundspeak Inc.";
            this.label1.Text = "WinCachebox";
            this.label2.Text = "Team Cachebox (c) 2024";
            pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            String revision = "Revision: " + Global.sCurrentRevision;
            if (Global.RevisionSuffix.Length > 0)
                revision += " - " + Global.RevisionSuffix;
            labelRevision.Text = revision;
        }
        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        private void VisitLink()
        {
            // Change the color of the link text by setting LinkVisited   
            // to true.  
            linkLabel1.LinkVisited = true;
            //Call the Process.Start method to open the default browser   
            //with a URL:  
            System.Diagnostics.Process.Start("https://www.geocaching.com");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }
        
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.Cursor = Cursors.Hand;
        }
    }
}
