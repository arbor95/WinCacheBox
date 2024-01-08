using System;
using System.Collections.Generic;
using System.Globalization;

namespace WinCachebox
{
    public class Coordinate
  {
    public bool Valid = false;

    public Coordinate()
    {
      Latitude = Longitude = Elevation = 0;
    }

    public Coordinate(Coordinate parent)
    {
      this.latitude = parent.latitude;
      this.longitude = parent.longitude;
      this.elevation = parent.elevation;
      this.Valid = parent.Valid;
    }

    public Coordinate(double latitude, double longitude)
    {
      this.Latitude = latitude;
      this.Longitude = longitude;
      this.elevation = 0;
      Valid = true;
    }

    public Coordinate(double latitude, double longitude, double elevation)
    {
      this.Latitude = latitude;
      this.Longitude = longitude;
      this.Elevation = elevation;
      Valid = true;
    }

    public bool IsValid()
    {
        if (!Valid)
            return false;
        if ((latitude == 0) && (longitude == 0))
            return false;
        return true;
    }

    // zeigt den Koordinaten-Dialog an, wo die Koordinate geändert werden kann.
    public bool Edit()
    {
      Views.Coordinates dialog = new Views.Coordinates(this);
      if ((dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) && (dialog.Coord.Valid))
      {
        this.latitude = dialog.Coord.Latitude;
        this.longitude = dialog.Coord.Longitude;
        this.elevation = dialog.Coord.Elevation;
        this.Valid = dialog.Coord.Valid;
        return true;
      }
      else
        return false;
    }
    // Parse Coordinates from String
    public Coordinate(string text)
    {
      text = text.ToUpper();
      Valid = false;

      // UTM versuche
      string[] utm = text.Trim().Split(' ');
      if (utm.Length == 3)
      {
        {
          string zone = utm[0];
          string seasting = utm[1];
          string snording = utm[2];
          try
          {
            double nording = Convert.ToDouble(snording);
            double easting = Convert.ToDouble(seasting);
            UTM.Convert convert = new UTM.Convert();
            double ddlat = 0;
            double ddlon = 0;
            convert.iUTM2LatLon(nording, easting, zone, ref ddlat, ref ddlon);
            // Ergebnis runden, da sonst Koordinaten wie 47° 60' herauskommen!
            ddlat = Math.Round(ddlat, 6);
            ddlon = Math.Round(ddlon, 6);
            this.Valid = true;
            this.latitude = ddlat;
            this.longitude = ddlon;
            return;
          } catch(Exception)
          {
          }
        }
      }


      text = text.Replace("'", "");
      text = text.Replace("\"", "");
      text = text.Replace("\r", "");
      text = text.Replace("\n", "");
      text = text.Replace("/", ""); 
      NumberFormatInfo ni = new NumberFormatInfo();
      text = text.Replace(".", Global.DecimalSeparator);
      text = text.Replace(",", Global.DecimalSeparator);
      double lat = 0;
      double lon = 0;
      int ilat = text.IndexOfAny(new char[] { Global.Translations.Get("N")[0], Global.Translations.Get("S")[0] });
      int ilon = text.IndexOfAny(new char[] { Global.Translations.Get("E")[0], Global.Translations.Get("W")[0] });
      if (ilat < 0) return;
      if (ilon < 0) return;
      if (ilat > ilon) return;
      char dlat = text[ilat];
      char dlon = text[ilon];
      string slat = "";
      string slon = "";
      if (ilat < 2)
      {
        slat = text.Substring(ilat + 1, ilon - ilat - 1).Trim().Replace("°", " ");
        slon = text.Substring(ilon + 1, text.Length - ilon - 1).Trim().Replace("°", " ");
      }
      else
      {
        slat = text.Substring(0, ilat).Trim().Replace("°", " ");
        slon = text.Substring(ilat+1, ilon - ilat - 1).Trim().Replace("°", " ");
      }

      string[] clat = slat.Split(' ');
      string[] clon = slon.Split(' ');
      List<string> llat = new List<string>();
      List<string> llon = new List<string>();
      foreach (string ss in clat)
      {
        if (ss != "")
          llat.Add(ss);
      }
      foreach (string ss in clon)
      {
        if (ss != "")
          llon.Add(ss);
      }

      try
      {
        if ((llat.Count == 1) && (llon.Count == 1))
        {
          // Decimal
          lat = Convert.ToDouble(llat[0]);
          lon = Convert.ToDouble(llon[0]);
        }
        else if ((llat.Count == 2) && (llon.Count == 2))
        {
          // Decimal Minute
          lat = Convert.ToInt32(llat[0]);
          lat += Convert.ToDouble(llat[1]) / 60;
          lon = Convert.ToInt32(llon[0]);
          lon += Convert.ToDouble(llon[1]) / 60;
        }
        else if ((llat.Count == 3) && (llon.Count == 3))
        {
          // Decimal - Minute - Second
          lat = Convert.ToInt32(llat[0]);
          lat +=(double)Convert.ToInt32(llat[1]) / 60;
          lat += Convert.ToDouble(llat[2]) / 3600;
          lon = Convert.ToInt32(llon[0]);
          lon += (double)Convert.ToInt32(llon[1]) / 60;
          lon += Convert.ToDouble(llon[2]) / 3600;
        }
      }
      catch (Exception)
      {
        Valid = false;
        return;
      }
      this.latitude = lat;
      this.longitude = lon;
      if (dlat == 'S')
        this.latitude = -this.latitude;
      if (dlon == 'W')
        this.longitude = -this.longitude;
      this.Valid = true;
      if (this.latitude > 180.00001)
        this.Valid = false;
      if (this.latitude < -180.00001)
        this.Valid = false;
      if (this.longitude > 180.00001)
        this.Valid = false;
      if (this.longitude < -180.00001)
        this.Valid = false;
    }

