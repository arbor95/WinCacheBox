using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinCachebox.Geocaching;
using System.Threading;
using WeifenLuo.WinFormsUI.Docking;
using WinCachebox.Types;

namespace WinCachebox.Views
{
    public partial class DescriptionView : DockContent
  {
    public static DescriptionView View = null;
    private Cache aktCache = null;
    private string currentHtml = "";
   
    Thread loaderThread = null;
    List<String> nonLocalImagesUrl;
    List<String> nonLocalImages;
    Graphics graphics = null;

    public DescriptionView()
    {
      View = this;
      InitializeComponent();
      graphics = CreateGraphics();
      Global.TargetChanged += new Global.TargetChangedHandler(OnTargetChanged);

    }

    public void SelectedCacheChanged()
    {
                      // HTML erzeugen
      if (Global.SelectedCache == null)
        return;
      lock (graphics)
      {
        String html = DescriptionImageGrabber.ResolveImages(Global.SelectedCache, Global.SelectedCache.Description, true, out nonLocalImages, out nonLocalImagesUrl);
#if DEBUG
        Global.AddLog("DescriptionView.OnShow");
#endif
        setBrowserHtml(html);
      }

      // Falls nicht geladene Bilder vorliegen und eine Internetverbindung
      // erlaubt ist, diese laden und Bilder erneut auflösen
      if (Config.GetBool("AllowInternetAccess") && nonLocalImagesUrl.Count > 0)
      {
        if (loaderThread == null)
        {
          Cursor.Current = Cursors.WaitCursor;
                    loaderThread = new Thread(new ThreadStart(loaderThreadEntryPoint))
                    {
                        Priority = ThreadPriority.BelowNormal
                    };
                    loaderThread.Start();
        }
      }
    }

    public void setBrowserHtml(String html)
    {
      if (!Config.GetBool("DescriptionNoAttributes"))
        html = getAttributesHtml(Global.SelectedCache) + html;

      if (currentHtml == html)
        return;

      currentHtml = html;

      webBrowser1.DocumentText = html;
    }

    private string getAttributesHtml(Cache cache)
    {
      StringBuilder sb = new StringBuilder();



      List<Attributes> attrs = cache.getAttributes();

      if (attrs == null || !(attrs.Count>0)) return "";

       foreach (Attributes att in attrs)
       {
           sb.Append("<form action=\"Attr\">");
           sb.Append("<input name=\"Button\" type=\"image\" src=\"file://" + Global.ExePath + "\\data\\Attributes\\"
                  + att.getImageName() + ".png\" value=\" " + att.getImageName() + " \">");
      }
       sb.Append("</form>");

       
     
      if (sb.Length > 0)
        sb.Append("<br>");
      return sb.ToString();
    }
    
    private void DescriptionView_Load(object sender, EventArgs e)
    {
    }

    void OnTargetChanged(Cache cache, Waypoint waypoint)
    {
      if (cache == aktCache)
        return;  // nur der Waypoint hat sich geändert...
      aktCache = cache;
      SelectedCacheChanged();
    }

    void loaderThreadEntryPoint()
    {
      bool imagesFetched = false;

      while (nonLocalImagesUrl != null && nonLocalImagesUrl.Count > 0)
      {
        String local, url;
        lock (graphics)
        {
          local = nonLocalImages[0];
          url = nonLocalImagesUrl[0];
          nonLocalImagesUrl.RemoveAt(0);
          nonLocalImages.RemoveAt(0);
        }

        if (DescriptionImageGrabber.Download(url, local))
        {
          imagesFetched = true;

          if (Global.GetAvailableDiscSpace(Config.GetString("DescriptionImageFolder")) < (1024 * 1024))
          {
            MessageBox.Show("You are running low on memory! Internet connection will close now.", "Low Memory!");
            Config.Set("AllowInternetAccess", false);
            Config.AcceptChanges();
            break;
          }
        }
#if DEBUG
        else
          Global.AddLog("DesciptionView.loaderThreadEntryPoint: loading " + url + " to " + local + " failed");
#endif
      }
      loaderThread = null;

      // Fertig!
      if (imagesFetched)
        reresolveImages();
    }
  
    /// <summary>
    /// Aktualisiert das html nachdem die Bilder geladen wurden
    /// </summary>
    public void reresolveImages()
    {
      if (InvokeRequired)
      {
        Invoke(new Global.emptyDelegate(reresolveImages));
        return;
      }

#if DEBUG
      Global.AddLog("DescriptionView.reresolveImages()");
#endif

      Cursor.Current = Cursors.Default;

      String html = DescriptionImageGrabber.ResolveImages(Global.SelectedCache, Global.SelectedCache.Description, !Config.GetBool("AllowInternetAccess"), out nonLocalImagesUrl, out nonLocalImages);
      setBrowserHtml(html);
    }

  }
}
