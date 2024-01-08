using System;

namespace WinCachebox.Geocaching
{
    public class BoundingBox
  {
    public double MinLatitude = double.MaxValue;

    public double MaxLatitude = double.MinValue;

    public double MinLongitude = double.MaxValue;

    public double MaxLongitude = double.MinValue;

    public bool Valid = false;

    public BoundingBox()
    {

    }

    public BoundingBox(double latitude, double longitude)
    {
      MinLatitude = MaxLatitude = latitude;
      MinLongitude = MaxLongitude = longitude;
      Valid = true;
    }

    public void Add(double latitude, double longitude)
    {
      MinLatitude = Math.Min(MinLatitude, latitude);
      MaxLatitude = Math.Max(MaxLatitude, latitude);
      MinLongitude = Math.Min(MinLongitude, longitude);
      MaxLongitude = Math.Max(MaxLongitude, longitude);
      Valid = true;
    }

    public override bool Equals(object obj)
    {
      if (obj.GetType() != this.GetType())
        return false;

      BoundingBox box = obj as BoundingBox;

      return (box.MaxLatitude == this.MaxLatitude &&
              box.MinLatitude == this.MinLatitude &&
              box.MaxLongitude == this.MaxLongitude &&
              box.MinLongitude == this.MinLongitude &&
              box.Valid == this.Valid);
    }

    public override int GetHashCode()
    {
      return (int)((10000000 * MinLatitude) + (MinLongitude * 13777777) + (MaxLatitude - MinLatitude) * 10000000);
    }
  }
}