    private double latitude = 0;
    public double Latitude
    {
      set
      {
        latitude = value;
        if ((latitude != 0) && (longitude != 0))
          Valid = true;
      }
      get
      {
        return latitude;
      }
    }

    private double longitude = 0;
    public double Longitude
    {
      set
      {
        longitude = value;
        if ((latitude != 0) && (longitude != 0))
          Valid = true;
      }
      get
      {
        return longitude;
      }
    }

    private double elevation = 0;
    public double Elevation
    {
      set
      {
        elevation = value;
      }
      get
      {
        return elevation;
      }
    }

    /// <summary>
    /// Projiziert die übergebene Koordinate
    /// </summary>
    /// <param name="Latitude">Breitengrad</param>
    /// <param name="Longitude">Längengrad</param>
    /// <param name="Direction">Richtung</param>
    /// <param name="Distance">Distanz</param>
    /// <returns>Die projizierte Koordinate</returns>
    public static Coordinate Project(Coordinate coord, double Direction, double Distance)
    {
      return Project(coord.latitude, coord.longitude, Direction, Distance);
    }

    public static Coordinate Project(double Latitude, double Longitude, double Direction, double Distance)
    {
      // nach http://www.zwanziger.de/gc_tools_projwp.html
      Coordinate result = new Coordinate();

      // Bearing auf [0..360] begrenzen
      while (Direction > 360)
        Direction -= 360;

      while (Direction < 0)
        Direction += 360;

      double c = Distance / 6378137.0;

      if (UnitFormatter.ImperialUnits)
        c = c / 0.9144f;

      double a = (Latitude >= 0) ? (90 - Latitude) * Math.PI / 180 : Latitude * Math.PI / 180;

      double q = (360 - Direction) * Math.PI / 180.0;
      double b = Math.Acos(Math.Cos(q) * Math.Sin(a) * Math.Sin(c) + Math.Cos(a) * Math.Cos(c));

      result.latitude = 90 - (b * 180 / Math.PI);
      if (result.latitude > 90)
        result.latitude -= 180;

      double g = ((a + b) == 0) ? 0 : Math.Acos((Math.Cos(c) - Math.Cos(a) * Math.Cos(b)) / (Math.Sin(a) * Math.Sin(b)));

      if (Double.IsNaN(g))
        g = 0;

      if (Direction <= 180)
        g = -g;

      result.longitude = Longitude - g * 180 / Math.PI;

      result.Valid = true;
      return result;
    }

