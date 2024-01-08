using System;
using System.Drawing;
using System.Windows.Forms;
using WinCachebox.Geocaching;

namespace WinCachebox.Views
{
    public partial class CacheInfoView : UserControl
  {
    public static CacheInfoView View;
    
    public CacheInfoView()
    {
      View = this;
      InitializeComponent();
      Global.TargetChanged += new Global.TargetChangedHandler(OnTargetChanged);
    }

    public void SelectedCacheChanged()
    {
        this.label2.Text = Global.Translations.Get("difficulty", "Dif");
        this.label3.Text = Global.Translations.Get("terrain", "Ter");
        this.label4.Text = Global.Translations.Get("size", "Size");
        if (Global.SelectedCache == null)
        {
            this.lName.Text = Global.Translations.Get("civ0", "No cache choosen");
            this.label1.Text = Global.Translations.Get("civ1", "Please choose a geocache in the list on the left");
            this.labelSize.Text = Global.Translations.Get("civ5", "(unknown)");
            return;
        }

      lName.Text = Global.SelectedCache.Name;
      pictureBoxType.Image = (Global.SelectedCache.Found) ? Global.CacheIconsBigFound[(int)Global.SelectedCache.Type] : Global.CacheIconsBig[(int)Global.SelectedCache.Type];
      label1.Text = Global.Translations.Get("civ12", "by ") + Global.SelectedCache.PlacedBy + ", " + Global.SelectedCache.DateHidden.ToShortDateString();
      pictureBoxDifficulty.Image = Global.StarIcons[(int)Math.Floor(Global.SelectedCache.Difficulty * 2.01f)];
      pictureBoxTerrain.Image = Global.StarIcons[(int)Math.Floor(Global.SelectedCache.Terrain * 2.01f)];
      String[] sizes = new String[] { //
          Global.Translations.Get("civ7", "Unknown"), //
          Global.Translations.Get("civ8", "Micro"), //
          Global.Translations.Get("civ9", "Small"), //
          Global.Translations.Get("civ10", "Regular"), //
          Global.Translations.Get("civ11", "Large"), //
          };
      labelSize.Text = "(" + sizes[Math.Min(Global.SelectedCache.Size, sizes.Length - 1)] + ")";
      pictureBoxContainerSize.Image = Global.ContainerSizeIcons[Math.Min(Global.SelectedCache.Size, Global.ContainerSizeIcons.Count - 1)];
      if (Global.SelectedCache.NumTravelbugs == 0)
      {
        pictureBoxTb.Visible = false;
        labelTbMultiply.Text = "";
      }
      else
      {
        pictureBoxTb.Image = Global.Icons[0];
        pictureBoxTb.Visible = true;

        if (Global.SelectedCache.NumTravelbugs > 1)
          labelTbMultiply.Text = Global.SelectedCache.NumTravelbugs.ToString() + "x";
      }
    }
  
    void OnTargetChanged(Cache cache, Waypoint waypoint)
    {
      SelectedCacheChanged();
    }

    private void lName_Click(object sender, EventArgs e)
    {
        System.Diagnostics.Process.Start(Global.SelectedCache.Url);
    }

    private void lName_MouseEnter(object sender, EventArgs e)
    {
        lName.ForeColor = Color.Blue;
        lName.Font = new Font(lName.Font, FontStyle.Underline | FontStyle.Bold);
    }

    private void lName_MouseLeave(object sender, EventArgs e)
    {
        lName.ForeColor = Color.Black;
        lName.Font = new Font(lName.Font, FontStyle.Bold);
    }

  }
}
