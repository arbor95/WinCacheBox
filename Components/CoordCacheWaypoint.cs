using System;
using System.Windows.Forms;
using WinCachebox.Geocaching;

namespace WinCachebox.Components
{
    public partial class CoordCacheWaypoint : UserControl
  {
    private Cache cache = null;
    private Waypoint waypoint = null;
    private Coordinate coord;

    public CoordCacheWaypoint()
    {
      this.coord = new Coordinate();
      InitializeComponent();
      setCoordinate(coord);
    }

    private void setCacheName()
    {
      bCache.Text = "...";
      if (cache != null)
        bCache.Text = cache.Name;
      if (waypoint != null)
        bCache.Text += " / " + waypoint.Title;
    }

    private void setCoordinate(Coordinate coord)
    {
      this.coord = coord;
      bCoord.Text = coord.FormatCoordinate();
    }

    private void setCache(Cache cache)
    {
      this.cache = cache;
      if (cache == null)
        return;
      setCoordinate(new Coordinate(cache.Coordinate));
      setCacheName();
    }
    
    private void setWaypoint(Waypoint waypoint)
    {
      this.waypoint = waypoint;
      if (waypoint == null)
        return;
      setCoordinate(new Coordinate(waypoint.Coordinate));
      setCacheName();
    }

    public Cache Cache { get { return cache; } set { setCache(value); } }
    public Waypoint Waypoint { get { return waypoint; } set { setWaypoint(value); } }
    public Coordinate Coordinate { get { return coord; } set { setCoordinate(value); } }

    private void bCache_Click(object sender, EventArgs e)
    {
      Cache acache = null;
      Waypoint awaypoint = null;

      if (Views.Forms.GetCacheWaypoint.Get(out acache, out awaypoint))
      {
        cache = null;
        waypoint = null;
        if (acache != null)
          setCache(acache);
        if (awaypoint != null)
          setWaypoint(awaypoint);
      }
    }

    private void bCoord_Click(object sender, EventArgs e)
    {
      Coordinate old = new Coordinate(coord);
      if (old.Edit())
      {
        if ((old.Latitude != coord.Latitude) || (old.Longitude != coord.Longitude))
        {
          setWaypoint(null);
          setCache(null);
          bCache.Text = "other Waypoint...";
          setCoordinate(old);
        }
      }
    }

    public string GetDescription()
    {
      return bCache.Text;
    }
  }
}
