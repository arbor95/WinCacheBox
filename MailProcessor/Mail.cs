using System;
using System.Collections.Generic;
using System.IO;

namespace WinCachebox.MailProcessor
{
    public class Mail
  {
    /// <summary>
    /// Index der Mail bei dem POP3-Server
    /// </summary>
    public int Index = -1;

    /// <summary>
    /// Lookup für die Header
    /// </summary>
    Dictionary<String, String> headers = new Dictionary<string, string>();

    /// <summary>
    /// Boundary bei Multipart-Mails
    /// </summary>
    public String MimeBoundary = String.Empty;

    /// <summary>
    /// Instanziiert Mail anhand einer TOP-Anfrage
    /// </summary>
    /// <param name="index">Index der Mail auf dem POP3 Server</param>
    /// <param name="reader">Stream die Kommunikation</param>
    public Mail(int index, StreamReader reader)
    {
      Index = index;
      String lastKey = String.Empty;

      while (true)
      {
        String line = reader.ReadLine().Trim();
        if (line == ".")
          break;

        int idx = line.IndexOf(':');

        if (idx != -1)
        {
          String key = line.Substring(0, idx).ToLower();
          String value = line.Substring(idx + 1).Trim();

          if (!headers.ContainsKey(key))
            headers.Add(key, value);

          lastKey = key;
        }
        else
          if (lastKey != String.Empty)
            headers[lastKey] += line;
      }

      if (headers.ContainsKey("content-type"))
      {
        String ct = headers["content-type"];
        int startIdx = ct.IndexOf("boundary=") + 9;
        int endIdx = ct.IndexOf("\"", startIdx + 1);
        if (endIdx == -1)
          MimeBoundary = headers["content-type"].Substring(startIdx);
        else
          MimeBoundary = headers["content-type"].Substring(startIdx, endIdx - startIdx + 1);

        MimeBoundary = MimeBoundary.Replace('\"', ' ').Trim();
      }
    }

    public String From
    {
      get
      {
        if (headers.ContainsKey("from"))
          return headers["from"];

        return "";
      }
    }

    public String To
    {
      get
      {
        if (headers.ContainsKey("to"))
          return headers["to"];

        return "";
      }
    }

    public DateTime Date
    {
      get
      {
        if (headers.ContainsKey("date"))
          return DateTime.Parse(headers["date"]);

        return DateTime.MinValue;
      }
    }

    public String Subject
    {
      get
      {
        if (headers.ContainsKey("subject"))
          return headers["subject"];

        return "";
      }
    }

    public override bool Equals(object obj)
    {
      Mail a = (Mail)obj;
      return a.Index == this.Index;
    }

    public override int GetHashCode()
    {
      return headers.GetHashCode();
    }
  }
}
