using System.IO;

namespace WinCachebox.Map
{
    public class BoundingBox
  {
    public int MinX;
    public int MaxX;
    public int MinY;
    public int MaxY;
    public int Zoom;
    public long OffsetToIndex;
    public int Stride;

    public const int SizeOf = 28;

    public BoundingBox(int zoom, int minX, int maxX, int minY, int maxY, long offset)
    {
      MinX = minX;
      MaxX = maxX;
      MinY = minY;
      MaxY = maxY;
      Zoom = zoom;
      OffsetToIndex = offset;
      Stride = MaxX - MinX + 1;
    }

    public BoundingBox(BinaryReader reader)
    {
      Zoom = reader.ReadInt32();
      MinX = reader.ReadInt32();
      MaxX = reader.ReadInt32();
      MinY = reader.ReadInt32();
      MaxY = reader.ReadInt32();
      OffsetToIndex = reader.ReadInt64();
      Stride = MaxX - MinX + 1;
    }

    public int NumTilesTotal
    {
      get
      {
        return (MaxX - MinX + 1) * (MaxY - MinY + 1);
      }
    }

    public bool Contains(int x, int y)
    {
      return x <= MaxX && x >= MinX && y <= MaxY && y >= MinY;
    }

    public void Write(BinaryWriter writer)
    {
      writer.Write(Zoom);
      writer.Write(MinX);
      writer.Write(MaxX);
      writer.Write(MinY);
      writer.Write(MaxY);
      writer.Write(OffsetToIndex);
    }

    public static BoundingBox ReadInstance(BinaryReader reader)
    {
      return new BoundingBox(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt64());
    }
  }
}
