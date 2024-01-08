using System;
//using Sounds;

namespace WinCachebox.Locator
{
    public abstract class Locator : IDisposable
  {
    /// <summary>
    /// Delegate des Handlers für das LocationDataReceivedHandler-Event
    /// </summary>
    /// <param name="sender">Location-Instanz, dessen Koordinaten
    /// sich geändert haben</param>
    public delegate void LocationDataReceivedHandler(Locator sender);

    /// <summary>
    /// Ereignis, das bei empfangenen Daten generiert wird.
    /// </summary>
    public static event LocationDataReceivedHandler LocationDataReceived;

    /// <summary>
    /// Ereignis, das bei veränderter Koordinate generiert wird.
    /// </summary>
    public static event LocationDataReceivedHandler PositionChanged;

    /// <summary>
    /// Aktuelle Position des Empfängers
    /// </summary>
    public Coordinate Position = new Coordinate();

    /// <summary>
    /// Letzte gültige Position des Empfängers
    /// </summary>
    public Coordinate LastValidPosition = new Coordinate();

    /// <summary>
    /// Geschwindigkeit über Grund in km/h
    /// </summary>
    public float SpeedOverGround = 0;

    /// <summary>
    /// Kurs
    /// </summary>
    public float Heading = 0;

    /// <summary>
    /// Horizontal Dilusion of Precision
    /// </summary>
    public float HDOP = 0;

    /// <summary>
    /// true, falls das Gps geöffnet werden konnte
    /// </summary>
    public abstract bool IsGpsAvailable();

    /// <summary>
    /// Überprüft, ob das Gps schonmal reagiert hat
    /// </summary>
    /// <returns>true, falls ein GPS gefunden wurde und mit dem Treiber quatscht</returns>
    public abstract bool IsGpsResponding();

    /// <summary>
    /// Zuletzt verwendeter Connection String
    /// </summary>
    public String ConnectionString = String.Empty;

    /// <summary>
    /// Scannt nach einem GPS-Device. Setzt ConnectionString
    /// </summary>
    /// <returns>true, falls ein Device gefunden wurde, sonst false</returns>
    public abstract bool Scan();

    /// <summary>
    /// Anzahl der zur Positionsbestimmung genutzten Satelliten
    /// </summary>
    public int NumSatellites = 0;

    /// <summary>
    /// Öffnet das Gps-Device
    /// </summary>
    /// <param name="connectionData">String mit Verbindungsdaten</param>
    public abstract void Open(String connectionData);

    /// <summary>
    /// Öffnet das Gps mit dem zuletzt verwendeten ConnectionString
    /// </summary>
    /// <returns>true, falls Port geöffnet werden konnte. Sonst false.</returns>
    public abstract bool Open();

    /// <summary>
    /// Schliesst das Gps-Device
    /// </summary>
    public abstract void Close();

    public abstract void Dispose();

    /// <summary>
    /// Zuletzt an die OnPositionChanged-Handler weitergegebene Koordinate
    /// </summary>
    private Coordinate lastFired = new Coordinate();

    public bool IsNearWaypoint = false;
    public int ApproachDistance = Config.GetInt("SoundApproachDistance");

    /// <summary>
    /// Feuert die Ereignishandler
    /// </summary>
    protected virtual void OnLocationDataReceived()
    {
      if (Position.Valid)
        Global.LastValidPosition = new Coordinate(Position);

      if (LocationDataReceived != null)
        LocationDataReceived(this);

      if (lastFired.Equals(Position))
        return;

      lastFired = new Coordinate(Position);

      if (PositionChanged != null)
        PositionChanged(this);


      // 50meter near the cache or the selected waypoint
      if (!(Global.SelectedWaypoint == null))
      {
        // a waypoint is selected
        if (Global.SelectedWaypoint.Distance <= ApproachDistance)
        {
          if (!IsNearWaypoint)
          {
            IsNearWaypoint = true;
//            Sound oSound = new Sound(Global.AppPath + "\\data\\sounds\\Approach.wav");
//            oSound.Play();
          }
        }
        else
        {
          // hysterese -> set it to false just when more than 75meters away
          if (Global.SelectedWaypoint.Distance >= ApproachDistance + 25)
            IsNearWaypoint = false;
        }
      }
      else
      {
        if (!(Global.SelectedCache == null))
        {
          // no waypoint is selected, but just a cache
          if (Global.SelectedCache.Distance(true) <= ApproachDistance)
          {
            if (!IsNearWaypoint)
            {
              IsNearWaypoint = true;
//              Sound oSound = new Sound(Global.AppPath + "\\data\\sounds\\Approach.wav");
//              oSound.Play();
            }
          }
          else
          {
            // hysterese -> set it to false just when more than 75meters away
            if (Global.SelectedCache.Distance(true) >= ApproachDistance + 25)
              IsNearWaypoint = false;
          }
        }
      }
    }
  }
}
