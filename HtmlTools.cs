using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WinCachebox
{
    public class HtmlTools : IDisposable
  {
    public HtmlTools()
    {

    }

    public void Dispose()
    {
      TextBlocks.Clear();
      templates.Clear();
    }

    /// <summary>
    /// Liste mit Platzhaltern und dem Text, durch den sie ersetzt werden sollen
    /// </summary>
    public Dictionary<String, String> TextBlocks = new Dictionary<string, string>();

    Dictionary<String, String> templates = new Dictionary<string, string>();

    /// <summary>
    /// Liefert das durch template angegebene Template und ersetzt
    /// alle Vorkommnisse eines Schlüssels durch seinen Wert. Achtung:
    /// Ersetzt man einen Schlüssel durch sich selbst, so kann es hier
    /// zu Deadlocks kommen!
    /// </summary>
    /// <param name="template">Name des Templates</param>
    /// <returns>Das ersetzte Template</returns>
    public String ApplyTemplate(String template)
    {
      System.Diagnostics.Debug.Assert(templates.ContainsKey(template), "Unbekanntes Template " + template + "!");

      String result = templates[template];

      foreach (String key in TextBlocks.Keys)
      {
        //int index;
        //while ((index = result.IndexOf(key)) >= 0)
        //{
        //    String left = result.Substring(0, index);
        //    String right = result.Substring(index + key.Length);
        //    result = left + TextBlocks[key] + right;
        //}


        //this takes half the time of the solution above
        result = result.Replace(key, TextBlocks[key]);
      }

      return result;
    }

    /// <summary>
    /// Liest eine Template-Datei ein
    /// </summary>
    /// <param name="filename">Dateiname des Templates</param>
    public void ReadTemplate(String ressourceName)
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      string strRes = "Cachebox." + ressourceName;
      StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(strRes));

      String curTemplate = "Header";
      String line;

      while ((line = reader.ReadLine()) != null)
      {
        // Handelt es sich um einen HTML-Kommentar? Wenn ja,
        // neuen Template-Namen übernehmen
        if (line.StartsWith("<!-- ") && line.EndsWith(" -->"))
        {
          curTemplate = line.Substring(5, line.Length - 9);
          continue;
        }

        if (templates.ContainsKey(curTemplate))
          templates[curTemplate] += line + "\n";
        else
          templates.Add(curTemplate, line + "\n");
      }

      reader.Close();
    }

    /// <summary>
    /// Wandelt in-place bbcode in html um
    /// </summary>
    /// <param name="html">der zu konvertierende String</param>
    public static void BBCodeToHtml(ref String html)
    {
      html = html.Replace("[b]", "<b> ");
      html = html.Replace("[/b]", "</b> ");
      html = html.Replace("[i]", "<i> ");
      html = html.Replace("[/i]", "</i> ");
      html = html.Replace("[u]", "<u> ");
      html = html.Replace("[/u]", "</u> ");
    }

    /// <summary>
    /// Wandelt html in text um
    /// </summary>
    /// <param name="html">der zu konvertierende String</param>
    public static string StripHTML(string html)
    {
      try
      {
        string result;

        // Remove HTML Development formatting
        result = html.Replace("\r", string.Empty);
        result = result.Replace("\n", string.Empty);
        result = result.Replace("\t", string.Empty);
        result = System.Text.RegularExpressions.Regex.Replace(result, @"[ ]+", " ");

        // Remove the header (prepare first by clearing attributes)
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*head[^>]*>", "<head>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*[/][ ]*head[ ]*>", "</head>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "<head>.*</head>", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // remove all scripts (prepare first by clearing attributes)
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*script[^>]*>", "<script>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*[/][ ]*script[ ]*>", "</script>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<script>.*?</script>", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // remove all styles (prepare first by clearing attributes)
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*style[^>]*>", "<style>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*[/][ ]*style[ ]*>", "</style>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 "<style>.*?</style>", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // insert tabs in spaces of <td> tags
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*td[^>]*>", "\t",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // insert line breaks in places of <BR> and <LI> tags
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*br[ ]*[/]*>", "\r\n",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*li[ ]*[/]*>", "\r\n * ",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // insert line paragraphs (double line breaks) in place
        // if <P>, <DIV> <TR> <H1> tags
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*div[^>]*>", "\r\n\r\n",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*tr[^>]*>", "\r\n\r\n",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*p[^>]*>", "\r\n\r\n",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*h[0-9][^>]*>", "\r\n\r\n",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                @"<[ ]*/h[0-9][^>]*>", "\r\n\r\n",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // replace horizontal ruler with some dashes
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[ ]*hr[ ]*[/]*>", "\r\n----------\r\n",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove remaining tags like <a>, links, images,
        // comments etc - anything that's enclosed inside < >
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"<[^>]*>", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // replace special characters:
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @" ", " ",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&bull;", " * ",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&lsaquo;", "<",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&rsaquo;", ">",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&trade;", "(tm)",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&frasl;", "/",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&lt;", "<",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&gt;", ">",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&copy;", "(c)",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&reg;", "(r)",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"&(.{2,6});", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        return result.Trim();
      }
      catch
      {
        return html;
      }
    }
  }
}
