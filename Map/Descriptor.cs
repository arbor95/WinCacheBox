using System;

namespace WinCachebox.Map
{
    public struct PointD
  {
    /// <summary>
    /// X
    /// </summary>
    public double X;

    /// <summary>
    /// Y
    /// </summary>
    public double Y;

    /// <summary>
    /// Standardkonstruktor
    /// </summary>
    /// <param name="x">X</param>
    /// <param name="y">Y</param>
    public PointD(double x, double y)
    {
      this.X = x;
      this.Y = y;
    }
  };

  public class Descriptor : IComparable
  {
    public static int[] TilesPerLine = null;
    public static int[] TilesPerColumn = null;
    static int[] tileOffset = null;

    public static void Init()
    {
      int maxZoom = 25;

      TilesPerLine = new int[maxZoom];
      TilesPerColumn = new int[maxZoom];
      tileOffset = new int[maxZoom];

      tileOffset[0] = 0;

      for (int i = 0; i < maxZoom - 1; i++)
      {
        TilesPerLine[i] = (int)(2 * Math.Pow(2, i));
        TilesPerColumn[i] = (int)Math.Pow(2, i);
        tileOffset[i + 1] = tileOffset[i] + (int)(TilesPerLine[i] * TilesPerColumn[i]);
      }
    }

    /// <summary>
    /// X-Koordinate der Kachel
    /// </summary>
    public int X;

    /// <summary>
    /// Y-Koordinate der Kachel
    /// </summary>
    public int Y;

    /// <summary>
    /// Zoom-Stufe der Kachel
    /// </summary>
    public int Zoom;

    /// <summary>
    /// Erzeugt einen neuen Deskriptor mit den übergebenen Parametern
    /// </summary>
    /// <param name="x">X</param>
    /// <param name="y">Y</param>
    /// <param name="zoom">Zoom-Stufe</param>
    public Descriptor(int x, int y, int zoom)
    {
      X = x;
      Y = y;
      Zoom = zoom;
    }

    /// <summary>
    /// Copy-Konstruktor
    /// </summary>
    /// <param name="original">Zu klonende Instanz</param>
    public Descriptor(Descriptor original)
    {
      this.X = original.X;
      this.Y = original.Y;
      this.Zoom = original.Zoom;
    }

    /// <summary>
    /// Erzeugt einen neuen Deskriptor mit anderer Zoom-Stufe
    /// </summary>
    /// <param name="newZoomLevel"></param>
    /// <returns></returns>
    public Descriptor AdjustZoom(int newZoomLevel)
    {
      int zoomDiff = newZoomLevel - Zoom;
      int pow = (int)Math.Pow(2, Math.Abs(zoomDiff));

      return new Descriptor(X * pow, Y * pow, newZoomLevel);
    }

    /// <summary>
    /// Berechnet aus dem übergebenen Längengrad die X-Koordinate im
    /// OSM-Koordinatensystem der gewünschten Zoom-Stufe
    /// </summary>
    /// <param name="zoom">Zoom-Stufe, in der die Koordinaten ausgedrückt werden sollen</param>
    /// <param name="longitude">Longitude</param>
    /// <returns></returns>
    public static double LongitudeToTileX(int zoom, double longitude)
    {
      return (longitude + 180.0) / 360.0 * Math.Pow(2, zoom);
    }

    /// <summary>
    /// Berechnet aus dem übergebenen Breitengrad die Y-Koordinate im
    /// OSM-Koordinatensystem der gewünschten Zoom-Stufe
    /// </summary>
    /// <param name="zoom">Zoom-Stufe, in der die Koordinaten ausgedrückt werden sollen</param>
    /// <param name="longitude">Latitude</param>
    /// <returns></returns>
    public static double LatitudeToTileY(int zoom, double latitude)
    {
      double latRad = latitude * Math.PI / 180.0;

      return (1 - Math.Log(Math.Tan(latRad) + (1.0 / Math.Cos(latRad))) / Math.PI) / 2 * Math.Pow(2, zoom);
    }

    /// <summary>
    /// Berechnet aus der übergebenen OSM-X-Koordinate den entsprechenden
    /// Längengrad
    /// </summary>
    /// <param name="zoom">OSM-Zoom-Stufe</param>
    /// <param name="x">OSM-X-Koordinate</param>
    /// <returns>Longitude / Längengrad</returns>
    public static double TileXToLongitude(int zoom, double x)
    {
      // x = (longitude + 180.0) / 360.0 * Math.Pow(2, zoom);
      // x * 360 * 2^zoom 
      return -180.0 + (360.0 * x) / Math.Pow(2, zoom);
    }

    /// <summary>
    /// Berechnet aus der übergebenen OSM-Y-Koordinate den entsprechenden
    /// Breitengrad
    /// </summary>
    /// <param name="zoom">OSM-Zoom-Stufe</param>
    /// <param name="y">OSM-Y-Koordinate</param>
    /// <returns>Latitude / Breitengrad</returns>
    public static double TileYToLatitude(int zoom, double y)
    {
      double xNom = Math.Exp(2 * Math.PI) - Math.Exp(4 * Math.PI * Math.Pow(2, -zoom) * y);
      double xDen = Math.Exp(2 * Math.PI) + Math.Exp(4 * Math.PI * Math.Pow(2, -zoom) * y);

      double yNom = 2 * Math.Exp(-Math.PI * (-1 + Math.Pow(2, 1 - zoom) * y));
      double yDen = Math.Exp(-2 * Math.PI * (-1 + Math.Pow(2, 1 - zoom) * y)) + 1;

      return Math.Atan2(xNom / xDen, yNom / yDen) * 180.0 / Math.PI;
    }

    /// <summary>
    /// Berechnet die Pixel-Koordinaten auf dem Bildschirm. Es wird auf die
    /// Kachelecke oben links noch ein Offset addiert. Will man also die
    /// Koordinaten der Ecke unten links haben, übergibt man xOffset=0,yOffset=1
    /// </summary>
    /// <param name="xOffset"></param>
    /// <param name="yOffset"></param>
    /// <returns>Pixelkoordinaten </returns>
    public PointD ToWorld(int xOffset, int yOffset, int desiredZoom)
    {
      double adjust = Math.Pow(2, (desiredZoom - Zoom));
      return new PointD((X + xOffset) * adjust * 256, (Y + yOffset) * adjust * 256);
    }

    public static PointD ToWorld(double X, double Y, int zoom, int desiredZoom)
    {
      double adjust = Math.Pow(2, (desiredZoom - zoom));
      return new PointD(X * adjust * 256, Y * adjust * 256);
    }

    public override bool Equals(object obj)
    {
      Descriptor desc = (Descriptor)obj;
      return (desc.X == this.X) && (desc.Y == this.Y) && (desc.Zoom == this.Zoom);
    }

    public override int GetHashCode()
    {
      return (tileOffset[Zoom] + TilesPerLine[Zoom] * Y + X);
    }

    public override string ToString()
    {
      return "X = " + X.ToString() + ", Y = " + Y.ToString() + ", Zoom = " + Zoom.ToString();
    }

    public int CompareTo(object obj)
    {
      int hashcode = this.GetHashCode();
      int objHashcode = ((Descriptor)obj).GetHashCode();

      if (hashcode == objHashcode)
        return 0;

      if (hashcode < objHashcode)
        return -1;

      return 1;
    }
  }
}
