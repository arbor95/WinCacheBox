using System;
using System.Collections.Generic;

namespace WinCachebox.CBSolver
{
    public class FunctionCategories : SortedList<string, Functions>
  {
    public FunctionCategories()
    {
      Functions functions = new Functions("solverGroupText");
      functions.Add(new FunctionAlphaSum());
      functions.Add(new FunctionAlphaPos());
      functions.Add(new FunctionHandyCode());
      functions.Add(new FunctionHandySum());
      functions.Add(new FunctionLength());
      functions.Add(new FunctionReverse());
      functions.Add(new FunctionRot13());
      functions.Add(new FunctionMid());
      this.Add(functions.Name, functions);
      functions = new Functions("solverGroupNumbers");
      functions.Add(new FunctionQuersumme());
      functions.Add(new FunctionIQuersumme());
      functions.Add(new FunctionQuerprodukt());
      functions.Add(new FunctionIQuerprodukt());
      functions.Add(new FunctionRom2Dec());
      functions.Add(new FunctionPrimenumber());
      functions.Add(new FunctionPrimeIndex());
      functions.Add(new FunctionInt());
      functions.Add(new FunctionRound());
      functions.Add(new FunctionPi());
      this.Add(functions.Name, functions);
      functions = new Functions("solverGroupCoordinates");
      functions.Add(new FunctionProjection());
      functions.Add(new FunctionIntersection());
      functions.Add(new FunctionCrossbearing());
      functions.Add(new FunctionBearing());
      functions.Add(new FunctionDistance());
      this.Add(functions.Name, functions);

    }

    internal bool InsertEntities(TempEntity tEntity, EntityList entities)
    {
      foreach (Functions functions in this.Values)
      {
        if (functions.InsertEntities(tEntity, entities))
          return true;
      }
      return false;
    }

    internal bool isFunction(string s)
    {
      foreach (Functions functions in this.Values)
      {
        if (functions.isFunction(s))
          return true;
      }
      return false;
    }
  }

  public class Functions : List<Function>
  {
    private string name;
    public Functions(string name)
    {
      this.name = name;
    }
    public string Name { get { return Global.Translations.Get(name); } }
    
    internal bool InsertEntities(TempEntity tEntity, EntityList entities)
    {
      foreach (Function function in this)
      {
        if (function.InsertEntities(tEntity, entities))
          return true;
      }
      return false;
    }
    internal bool isFunction(string s)
    {
      foreach (Function function in this)
      {
        if (function.isFunction(s))
          return true;
      }
      return false;
    }
  }

  public abstract class Function
  {
    public string Name { get { return getName(); } }
    public List<string> Names = new List<string>();
    public string Description { get{ return getDescription(); } }

    public Function()
    {
    }

    public abstract string getName();
    public abstract string getDescription();
    public abstract string Calculate(string[] parameter);

    private bool checkIsFunction(string function, TempEntity tEntity, EntityList entities)
    {
      try
      {
        function = function.ToLower();
        int pos = tEntity.Text.ToLower().IndexOf(function.ToLower());
        if (pos < 0)
          return false;
        int pos1 = pos + function.Length;  // 1. #
        if (tEntity.Text[pos1] != '#')
          return false;
        if (pos1 + 1 >= tEntity.Text.Length)
          return false;
        int pos2 = tEntity.Text.ToLower().IndexOf('#', pos1 + 1);
        if (pos2 < pos1)
          return false;
        if (pos2 != tEntity.Text.Length - 1)
          return false;
        if (pos == 0)
        {
          // Insert new Entity 
          TempEntity rechts = new TempEntity(-1, tEntity.Text.Substring(pos1, pos2 - pos1 + 1));
          entities.Insert(rechts);
          FunctionEntity fEntity = new FunctionEntity(-1, this, rechts);
          string var = entities.Insert(fEntity);
          tEntity.Text = var;
          return true;
        }
        else
          return false;
      }
      catch (Exception exc)
      {
          exc.GetType(); //Warning vermeiden _ avoid warning
          return false;
      }
    }

