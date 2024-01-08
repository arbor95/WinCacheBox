using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WinCachebox.Geocaching;

namespace WinCachebox.Views.Forms
{
    public partial class EditWaypoint : Form
  {
    public static bool EditWaypointDialog(Waypoint waypoint, Cache cache)
    {
      EditWaypoint ewd = new EditWaypoint(waypoint, cache);
      return ewd.ShowDialog() == DialogResult.OK;
    }

    Dictionary<CacheTypes, int> lookup = new Dictionary<CacheTypes, int>();
    private Waypoint waypoint;
    private Cache cache;
    private Coordinate coord = new Coordinate();
    private bool changeText = false;
    public EditWaypoint(Waypoint waypoint, Cache cache)
    {
      this.waypoint = waypoint;
      this.cache = cache;
      InitializeComponent();
      lookup.Add(CacheTypes.ReferencePoint, 0);
      lookup.Add(CacheTypes.MultiStage, 1);
      lookup.Add(CacheTypes.MultiQuestion, 2);
      lookup.Add(CacheTypes.Trailhead, 3);
      lookup.Add(CacheTypes.ParkingArea, 4);
      lookup.Add(CacheTypes.Final, 5);
    }

    private void EditWaypoint_Load(object sender, EventArgs e)
    {
      tbGC.Mask = "&&";
      for (int i = 2; i < cache.GcCode.Length; i++)
      {
        tbGC.Mask += "\\" + cache.GcCode[i];
      }
      coord = new Coordinate(waypoint.Coordinate);

      bCoord.Text = coord.FormatCoordinate();
      tbTitel.Text = waypoint.Title;
      tbDescription.Text = waypoint.Description;
      tbClue.Text = waypoint.Clue;
      tbGC.Text = waypoint.GcCode;
      cbTyp.SelectedIndex = lookup[waypoint.Type];
      lCacheName.Text = cache.Name;
      changeText = true;
    }

    private void bCoord_Click(object sender, EventArgs e)
    {
      if (coord.Edit())
        bCoord.Text = Global.FormatLatitudeDM(coord.Latitude) + " / " + Global.FormatLongitudeDM(coord.Longitude);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (coord.Valid)
      {
        waypoint.Latitude = coord.Latitude;
        waypoint.Longitude = coord.Longitude;
      }
      waypoint.Title = tbTitel.Text;
      waypoint.Clue = tbClue.Text;
      waypoint.Description = tbDescription.Text;
      waypoint.GcCode = tbGC.Text;

      CacheTypes wpType = CacheTypes.Undefined;
      foreach (CacheTypes type in lookup.Keys)
      if (cbTyp.SelectedIndex == lookup[type])
      {
        wpType = type;
        break;
      }

      waypoint.Type = wpType;
      waypoint.IsUserWaypoint = true;
    }

    private void label4_Click(object sender, EventArgs e)
    {

    }

    private void label2_Click(object sender, EventArgs e)
    {

    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void label5_Click(object sender, EventArgs e)
    {

    }

    private void button3_Click(object sender, EventArgs e)
    {
      Views.Forms.ProjectionForm projectionForm = new Views.Forms.ProjectionForm(cache, waypoint);
      if (projectionForm.ShowDialog() == DialogResult.OK)
      {
        coord = projectionForm.Result;

        if ((cache == projectionForm.Cache) && (waypoint == projectionForm.Waypoint))
        {
          // Waypoint wurde von sich aus projeziert
          bCoord.Text = coord.FormatCoordinate();

          tbTitel.Text += " (projected)";
          tbDescription.Text += Environment.NewLine + projectionForm.Notes(false);
        }
        else
        {
          // Waypoint wurde von einem anderen Waypoint aus projeziert
          bCoord.Text = coord.FormatCoordinate();

          tbTitel.Text = projectionForm.Description + " (projected)";
          tbDescription.Text = projectionForm.Notes(true);
        }
      }
    }

    private void cbTyp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (changeText)
            tbTitel.Text = cbTyp.Text;
    }
  }
}
