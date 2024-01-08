using System;
using System.Collections.Generic;

namespace WinCachebox.CBSolver
{
    public class Solver : List<SolverZeile>
  {
    // Liste mit den Operatoren, werden in dieser Reihenfolge abgearbeitet (. vor -)...
    static internal SortedList<int, List<string>> operatoren = new SortedList<int, List<string>>();
    static internal FunctionCategories functions = new FunctionCategories();   
    // hier werden die Loesungen aller Variablen gespeichert
    static internal SortedList<string, string> variablen = new SortedList<string, string>();
    static public SortedList<string, string> Variablen { get { return variablen; } }
    static public SortedList<string, int> MissingVariables = null;

    string source;
    public Solver(string source)
    {
      if (operatoren.Count == 0)
      {
        List<string> ops = new List<string>();
        ops.Add("=");
        operatoren.Add(0, ops);

        ops = new List<string>();
        ops.Add(":");
        operatoren.Add(1, ops);

        ops = new List<string>();
        ops.Add("-");
        ops.Add("+");
        operatoren.Add(2, ops);

        ops = new List<string>();
        ops.Add("/");
        ops.Add("*");
        operatoren.Add(3, ops);

        ops = new List<string>();
        ops.Add("^");
        operatoren.Add(4, ops);

      }
      this.source = source;
    }

    public bool Solve()
    {
      MissingVariables = null;
      CBSolver.Solver.variablen.Clear();
      int pos = 0;
      while (pos < source.Length)
      {
        int pos2 = source.IndexOf(Environment.NewLine, pos);
        if (pos2 < 0)
          break;
        string s = source.Substring(pos, pos2 - pos);
        int pos3 = s.IndexOf('#');
        if (pos3 > 0)   // Kommentar entfernen
          s = s.Substring(0, pos3 - 1);
        else if (pos3 == 0)
          s = "";
        this.Add(new SolverZeile(this, s));
        pos = pos2 + Environment.NewLine.Length;
      }
      // letzte Zeile auch noch einfuegen
      if (pos < source.Length)
      {
        string ss = source.Substring(pos, source.Length - pos);
        this.Add(new SolverZeile(this, ss));
      }
      if (!parseZeilen())
        return false;
      return true;
    }
    private bool parseZeilen()
    {
      foreach (SolverZeile zeile in this)
      {
        if (!zeile.Parse())
          return false;
      }
      return true;
    }
  }

  public class SolverZeile
  {
    private Solver solver;
    private string text;
    private EntityList entities = new EntityList();
    private string solution = "";
    public SolverZeile(Solver solver, string text)
    {
      this.solver = solver;
      this.text = text;
    }

    public bool Parse()
    {
      // alle Klammern herausfinden
      while (true)
      {
        // erste innerste Klammer Auf suchen
        int firstAuf = -1;
        int nextZu = -1;
        if (getFirstKlammer(out firstAuf, out nextZu))
        {
          if (nextZu <= firstAuf)
          {
            solution = "Missing )!";
            return true;
          }
          // ; suchen fuer Trennung zwischen Paramtern...
          string anweisung = text.Substring(firstAuf + 1, nextZu - firstAuf - 1);
          string var = entities.Insert(anweisung);
          text = text.Substring(0, firstAuf).Trim() + var + text.Substring(nextZu + 1, text.Length - nextZu - 1).Trim();
        }
        else
        {
          // Rest als tempvar einfuegen
          string var = entities.Insert(text);
          break;
        }
      }

      searchOperators(true);

      // Funktionsparamter mit ';' getrennt suchen
      searchFunctionParameters();

      // zuerst Strings heraussuchen
      searchStrings();
      /*
            // alle Operatoren herausfinden
            searchOperators();

            // Functionen suchen
            searchFunctions();
            */

      // Auflistungen, Variablen, Konstanten und Strings
      searchLists();

      // alle Operatoren herausfinden
      searchOperators(false);

      // Functionen suchen
      searchFunctions();
      
      // Variablen, Konstanten und Strings
      searchVariables();
      
      entities.Pack();

      // Alle Entities holen, die Bestandteil eines anderen sind
      List<Entity> list = new List<Entity>();
      foreach (Entity ent in entities.Values)
      {
        ent.GetAllEntities(list);
      }
      // diese koennen dann geloescht werden
      foreach (Entity ent in list)
      {
        entities.Remove(ent.Id);
      }
      // es sollte nur 1 Entity uebrig bleiben. Dies beinhaltet dann die komplette Formel
      if (entities.Values.Count >= 1)
      {
        solution = entities.Values[0].Berechne();
        entities.Values[0].GetAllEntities(list);
        SortedList<string, int> missingVariables = null;
        foreach (Entity tent in list)
        {
          // store Missing Variables in global List
          // store Missing Variables in local List too (only for this line)
          if (tent is TempEntity)
          {
            if ((tent as TempEntity).Text.Trim() == "")
              continue;
            // store in global list
            if (Solver.MissingVariables == null)
              Solver.MissingVariables = new SortedList<string, int>();
            if (!Solver.MissingVariables.Keys.Contains((tent as TempEntity).Text))
              Solver.MissingVariables.Add((tent as TempEntity).Text, 0);
            // Check for duplicate entries with different case sensitivity
            for (int i = 0; i < Solver.MissingVariables.Count; i++)
            {
                for (int j = i + 1; j < Solver.MissingVariables.Count; j++)
                {
                    if (Solver.MissingVariables.Keys[i].ToUpper() == Solver.MissingVariables.Keys[j].ToUpper())
                    {
                        Solver.MissingVariables.RemoveAt(j);
                    }
                }
            }
            // store in local list
            if (missingVariables == null)
              missingVariables = new SortedList<string, int>();
            if (!missingVariables.ContainsKey((tent as TempEntity).Text))
              missingVariables.Add((tent as TempEntity).Text, 0);
          }
        }
        if ((missingVariables != null) && (missingVariables.Count > 0))
        {
          solution = "missing: ";
          bool first = true;
          foreach (string s in missingVariables.Keys)
          {
            if (!first)
              solution += ", ";
            first = false;
            solution += s;
          }
        }
      }
      return true;
    }

