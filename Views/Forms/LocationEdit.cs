using System;
using System.Windows.Forms;
using WinCachebox.Geocaching;

namespace WinCachebox.Views.Forms
{
    public partial class LocationEdit : Form
  {
    public static bool Edit(Location location)
    {
      LocationEdit le = new LocationEdit(location);
      return le.ShowDialog() == DialogResult.OK;
    }

    private Location location;
    private Coordinate coordinate;
    public LocationEdit(Location location)
    {
      this.location = location;
      this.coordinate = new Coordinate(location.Coordinate);

      InitializeComponent();
      this.Text = Global.Translations.Get("Location");
      this.button2.Text = "&" + Global.Translations.Get("cancel");
      this.button1.Text = "&" + Global.Translations.Get("ok");
      this.label1.Text = Global.Translations.Get("name") + ":";
      fillCoord();
      tbName.Text = location.Name;
    }

    private void fillCoord()
    {
      bCoord.Text = coordinate.FormatCoordinate();
    }

    private void bCoord_Click(object sender, EventArgs e)
    {
      if (coordinate.Edit())
        fillCoord();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (tbName.Text == "")
      {
        DialogResult = System.Windows.Forms.DialogResult.None;
        tbName.Focus();
        return;
      }
      if (!coordinate.Valid)
      {
        DialogResult = System.Windows.Forms.DialogResult.None;
        bCoord.Focus();
        return;
      }
      location.Name = tbName.Text;
      location.Coordinate = coordinate;
    }
  }
}