    public bool InsertEntities(TempEntity tEntity, EntityList entities)
    {
      if (checkIsFunction(Name, tEntity, entities))
        return true;
      foreach (string name2 in Names)
      {
        if (checkIsFunction(name2, tEntity, entities))
          return true;
      }
      return false;
    }
    public bool isFunction(string s)
    {
      if (Name.ToLower() == s.ToLower())
        return true;
      foreach (string name2 in Names)
      {
        if (name2.ToLower() == s.ToLower())
          return true;
      }
      return false;
    }
  }
  // ************************************************************************
  // ********************** AlphaSum ****************************************
  // ************************************************************************
  public class FunctionAlphaSum : Function
  {
    public FunctionAlphaSum() 
    {
      Names.Add("AlphaSum");
      Names.Add("AS");
    }
    public override string getName() { return Global.Translations.Get("solverFuncAlphaSum"); }
    public override string getDescription() { return Global.Translations.Get("solverDescAlphaSum"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      int result = 0;
      if (parameter[0].Length == 0)
        return "0";
      parameter[0] = parameter[0].ToLower();
      foreach (char c in parameter[0])
      {
        if ((c >= 'a') && (c <= 'z')) 
			result += (int)c - (int)('a') + 1;
      }
      return result.ToString();
    }
  }


  // ************************************************************************
  // ********************** AlphaPos ****************************************
  // ************************************************************************
  public class FunctionAlphaPos : Function
  {
    public FunctionAlphaPos()
    {
      Names.Add("AlphaPos");
      Names.Add("AP");
    }
    public override string getName() { return Global.Translations.Get("solverFuncAlphaPos"); }
    public override string getDescription() { return Global.Translations.Get("solverDescAlphaPos"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      string wert = parameter[0].Trim().ToLower();
      if (wert == "")
        return "0";
      char c = wert[0];
      int result = (int)c - (int)('a') + 1;
      return result.ToString();

    }
  }
  // ************************************************************************
  // ********************** HandyCode ***************************************
  // ************************************************************************
  public class FunctionHandyCode : Function
  {
    public FunctionHandyCode()
    {
      Names.Add("PhoneCode");
      Names.Add("HandyCode");
      Names.Add("PC");
      Names.Add("HC");
    }
    public override string getName() { return Global.Translations.Get("solverFuncPhoneCode"); }
    public override string getDescription() { return Global.Translations.Get("solverDescPhoneCode"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      string wert = parameter[0].Trim().ToLower();
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
      i -= 4;
      if (i <= 0) return "9";
      return "0";
    }
  }

