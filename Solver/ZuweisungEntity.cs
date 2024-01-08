using System.Collections.Generic;

namespace WinCachebox.CBSolver
{
    public class ZuweisungEntity : Entity
  {
    private Entity links;
    private Entity rechts;
    public ZuweisungEntity(int id, Entity links, Entity rechts)
      : base(id)
    {
      this.links = links;
      this.rechts = rechts;
    }

    public override void ReplaceTemp(Entity source, Entity dest)
    {
      if (links == source)
        links = dest;
      if (rechts == source)
        rechts = dest;
      links.IsLinks = true;
    }

    public override void GetAllEntities(List<Entity> list)
    {
      list.Add(links);
      list.Add(rechts);
    }

    public override string Berechne()
    {
      string lLinks = "";
      string lRechts = rechts.Berechne();

      // links muss der Name einer Variablen sein (=TempEntity)
      if (links is VariableEntity)
      {
        lLinks = (links as VariableEntity).Name.ToLower();
        // auf gueltigen Variablennamen ueberpruefen
        bool ungueltig = false;
        bool firstChar = true;
        foreach (char c in lLinks)
        {
          bool isBuchstabe = ((c >= 'a') || (c <= 'z'));
          bool isZahl = ((c >= '0') || (c <= '9'));
          if (firstChar && (!isBuchstabe))
            ungueltig = true;
          if (!(isBuchstabe || isZahl))
            ungueltig = true;
          firstChar = false;
        }
        if (ungueltig)
          return "Fehler";
        // lLinks ist gueltiger Variablenname
        if (!CBSolver.Solver.variablen.ContainsKey(lLinks))
        {
          // neue Variable hinzfuegen
          CBSolver.Solver.variablen.Add(lLinks, lRechts);
        }
        else
        {
          // Variable aendern
          CBSolver.Solver.variablen[lLinks] = lRechts;
        }
        return lRechts;
      }
      else if (links is CoordinateEntity)
      {
        return (links as CoordinateEntity).SetCoordinate(lRechts);
      } 
      else
        return "Fehler";
    }

    public override string ToString()
    {
      return "Z" + id + "(" + links + "," + rechts + ")";
    }
  }
}
