using System;
using System.Collections.Generic;

namespace WinCachebox.CBSolver
{
    public class OperatorEntity : Entity
  {
    private Entity links;
    private Entity rechts;
    private string op;
    public OperatorEntity(int id, Entity links, string op, Entity rechts)
      : base(id)
    {
      this.links = links;
      this.op = op;
      this.rechts = rechts;
    }

    public override void ReplaceTemp(Entity source, Entity dest)
    {
      if (links == source)
        links = dest;
      if (rechts == source)
        rechts = dest;
    }

    public override void GetAllEntities(List<Entity> list)
    {
      list.Add(links);
      list.Add(rechts);
    }

    public override string Berechne()
    {
      string lLinks = links.Berechne();
      string lRechts = rechts.Berechne();
      string result = "";
      try
      {
        double dLinks = Convert.ToDouble(lLinks);
        double dRechts = Convert.ToDouble(lRechts);
        switch (op)
        {
          case "+":
            result = (dLinks + dRechts).ToString();
            break;
          case "-":
            result = (dLinks - dRechts).ToString();
            break;
          case "*":
            result = (dLinks * dRechts).ToString();
            break;
          case "/":
            result = (dLinks / dRechts).ToString();
            break;
          case ":":
            result = dLinks.ToString();
            while (result.Length < dRechts)
              result = '0' + result;
            break;
          case "^":
            result = Math.Pow(dLinks, dRechts).ToString();
            break;
        }
      }
      catch (Exception ex)
      {
        // Fehler ausgeben.
        return ex.Message;
      }
      return result;
    }
    public override string ToString()
    {
      return "O" + id + op + "(" + links + "," + rechts + ")";
    }
  }
}
