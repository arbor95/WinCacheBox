using System;
using System.Drawing;

namespace WinCachebox.Map
{
    public class Tile : IDisposable
  {
    public enum TileState
    {
      Scheduled,
      Present,
      LowResolution,
      Disposed
    };

    public Descriptor Descriptor = null;

    public TileState State;

    /// <summary>
    /// Textur der Kachel
    /// </summary>
    public Bitmap Image = null;

    /// <summary>
    /// Frames seit dem letzten Zugriff auf die Textur
    /// </summary>
    public int Age = 0;

    public Tile(Descriptor desc, Bitmap image, TileState state)
    {
      Descriptor = desc;
      Image = image;
      State = state;
    }

    public void Dispose()
    {
      if (Image != null)
        Image.Dispose();
    }

    public override string ToString()
    {
      return State.ToString() + ", " + Descriptor.ToString();
    }
  }
}
