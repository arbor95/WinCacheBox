using System;
using System.Collections.Generic;

namespace WinCachebox.CBSolver
{
    public class FunctionEntity : Entity
  {
    private Function function;
    private Entity entity;
    public FunctionEntity(int id, Function function, Entity entity)
      : base(id)
    {
      this.function = function;
      this.entity = entity;
    }

    public override void ReplaceTemp(Entity source, Entity dest)
    {
      if (entity == source)
        entity = dest;
    }

    public override void GetAllEntities(List<Entity> list)
    {
      list.Add(entity);
    }

    public override string Berechne()
    {
      string[] str;
      if (entity is ParameterEntity)
      {
        str = (entity as ParameterEntity).GetParameter();
      }
      else
      {
        string argument = entity.Berechne();
        str = new string[1];
        str[0] = argument;
      }
      return function.Calculate(str);
    }

    public override string ToString()
    {
      return "F:" + function + "(" + entity.ToString() + ")";
    }


    private string AlphaPos(string wert)
    {
      wert = wert.Trim().ToLower();
      if (wert == "")
        return "0";
      char c = wert[0];
      int result = (int)c - (int)('a') + 1;
      return result.ToString();
    }

    private string HandyCode(string wert)
    {
      wert = wert.Trim().ToLower();
      if (wert == "")
        return "0";
      char c = wert[0];
      int i = (int)c - (int)('a') + 1;
      i -= 3;
      if (i <= 0) return "2";
      i -= 3;
      if (i <= 0) return "3";
      i -= 3;
      if (i <= 0) return "4";
        i -= 3;
      if (i <= 0) return "5";
        i -= 3;
      if (i <= 0) return "6";
      i -= 4;
      if (i <= 0) return "7";
      i -= 3;
      if (i <= 0) return "8";
      i -= 3;
      if (i <= 0) return "9";
      return "0";
    }

    private string AlphaSum(string wert)
    {
      int result = 0;
      wert = wert.ToLower();
      foreach (char c in wert)
      {
        result += (int)c - (int)('a') + 1;
      }
      return result.ToString();
    }

    private string HandySum(string wert)
    {
      int result = 0;
      wert = wert.ToLower();
      foreach (char c in wert)
      {
        int i = (int)c - (int)('a') + 1;
        if ((i < 1) || (i > 26))
          continue;   // nur Buchstaben!!!
        i -= 3;
        if (i <= 0) { result += 2; continue; }
        i -= 3;
        if (i <= 0) { result += 3; continue; }
        i -= 3;
        if (i <= 0) { result += 4; continue; }
        i -= 3;
        if (i <= 0) { result += 5; continue; }
        i -= 3;
        if (i <= 0) { result += 6; continue; }
        i -= 4;
        if (i <= 0) { result += 7; continue; }
        i -= 3;
        if (i <= 0) { result += 8; continue; }
        i -= 3;
        if (i <= 0) { result += 9; continue; }
      }
      return result.ToString();
    }

    private string Length(string wert)
    {
      return wert.Length.ToString();
    }

    private string Qs(string wert)
    {
      int result = 0;
      foreach (char c in wert)
      {
        int i = (int)c - 48;
        if ((i >= 0) && (i <= 9))
          result += i;
      }
      return result.ToString();
    }

    private string Iqs(string wert)
    {
      while (wert.Length > 1)
      {
        wert = Qs(wert);
      }
      return wert;
    }

    private string Qp(string wert)
    {
        int result = 1;
        foreach (char c in wert)
        {
            int i = (int)c - 48;
            if ((i >= 0) && (i <= 9))
                result *= i;
        }
        return result.ToString();
    }

    private string Iqp(string wert)
    {
        while (wert.Length > 1)
        {
            wert = Qp(wert);
        }
        return wert;
    }

    private string Rom2Dec(string wert)
    {
        string ziffern = "IVXLCDM";
        int[] werte = new int[] { 1, 5, 10, 50, 100, 500, 1000 };
        int result = 0;
        int i, idx0 = 0, idx1 = 0;

        wert = wert.ToUpper();

        if (wert.Length > 1)
        {
            for (i = 0; i < wert.Length - 1; i++)
            {
                idx0 = ziffern.IndexOf(wert[i + 0], 0);
                idx1 = ziffern.IndexOf(wert[i + 1], 0);

                if (idx0 < idx1)
                {
                    result -= werte[idx0];
                }
                else
                {
                    result += werte[idx0];
                }
            }
            result += werte[idx1];
        }
        else
        {
            result = werte[ziffern.IndexOf(wert[0], 0)];
        }
        return result.ToString();
    }

    public static bool IsPrimeNumber(long testNumber)
    {
      if (testNumber < 2) return false;
      if (testNumber == 2) return true;
      // 2 explizit testen, da die Schliefe an 3 startet
      if (testNumber % 2 == 0) return false;

      long upperBorder = (long)System.Math.Round(System.Math.Sqrt(testNumber), 0);
      // Alle ungeraden Zahlen bis zur Wurzel pruefen
      for (long i = 3; i <= upperBorder; i = i + 2)
        if (testNumber % i == 0) return false;
      return true;
    }
    
    private string PrimeNumber(string wert)
    {
      int number = 0;
      try
      {
        number = Convert.ToInt32(wert);
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
      int anz = 0;
      int akt = 0;
      do
      {
        akt++;
        if (IsPrimeNumber(akt))
          anz++;
      } while (anz < number);
      return akt.ToString();
    }

    private string PrimeIndex(string wert)
    {
      int number = 0;
      try
      {
        number = Convert.ToInt32(wert);
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
      if (!IsPrimeNumber(number))
        return "0";

      int anz = 0;
      int akt = 0;
      while (number >= akt)
      {
        if (IsPrimeNumber(akt))
        {
          anz++;
        }
        akt++;
      }
      return anz.ToString();
    }
  }
}
