using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WinCachebox.Geocaching;
using WinCachebox.CBSolver;
using WeifenLuo.WinFormsUI.Docking;

namespace WinCachebox.Views
{
    public partial class SolverView : DockContent
  {
    public static SolverView View = null;
    private Cache aktCache = null;
    private bool changed = false;

    public SolverView()
    {
      View = this;
      InitializeComponent();
      Global.TargetChanged += new Global.TargetChangedHandler(OnTargetChanged);
    }

    void OnTargetChanged(Cache cache, Waypoint waypoint)
    {
      if (changed)
      {
        // Änderungen des Solvertextes als Solver speichern
        if (aktCache != null)
        {
          aktCache.Solver = textBox1.Text;
        }
      }
      if (cache == aktCache)
        return;  // nur der Waypoint hat sich geändert...

      aktCache = cache;
      if(cache != null)
        textBox1.Text = cache.Solver;
      tbLösung.Text = "";
      changed = false;
      SelectedCacheChanged();
    }

    public void SelectedCacheChanged()
    {
      if (aktCache != null)
        textBox1.Text = aktCache.Solver;
      else
        textBox1.Text = "";
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Solve();
    }

    public void Solve()
    {
      Solver solver = new Solver(textBox1.Text);
      if (!solver.Solve())
      {
        MessageBox.Show(Global.Translations.Get("Error"));
      }
      tbLösung.Clear();
      foreach (SolverZeile zeile in solver)
      {
        tbLösung.Text += zeile.Solution + Environment.NewLine;
      }

      if ((Solver.MissingVariables != null) && (Solver.MissingVariables.Count > 0))
      { 
        // es sind nicht alle Variablen zugewiesen
        // Abfrage, ob die Deklarationen eingefügt werden sollen
        string message = "";
        foreach (string s in Solver.MissingVariables.Keys)
        {
          if (message != "")
            message += ", ";
          message += s;
        }
        if (MessageBox.Show("Insert declarations for the missing variables:" + Environment.NewLine + message, "Missing variables", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          string missing = "";
          foreach (string s in Solver.MissingVariables.Keys)
          {
            missing += s + "=" + Environment.NewLine;
            tbLösung.Text = Environment.NewLine + tbLösung.Text;
          }
          textBox1.Text = missing + textBox1.Text;
        }
      }
    }

    private void bAddFinal_Click(object sender, EventArgs e)
    {
      if (aktCache == null)
        aktCache = Global.SelectedCache;
      if (aktCache == null) return;
      string s = tbLösung.SelectedText;
      Coordinate coord = new Coordinate(s);
      if (coord.Valid)
      {
        Waypoint final = aktCache.GetFinalWaypoint;
        Waypoint waypoint = null;
        if (final != null)
        {
          DialogResult result = MessageBox.Show("Do you want to insert this coordinates into the existing final waypoint " + final.Title + "?", "Final found", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
          if (result == DialogResult.Yes)
          {
            waypoint = final;
          }
          if (result == DialogResult.Cancel)
            return;
        }

        String newGcCode = Waypoint.CreateFreeGcCode(aktCache.GcCode);
        if (waypoint == null)
          waypoint = new Waypoint(newGcCode, CacheTypes.Final, "", coord.Latitude, coord.Longitude, aktCache.Id, "", "Final");
        else
        {
          waypoint.Coordinate = new Coordinate(coord);
        }
        if (Views.Forms.EditWaypoint.EditWaypointDialog(waypoint, aktCache))
        {
          if (waypoint == final)
          {
            waypoint.UpdateDatabase();
            for (int msi = 0; msi < Cache.MysterySolutions.Count; msi++)
            {
              Cache.MysterySolution sol = Cache.MysterySolutions[msi];
              if ((sol.Cache == aktCache) && (sol.Waypoint == waypoint))
              {
                sol.Latitude = waypoint.Latitude;
                sol.Longitude = waypoint.Longitude;
              }
            }
          }
          else
          {
            // new final waypoint is created
            aktCache.Waypoints.Add(waypoint);
            waypoint.WriteToDatabase();
                        // add new final to MysterySolutions
                        Cache.MysterySolution sol = new Cache.MysterySolution
                        {
                            Cache = aktCache,
                            Waypoint = waypoint,
                            Latitude = waypoint.Latitude,
                            Longitude = waypoint.Longitude
                        };
                        Cache.MysterySolutions.Add(sol);
          }
          Global.SelectedWaypoint = waypoint;
          CacheListView.View.WpTypeChanged();
          WaypointView.View.Refresh();
        }
      }
      else
      {
        if (s == "")
          MessageBox.Show("Please select the coordinates in the Text!");
        else
          MessageBox.Show(s + Environment.NewLine + "are no valid Coordinates!");
      }
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      changed = true;
    }

    private void bSetCenter_Click(object sender, EventArgs e)
    {
      if (aktCache == null)
        aktCache = Global.SelectedCache;
      if (aktCache == null) return;
      string s = tbLösung.SelectedText;
      Coordinate coord = new Coordinate(s);
      if (coord.Valid)
      {
        Global.LastValidPosition = coord;
        Global.SetMarker(coord);
      }
      else
      {
        if (s == "")
          MessageBox.Show("Please select the coordinates in the Text!");
        else
          MessageBox.Show(s + Environment.NewLine + "are no valid Coordinates!");
      }
    }

    private void fFunction_Click(object sender, EventArgs e)
    {
      string s = CBSolver.GetSolverFunction.Get();
      if (s != "")
      {
        int pos = s.IndexOf('|');
        string s2 = "";
        if (pos > 0)
        {
          s2 = s.Substring(pos + 1, s.Length - pos - 1);
          s = s.Substring(0, pos);
        }

        int start = textBox1.SelectionStart;
        String text = textBox1.Text.Substring(0, start) + s + s2 + textBox1.Text.Substring(textBox1.SelectionStart + textBox1.SelectionLength);
        textBox1.Text = text;
        textBox1.SelectionStart = start + s.Length;
        textBox1.SelectionLength = 0;
        textBox1.Focus();
      }
    }

    private bool HasFinal(Cache cache)
    {
        List<Waypoint> wps = cache.Waypoints;
        foreach (Waypoint wp in wps)
        {
          if (wp.Type == CacheTypes.Final)
            return true;
        }
        return false;
    }

    private void bInsertWaypoint_Click(object sender, EventArgs e)
    {
        if (aktCache == null)
            return;
        String newGcCode = Waypoint.CreateFreeGcCode(Global.SelectedCache.GcCode);
        Waypoint waypoint = null;
        if (aktCache.Type == CacheTypes.Multi)
            waypoint = new Waypoint(newGcCode, CacheTypes.MultiStage, "", 0, 0, aktCache.Id, "", "New Stage of a Multicache");
        else if (aktCache.Type == CacheTypes.Mystery)
            if (HasFinal(aktCache))
                waypoint = new Waypoint(newGcCode, CacheTypes.ReferencePoint, "", 0, 0, aktCache.Id, "", "New Reference Point");
            else
                waypoint = new Waypoint(newGcCode, CacheTypes.Final, "", 0, 0, aktCache.Id, "", "New Final");
        else
            waypoint = new Waypoint(newGcCode, CacheTypes.ReferencePoint, "", 0, 0, aktCache.Id, "", "New Reference Point");
        if (Views.Forms.EditWaypoint.EditWaypointDialog(waypoint, aktCache))
        {
            aktCache.Waypoints.Add(waypoint);
            waypoint.WriteToDatabase();
            int SelStart = textBox1.SelectionStart;
            int TextLen = textBox1.Text.Length;
            String s = textBox1.Text;
            // Einfügen in nächster Zeile
            int iPos = s.Substring(SelStart).IndexOf(Environment.NewLine);
            SelStart = iPos < 0 ? TextLen : SelStart += Environment.NewLine.Length + iPos;
            String sIns = "$" + waypoint.GcCode + "=" + Environment.NewLine;
            textBox1.Text = s.Substring(0, SelStart) + sIns + s.Substring(SelStart+textBox1.SelectionLength);
                textBox1.SelectionStart = SelStart + sIns.Length;
            Global.SelectedWaypoint = waypoint;
           //  WaypointView.fillCacheList();
            SelectedCacheChanged();
            CacheListView.View.WpTypeChanged();
            if (waypoint.Type == CacheTypes.Final)
            {
                    // only add this waypoint to MysterySolutions when it is a Final
                    Cache.MysterySolution sol = new Cache.MysterySolution
                    {
                        Cache = aktCache,
                        Waypoint = waypoint,
                        Latitude = waypoint.Latitude,
                        Longitude = waypoint.Longitude
                    };
                    Cache.MysterySolutions.Add(sol);
            }
      }
    }

  }
}
