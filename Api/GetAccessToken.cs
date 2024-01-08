using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace WinCachebox.Api
{
    public partial class GetAccessToken : Form
    {
    public static String Execute()
        {
            try
            {
                String url = "https://gc-oauth.longri.de/index?Version=WCB";
                Global.AddLog("GetAccessToken by System.Windows.Forms.WebBrowser: " + url);
                GetAccessToken gac = new GetAccessToken(url);
                gac.ShowDialog();
                return gac.getAccessToken();
            }
            catch (Exception exc)
            {
                Global.AddLog("GetAccessToken Execute: " + exc.ToString());
            }
            return "";
        }

        private string getAccessToken()
        {
            return accessToken;
        }

        private String url;
        private String accessToken;

        public GetAccessToken(String url)
        {
            this.url = url;
            InitializeComponent();
            webBrowser1.Url = new Uri(url);
            accessToken = "";
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                String s = e.Url.ToString();
                Global.AddLog("WebBrowser got Result from: " + s);
                String search = "Access token: ";
                int pos = webBrowser1.DocumentText.IndexOf(search);
                if (pos < 0)
                    return;
                int pos2 = webBrowser1.DocumentText.IndexOf("</span>", pos);
                if (pos2 < pos)
                    return;
                // zwischen pos und pos2 sollte ein gültiges AccessToken sein!!!
                accessToken = webBrowser1.DocumentText.Substring(pos + search.Length, pos2 - pos - search.Length);
                Global.AddLog("Now we have the API key: " + accessToken);
                this.Close();
            }
            catch (Exception exc)
            {
                Global.AddLog("webBrowser1_DocumentCompleted: " + exc.ToString());
            }
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
           // webBrowser1.Cursor = Cursors.WaitCursor;
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
           // webBrowser1.Cursor = Cursors.Default;
        }
    }
}
