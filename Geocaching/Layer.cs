using System;
using System.IO;
using System.Net;

namespace WinCachebox.Map
{
    public class Layer
  {
    public String Name = String.Empty;

    public String FriendlyName = String.Empty;

    public String Url = String.Empty;

    public Layer(String name, String friendlyName, String url)
    {
      Name = name;
      FriendlyName = friendlyName;
      Url = url;
    }

    public bool DownloadTile(Descriptor desc)
    {
      return DownloadFile(GetUrl(desc), GetLocalFilename(desc));
    }

    public static bool DownloadFile(String Url, String Filename)
    {
      String path = Filename.Substring(0, Filename.LastIndexOf("\\"));

      // Verzeichnis anlegen
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);

      // Kachel laden
      HttpWebRequest webRequest = null;
      WebResponse webResponse = null;
      Stream stream = null;
      Stream responseStream = null;

      try
      {
        webRequest = (HttpWebRequest)WebRequest.Create(Url);
        webRequest.Timeout = 15000;
        webResponse = webRequest.GetResponse();
        webRequest.Proxy = Global.Proxy;

        if (!webRequest.HaveResponse)
          return false;

        responseStream = webResponse.GetResponseStream();
        byte[] result = Global.ReadFully(responseStream, 64000);

        // Datei schreiben
        stream = new FileStream(Filename, FileMode.Create);
        stream.Write(result, 0, result.Length);
      }
      catch (Exception)
      {
        //System.Windows.Forms.MessageBox.Show(exc.ToString());
        return false;
      }
      finally
      {
        if (stream != null)
        {
          stream.Close();
          stream = null;
        }

        if (responseStream != null)
        {
          responseStream.Close();
          responseStream = null;
        }

        if (webResponse != null)
        {
          webResponse.Close();
          webResponse = null;
        }


        if (webRequest != null)
        {
          webRequest.Abort();
          webRequest = null;
        }
        GC.Collect();
      }
      return true;
    }

    public string GetUrl(Descriptor desc)
    {
      return Url + desc.Zoom.ToString() + "/" + desc.X.ToString() + "/" + desc.Y.ToString() + ".png";
    }

    public string GetLocalFilename(Descriptor desc)
    {
      return GetLocalPath(desc) + "\\" + desc.Y.ToString() + ".png";
    }

    public string GetLocalPath(Descriptor desc)
    {
      return Config.GetString("TileCacheFolder") + "\\" + Name + "\\" + desc.Zoom.ToString() + "\\" + desc.X.ToString();
    }
  }
}