    public static Coordinate Intersection(Coordinate coord1, Coordinate coord2, Coordinate coord3, Coordinate coord4)
    {
      Coordinate result = null;

      double[] x = new double[4];
      double[] y = new double[4];
      x[0] = coord1.longitude;
      y[0] = coord1.latitude;
      x[1] = coord2.longitude;
      y[1] = coord2.latitude;
      x[2] = coord3.longitude;
      y[2] = coord3.latitude;
      x[3] = coord4.longitude;
      y[3] = coord4.latitude;

      // Steigungen
      double steig1 = (y[1] - y[0]) / (x[1] - x[0]);
      double steig2 = (y[3] - y[2]) / (x[3] - x[2]);
      // Nullwerte
      double null1 = y[0] - x[0] * steig1;
      double null2 = y[2] - x[2] * steig2;
      // Schnittpunkt
      double X = (null2 - null1) / (steig1 - steig2);
      double Y = steig1 * X + null1;
      // Konvertieren in Lat-Lon

      result = new Coordinate(Y, X);
      return result;
    }

    public static Coordinate Crossbearing(Coordinate coord1, double direction1, Coordinate coord2, double direction2)
    {

      double distance = (float)Datum.WGS84.Distance(coord1.Latitude, coord1.Longitude, coord2.Latitude, coord2.Longitude);
      Coordinate coord3 = Project(coord1, direction1, distance);
      Coordinate coord4 = Project(coord2, direction2, distance);

      return Intersection(coord1, coord3, coord2, coord4);
 
      /*
      double dir1rad = direction1 / 180 * Math.PI;
      double dir2rad = direction2 / 180 * Math.PI;
      double distance = (float)Datum.WGS84.Distance(coord1.Latitude, coord1.Longitude, coord2.Latitude, coord2.Longitude) / 1000;
      if (distance < 0.000001) return null;
      int maxRadius = 6378;
      double radDistance = distance / maxRadius;
      double phiAB = Bearing(coord1, coord2) / 180 * Math.PI;   // Bearing ist falsch!!!!!!!!!!
      double phiBA = Bearing(coord2, coord1) / 180 * Math.PI;
      double psi = phiAB - dir1rad / 180 * Math.PI;
      double phi = dir2rad / 180 * Math.PI - phiBA;

      double bInRad = radDistance * Math.Sin(phi) / Math.Sin(phi + psi);
      double b = bInRad * maxRadius;
      double aInRad = radDistance * Math.Sin(psi) / Math.Sin(phi + psi);
      double a = aInRad * maxRadius;
      double phiAN = phiAB - psi;
      double phiANDegrees = phiAN * 180.0 / Math.PI;
      double phiBN = phiBA + phi;
      double phiBNDegrees = phiBN * 180.0 / Math.PI;
      Coordinate result2 = Project(coord2, phiBNDegrees, a);
      Coordinate result = Project(coord1, phiANDegrees, b);
      double errorDistance = (float)Datum.WGS84.Distance(result.Latitude, result.Longitude, result2.Latitude, result2.Longitude) / 1000;
      //if the distance between the points is to large, we will restart the calculation with the new points found.
      //since the error is mostly very small these iterations are seldom used and the needed depth is very low.
      //First we will make sure, that this calculation will terminate
      if (distance < errorDistance)
      {
        return null;
      }
      if (errorDistance * 1000 > 1)
      {
        return Crossbearing(result, direction1, result2, direction2);
      }
      return result2;
      */
      /*
	private CWPoint crossbearingCalculation(CWPoint point1, CWPoint point2, double rAN, double rBN) throws Exception {
		//see german wikipedia keyword vorwaertsschnitt for the calculation.
		//peilung von a->b
		//Yes we will make an error, therefore we have to calculate the target-point iteratively.
		//Testcode for crossbearing:
		// MP="S35 47.100 W089 43.200" # MP is centre of circle, could be any waypoint
		// A=project(MP,0,1000); B=project(MP,120,1000) # Points of equilateral triangle on circle
		// C1=project(MP,240,1000); C2=cb(A,210 ,B,270)
		//	C1 "=" C2
		final int maxRadius = 6378;
    	double distance = point1.getDistance(point2);
    	if (Math.abs (distance) <= 0.0000000001){
    		err (MyLocale.getMsg(1742,"Crossbearing: distance between points to small"));
    	}
    	double distanceInRad = distance / maxRadius;
	    double phiAB = point1.getBearing(point2);
	    if (Global.getPref().solverDegMode) phiAB=phiAB / 180.0 * java.lang.Math.PI;
	    double phiBA = point2.getBearing(point1);
	    if (Global.getPref().solverDegMode) phiBA=phiBA / 180.0 * java.lang.Math.PI;

	    double psi = phiAB - rAN;
	    double phi = rBN - phiBA;

	    //calculate projetiondistance
	    double bInRad = distanceInRad * java.lang.Math.sin(phi) / java.lang.Math.sin(phi+psi);
	    double b = bInRad * maxRadius ;//* (1-flattening);
	    double aInRad = distanceInRad * java.lang.Math.sin(psi) / java.lang.Math.sin(phi+psi);
	    double a = aInRad * maxRadius ;//* (1-flattening);
	    double phiAN = phiAB - psi;
	    double phiANDegrees = phiAN * 180.0 / java.lang.Math.PI;
	    double phiBN = phiBA + phi;
	    double phiBNDegrees = phiBN * 180.0 / java.lang.Math.PI;
	    CWPoint result2 = point2.project(phiBNDegrees, a);
	    CWPoint result = point1.project(phiANDegrees, b);
	    double errorDistance = result.getDistance(result2);
	    //if the distance between the points is to large, we will restart the calculation with the new points found.
	    //since the error is mostly very small these iterations are seldom used and the needed depth is very low.
	    //First we will make sure, that this calculation will terminate
	    if (distance < errorDistance){
    		err (MyLocale.getMsg(1743,"Crossbearing calculation failed. Please inform the developers at geoclub.de"));
	    }
	    if (errorDistance * 1000 > 1){
	    	return crossbearingCalculation(result, result2, rAN, rBN);
	    }
		return result2;
	}
       */
    }

