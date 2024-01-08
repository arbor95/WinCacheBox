using System;
using System.Collections.Generic;

namespace WinCachebox.CBSolver
{
    // wird temporaer waehrend der Auswertung benoetigt.
    // speichert einen string
    // und zusaetzlich noch eine Liste mit bereits ausgewerteten Entities
    public class TempEntity : Entity
  {
    private string text;
    SortedList<int, Entity> entities = new SortedList<int, Entity>();

    public TempEntity(int id, string text)
      : base(id)
    {
      this.text = text.Trim();
    }

    public override void ReplaceTemp(Entity source, Entity dest)
    {
      this.Text = this.Text.Replace("#" + source.Id + "#", "#" + dest.Id + "#");
    }

    public override string ToString()
    {
      return "T" + id + "(" + text + ")";
    }

    public override string Berechne()
    {
      // dies kann eine Zahl, ein String oder eine Variable sein!
      text = text.Trim();
      try
      {
        double zahl = Convert.ToDouble(text);
        return zahl.ToString();
      }
      catch (Exception)
      {
        // Exception -> keine Zahl
      }
      if ((text.Length >= 2) && (text[0] == '"') && (text[text.Length - 1] == '"'))
        return text.Substring(1, text.Length - 2);
      // text ist keine Zahl und kein String

      // dies ist eine Variable, es koenne aber auch mehrere Variablen und Texte hintereinander stehen
      // -> deren Wert ausgeben
      string[] ss = text.Split(' ');
      string result = "";
      foreach (string s in ss)
      {
        if (CBSolver.Solver.variablen.ContainsKey(s.Trim().ToLower()))
        {
          result += CBSolver.Solver.variablen[s.Trim().ToLower()];
        }
        else
          result += s;
      }
      return result;
    }
    public string Text { get { return text; } set { text = value; } }
  }

  public class EntityList : SortedList<int, Entity>
  {
    public string Insert(string anweisung)
    {
      int id = this.Count;
      this.Add(id, new TempEntity(id, anweisung));
      return "#" + id.ToString() + "#";
    }
    public string Insert(Entity entity)
    {
      int id = this.Keys[this.Count - 1] + 1;
      entity.Id = id;
      this.Add(id, entity);
      return "#" + id.ToString() + "#";
    }

    // anhand dem Text #3# das Entity nr. 3 zurueckliefern.
    // Wenn der Text nicht diesem Kriterium entspricht -> null
    private Entity getEntity(string var)
    {
      var = var.Trim(' ');
      if (var.Length < 3) return null;
      if (var[0] != '#') return null;
      if (var[var.Length - 1] != '#') return null;
      string sNr = var.Substring(1, var.Length - 2);
      try
      {
        int id = Convert.ToInt32(sNr);
        return this[id];
      }
      catch (Exception)
      {
        return null;
      }
    }

    public void Pack()
    {
      // alle TempEntities herausloeschen, die nur einen Verweis auf ein anderes TempEntity haben
      int ie = 0;
      for (ie = 0; ie < this.Count; ie++)
      {
        Entity entity = this.Values[ie];
        TempEntity tEntity = entity as TempEntity;
        if (tEntity == null) continue;
        Entity inhalt = getEntity(tEntity.Text);
        if (inhalt != null)
        {
          // dieses Entity loeschen und alle Verweise auf dieses Entity umleiten auf inhalt
          foreach (Entity ee in this.Values)
          {
            ee.ReplaceTemp(entity, inhalt);
            /*            TempEntity tee = ee as TempEntity;
                        if (tee == null) continue;
                        tee.Text = tee.Text.Replace("#" + tEntity.Id + "#", "#" + inhalt.Id + "#");*/
          }
          this.Remove(entity.Id);
          ie--;
        }
      }
    }
  }
}