  // ************************************************************************
  // ********************** HandySum  ***************************************
  // ************************************************************************
  public class FunctionHandySum : Function
  {
    public FunctionHandySum()
    {
      Names.Add("PhoneSum");
      Names.Add("HandySum");
      Names.Add("PS");
      Names.Add("HS");
    }
    public override string getName() { return Global.Translations.Get("solverFuncPhoneSum"); }
    public override string getDescription() { return Global.Translations.Get("solverDescPhoneSum"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      int result = 0;
      string wert = parameter[0].ToLower();
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
        i -= 4;
        if (i <= 0) { result += 9; continue; }
      }
      return result.ToString();
    }
  }
  // ************************************************************************
  // ********************** Length    ***************************************
  // ************************************************************************
  public class FunctionLength : Function
  {
    public FunctionLength()
    {
      Names.Add("Length");
      Names.Add("Länge");
      Names.Add("Len");
    }
    public override string getName() { return Global.Translations.Get("solverFuncLength"); }
    public override string getDescription() { return Global.Translations.Get("solverDescLength"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      return parameter[0].Length.ToString();

    }
  }
  // ************************************************************************
  // ********************** Quersumme (CrossTotal) **************************
  // ************************************************************************
  public class FunctionQuersumme : Function
  {
    public FunctionQuersumme()
    {
      Names.Add("Crosstotal");
      Names.Add("Quersumme");
      Names.Add("CT");
      Names.Add("QS");
    }
    public override string getName() { return Global.Translations.Get("solverFuncCrosstotal"); }
    public override string getDescription() { return Global.Translations.Get("solverDescCrosstotal"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      string wert = parameter[0].Trim();
      int result = 0;
      foreach (char c in wert)
      {
        int i = (int)c - 48;
        if ((i >= 0) && (i <= 9))
          result += i;
      }
      return result.ToString();
    }
  }
  // ************************************************************************
  // ********************** Iterierte (einstellige) Quersumme (Iterated CrossTotal) **************************
  // ************************************************************************
  public class FunctionIQuersumme : Function
  {
    public FunctionIQuersumme()
    {
      Names.Add("ICrosstotal");
      Names.Add("IQuersumme");
      Names.Add("ICT");
      Names.Add("IQS");
    }
    public override string getName() { return Global.Translations.Get("solverFuncICrosstotal"); }
    public override string getDescription() { return Global.Translations.Get("solverDescICrosstotal"); }
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
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      string wert = parameter[0].Trim();
      while (wert.Length > 1)
      {
        wert = Qs(wert);
      }
      return wert;
    }
  }
  // ************************************************************************
  // ********************** Querprodukt (CrossProduct) **************************
  // ************************************************************************
  public class FunctionQuerprodukt : Function
  {
    public FunctionQuerprodukt()
    {
      Names.Add("Crossproduct");
      Names.Add("Querprodukt");
      Names.Add("CP");
      Names.Add("QP");
    }
    public override string getName() { return Global.Translations.Get("solverFuncCrossproduct"); }
    public override string getDescription() { return Global.Translations.Get("solverDescCrossprocuct"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      string wert = parameter[0].Trim();
      int result = 1;
      foreach (char c in wert)
      {
        int i = (int)c - 48;
        if ((i >= 0) && (i <= 9))
          result *= i;
      }
      return result.ToString();
    }
  }
  // ************************************************************************
  // *********** iteriertes Querprodukt (iterated CrossProduct) *************
  // ************************************************************************
  public class FunctionIQuerprodukt : Function
  {
    public FunctionIQuerprodukt()
    {
      Names.Add("ICrossproduct");
      Names.Add("IQuerprodukt");
      Names.Add("ICP");
      Names.Add("IQP");
    }
    public override string getName() { return Global.Translations.Get("solverFuncICrossproduct"); }
    public override string getDescription() { return Global.Translations.Get("solverDescICrossproduct"); }
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
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      string wert = parameter[0].Trim();
      while (wert.Length > 1)
      {
        wert = Qp(wert);
      }
      return wert;
    }
  }
  // ************************************************************************
  // ********************** Rom2Dec *****************************************
  // ************************************************************************
  public class FunctionRom2Dec : Function
  {
    public FunctionRom2Dec()
    {
      Names.Add("Rom2Dec");
    }
    public override string getName() { return Global.Translations.Get("solverFuncRom2Dec"); }
    public override string getDescription() { return Global.Translations.Get("solverDescRom2Dec"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      string wert = parameter[0].Trim();
      string ziffern = "IVXLCDM";
      int[] werte = new int[] { 1, 5, 10, 50, 100, 500, 1000 };
      int result = 0;
      int i, idx0 = 0, idx1 = 0;

      wert = wert.ToUpper();
      try
      {
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
      }
      catch (Exception)
      {
          return "Error";
      }
      return result.ToString();
    }
  }
  // ************************************************************************
  // ********************** Primenumber (Primzahl)  *************************
  // ************************************************************************
  public class FunctionPrimenumber : Function
  {
    public FunctionPrimenumber()
    {
      Names.Add("Primenumber");
      Names.Add("Primzahl");
    }
    public override string getName() { return Global.Translations.Get("solverFuncPrimenumber"); }
    public override string getDescription() { return Global.Translations.Get("solverDescPrimenumber"); }
    private bool IsPrimeNumber(long testNumber)
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
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      string wert = parameter[0].Trim();
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
  }
  // ************************************************************************
  // ********************** Primeindex (Index einer Primzahl)  **************
  // ************************************************************************
  public class FunctionPrimeIndex : Function
  {
    public FunctionPrimeIndex()
    {
      Names.Add("PrimeIndex");
      Names.Add("PrimIndex");
    }
    public override string getName() { return Global.Translations.Get("solverFuncPrimeIndex"); }
    public override string getDescription() { return Global.Translations.Get("solverDescPrimeIndex"); }
    private bool IsPrimeNumber(long testNumber)
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
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      string wert = parameter[0].Trim();
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
  // ************************************************************************
  // ********************** Projektion **************************************
  // ************************************************************************
  public class FunctionProjection : Function
  {
    public FunctionProjection()
    {
      Names.Add("Projection");
      Names.Add("Projektion");
    }
    public override string getName() { return Global.Translations.Get("solverFuncProjection"); }
    public override string getDescription() { return Global.Translations.Get("solverDescProjection"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 3)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "3");
      }
      Coordinate coord = new Coordinate(parameter[0]);
      if (!coord.Valid)
      {
        return "Parameter 1 (coord) must be a Coordinate!";
      }
      double distance;
      double angle;
      try
      {
        distance = Convert.ToDouble(parameter[1]);
      }
      catch (Exception)
      {
        return "Parameter 2 (distance) must be number!";
      }
      try
      {
        angle = Convert.ToDouble(parameter[2]);
      }
      catch (Exception)
      {
        return "Parameter 3 (angle) must be number!";
      }

