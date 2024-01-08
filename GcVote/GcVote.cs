using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Globalization;
using System.Xml;
using System.Data;

namespace WinCachebox.Geocaching
{
    public class GcVote
  {
    public struct RatingData
    {
      public String Waypoint;
      public int Vote;
      public float Rating;
    };

    public static RatingData GetRating(String User, String password, String Waypoint)
    {
      List<String> waypoint = new List<string>();
      waypoint.Add(Waypoint);
      List<RatingData> result = GetRating(User, password, waypoint);

      if (result == null || result.Count == 0)
        return new RatingData();
      else
        return result[0];
    }

    public static List<RatingData> GetRating(String User, String password, List<String> Waypoints)
    {
      List<RatingData> result = new List<RatingData>();

      String data = "userName=" + User + "&password=" + password + "&waypoints=";
      for (int i = 0; i < Waypoints.Count; i++)
      {
        data += Waypoints[i];
        if (i < (Waypoints.Count - 1))
          data += ",";
      }

      byte[] dataBytes = new ASCIIEncoding().GetBytes(data);

      HttpWebRequest webRequest = null;
      WebResponse webResponse = null;

      try
      {
        webRequest = (HttpWebRequest)WebRequest.Create("http://gcvote.de/getVotes.php");
        webRequest.Method = "POST";

        webRequest.UserAgent = "cachebox";
        webRequest.Timeout = 15000;
        webRequest.Proxy = Global.Proxy;
        webRequest.ContentLength = dataBytes.Length;
        webRequest.ContentType = "application/x-www-form-urlencoded";
        Stream outStream = webRequest.GetRequestStream();
        outStream.Write(dataBytes, 0, dataBytes.Length);
        outStream.Close();

        webResponse = webRequest.GetResponse();

        if (!webRequest.HaveResponse)
          return null;

        StreamReader response = new StreamReader(webResponse.GetResponseStream());

        // alles lesen
        String xmlLine = String.Empty;
        String line = null;
        while ((line = response.ReadLine()) != null)
        {
          xmlLine += line;
        }

        XmlDocument xml = new XmlDocument();
        xml.Load(new StringReader(xmlLine));


        XmlNodeList list = xml.GetElementsByTagName("vote");
        foreach (XmlNode node in list)
        {
          try
          {
                        RatingData ratingData = new RatingData
                        {
                            Rating = float.Parse(node.Attributes["voteAvg"].Value, NumberFormatInfo.InvariantInfo)
                        };
                        String userVote = node.Attributes["voteUser"].Value;
            ratingData.Vote = (userVote == "") ? 0 : (int)float.Parse(userVote, NumberFormatInfo.InvariantInfo);
            ratingData.Waypoint = node.Attributes["waypoint"].Value;
            result.Add(ratingData);
          }
          catch (Exception exc)
          {
            MessageBox.Show(exc.ToString());
          }
        }
      }
      catch (Exception)
      {
        return null;
      }
      finally
      {
        if (webResponse != null)
        {
          webResponse.Close();
          webResponse = null;
        }

        webRequest = null;
      }

      return result;
    }

    public static bool SendVotes(long Id, String Url, String GcCode, int Vote)
    {
      String guid = Url.Substring(Url.IndexOf("guid=") + 5).Trim();

      if (SetVote(Config.GetString("GcLogin"), Config.GetStringEncrypted("GcVotePassword"), guid, GcCode, Vote))
      {
        CBCommand command = Database.Data.CreateCommand("update Caches set VotePending=@votepending where Id=@id");
        command.ParametersAdd("@votepending", DbType.Boolean, false);
        command.ParametersAdd("@id", DbType.Int64, Id);
        command.ExecuteNonQuery();
        command.Dispose();
        return true;
      }
      return false;
    }

    public static bool SetVote(String User, String password, String guid, String waypoint, int Vote)
    {
      List<RatingData> result = new List<RatingData>();

      String data = "userName=" + User + "&password=" + password + "&voteUser=" + Vote.ToString() + "&cacheId=" + guid.Trim() + "&waypoint=" + waypoint;

      HttpWebRequest webRequest = null;
      WebResponse webResponse = null;

      try
      {
        webRequest = (HttpWebRequest)WebRequest.Create("http://dosensuche.de/GCVote/setVote.php?" + data);
        webRequest.UserAgent = "cachebox";
        webRequest.Timeout = 15000;
        webRequest.Proxy = Global.Proxy;
        webResponse = webRequest.GetResponse();

        if (!webRequest.HaveResponse)
          return false;

        StreamReader response = new StreamReader(webResponse.GetResponseStream());

        // alles lesen
        String line = (response != null) ? response.ReadLine() : "";

        return line == "OK";
      }
      catch (Exception exc)
      {
#if DEBUG
        Global.AddLog("GcVote.SendVote(): " + exc.ToString());
#endif
      }
      finally
      {
        if (webResponse != null)
        {
          webResponse.Close();
          webResponse = null;
        }

        webRequest = null;
      }

      return false;
    }
  }
}
