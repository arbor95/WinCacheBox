using System.Collections.Generic;

namespace WinCachebox.CBSolver
{
    public class ParameterEntity : Entity
  {
    List<Entity> liste = new List<Entity>();
    public ParameterEntity(int id)
      : base(id)
    {
    }

    public override void ReplaceTemp(Entity source, Entity dest)
    {
      for (int i = 0; i < liste.Count; i++)
      {
        Entity entity = liste[i];
        if (entity == source)
        {
          liste.RemoveAt(i);
          liste.Insert(i, dest);
        }
      }
    }

    public override void GetAllEntities(List<Entity> list)
    {
      foreach (Entity entity in liste)
        list.Add(entity);
    }

    public override string Berechne()
    {
      string result = "";
      foreach (Entity entity in liste)
      {
        result += entity.Berechne();
      }
      return result;
    }

    public string[] GetParameter()
    {
      string[] result = new string[liste.Count];
      for (int i = 0; i < liste.Count; i++)
      {
        Entity entity = liste[i];

        result[i] = entity.Berechne();
      }
      return result;
    }

    public override string ToString()
    {
      string result = "P" + id + ":(";
      foreach (Entity entity in liste)
        result += entity.ToString() + ";";
      result = result.Substring(0, result.Length - 1);
      result += ")";
      return result;
    }

    public List<Entity> Liste { get { return liste; } }
  }
}
