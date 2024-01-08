
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace WinCachebox
{
    public class CookieManager
  {
    private Dictionary<string, string> cookieValues;
    private bool XWapProxySetCookie = false;

    public Dictionary<string, string> CookieValues
    {
      get
      {
        if (this.cookieValues == null)
        {
          this.cookieValues = new Dictionary<string, string>();
        }

        return this.cookieValues;
      }
    }

    public void PublishCookies(HttpWebRequest webRequest)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("Cookie: ");
      foreach (string key in this.CookieValues.Keys)
      {
        sb.Append(key);
        sb.Append("=");
        sb.Append(this.CookieValues[key]);
        sb.Append("; ");
        //sb.Append("$Path=\"/\"; ");
      }

      webRequest.Headers.Add(sb.ToString());
      sb = null;
      webRequest = null;
    }

    public void StoreCookies(HttpWebResponse webResponse)
    {
      for (int x = 0; x < webResponse.Headers.Count; x++)
      {
        if (webResponse.Headers.Keys[x].ToLower().Equals("set-cookie"))
        {
          this.AddRawCookie(webResponse.Headers[x]);
        }
        else if (webResponse.Headers.Keys[x].ToLower().Equals("x-wap-proxy-set-cookie"))
        {
          if (webResponse.Headers[x].ToLower().Equals("state"))
          {
            XWapProxySetCookie = true;
          }
        }
      }

      webResponse = null;
    }

    public bool CheckXWapProxySetCookie()
    {
      return XWapProxySetCookie;
    }


    private void AddRawCookie(string rawCookieData)
    {
      string key = null;
      string value = null;

      string[] entries = null;

      if (rawCookieData.IndexOf(",") > 0)
      {
        entries = rawCookieData.Split(',');
      }
      else
      {
        entries = new string[] { rawCookieData };
      }

      foreach (string entry in entries)
      {
        string cookieData = entry.Trim();

        if (cookieData.IndexOf(';') > 0)
        {
          string[] temp = cookieData.Split(';');
          cookieData = temp[0];
        }

        int index = cookieData.IndexOf('=');
        if (index > 0)
        {
          key = cookieData.Substring(0, index);
          value = cookieData.Substring(index + 1);
        }

        if (key != null && value != null)
        {
          this.CookieValues[key] = value;
        }

        cookieData = null;
      }

      rawCookieData = null;
      entries = null;
      key = null;
      value = null;
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("[");
      foreach (string key in this.CookieValues.Keys)
      {
        sb.Append("{");
        sb.Append(key);
        sb.Append(",");
        sb.Append(this.CookieValues[key]);
        sb.Append("}, ");
      }
      if (this.CookieValues.Keys.Count > 0)
      {
        sb.Remove(sb.Length - 2, 2);
      }
      sb.Append("]");

      return sb.ToString();
    }
  }
}
