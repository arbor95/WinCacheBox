using System;
using System.Windows.Forms;

namespace WinCachebox
{
    public partial class ReplicationConflictForm : Form
  {
    public enum ReplicationConflictResult
    {
      DoNotSolve = 0,
      UseOriginal = 1,
      UseCopy = 2
    }

    public static ReplicationConflictResult ShowConflict(string cacheName, ChangeTypeEnum changeTyp, string original, string copy, bool synchronization)
    {
      ReplicationConflictForm rcf = new ReplicationConflictForm(cacheName, changeTyp, original, copy, synchronization);
      if (rcf.ShowDialog() == DialogResult.Cancel)
        return ReplicationConflictResult.DoNotSolve;

      return rcf.ConflictResult;
    }

    public ReplicationConflictResult ConflictResult = ReplicationConflictResult.DoNotSolve;
    private bool synchronization = false;

    public ReplicationConflictForm(string cacheName, ChangeTypeEnum changeTyp, string original, string copy, bool synchronization)
    {
        this.synchronization = synchronization;
        InitializeComponent();

        lCacheName.Text = cacheName;
        string s = "";
        if (synchronization)
            s = "Since the last Synchronisation the" + Environment.NewLine;
        else
            s = "The information" + Environment.NewLine;
        switch (changeTyp)
        {
            case ChangeTypeEnum.SolverText:
                s += "Solver Text" + Environment.NewLine;
                break;
            case ChangeTypeEnum.NotesText:
                s += "Note Text" + Environment.NewLine;
                break;
            case ChangeTypeEnum.DeleteWaypoint:
            case ChangeTypeEnum.NewWaypoint:
            case ChangeTypeEnum.WaypointChanged:
                s += "Waypoint" + Environment.NewLine;
                break;
        }
        if (synchronization)
            s += "was changed in both databases!";
        else
            s += "is different in both databases!";
        label1.Text = s;
        tbOriginal.Text = original;
        tbCopy.Text = copy;
        if (!synchronization)
        {
            bCopy.Text = "Use Value of Import-DB";
            bCancel.Text = "Cancel Import";
        }
    }

    private void bOriginal_Click(object sender, EventArgs e)
    {
      ConflictResult = ReplicationConflictResult.UseOriginal;
      DialogResult = System.Windows.Forms.DialogResult.OK;
    }

    private void bCopy_Click(object sender, EventArgs e)
    {
      ConflictResult = ReplicationConflictResult.UseCopy;
      DialogResult = System.Windows.Forms.DialogResult.OK;
    }
  }
}
