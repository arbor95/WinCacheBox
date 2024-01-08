using System;

namespace WinCachebox
{
    public class Datum
  {
    public struct Vector3
    {
      public float X;
      public float Y;
      public float Z;

      public static Vector3 operator +(Vector3 v1, Vector3 v2)
      {
        Vector3 result;
        result.X = v1.X + v2.X;
        result.Y = v1.Y + v2.Y;
        result.Z = v1.Z + v2.Z;
        return result;
      }

      public static Vector3 operator -(Vector3 v1, Vector3 v2)
      {
        Vector3 result;
        result.X = v1.X - v2.X;
        result.Y = v1.Y - v2.Y;
        result.Z = v1.Z - v2.Z;
        return result;
      }

      public float Length()
      {
        return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
      }
    };

    public static Datum WGS84 = new Datum(6378137, 1.0 / 298.257223563, 0, 0, 0);

    protected double a;
    protected double b;
    protected double e;
    protected double e_;
    protected double f;
    protected double xShift;
    protected double yShift;
    protected double zShift;

    public Datum(double a, double f, double xShift, double yShift, double zShift)
    {
      this.a = a;
      this.f = f;
      this.b = a * (1 - f);
      this.xShift = xShift;
      this.yShift = yShift;
      this.zShift = zShift;

      e = Math.Sqrt((a * a - b * b) / (a * a));
      e_ = Math.Sqrt((a * a - b * b) / (b * b));
    }

    public Vector3 ToECEF(double latitude, double longitude, double elevation)
    {
      return ToECEF(this, latitude, longitude, elevation);
    }

    public static Vector3 ToECEF(Datum datum, double latitude, double longitude, double elevation)
    {
      double radLat = (Math.PI / 180) * latitude;
      double radLon = (Math.PI / 180) * longitude;

      double N = datum.radiusOfCurvature(latitude);

      Vector3 Result;
      Result.X = (float)((N + elevation) * Math.Cos(radLat) * Math.Cos(radLon) + datum.xShift);
      Result.Y = (float)((N + elevation) * Math.Cos(radLat) * Math.Sin(radLon) + datum.yShift);
      Result.Z = (float)(((datum.b * datum.b * N) / (datum.a * datum.a)) * Math.Sin(radLat) + datum.zShift);

      return Result;
    }

    public float Distance(double latFrom, double lonFrom, double latTo, double lonTo)
    {
      Vector3 diff = ToECEF(this, latFrom, lonFrom, 0) - ToECEF(this, latTo, lonTo, 0);
      return diff.Length();
    }

    public Coordinate FromECEF(Vector3 position, Datum datum)
    {
            Coordinate coord = new Coordinate
            {
                Longitude = Math.Atan2(position.Y, position.X) * 180.0 / Math.PI
            };

            double p = Math.Sqrt((double)position.X * (double)position.X + (double)position.Y * (double)position.Y);
      double theta = Math.Atan2((double)position.Z * datum.a, p * datum.b);
      double phi = Math.Atan2((double)position.Z + e_ * e_ * datum.b * Math.Pow(Math.Sin(theta), 3), p - datum.e * datum.e * datum.a * Math.Pow(Math.Cos(theta), 3));

      coord.Latitude = phi * 180.0 / Math.PI;
      coord.Elevation = p / Math.Cos(phi) - radiusOfCurvature(coord.Latitude);

      return coord;
    }

    protected double radiusOfCurvature(double latitude)
    {
      return a / Math.Sqrt(1 - e * e * Math.Pow(Math.Sin(latitude * Math.PI / 180.0), 2));
    }
  }
}
