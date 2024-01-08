using System.Collections.Generic;

namespace WinCachebox.CBSolver
{
    public class ConstantEntity : Entity
  {
    double wert;
    public ConstantEntity(int id, double wert)
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
      return wert.ToString();
    }
    public override string ToString()
    {
      return "C" + id + ":(" + wert.ToString() + ")";
    }
  }
}
