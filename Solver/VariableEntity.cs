using System.Collections.Generic;

namespace WinCachebox.CBSolver
{
    // Speichert einen Wert in eine Variable
    public class VariableEntity : Entity
  {
    string name;
    public VariableEntity(int id, string name) : base(id)
    {
      this.name = name;
    }

    public override void GetAllEntities(List<Entity> list)
    {      
    }

    public override void ReplaceTemp(Entity source, Entity dest)
    {
    }

    public override string Berechne()
    {
      if (CBSolver.Solver.variablen.ContainsKey(name.ToLower()))
      {
        return CBSolver.Solver.variablen[name.ToLower()];
      }
      else
        return "Fehler";
    }

    public override string ToString()
    {
      return "V" + id + ":(" + name + ")";
    }

    public string Name { get { return name; } }
  }
}