    // sucht die erste Klammer mit der maximalen Verschachtelungstiefe
    private bool getFirstKlammer(out int firstAuf, out int nextZu)
    {
      firstAuf = -1;
      nextZu = -1;
      int max = 0;
      int tiefe = 0;
      bool found = false;
      int pos = 0;
      bool useNextZu = false;
      foreach (char c in text)
      {
        if ((c == '(') || (c == '[') || (c == '{'))
        {
          tiefe++;
          if (tiefe > max)
          {
            // dies ist bisher die erste Klammer mit der Verschachtelungstiefe tiefe
            max = tiefe;
            firstAuf = pos;
            found = true;
            useNextZu = true;
          }
        }
        else if ((c == ')') || (c == ']') || (c == '}'))
        {
          tiefe--;
          if (useNextZu)
          {
            nextZu = pos;
            useNextZu = false;
          }
        }
        pos++;
      }
      return found;
    }

    private void searchOperators(bool nurGleich)
    {
      int ie = 0;
      for (ie = 0; ie < entities.Count; ie++)
      {
        Entity entity = entities.Values[ie];
        TempEntity tEntity = entity as TempEntity;
        if (tEntity == null) continue;
        if (tEntity.Text == "") continue;
        if (tEntity.Text[0] == '"') continue;   // in String mit Anfuehrungszeichen kann kein Operator stecken!
        foreach (List<string> ops in CBSolver.Solver.operatoren.Values)
        {
          while (true)
          {
            int pos = -1;
            string op = "";
            foreach (string top in ops)
            {
              if (nurGleich && (top != "="))
                continue;
              // letztes Auftreten eines dieser Operatoren suchen
              int tpos = tEntity.Text.LastIndexOf(top);
              if ((tpos >= 0) && (tpos > pos))
              {
                pos = tpos;
                op = top;
              }
            }

            if (pos >= 1)
            {
              // OperatorEntity einfuegen
              TempEntity links = new TempEntity(-1, tEntity.Text.Substring(0, pos));
              entities.Insert(links);
              TempEntity rechts = new TempEntity(-1, tEntity.Text.Substring(pos + op.Length, tEntity.Text.Length - pos - op.Length));
              entities.Insert(rechts);
              Entity oEntity;
              if (op == "=")
                oEntity = new ZuweisungEntity(-1, links, rechts);
              else
                oEntity = new OperatorEntity(-1, links, op, rechts);
              string var = entities.Insert(oEntity);
              tEntity.Text = var;
            }
            else
              break;
          }
        }
      }
      entities.Pack();
    }
    private void searchFunctionParameters()
    {
      int ie = 0;
      for (ie = 0; ie < entities.Count; ie++)
      {
        Entity entity = entities.Values[ie];
        TempEntity tEntity = entity as TempEntity;
        if (tEntity == null) continue;
        if (tEntity.IsLinks)
          continue;
        string s = tEntity.Text.Trim();

        ParameterEntity pEntity = new ParameterEntity(entity.Id);

        while (s.Length > 0)
        {
          s = s.Trim();
          // neues Entity bis zum naechsten ";" oder bis zum Ende
          int pos = s.IndexOf(';');

          if (pos < 0)
            pos = s.Length;
          TempEntity te = new TempEntity(-1, s.Substring(0, pos));
          pEntity.Liste.Add(te);

          if (pos == s.Length)
            s = "";
          else
            s = s.Substring(pos + 1, s.Length - pos - 1);
        }
        if (pEntity.Liste.Count > 1)
        {
          // Auflistung nur erstellen, wenn mehr als 1 Eintrag!!!
          foreach (Entity eee in entities.Values)
            eee.ReplaceTemp(entities[entities.Keys[ie]], pEntity);
          entities[entities.Keys[ie]] = pEntity;

          foreach (Entity ent in pEntity.Liste)
            entities.Insert(ent);
        }

      }
      entities.Pack();
    }
    private void searchFunctions()
    {
      // functionen heraussuchen
      int ie = 0;
      for (ie = 0; ie < entities.Count; ie++)
      {
        Entity entity = entities.Values[ie];
        TempEntity tEntity = entity as TempEntity;
        if (tEntity == null) continue;
        if (tEntity.Text == "") continue;
        if (tEntity.Text[0] == '"') continue;   // in String mit Anfuehrungszeichen kann keine Funktion stecken!
        while (true)
        {
          if (!CBSolver.Solver.functions.InsertEntities(tEntity, entities))
            break;
        }
      }
      entities.Pack();
    }
    private void searchStrings()
    {
      int ie = 0;
      for (ie = 0; ie < entities.Count; ie++)
      {
        Entity entity = entities.Values[ie];
        TempEntity tEntity = entity as TempEntity;
        if (tEntity == null) continue;
        if (tEntity.IsLinks)
          continue;
        string s = tEntity.Text.Trim();

        AuflistungEntity aEntity = new AuflistungEntity(entity.Id);
        while (s.Length > 0)
        {
          s = s.Trim();
          int pos = s.IndexOf('"');
          if (pos < 0) break;
          int pos2 = s.IndexOf('"', pos + 1);
          if (pos2 < pos) break;
          // String von pos bis pos2
          if (pos > 0)
          {
            // alles vor dem ersten "" abtrennen
            TempEntity te = new TempEntity(-1, s.Substring(0, pos));
            aEntity.Liste.Add(te);
            s = s.Substring(pos, s.Length - pos);
            pos2 -= pos;
            pos = 0;
          }
          // String abtrennen
          TempEntity te2 = new TempEntity(-1, s.Substring(0, pos2 + 1));
          aEntity.Liste.Add(te2);
          s = s.Substring(pos2 + 1, s.Length - pos2 - 1);
        }
        if ((aEntity.Liste.Count > 1) || ((aEntity.Liste.Count == 1) && (s.Length > 0)))
        {
          if (s.Length > 0)
          {
            // Rest auch noch in die Auflistung aufnehmen
            TempEntity te = new TempEntity(-1, s);
            aEntity.Liste.Add(te);
          }
          // Auflistung nur erstellen, wenn mehr als 1 Eintrag!!!
          foreach (Entity eee in entities.Values)
            eee.ReplaceTemp(entities[entities.Keys[ie]], aEntity);
          entities[entities.Keys[ie]] = aEntity;

          foreach (Entity ent in aEntity.Liste)
            entities.Insert(ent);
        }
      }
      entities.Pack();
    }