    public void Clip()
    {
      while (Longitude > 180)
        Longitude -= 180;

      while (Longitude < -180)
        Longitude += 180;
    }

    /// <summary>
    /// Berechnet den Kurs von from nach to auf einer Kugel
    /// </summary>
    /// <param name="fromLatitude"></param>
    /// <param name="fromLongitude"></param>
    /// <param name="toLatitude"></param>
    /// <param name="toLongitude"></param>
    /// <returns></returns>
    public static double Bearing(Coordinate coord1, Coordinate coord2)
    {
      return Bearing(coord1.latitude, coord1.longitude, coord2.latitude, coord2.longitude);
    }
    public static double Bearing(double fromLatitude, double fromLongitude, double toLatitude, double toLongitude)
    {
      if (fromLatitude == toLatitude && fromLongitude == toLongitude)
        return 0;

      double latFromRad = fromLatitude * Math.PI / 180.0;
      double latToRad = toLatitude * Math.PI / 180.0;
      double lonFromRad = fromLongitude * Math.PI / 180.0;
      double lonToRad = toLongitude * Math.PI / 180.0;

      double x = Math.Cos(latFromRad) * Math.Sin(latToRad) - Math.Sin(latFromRad) * Math.Cos(latToRad) * Math.Cos(lonFromRad - lonToRad);
      double y = -Math.Sin(lonFromRad - lonToRad) * Math.Cos(latToRad);

      return -(Math.Atan2(y, x) * 180.0 / Math.PI);
    }

    public override string ToString()
    {
      return string.Format("{0}; {1}; {2}", this.Latitude, this.Longitude, this.Elevation);
    }

    public string FormatCoordinate()
    {
      if (Valid)
        return Global.FormatLatitudeDM(Latitude) + " / " + Global.FormatLongitudeDM(Longitude);
      else
        return Global.Translations.Get("notValid", "not Valid");
    }
    public static String FormatDistance(float distance)
    {
      if (distance <= 500)
        return String.Format("{0:0}", distance) + "m";

      if (distance < 10000)
        return String.Format("{0:0.00}", distance / 1000) + "km";

      return String.Format("{0:0.0}", distance / 1000) + "km";
    }

    public override bool Equals(object obj)
    {
      if (this.GetType() != obj.GetType())
        return false;

      Coordinate coord = obj as Coordinate;

      return this.latitude == coord.latitude && this.longitude == coord.longitude && this.elevation == coord.elevation && this.Valid == coord.Valid;
    }

    public override int GetHashCode()
    {
      return (int)(latitude * 10000 + longitude * 10000000);
    }
  }
}
