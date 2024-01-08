using System.Collections.Generic;

namespace WinCachebox.CBSolver
{
    public class Entity
  {
    protected int id;
    protected bool isLinks;   // wird auf true, wenn dies links vom = ist.
    public Entity(int id)
    {
      this.id = id;
      isLinks = false;
    }

    // alle Vorkommen von source durch dest ersetzen, da source nur ein Verweis auf dest ist!
    public virtual void ReplaceTemp(Entity source, Entity dest)
    {
    }

    // alle Entities herausgeben, die in diesem enthalten sind
    public virtual void GetAllEntities(List<Entity> list)
    {
    }

    public virtual string Berechne()
    {
      return "";
    }

    public int Id { get { return id; } set { id = value; } }
    public bool IsLinks { get { return isLinks; } set { isLinks = value; } }
  }
}
