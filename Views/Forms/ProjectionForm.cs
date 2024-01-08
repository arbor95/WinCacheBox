using System;
using System.Windows.Forms;
using WinCachebox.Geocaching;

namespace WinCachebox.Views.Forms
{
    public partial class ProjectionForm : Form
  {
    private Coordinate result = new Coordinate();
    public ProjectionForm(Cache cache, Waypoint waypoint)
    {
      InitializeComponent();

      bCoord.Cache = cache;
      bCoord.Waypoint = waypoint;

/*      if (UnitFormatter.ImperialUnits)
        label2.Text = "yd";
      else
        label2.Text = "m";*/

      bResult.Text = result.FormatCoordinate();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      berechne();
      DialogResult = System.Windows.Forms.DialogResult.OK;
    }

    public Coordinate Result { get { return result; } }
    // description of the source point
    public string Description { get { return bCoord.GetDescription(); } }
    public string Notes(bool withCache)
    {
      string result = "";
      if (withCache)
        result += bCoord.GetDescription() + Environment.NewLine;
      result += "Projected by" + Environment.NewLine;
      result += tbDistance.Text + "m - " + tbBearing.Text + "°";
      return result;
    }

    private void button4_Click(object sender, EventArgs e)
    {
      string var = Forms.GetVariable.Get();
      tbDistance.Text = var;
    }

    private void button5_Click(object sender, EventArgs e)
    {
      string var = Forms.GetVariable.Get();
      tbBearing.Text = var;
    }

    private void tbDistance_TextChanged(object sender, EventArgs e)
    {
      berechne();
      bResult.Text = result.FormatCoordinate();
    }

    private void berechne()
    {
      double distance = 0;
      double bearing = 0;
      result.Valid = false;
      
      try
      {
        distance = Convert.ToDouble(tbDistance.Text);
      }
      catch (Exception)
      {
        tbDistance.Focus();
        return;
      }
      try
      {
        bearing = Convert.ToDouble(tbBearing.Text);
      }
      catch (Exception)
      {
        tbBearing.Focus();
        return;
      }
      if (distance <= 0)
      {
        tbDistance.Focus();
        return;
      }
      if ((bearing < 0) || (bearing > 360))
      {
        tbBearing.Focus();
        return;
      }
      result = Coordinate.Project(bCoord.Coordinate.Latitude, bCoord.Coordinate.Longitude, bearing, distance);
    }

    private void bResult_Click(object sender, EventArgs e)
    {
      if (result.Edit())
        bResult.Text = result.FormatCoordinate();

    }

    public Waypoint Waypoint { get { return bCoord.Waypoint; } }
    public Cache Cache { get { return bCoord.Cache; } }
  }
}
