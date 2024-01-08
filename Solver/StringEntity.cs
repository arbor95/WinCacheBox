using System.Collections.Generic;

namespace WinCachebox.CBSolver
{
    public class StringEntity : Entity
  {
    string wert;
    public StringEntity(int id, string wert)
      : base(id)
    {
      this.wert = wert;
    }

    public override void GetAllEntities(List<Entity> list)
    {
    }

    public override void ReplaceTemp(Entity source, Entity dest)
    {
    }

    public override string Berechne()
    {
      return wert;
    }
    public override string ToString()
    {
      return "S:" + id + ":(" + wert + ")";
    }
  }
}
