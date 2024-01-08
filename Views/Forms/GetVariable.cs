using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinCachebox.Views.Forms
{
    public partial class GetVariable : Form
  {
    public static string Get()
    {
      GetVariable gv = new GetVariable();
      if (gv.ShowDialog() == DialogResult.OK)
      {
        return gv.GetResult();
      } 
      else
        return "";
    }

    public GetVariable()
    {
      InitializeComponent();
    }

    private void GetVariable_Load(object sender, EventArgs e)
    {
      fillVariablenList();
    }

    private void fillVariablenList()
    {
      lvVariable.Items.Clear();
      if (CBSolver.Solver.variablen.Count == 0)
      {
        SolverView.View.Solve();
      }
      foreach(KeyValuePair<string, string> variable in CBSolver.Solver.Variablen)
      {
        ListViewItem lvi = lvVariable.Items.Add(variable.Key);
        lvi.SubItems.Add(variable.Value);
      }
    }

    private string GetResult()
    {
      if (lvVariable.SelectedItems.Count == 0)
        return "";
      if (lvVariable.SelectedItems[0].SubItems.Count == 0)
        return "";

      return (lvVariable.SelectedItems[0].SubItems[1].Text);
    }
  }
}
