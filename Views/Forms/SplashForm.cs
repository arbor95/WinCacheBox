using System;
using System.Windows.Forms;

namespace WinCachebox.Views
{
    public partial class SplashForm : Form
  {
    public SplashForm()
    {
      InitializeComponent();
    }

    public void updateAction(String action, int count)
    {
      labelAction.Text = action;
      if (count > progressBar.Value)
        progressBar.Value = count;

      Application.DoEvents();
    }
  }
}