    internal static bool IsOperator(string s)
    {
      foreach (List<string> olist in CBSolver.Solver.operatoren.Values)
      {
        foreach (string op in olist)
        {
          if (op == s)
            return true;
        }
      }
      return false;
    }
    internal class tmpListEntity
    {
      internal string text;
      internal bool isFunction = false;
      internal bool isOperator = false;
      internal bool isVariable = false;
      internal tmpListEntity(string text)
      {
        this.text = text;
        if (text.Length == 0)
          return;
        this.isFunction = CBSolver.Solver.functions.isFunction(text);
        this.isOperator = IsOperator(text);
        isVariable = ((text[0] == '#') && (text[text.Length - 1] == '#'));
      }
    }
    private void searchLists()
    {
      int ie = 0;
      for (ie = 0; ie < entities.Count; ie++)
      {
        Entity entity = entities.Values[ie];
        TempEntity tEntity = entity as TempEntity;
        if (tEntity == null) continue;
        if (tEntity.IsLinks)
          continue;
        string s = tEntity.Text.Trim();
        if (s == "")
          continue;
        if (s[0] == '"')
          continue;    // im String nichts trennen
        AuflistungEntity aEntity = new AuflistungEntity(entity.Id);

        List<tmpListEntity> sList = new List<tmpListEntity>();
        string tmp = "";
        bool isVariable = false;
        for (int ii = 0; ii < s.Length; ii++)
        {
          if ((s[ii] == ' ') && (!isVariable))
          {
            if (tmp.Length > 0)
              sList.Add(new tmpListEntity(tmp));
            tmp = "";
          }
          else if (s[ii] == '#')
          {
            if (!isVariable)
            {
              if (tmp.Length > 0)
                sList.Add(new tmpListEntity(tmp));
              tmp = "#";
              isVariable = true;  // hier beginnt eine interne Variable
            }
            else
            {
              isVariable = false;  // hier endet die Variable
              sList.Add(new tmpListEntity(tmp + "#"));
              tmp = "";
            }
          }
          else if (IsOperator(s[ii].ToString()))
          {
            if (tmp.Length > 0)
              sList.Add(new tmpListEntity(tmp));
            tmp = s[ii].ToString();
            sList.Add(new tmpListEntity(tmp));
            tmp = "";
          } else
            tmp += s[ii];
        }
        if (tmp.Length > 0)
          sList.Add(new tmpListEntity(tmp));
        tmp = "";
        for (int li = 0; li < sList.Count; li++)
        {
          tmpListEntity tmp1 = sList[li];
          tmp += tmp1.text;
          if (li == sList.Count - 1)
          {
            TempEntity temp = new TempEntity(-1, tmp);
            aEntity.Liste.Add(temp);
            break;
          }
          tmpListEntity tmp2 = sList[li + 1];
          bool trennen = true;
          // nicht trennen, wenn eine der beiden Seiten ein Operator ist
          if ((tmp1.isOperator) || (tmp2.isOperator))
            trennen = false;
          // nicht trennen nach funktionsnamen
          if (tmp1.isFunction)
            trennen = false;
          
          if (trennen)
          {
            TempEntity temp = new TempEntity(-1, tmp);
            aEntity.Liste.Add(temp);
            tmp = "";
          }
        }
        if (aEntity.Liste.Count > 1)
        {
          // Auflistung nur erstellen, wenn mehr als 1 Eintrag!!!
          foreach (Entity eee in entities.Values)
            eee.ReplaceTemp(entities[entities.Keys[ie]], aEntity);
          entities[entities.Keys[ie]] = aEntity;

          foreach (Entity ent in aEntity.Liste)
            entities.Insert(ent);
        }

      }
      entities.Pack();
    }
    private void searchVariables()
    {
      int ie = 0;
      for (ie = 0; ie < entities.Count; ie++)
      {
        Entity entity = entities.Values[ie];
        TempEntity tEntity = entity as TempEntity;
        if (tEntity == null) continue;
        string s = tEntity.Text.Trim();
        if (s == "") continue;
        if (s[0] == '$')
        {
          // GC-Koordinate suchen
          CoordinateEntity cEntity = new CoordinateEntity(-1, s.Substring(1, s.Length - 1));
          string var = entities.Insert(cEntity);
          tEntity.Text = var;
          s = "";
        }
        if ((s.Length >= 2) && (s[0] == '"') && (s[s.Length - 1] == '"'))
        {
          // dies ist ein String -> in StringEntity umwandeln
          StringEntity sEntity = new StringEntity(-1, s.Substring(1, s.Length - 2));
          string var = entities.Insert(sEntity);
          tEntity.Text = var;
          s = "";
        }
        if (s.Length > 0)
        {
          // evtl. als Zahl versuchen
          try
          {
            string sz = s;
            sz = sz.Replace(".", Global.DecimalSeparator);
            sz = sz.Replace(",", Global.DecimalSeparator);

            double zahl = Convert.ToDouble(sz);
            ConstantEntity cEntity = new ConstantEntity(-1, zahl);
            string var = entities.Insert(cEntity);
            tEntity.Text = var;
            s = "";
          }
          catch (Exception)
          {
            // Exception -> keine Zahl
          }
        }
        if (s.Length > 0)
        {
          // evtl. eine Variable
          if (tEntity.IsLinks)
          {
            // Variable bei Bedarf erzeugen
            if (!CBSolver.Solver.variablen.ContainsKey(s.ToLower()))
              CBSolver.Solver.variablen.Add(s.ToLower(), "");
          }
          if (CBSolver.Solver.variablen.ContainsKey(s.ToLower()))
          {
            VariableEntity vEntity = new VariableEntity(-1, s.ToLower());
            string var = entities.Insert(vEntity);
            tEntity.Text = var;
            s = "";
          }
        }

      }
      entities.Pack();
    }

    public string Solution { get { return solution; } }
  }
}