      Coordinate result = Coordinate.Project(coord.Latitude, coord.Longitude, angle, distance);
      if (!result.Valid)
        return "Error: Projection";

      return result.FormatCoordinate();
    }
  }
  // ************************************************************************
  // ********************** Intersection (Schnittpunkt) *********************
  // ************************************************************************
  public class FunctionIntersection : Function
  {
    public FunctionIntersection()
    {
      Names.Add("Intersection");
      Names.Add("Schnittpunkt");
    }
    public override string getName() { return Global.Translations.Get("solverFuncIntersection"); }
    public override string getDescription() { return Global.Translations.Get("solverDescIntersection"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 4)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "4");
      }
      try
      {
        Coordinate[] coord = new Coordinate[4];
        double[] x = new double[4];
        double[] y = new double[4];
        string[] zone = new string[4];
        for (int i = 0; i < 4; i++)
        {
          coord[i] = new Coordinate(parameter[i]);
          if (!coord[i].Valid) { return "Parameter " + (i + 1).ToString() + " must be a Coordinate!"; }
        }
        return Coordinate.Intersection(coord[0], coord[1], coord[2], coord[3]).FormatCoordinate();
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
  }
  // ************************************************************************
  // ********************** Crossbearing (Kreuzpeilung) *********************
  // ************************************************************************
  public class FunctionCrossbearing : Function
  {
      public FunctionCrossbearing()
      {
          Names.Add("Crossbearing");
          Names.Add("Kreuzpeilung");
      }
      public override string getName() { return Global.Translations.Get("solverFuncCrossbearing"); }
      public override string getDescription() { return Global.Translations.Get("solverDescCrossbearing"); }
      public override string Calculate(string[] parameter)
      {
          if (parameter.Length != 4)
          {
              return Global.Translations.Get("solverErrParamCount").Replace("%s", "4");
          }
          try
          {
              Coordinate[] coord = new Coordinate[4];
              double[] angle = new double[2];
              for (int i = 0; i < 2; i++)
              {
                  coord[i] = new Coordinate(parameter[i*2]);
                  if (!coord[i].Valid) { return "Parameter " + (i*2 + 1).ToString() + " must be a Coordinate!"; }
                  // Bearing
                  try
                  {
                      angle[i] = Convert.ToDouble(parameter[i * 2 + 1]);
                  }
                  catch (Exception)
                  {
                      return "Parameter " + (i * 2 + 2).ToString() + " must be a number!";
                  }
              }

              return Coordinate.Crossbearing(coord[0], angle[0], coord[1], angle[1]).FormatCoordinate();
          }
          catch (Exception ex)
          {
              return ex.Message;
          }
      }
    }


  // ************************************************************************
  // ********************** Gearing (Peilung) *******************************
  // ************************************************************************
  public class FunctionBearing : Function
  {
    public FunctionBearing()
    {
      Names.Add("Bearing");
      Names.Add("Peilung");
    }
    public override string getName() { return Global.Translations.Get("solverFuncBearing"); }
    public override string getDescription() { return Global.Translations.Get("solverDescBearing"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 2)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "2");
      }
      try
      {
        Coordinate[] coord = new Coordinate[2];
        for (int i = 0; i < 2; i++)
        {
          coord[i] = new Coordinate(parameter[i]);
          if (!coord[i].Valid) { return "Parameter " + (i + 1).ToString() + " must be a Coordinate!"; }
        }
        double bearing = -Coordinate.Bearing(coord[0], coord[1]);
        if (bearing < 0)
          bearing = bearing + 360;
        return bearing.ToString();
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
  }

