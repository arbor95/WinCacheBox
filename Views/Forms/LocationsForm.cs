using System;
using System.Windows.Forms;
using WinCachebox.Geocaching;

namespace WinCachebox.Views.Forms
{
    public partial class LocationsForm : Form
  {
    public static void Edit()
    {
      LocationsForm lf = new LocationsForm();
      lf.ShowDialog();
    }
    
    private Location aktLocation = null;

    public LocationsForm()
    {
      InitializeComponent();
      this.Text = Global.Translations.Get("Locations");
      this.button1.Text = "&" + Global.Translations.Get("ok");
      this.bNew.Text = "&" + Global.Translations.Get("new");
      this.bEdit.Text = "&" + Global.Translations.Get("edit");
      this.bDelete.Text = "&" + Global.Translations.Get("delete");
      gLocations.SelectionMode = SourceGrid.GridSelectionMode.Row;
      fillLocations();
    }

    void gLocations_SortedRangeRows(object sender, SourceGrid.SortRangeRowsEventArgs e)
    {
      foreach (SourceGrid.GridRow row in gLocations.Rows)
      {
        if (row.Tag == null)
          continue;
        aktLocation = row.Tag as Location;
      }      
    }

    private void fillLocations()
    {
      if ((aktLocation == null) && (Geocaching.Location.Locations.Count > 0))
        aktLocation = Geocaching.Location.Locations[0];

      gLocations.RowsCount = 0;
      gLocations.FixedRows = 1;
      gLocations.ColumnsCount = 2;
      gLocations.Columns[0].Width = 100;
      gLocations.Columns[1].Width = 100;

      gLocations.Rows.Insert(0);
      gLocations[0, 0] = new SourceGrid.Cells.ColumnHeader(Global.Translations.Get("cachename"));
      gLocations[0, 1] = new SourceGrid.Cells.ColumnHeader(Global.Translations.Get("coordinate"));

      SourceGrid.Position pos = new SourceGrid.Position(0, 0);
      int i = 1;
      foreach (Location location in Geocaching.Location.Locations)
      {
        gLocations.Rows.Insert(i);
        gLocations.Rows[i].Tag = location;

        SourceGrid.Cells.Cell cell;

        cell = new SourceGrid.Cells.Cell(location.Name);
        gLocations[i, 0] = cell;

        gLocations[i, 1] = new SourceGrid.Cells.Cell(location.Coordinate.FormatCoordinate());

        if (location == aktLocation)
        {
          pos = new SourceGrid.Position(i, 0);
        }
        i++;
      }
      gLocations.AutoSizeCells();
      if (pos != null)
        gLocations.Selection.Focus(pos, true);
      
    }

    private void bEdit_Click(object sender, EventArgs e)
    {
      if (aktLocation == null)
        return;
      if (aktLocation.Edit())
        fillLocations();
    }

    private void LocationsForm_Load(object sender, EventArgs e)
    {
      gLocations.Selection.SelectionChanged += new SourceGrid.RangeRegionChangedEventHandler(Selection_SelectionChanged);
    }
  
    void Selection_SelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
    {
      if (e.AddedRange == null)
        return;
      if (e.AddedRange.Count <= 0)
        return;
      if (e.AddedRange[0].Start.Row < 1)
        return;
      rowChanged(e.AddedRange[0].Start.Row);
    }
  
    private void rowChanged(int row)
    {
      if (gLocations.Rows[row] == null)
        return;
      Location location = gLocations.Rows[row].Tag as Location;
      if (location == null)
        return;
      aktLocation = location;
    }

    private void bDelete_Click(object sender, EventArgs e)
    {
      if (aktLocation == null)
        return;
      if (MessageBox.Show("Delete Location \"" + aktLocation.Name + "\"?", Global.Translations.Get("delete"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
      {
        aktLocation.Delete();
        aktLocation = null;
        Geocaching.Location.LoadLocations();
        fillLocations();
      }
    }

    private void bNew_Click(object sender, EventArgs e)
    {
      Location loc = new Location("", 0, 0);
      loc.Coordinate.Valid = false;
      if (loc.Edit())
      {
        aktLocation = loc;
        Geocaching.Location.Locations.Add(loc);
        fillLocations();
      }
    }
  }
}
