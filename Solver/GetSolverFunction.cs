using System;
using System.Windows.Forms;

namespace WinCachebox.CBSolver
{
    public partial class GetSolverFunction : Form
  {
    public static string Get()
    {
      GetSolverFunction gsf = new GetSolverFunction();
      if (gsf.ShowDialog() == DialogResult.OK)
        return gsf.Result();
      else
        return "";
    }

    private Functions aktFunctions = null;
    private Function aktFunction = null;
    private string aktName = "";
    public GetSolverFunction()
    {
      InitializeComponent();
    }

    public string Result()
    {
      return aktName + "(|)";
    }

    private void GetSolverFunction_Load(object sender, EventArgs e)
    {
      this.Text = Global.Translations.Get("getSolverFunction");
      button1.Text = Global.Translations.Get("ok");
      button2.Text = Global.Translations.Get("cancel");
      
      if (CBSolver.Solver.functions == null)
        CBSolver.Solver.functions = new FunctionCategories();
      foreach (CBSolver.Functions functions in CBSolver.Solver.functions.Values)
      {
        ListViewItem lvi = lvGroups.Items.Add(functions.Name);
        lvi.Tag = functions;
      }
      if (lvGroups.Items.Count > 0)
      {
        lvGroups.SelectedIndices.Clear();
        lvGroups.SelectedIndices.Add(0);
      }
    }

    private void lvGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (lvGroups.SelectedIndices.Count == 0)
        return;
      int sel = lvGroups.SelectedIndices[0];
      aktFunctions = (lvGroups.Items[sel].Tag) as Functions;
      fillFunctionList();
      if (lvFuctions.Items.Count > 0)
      {
        lvFuctions.SelectedIndices.Clear();
        lvFuctions.SelectedIndices.Add(0);
      }
    }

    private void fillFunctionList()
    {
      lvFuctions.Items.Clear();
      if (aktFunctions == null)
        return;
      foreach (Function function in aktFunctions)
      {
        ListViewItem lvi = lvFuctions.Items.Add(function.Name);
        lvi.Tag = function;
      }
    }

    private void lvFuctions_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (lvFuctions.SelectedIndices.Count == 0)
        return;
      int sel = lvFuctions.SelectedIndices[0];
      aktFunction = (lvFuctions.Items[sel].Tag) as Function;
      if (aktFunction == null)
        return;
      int selectedIndex = -1;
      tbDescription.Text = aktFunction.Description;
      lvOtherNames.Items.Clear();
      foreach (string s in aktFunction.Names)
      {
        lvOtherNames.Items.Add(s);
        if (s.ToLower() == aktFunction.Name.ToLower())
          selectedIndex = lvOtherNames.Items.Count - 1;
      }
      lvOtherNames.SelectedIndices.Clear();
      if (selectedIndex < 0)
        selectedIndex = 0;
      lvOtherNames.SelectedIndices.Add(selectedIndex);
    }

    private void lvOtherNames_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (lvOtherNames.SelectedIndices.Count == 0)
        return;
      int sel = lvOtherNames.SelectedIndices[0];
      aktName = lvOtherNames.Items[sel].Text;
    }
  }
}