  // ************************************************************************
  // ********************** Distance (Abstand) ******************************
  // ************************************************************************
  public class FunctionDistance : Function
  {
    public FunctionDistance()
    {
      Names.Add("Distance");
      Names.Add("Abstand");
    }
    public override string getName() { return Global.Translations.Get("solverFuncDistance"); }
    public override string getDescription() { return Global.Translations.Get("solverDescDistance"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 2)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "2");
      }
      try
      {
        Coordinate[] coord = new Coordinate[2];
        for (int i = 0; i < 2; i++)
        {
          coord[i] = new Coordinate(parameter[i]);
          if (!coord[i].Valid) { return "Parameter " + (i + 1).ToString() + " must be a Coordinate!"; }
        }
        double distance = (float)Datum.WGS84.Distance(coord[0].Latitude, coord[0].Longitude, coord[1].Latitude, coord[1].Longitude);
        return distance.ToString();
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
  }
  // ************************************************************************
  // ********************** Int (Ganzzahl) **********************************
  // ************************************************************************
  public class FunctionInt : Function
  {
    public FunctionInt()
    {
      Names.Add("Int");
      Names.Add("Ganzzahl");
    }
    public override string getName() { return Global.Translations.Get("solverFuncInt"); }
    public override string getDescription() { return Global.Translations.Get("solverDescInt"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "2");
      }
      try
      {
        double number = 0;
        try
        {
          number = Convert.ToDouble(parameter[0]);
        }
        catch (Exception)
        {
          return "Parameter must be a number!";
        }
        return ((int)number).ToString();
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
  }
  // ************************************************************************
  // ********************** Round (Runden) **********************************
  // ************************************************************************
  public class FunctionRound : Function
  {
    public FunctionRound()
    {
      Names.Add("Round");
      Names.Add("Runden");
    }
    public override string getName() { return Global.Translations.Get("solverFuncRound"); }
    public override string getDescription() { return Global.Translations.Get("solverDescRound"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 2)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "2");
      }
      try
      {
        double number = 0;
        double digits = 0;
        try
        {
          number = Convert.ToDouble(parameter[0]);
        }
        catch (Exception)
        {
          return "Parameter 0 must be a number!";
        }
        try
        {
          digits = Convert.ToDouble(parameter[1]);
        }
        catch (Exception)
        {
          return "Parameter 1 must be a number!";
        }
        return Math.Round(number, (int)digits).ToString();
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
  }
  // ************************************************************************
  // ********************** Pi **********************************************
  // ************************************************************************
  public class FunctionPi : Function
  {
    public FunctionPi()
    {
      Names.Add("Pi");
    }
    public override string getName() { return Global.Translations.Get("solverFuncPi"); }
    public override string getDescription() { return Global.Translations.Get("solverDescPi"); }
    public override string Calculate(string[] parameter)
    {
      if ((parameter.Length != 1) || (parameter[0] != ""))
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "0");
      }
      try
      {
        return Math.PI.ToString();
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
  }
  // ************************************************************************
  // ********************** Reverse *****************************************
  // ************************************************************************
  public class FunctionReverse : Function
  {
    public FunctionReverse()
    {
      Names.Add("Reverse");
    }
    public override string getName() { return Global.Translations.Get("solverFuncReverse"); }
    public override string getDescription() { return Global.Translations.Get("solverDescReverse"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      string result = "";
      foreach(char c in parameter[0])
        result  = c + result;
      return result;

    }
  }
  // ************************************************************************
  // ********************** Rot13 *****************************************
  // ************************************************************************
  public class FunctionRot13 : Function
  {
    public FunctionRot13()
    {
      Names.Add("Rot13");
    }
    public override string getName() { return Global.Translations.Get("solverFuncRot13"); }
    public override string getDescription() { return Global.Translations.Get("solverDescRot13"); }
    public override string Calculate(string[] parameter)
    {
      if (parameter.Length != 1)
      {
        return Global.Translations.Get("solverErrParamCount").Replace("%s", "1");
      }
      return Global.Rot13(parameter[0]);
    }
  }
  //Rot13
  // ************************************************************************
  // ********************** Mid *********************************************
  // ************************************************************************
  public class FunctionMid : Function
  {
      public FunctionMid()
      {
          Names.Add("Mid");
      }
      public override string getName() { return Global.Translations.Get("solverFuncMid"); }
      public override string getDescription() { return Global.Translations.Get("solverDescMid"); }
      public override string Calculate(string[] parameter)
      {
          if ((parameter.Length < 2) || (parameter.Length > 3))
          {
              return Global.Translations.Get("solverErrParamCount").Replace("%s", "2-3");
          }
          string Wert = parameter[0].Trim();
          int iPos, iCount;
          try
          {
              iPos = Convert.ToInt32(parameter[1]);
          }
          catch (Exception)
          {
              return "Parameter 2 (Position) must be number!";
          }
          try
          {
              if (parameter.Length == 2)
                  iCount = 1;
              else
                  iCount = Convert.ToInt32(parameter[2]);
          }
          catch (Exception)
          {
              return "Parameter 3 (Count) must be number!";
          }
          if (iPos > Wert.Length)
          {
              return "Position must be less than length of string";
          }

          return Wert.Substring(iPos-1, iCount);
      }    
  }

}
