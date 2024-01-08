using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
//using Cachebox.Views;

namespace WinCachebox
{
    public class Config
  {
    //static DataSet dataSet = null;
    //static SqlCeDataAdapter adapter = null;

    static Dictionary<String, String> keyLookup = new Dictionary<string, string>();

    static bool initialized = false;

    public static bool ExistsConfig()
    {
        String filename = Global.AppPath + Global.configFileName;
      return File.Exists(filename);
    }

    static void checkInitialization()
    {
      if (initialized)
        return;
      if (Form1.MainForm == null)
          return;

      initialized = true;

      String filename = Global.AppPath + Global.configFileName;

      if (File.Exists(Global.AppPath + Global.configFileName))
        try
        {
            TextReader reader = new StreamReader(Global.AppPath + Global.configFileName);

          String line;
          while ((line = reader.ReadLine()) != null)
          {
            int idx = line.IndexOf('=');
            if (idx < 0)
            {
#if DEBUG
              Global.AddLog("Config.checkInitialization: malformed config line: '" + line + "'");
#endif
              continue;
            }

            String key = line.Substring(0, idx);
            String value = line.Substring(idx + 1);
            if (!keyLookup.ContainsKey(key))
              keyLookup.Add(key, value);
            else
              keyLookup[key] = value;
          }

          reader.Close();
        }
        catch (Exception exc)
        {
#if DEBUG
          Global.AddLog("Config.checkInitialization: " + exc.ToString());
#endif
        }

      validateDefaultConfigFile();
    }

    public static void validateDefaultConfigFile()
    {
            validateSetting("SelectedLanguage", "en");
            validateSetting("DatabasePath", Global.AppPath + "\\cachebox.db3");
      validateSetting("TileCacheFolder", Global.AppPath + "\\Cache");
      validateSetting("PocketQueryFolder", Global.AppPath + "\\PocketQuery");
      validateSetting("DescriptionImageFolder", Global.AppPath + "\\Repository\\Images");
      validateSetting("MapPackFolder", Global.AppPath + "\\Repository\\Maps");
      validateSetting("SpoilerFolder", Global.AppPath + "\\Repository\\Spoilers");
      validateSetting("UserImageFolder", Global.AppPath + "\\User\\Media");
      validateSetting("TrackFolder", Global.AppPath + "\\User\\Tracks");
      validateSetting("FieldNotesHtmlPath", Global.AppPath + "\\User\\fieldnotes.html");
      validateSetting("FieldNotesGarminPath", Global.AppPath + "\\User\\geocache_visits.txt");
      validateSetting("GPXExportPath", Global.AppPath + "\\User\\cachebox_export.gpx");
      validateSetting("CacheWolfPath", Global.AppPath + "\\CacheWolf");
      validateSetting("SaveFieldNotesHtml", "true");

      validateSetting("Proxy", "");
      validateSetting("ProxyPort", "");
      validateSetting("ProxyUserName", "");
      validateSetting("ProxyPassword", "");
      validateSetting("ProxyDomain", "");
      validateSetting("DopMin", "0.2");
        validateSetting("DopWidth", "1");
        validateSetting("OsmDpiAwareRendering", "true");
        validateSetting("LogMaxMonthAge", "99999");
        validateSetting("LogMinCount", "99999");
        validateSetting("MapInitLatitude", "-1000");
        validateSetting("MapInitLongitude", "-1000");
        validateSetting("AllowInternetAccess", "true");
        validateSetting("AllowRouteInternet", "true");
        validateSetting("ImportGpx", "true");
        validateSetting("CacheMapData", "false");
        validateSetting("CacheImageData", "false");
        validateSetting("OsmMinLevel", "8");
        validateSetting("OsmMaxImportLevel", "16");
        validateSetting("OsmMaxLevel", "17");
        validateSetting("OsmCoverage", "1000");
        validateSetting("SuppressPowerSaving", "true");
        validateSetting("PlaySounds", "true");
        validateSetting("PopSkipOutdatedGpx", "true");
        validateSetting("MapHideMyFinds", "false");
        validateSetting("MapShowRating", "true");
        validateSetting("MapShowDT", "true");
        validateSetting("MapShowTitles", "true");
        validateSetting("ShowKeypad", "true");
        validateSetting("FoundOffset", "0");
        validateSetting("ImportLayerOsm", "true");
        validateSetting("CurrentMapLayer", "OSM");
        validateSetting("AutoUpdate", "http://www.getcachebox.net/latest-stable");
        validateSetting("NavigationProvider", "http://129.206.229.146/openrouteservice/php/OpenLSRS_DetermineRoute.php");
        validateSetting("TrackRecorderStartup", "false");
        validateSetting("MapShowCompass", "true");
        validateSetting("FoundTemplate", "<br>###finds##, ##time##, Found it with Cachebox!");
        validateSetting("DNFTemplate", "<br>##time##. Logged it with Cachebox!");
        validateSetting("NeedsMaintenanceTemplate", "Logged it with Cachebox!");
        validateSetting("ResortRepaint", "false");
        validateSetting("TrackDistance", "3");
        validateSetting("MapMaxCachesLabel", "12");
        validateSetting("MapMaxCachesDisplay_config", "10000");
        validateSetting("SoundApproachDistance", "50");
        validateSetting("mapMaxCachesDisplayLarge_config", "75");
        //      validateSetting("Filter", FilterPresets.presets[0]);
        validateSetting("ZoomCross", "16");
        validateSetting("GpsDriverMethod", "default");
        validateSetting("TomTomExportFolder", Global.AppPath + "\\user");
        validateSetting("GCAutoSyncCachesFound", "true");
        validateSetting("GCAdditionalImageDownload", "false");
        validateSetting("GCRequestDelay", "10");

        validateSetting("Camera_Resolution_Width", "640");
        validateSetting("Camera_Resolution_Height", "480");

        // 2014-06-22 - Ging-Buh - Default Setting (spoiler;hint) removed
        validateSetting("SpoilersDescriptionTags", "");

        validateSetting("ExportSpoilersRotate", "false");
        validateSetting("ExportSpoilersMaxPixels", "800");
        validateSetting("ExportImagesMaxPixels", "800");

        validateSetting("DontDeleteGpx", "false");
        validateSetting("ImportLogImages", "false");

        validateSetting("DistanceFilter", "0");

        AcceptChanges();
    }

