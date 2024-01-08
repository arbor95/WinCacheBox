using System;
using WinCachebox.Geocaching;
using WeifenLuo.WinFormsUI.Docking;

namespace WinCachebox.Views
{
    public partial class NotesView : DockContent
  {
    public static NotesView View = null;
    private Cache aktCache = null;
    private bool changed = false;

    public NotesView()
    {
      View = this;
      InitializeComponent();
      Global.TargetChanged += new Global.TargetChangedHandler(OnTargetChanged);
    }

    void OnTargetChanged(Cache cache, Waypoint waypoint)
    {
      if (changed)
      {
        // Änderungen des Solvertextes als Solver speichern
        if (aktCache != null)
        {
          aktCache.Note = textBox121.Text;
        }
      }
      if (cache == aktCache)
        return;  // nur der Waypoint hat sich geändert...

      aktCache = cache;
      if (cache != null)
        textBox121.Text = cache.Note;
      changed = false;
      SelectedCacheChanged();
    }

    public void SelectedCacheChanged()
    {
      if (aktCache == null)
        textBox121.Text = "";
      else 
        textBox121.Text = aktCache.Note;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      changed = true;
    }
  }
}
