using System;
using WinCachebox.Geocaching;
using WeifenLuo.WinFormsUI.Docking;

namespace WinCachebox.Views
{
    public partial class HintView : DockContent
  {
    public static HintView View = null;
    public HintView()
    {
      View = this;
      InitializeComponent();
      Global.TargetChanged += new Global.TargetChangedHandler(OnTargetChanged);
      button1.Text = Global.Translations.Get("decode", "&Decode");
    }

    void OnTargetChanged(Cache cache, Waypoint waypoint)
    {
      if (cache == null)
        return;
      textBox1.Text = cache.Hint;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      textBox1.Text = Global.Rot13(textBox1.Text);
    }
  }
}