    private static void validateSetting(String key, String value)
    {
      if (!keyLookup.ContainsKey(key))
        keyLookup.Add(key, value);
    }

    public static bool ExistsKey(String key)
    {
      checkInitialization();
      return keyLookup.ContainsKey(key);
    }

    public static String GetString(String key)
    {
      checkInitialization();

      if (keyLookup.ContainsKey(key))
      {
          return keyLookup[key];
      }
      else
          return "";
    }
    
    public static String GetStringEncrypted(String key)
    {
        string s;
        bool convert = false;
        if (ExistsKey(key + "Enc"))
        {
            s = GetString(key + "Enc");
            if (s != "")
            {
                // encrypted Key is found -> remove the old non encrypted
                if (ExistsKey(key))
                {
                    keyLookup.Remove(key);
                    AcceptChanges();
                }
                // decrypting
                byte[] b = Convert.FromBase64String(s);
                RC4(ref b, Key);
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                s = enc.GetString(b);
            }
        }
        else
        {
            // no encrypted Key is found -> search for non encrypted
            s = GetString(key);
            if (s != "")
            {
                // remove the old non encrypted and insert a new encrypted
                keyLookup.Remove(key);
                convert = true;
            }
        }

        if (convert)
        {
            SetEncrypted(key, s);
            AcceptChanges();
        }
        return s;
    }

    public static double GetDouble(String key)
    {
      checkInitialization();

      if (keyLookup.ContainsKey(key))
        return double.Parse(keyLookup[key], NumberFormatInfo.InvariantInfo);
      else
        return 0;
    }

    public static float GetFloat(String key)
    {
      checkInitialization();

      if (keyLookup.ContainsKey(key))
        return float.Parse(keyLookup[key], NumberFormatInfo.InvariantInfo);
      else
        return 0;
    }

    public static bool GetBool(String key)
    {
      checkInitialization();

      if (keyLookup.ContainsKey(key))
        return bool.Parse(keyLookup[key]);
      else
        return false;
    }

    public static int GetInt(String key)
    {
      checkInitialization();

      if (keyLookup.ContainsKey(key))
        try
        {
          return int.Parse(keyLookup[key], NumberFormatInfo.InvariantInfo);
        }
        catch (Exception) { }

      return -1;
    }

    public static void Set(String key, String value)
    {
      checkInitialization();

      if (keyLookup.ContainsKey(key))
        keyLookup[key] = value;
      else
        keyLookup.Add(key, value);
    }

    public static void SetEncrypted(String key, String value)
    {
        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        byte[] b = enc.GetBytes(value);
        RC4(ref b, Key);
        string encrypted = Convert.ToBase64String(b);
        if (ExistsKey(key))
            keyLookup.Remove(key);  // remove non decrypted key if exists
        Set(key + "Enc", encrypted);
    }

    public static void Set(String key, double value)
    {
      Set(key, value.ToString(NumberFormatInfo.InvariantInfo));
    }

    public static void Set(String key, float value)
    {
      Set(key, value.ToString(NumberFormatInfo.InvariantInfo));
    }

    public static void Set(String key, bool value)
    {
      Set(key, value.ToString());
    }

    public static void Set(String key, int value)
    {
      Set(key, value.ToString());
    }

    public static void AcceptChanges()
    {
        StreamWriter writer = new StreamWriter(Global.AppPath + Global.configFileName);

      foreach (String key in keyLookup.Keys)
        writer.WriteLine(key + "=" + keyLookup[key]);

      writer.Close();
    }

    static Byte[] Key = { 128, 56, 20, 78, 33, 225 };
    public static void RC4(ref Byte[] bytes, Byte[] key)
    {
        Byte[] s = new Byte[256];
        Byte[] k = new Byte[256];
        Byte temp;
        int i, j;

        for (i = 0; i < 256; i++)
        {
            s[i] = (Byte)i;
            k[i] = key[i % key.GetLength(0)];
        }

        j = 0;
        for (i = 0; i < 256; i++)
        {
            j = (j + s[i] + k[i]) % 256;
            temp = s[i];
            s[i] = s[j];
            s[j] = temp;
        }

        i = j = 0;
        for (int x = 0; x < bytes.GetLength(0); x++)
        {
            i = (i + 1) % 256;
            j = (j + s[i]) % 256;
            temp = s[i];
            s[i] = s[j];
            s[j] = temp;
            int t = (s[i] + s[j]) % 256;
            bytes[x] ^= s[t];
        }
    }
    
    public static string GetAccessToken()
    {
        String act = GetStringEncrypted("accessToken");
        if (act == "")
            return "";
        // Prüfen, ob das AccessToken für WinCB ist!!!
        if (act[0] != 'W')
            return "";
        return act.Substring(1, act.Length - 1);
    }
  }
}
