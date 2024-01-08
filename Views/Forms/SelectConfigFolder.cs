using System;
using System.Windows.Forms;
using System.IO;

namespace WinCachebox.Views.Forms
{
    public partial class SelectConfigFolder : Form
  {
    public static void Show()
    {
      SelectConfigFolder scf = new SelectConfigFolder();
      if (scf.ShowDialog() == DialogResult.OK)
      {
      }
    }

    public SelectConfigFolder()
    {
      InitializeComponent();
    }

    private void button3_Click(object sender, EventArgs e)
    {
      folderBrowserDialog1.SelectedPath = tbConfigPath.Text;
      if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        tbConfigPath.Text = folderBrowserDialog1.SelectedPath;
      }
    }

    private void rbUserDefined_CheckedChanged(object sender, EventArgs e)
    {
      tbConfigPath.Visible = true;
      bSelectConfigPath.Visible = true;
    }

    private void rbDefault_CheckedChanged(object sender, EventArgs e)
    {
      tbConfigPath.Visible = false;
      bSelectConfigPath.Visible = false;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (rbDefault.Checked)
      {
        // default settings -> Use Application Data Folder
        Global.AppPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WinCachebox";
      }
      else if (rbLocal.Checked)
      {
        Global.AppPath = Path.GetDirectoryName(Application.ExecutablePath);
      } else
      {
        // User defined setting -> use folder in tbConfigPath
        // Check whether this folder exists or it can be created
        if (tbConfigPath.Text == "")
        {
          // not a complete folder path
          MessageBox.Show("Please select a folder!");
          DialogResult = System.Windows.Forms.DialogResult.None;
          tbConfigPath.Focus();
          return;
        }
        string path = "";
        try
        {
          path = Path.GetFullPath(tbConfigPath.Text);
        } catch (Exception)
        {
          path = "";
        }
        if (path.ToLower() != tbConfigPath.Text.ToLower())
        {
          // not a complete folder path
          MessageBox.Show("[" + tbConfigPath.Text + "] is not a valid folder path!", Global.Translations.Get("Error"));
          DialogResult = System.Windows.Forms.DialogResult.None;
          tbConfigPath.Focus();
          return;
        }
        if (!Directory.Exists(path))
        {
          try
          {
            Directory.CreateDirectory(path);
          }
          catch (Exception)
          {
            // Folder could not be created!
            MessageBox.Show("Folder [" + path + "] could not be created!", Global.Translations.Get("Error"));
            DialogResult = System.Windows.Forms.DialogResult.None;
            tbConfigPath.Focus();
            return;
          }
        }
        Global.AppPath = tbConfigPath.Text;
      }
    }

    private void rbLocal_CheckedChanged(object sender, EventArgs e)
    {
      tbConfigPath.Visible = false;
      bSelectConfigPath.Visible = false;
    }
  }
}
