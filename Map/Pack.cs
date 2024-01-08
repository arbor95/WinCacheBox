using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace WinCachebox.Map
{
    public class Pack : IComparable
  {
    public static bool Cancel = false;

    public delegate void ProgressChanged(String message, int count, int total);
    public event ProgressChanged OnProgressChanged;

    public Layer Layer = null;

    /// <summary>
    /// true, falls dieses Pack mit OSM-Karten überlagert werden soll
    /// </summary>
    public bool IsOverlay = false;

    /// <summary>
    /// Maximales Alter einer der enthaltenen Kacheln
    /// </summary>
    public DateTime MaxAge = DateTime.MinValue;

    /// <summary>
    /// Filename des Map Packs
    /// </summary>
    public String Filename = String.Empty;

    public List<BoundingBox> BoundingBoxes = new List<BoundingBox>();

    /// <summary>
    /// Überprüft, ob der Descriptor in diesem Map Pack enthalten ist und liefert
    /// die BoundingBox, falls dies der Fall ist, bzw. null
    /// </summary>
    /// <param name="desc">Deskriptor, dessen </param>
    /// <returns></returns>
    public BoundingBox Contains(Descriptor desc)
    {
      foreach (BoundingBox bbox in BoundingBoxes)
        if (bbox.Zoom == desc.Zoom && desc.X <= bbox.MaxX && desc.X >= bbox.MinX && desc.Y <= bbox.MaxY && desc.Y >= bbox.MinY)
          return bbox;

      return null;
    }

    public Pack(Layer layer)
    {
      this.Layer = layer;
    }

    public Pack(Manager manager, String file)
    {
      Filename = file;

      Stream stream = new FileStream(file, FileMode.Open);
      BinaryReader reader = new BinaryReader(stream);

      String layerName = readString(reader, 32);
      String friendlyName = readString(reader, 128);
      String url = readString(reader, 256);
      Layer = manager.GetLayerByName(layerName, friendlyName, url);

      long ticks = reader.ReadInt64();
      MaxAge = new DateTime(ticks);

      int numBoundingBoxes = reader.ReadInt32();
      for (int i = 0; i < numBoundingBoxes; i++)
        BoundingBoxes.Add(new BoundingBox(reader));

      reader.Close();
      stream.Close();
    }


    public int NumTilesTotal
    {
      get
      {
        int result = 0;
        foreach (BoundingBox bbox in BoundingBoxes)
          result += bbox.NumTilesTotal;

        return result;
      }
    }

    public void GeneratePack(String filename, DateTime maxAge, int minZoom, int maxZoom, double minLat, double maxLat, double minLon, double maxLon)
    {
      MaxAge = maxAge;
      Filename = filename;

      CreateBoudingBoxesFromBounds(minZoom, maxZoom, minLat, maxLat, minLon, maxLon);
      FileStream stream = new FileStream(filename, FileMode.Create);
      BinaryWriter writer = new BinaryWriter(stream);
      Write(writer);
      writer.Flush();
      writer.Close();

      if (Cancel)
        File.Delete(filename);
    }

    public void CreateBoudingBoxesFromBounds(int minZoom, int maxZoom, double minLat, double maxLat, double minLon, double maxLon)
    {
      for (int zoom = minZoom; zoom <= maxZoom; zoom++)
      {
        int minX = (int)Descriptor.LongitudeToTileX(zoom, minLon);
        int maxX = (int)Descriptor.LongitudeToTileX(zoom, maxLon);

        int minY = (int)Descriptor.LatitudeToTileY(zoom, maxLat);
        int maxY = (int)Descriptor.LatitudeToTileY(zoom, minLat);

        BoundingBoxes.Add(new BoundingBox(zoom, minX, maxX, minY, maxY, 0));
      }
    }

    public delegate void ProgressDelegate(String msg, int zoom, int x, int y, int num, int total);

    void writeString(String text, BinaryWriter writer, int length)
    {
      if (text.Length > length)
        text = text.Substring(0, length);
      else
        while (text.Length < length)
          text += " ";

      System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
      byte[] asciiBytes = enc.GetBytes(text);

      for (int i = 0; i < length; i++)
        writer.Write(asciiBytes[i]);
    }

    String readString(BinaryReader reader, int length)
    {
      byte[] asciiBytes = new byte[length];
      for (int i = 0; i < length; i++)
        asciiBytes[i] = reader.ReadByte();

      System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
      return enc.GetString(asciiBytes, 0, asciiBytes.Length).Trim();
    }


    /// <summary>
    /// Speichert ein im lokalen Dateisystem vorliegendes Pack in den writer
    /// </summary>
    /// <param name="writer"></param>
    public void Write(BinaryWriter writer)
    {
      int numTilesTotal = NumTilesTotal;

      // Header
      writeString(Layer.Name, writer, 32);
      writeString(Layer.FriendlyName, writer, 128);
      writeString(Layer.Url, writer, 256);
      writer.Write(MaxAge.Ticks);
      writer.Write(BoundingBoxes.Count);

      // Offsets berechnen
      long offset = 32 + 128 + 256 + 8 + 4 + 8 + BoundingBoxes.Count * BoundingBox.SizeOf;
      for (int i = 0; i < BoundingBoxes.Count; i++)
      {
        BoundingBoxes[i].OffsetToIndex = offset;
        offset += BoundingBoxes[i].NumTilesTotal * 8;
      }

      // Bounding Boxes schreiben
      for (int i = 0; i < BoundingBoxes.Count; i++)
        BoundingBoxes[i].Write(writer);

      // Indexe erzeugen
      int cnt = 0;
      for (int i = 0; i < BoundingBoxes.Count; i++)
      {
        BoundingBox bbox = BoundingBoxes[i];

        for (int y = bbox.MinY; y <= bbox.MaxY && !Cancel; y++)
        {
          for (int x = bbox.MinX; x <= bbox.MaxX && !Cancel; x++)
          {
            // Offset zum Bild absaven
            writer.Write(offset);

            // Dateigröße ermitteln
            String local = Layer.GetLocalFilename(new Descriptor(x, y, bbox.Zoom));

            if (File.Exists(local))
            {
              FileInfo info = new FileInfo(local);
              if (info.CreationTime < MaxAge)
                Layer.DownloadTile(new Descriptor(x, y, bbox.Zoom));
            }
            else
              Layer.DownloadTile(new Descriptor(x, y, bbox.Zoom));

            // Nicht vorhandene Tiles haben die Länge 0
            if (!File.Exists(local))
              offset += 0;
            else
            {
              FileInfo info = new FileInfo(local);
              offset += info.Length;
            }



            if (OnProgressChanged != null)
              OnProgressChanged("Building index...", cnt++, numTilesTotal);
          }
        }
      }

      // Zur Längenberechnung
      writer.Write(offset);

      // So, und nun kopieren wir noch den Mist rein
      cnt = 0;
      for (int i = 0; i < BoundingBoxes.Count && !Cancel; i++)
      {
        BoundingBox bbox = BoundingBoxes[i];

        for (int y = bbox.MinY; y <= bbox.MaxY && !Cancel; y++)
        {
          for (int x = bbox.MinX; x <= bbox.MaxX && !Cancel; x++)
          {
            String local = Layer.GetLocalFilename(new Descriptor(x, y, bbox.Zoom));

            Stream imageStream = null;

            if (!File.Exists(local) || File.GetCreationTime(local) < MaxAge)
              if (!Layer.DownloadTile(new Descriptor(x, y, bbox.Zoom)))
                continue;

            imageStream = new FileStream(local, FileMode.Open);

            byte[] temp = Global.ReadFully(imageStream, 32000);
            writer.Write(temp);

            if (OnProgressChanged != null)
              OnProgressChanged("Linking package...", cnt++, numTilesTotal);
          }
        }
      }
    }

    public int CompareTo(object obj)
    {
      Pack cmp = obj as Pack;
      if (this.MaxAge < cmp.MaxAge)
        return -1;

      if (this.MaxAge > cmp.MaxAge)
        return 1;

      return 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bbox">Bounding Box</param>
    /// <param name="desc">Descriptor</param>
    /// <returns>Bitmap der Kachel</returns>
    public Bitmap LoadFromBoundingBox(BoundingBox bbox, Descriptor desc)
    {
      try
      {
        System.Diagnostics.Debug.Assert(bbox.Zoom == desc.Zoom);

        int index = (desc.Y - bbox.MinY) * bbox.Stride + (desc.X - bbox.MinX) - 1;
        long offset = bbox.OffsetToIndex + index * 8;

        Stream stream = new FileStream(Filename, FileMode.Open, FileAccess.Read);
        stream.Seek(offset, SeekOrigin.Begin);

        BinaryReader reader = new BinaryReader(stream);

        long tileOffset = reader.ReadInt64();
        long nextOffset = reader.ReadInt64();
        long length = nextOffset - tileOffset;

        if (length == 0)
          return null;

        stream.Seek(tileOffset, SeekOrigin.Begin);
        byte[] buffer = new byte[length];
        stream.Read(buffer, 0, (int)length);
        MemoryStream memStream = new MemoryStream(buffer);
        Bitmap result2 = new Bitmap(memStream);
        // Bitmap noch mal Kopieren, da sonst bei Google Maps Karten ein Fehler wegen Indexed Bitmap... aufgetreten ist.?.
        Bitmap result = new Bitmap(result2);
        result2.Dispose();
        stream.Close();
        memStream.Close();

        return result;
      }
      catch (OutOfMemoryException exc)
      {
#if DEBUG
        Global.AddLog("Pack.LoadFromBoundingBox: Out of memory!" + exc.ToString());
        Global.AddMemoryLog();
#endif
      }
      catch (Exception exc)
      {
#if DEBUG
        Global.AddLog("Pack.LoadFromBoundingBox(" + bbox.ToString() + ", " + desc.ToString() + "): " + exc.ToString());
#endif
      }

      return null;
    }
  }
}
