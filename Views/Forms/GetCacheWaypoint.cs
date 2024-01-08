using System;
using System.Windows.Forms;
using WinCachebox.Geocaching;

namespace WinCachebox.Views.Forms
{
    public partial class GetCacheWaypoint : Form
  {
    public static bool Get(out Cache cache, out Waypoint waypoint)
    {
      GetCacheWaypoint gcw = new GetCacheWaypoint();
      if (gcw.ShowDialog() == DialogResult.OK)
      {
        cache = gcw.waypointView1.aktCache;
        waypoint = gcw.waypointView1.aktWaypoint;
        return true;
      }
      else
      {
        cache = null;
        waypoint = null;
        return false;
      }
    }

    public GetCacheWaypoint()
    {
      InitializeComponent();
      cacheListView1.WaypointView = waypointView1;
    }

    private void GetCacheWaypoint_Load(object sender, EventArgs e)
    {
      cacheListView1.FilterChanged();
    }
  }
}
