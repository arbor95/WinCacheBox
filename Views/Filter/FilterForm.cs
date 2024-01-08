using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinCachebox.Geocaching;

namespace WinCachebox.Views
{
    public partial class FilterForm : Form
  {
    CellTristate NotAvailable;
    CellTristate Archived;
    CellTristate Finds;
    CellTristate Own;
    CellTristate ContainsTravelBugs;
    CellTristate Favorites;
    CellTristate HasUserData;
    CellTristate ListingChanged;
    CellTristate WithManualWaypoint;

    CellNumeric minDifficulty;
    CellNumeric maxDifficulty;
    CellNumeric minTerrain;
    CellNumeric maxTerrain;
    CellNumeric minContainerSize;
    CellNumeric maxContainerSize;
    CellNumeric minRating;
    CellNumeric maxRating;

    CellTristate[] enableCacheType;
    CellTristate[] attributesFilter;

    FilterProperties filter;
    Category aktCategory;

    public FilterForm(FilterProperties filter)
    {
      this.filter = filter;
      InitializeComponent();
      tabPage1.Text = Global.Translations.Get("name");
      tabPage2.Text = Global.Translations.Get("general");
      tabPage3.Text = Global.Translations.Get("d/t");
      tabPage4.Text = Global.Translations.Get("cachetype");
      tabPage5.Text = Global.Translations.Get("attributes");
      tabPage6.Text = Global.Translations.Get("country/state");
      tabPage7.Text = Global.Translations.Get("presets");
      button1.Text = Global.Translations.Get("ok");
      button2.Text = Global.Translations.Get("cancel");
      label4.Text = Global.Translations.Get("Filter") + " " + Global.Translations.Get("country");
      label3.Text = Global.Translations.Get("Filter") + " " + Global.Translations.Get("state");
      label1.Text = Global.Translations.Get("Filter") + " " + Global.Translations.Get("cachename");
      label2.Text = Global.Translations.Get("Filter") + " " + Global.Translations.Get("gpxfilename");
      label5.Text = Global.Translations.Get("Filter") + " " + Global.Translations.Get("gpxolderthan");

    }

    public FilterProperties getFilter() { return this.filter; }

    private void fillGpxFilenames()
    {
        // GPXFilenames
        gGPXFilenames.RowsCount = 1;
        gGPXFilenames.FixedRows = 1;
        gGPXFilenames.ColumnsCount = 1;
        gGPXFilenames[0, 0] = new SourceGrid.Cells.Header(Global.Translations.Get("subcategories"));
        int i = 1;
        foreach (GpxFilename gpx in aktCategory.Values)
        {
                CellTristate cell = new CellTristate(Global.Icons[20], /*gpx.Id.ToString() + " - " + */gpx.GpxFileName, 1, true)
                {
                    string2 = gpx.Imported.ToString(),
                    string3 = gpx.CacheCount.ToString() + " Cache"
                };
                if (gpx.CacheCount > 1)
                cell.string3 += "s";
            cell.height = 32;
            cell.setStateBool(gpx.Checked);
            gGPXFilenames.Rows.Insert(i);
            gGPXFilenames[i, 0] = cell;
            gGPXFilenames.Rows[i].Tag = gpx;
            cell.TargetChanged += new CellTristate.TargetChangedHandler(OnGpxChanged);
//            i++;
        }
        gGPXFilenames.AutoSizeCells();
    }

    void OnTargetChanged(CellTristate cell)
    {
        (cell.Row.Tag as Category).Checked = cell.getStateBool();
        foreach (GpxFilename gpx in aktCategory.Values)
        {
            gpx.Checked = aktCategory.Checked;
        }
        fillGpxFilenames();
    }

    void OnGpxChanged(CellTristate cell)
    {
        (cell.Row.Tag as GpxFilename).Checked = cell.getStateBool();
    }

    private void fillForm()
    {
        // Filter Name
        tbName.Text = filter.filterName;

        // Categories
        gCategory.RowsCount = 1;
        gCategory.FixedRows = 1;
        gCategory.ColumnsCount = 1;
        gCategory[0, 0] = new SourceGrid.Cells.Header(Global.Translations.Get("categories"));
        int i = 1;
        foreach (Category category in Global.Categories.Values)
        {
                CellTristate cell = new CellTristate(category.Pinned ? Global.Icons[27] : Global.Icons[20], /*category.Id.ToString() + " - " + */category.GpxFilenameWoNumber(), 1, true)
                {
                    string2 = category.LastImported().ToString(),
                    string3 = category.CacheCount().ToString() + " " + Global.Translations.Get("Cache"),
                    // if (category.CacheCount() > 1) cell.string3 += "s";
                    height = 32
                };
                cell.setStateBool(category.Checked);
            gCategory.Rows.Insert(i);
            gCategory[i, 0] = cell;
            gCategory.Rows[i].Tag = category;
            cell.TargetChanged += new CellTristate.TargetChangedHandler(OnTargetChanged);
            i++;
        }

        if (i > 1)
        {
            SourceGrid.Position pos = new SourceGrid.Position(1, 0);
            gCategory.Selection.SelectCell(pos, true);
        }
        gCategory.AutoSizeCells();
        //      fillGpxFilenames();
        // General
        gGeneral.RowsCount = 1;
        gGeneral.ColumnsCount = 1;

        i = 0;

        NotAvailable = new CellTristate(Global.Icons[25], Global.Translations.Get("not") + " " + Global.Translations.Get("searchable"), filter.NotAvailable, false);
        Archived = new CellTristate(Global.Icons[24], Global.Translations.Get("archived"), filter.Archived, false);
        Finds = new CellTristate(Global.Icons[2], Global.Translations.Get("myfinds"), filter.Finds, false);
        Own = new CellTristate(Global.MapIcons[20], Global.Translations.Get("own") + " " + Global.Translations.Get("Cache"), filter.Own, false);
        ContainsTravelBugs = new CellTristate(Global.Icons[10], Global.Translations.Get("contains") + " " + Global.Translations.Get("travelbugs"), filter.ContainsTravelbugs, false);
        Favorites = new CellTristate(Global.Icons[19], Global.Translations.Get("Favorite"), filter.Favorites, false);
        HasUserData = new CellTristate(Global.Icons[21], Global.Translations.Get("contains") + " " + Global.Translations.Get("userdata"), filter.HasUserData, false);
        ListingChanged = new CellTristate(Global.Icons[26], Global.Translations.Get("listing") + " " + Global.Translations.Get("changed"), filter.ListingChanged, false);
        WithManualWaypoint = new CellTristate(Global.Icons[26], Global.Translations.Get("manual") + " " + Global.Translations.Get("Waypoints"), filter.WithManualWaypoint, false);

        gGeneral.Rows.Insert(i);
        gGeneral[i++, 0] = NotAvailable;
        gGeneral.Rows.Insert(i);
        gGeneral[i++, 0] = Archived;
        gGeneral.Rows.Insert(i);
        gGeneral[i++, 0] = Finds;
        gGeneral.Rows.Insert(i);
        gGeneral[i++, 0] = Own;
        gGeneral.Rows.Insert(i);
        gGeneral[i++, 0] = ContainsTravelBugs;
        gGeneral.Rows.Insert(i);
        gGeneral[i++, 0] = Favorites;
        gGeneral.Rows.Insert(i);
        gGeneral[i++, 0] = HasUserData;
        gGeneral.Rows.Insert(i);
        gGeneral[i++, 0] = ListingChanged;
        gGeneral.Rows.Insert(i);
        gGeneral[i++, 0] = WithManualWaypoint;
        gGeneral.AutoSizeCells();

        // D / T
        gDT.RowsCount = 1;
        gDT.ColumnsCount = 1;

        i = 0;
        minDifficulty = new CellNumeric(Global.StarIcons, Global.Translations.Get("min") + Global.Translations.Get("difficulty"), 1, 5, filter.MinDifficulty, 0.5f);
        maxDifficulty = new CellNumeric(Global.StarIcons, Global.Translations.Get("max") + Global.Translations.Get("difficulty"), 1, 5, filter.MaxDifficulty, 0.5f);
        minTerrain = new CellNumeric(Global.StarIcons, Global.Translations.Get("min") + Global.Translations.Get("terrain"), 1, 5, filter.MinTerrain, 0.5f);
        maxTerrain = new CellNumeric(Global.StarIcons, Global.Translations.Get("max") + Global.Translations.Get("terrain"), 1, 5, filter.MaxTerrain, 0.5f);
        minContainerSize = new CellNumeric(Global.ContainerSizeIcons, Global.Translations.Get("min") + Global.Translations.Get("size"), 0, 4, filter.MinContainerSize, 1);
        maxContainerSize = new CellNumeric(Global.ContainerSizeIcons, Global.Translations.Get("max") + Global.Translations.Get("size"), 0, 4, filter.MaxContainerSize, 1);
        minRating = new CellNumeric(Global.StarIcons, Global.Translations.Get("min") + Global.Translations.Get("rating"), 0, 5, filter.MinRating, 0.5f);
        maxRating = new CellNumeric(Global.StarIcons, Global.Translations.Get("max") + Global.Translations.Get("rating"), 0, 5, filter.MaxRating, 0.5f);

        gDT.Rows.Insert(i);
        gDT[i++, 0] = minDifficulty;
        gDT.Rows.Insert(i);
        gDT[i++, 0] = maxDifficulty;
        gDT.Rows.Insert(i);
        gDT[i++, 0] = minTerrain;
        gDT.Rows.Insert(i);
        gDT[i++, 0] = maxTerrain;
        gDT.Rows.Insert(i);
        gDT[i++, 0] = minContainerSize;
        gDT.Rows.Insert(i);
        gDT[i++, 0] = maxContainerSize;
        gDT.Rows.Insert(i);
        gDT[i++, 0] = minRating;
        gDT.Rows.Insert(i);
        gDT[i++, 0] = maxRating;

        gDT.AutoSizeCells();
        // Cache Types
        gCacheType.RowsCount = 1;
        gCacheType.ColumnsCount = 1;

        i = 0;
        enableCacheType = new CellTristate[] 
      {
        new CellTristate(Global.CacheIconsBig[0], "Traditional", 1, true),
        new CellTristate(Global.CacheIconsBig[1], "Multi-Cache", 1, true),
        new CellTristate(Global.CacheIconsBig[2], "Mystery", 1, true),
        new CellTristate(Global.CacheIconsBig[3], "Webcam Cache", 1, true),
        new CellTristate(Global.CacheIconsBig[4], "Earthcache", 1, true),
        new CellTristate(Global.CacheIconsBig[5], "Event", 1, true),
        new CellTristate(Global.CacheIconsBig[6], "Mega Event", 1, true),
        new CellTristate(Global.CacheIconsBig[7], "Cache In Trash Out", 1, true),
        new CellTristate(Global.CacheIconsBig[8], "Virtual Cache", 1, true),
        new CellTristate(Global.CacheIconsBig[9], "Letterbox", 1, true),
        new CellTristate(Global.CacheIconsBig[10], "Wherigo", 1, true)
      };
        for (int ii = 0; ii < 11; ii++)
        {
            enableCacheType[ii].StateBool = filter.cacheTypes[ii];
            gCacheType.Rows.Insert(i);
            gCacheType[i++, 0] = enableCacheType[ii];
        }
        gCacheType.AutoSizeCells();

        // Attributes
        gAttributes.RowsCount = 1;
        gAttributes.ColumnsCount = 1;
        i = 0;
        attributesFilter = new CellTristate[] {
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_1_1.png"), Global.Translations.Get("at1","Dogs"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_2_1.png"), Global.Translations.Get("at2","Access or parking fee"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_3_1.png"), Global.Translations.Get("at3","Climbing gear"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_4_1.png"), Global.Translations.Get("at4","Boat"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_5_1.png"), Global.Translations.Get("at5","Scuba gear"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_6_1.png"), Global.Translations.Get("at6","Recommended for kids"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_7_1.png"), Global.Translations.Get("at7","Takes less than an hour"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_8_1.png"), Global.Translations.Get("at8","Scenic view"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_9_1.png"), Global.Translations.Get("at9","Significant hike"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_10_1.png"), Global.Translations.Get("at10","Difficult climbing"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_11_1.png"), Global.Translations.Get("at11","May require wading"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_12_1.png"), Global.Translations.Get("at12","May require swimming"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_13_1.png"), Global.Translations.Get("at13","Available at all times"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_14_1.png"), Global.Translations.Get("at14","Recommended at night"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_15_1.png"), Global.Translations.Get("at15","Available during winter"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_16_1.png"), Global.Translations.Get("at16","Kaktus"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_17_1.png"), Global.Translations.Get("at17","Poison plants"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_18_1.png"), Global.Translations.Get("at18","Dangerous Animals"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_19_1.png"), Global.Translations.Get("at19","Ticks"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_20_1.png"), Global.Translations.Get("at20","Abandoned mines"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_21_1.png"), Global.Translations.Get("at21","Cliff / falling rocks"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_22_1.png"), Global.Translations.Get("at22","Hunting"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_23_1.png"), Global.Translations.Get("at23","Dangerous Area"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_24_1.png"), Global.Translations.Get("at24","Wheelchair accessible"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_25_1.png"), Global.Translations.Get("at25","Parking available"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_26_1.png"), Global.Translations.Get("at26","Public transportation"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_27_1.png"), Global.Translations.Get("at27","Drinking water nearby"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_28_1.png"), Global.Translations.Get("at28","Public restrooms nearby"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_29_1.png"), Global.Translations.Get("at29","Telephone nearby"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_30_1.png"), Global.Translations.Get("at30","Picnic tables nearby"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_31_1.png"), Global.Translations.Get("at31","Camping available"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_32_1.png"), Global.Translations.Get("at32","Bicycles"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_33_1.png"), Global.Translations.Get("at33","Motorcycles"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_34_1.png"), Global.Translations.Get("at34","Quads"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_35_1.png"), Global.Translations.Get("at35","Off-road vehicles"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_36_1.png"), Global.Translations.Get("at36","Snowmobiles"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_37_1.png"), Global.Translations.Get("at37","Horses"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_38_1.png"), Global.Translations.Get("at38","Campfires"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_39_1.png"), Global.Translations.Get("at39","Thorns"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_40_1.png"), Global.Translations.Get("at40","Stealth required"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_41_1.png"), Global.Translations.Get("at41","Stroller accessible"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_42_1.png"), Global.Translations.Get("at42","Needs maintenance"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_43_1.png"), Global.Translations.Get("at43","Watch for livestock"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_44_1.png"), Global.Translations.Get("at44","Flashlight required"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_45_1.png"), Global.Translations.Get("at45","Lost & Found"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_46_1.png"), Global.Translations.Get("at46","Truck Driver/RV"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_47_1.png"), Global.Translations.Get("at47","Field Puzzle"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_48_1.png"), Global.Translations.Get("at48","UV Light Required"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_49_1.png"), Global.Translations.Get("at49","Snowshoes"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_50_1.png"), Global.Translations.Get("at50","Cross Country Skis"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_51_1.png"), Global.Translations.Get("at51","Special Tool Required"), 0, true),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_52_1.png"), Global.Translations.Get("at52","Night Cache"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_53_1.png"), Global.Translations.Get("at53","Park and Grab"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_54_1.png"), Global.Translations.Get("at54","Abandoned Structure"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_55_1.png"), Global.Translations.Get("at55","Short hike (less than 1km)"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_56_1.png"), Global.Translations.Get("at56","Medium hike (1km-10km)"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_57_1.png"), Global.Translations.Get("at57","Long Hike (+10km)"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_58_1.png"), Global.Translations.Get("at58","Fuel Nearby"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_59_1.png"), Global.Translations.Get("at59","Food Nearby"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_60_1.png"), Global.Translations.Get("at60","Wireless Beacon"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_61_1.png"), Global.Translations.Get("at61","Partnership Cache"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_62_1.png"), Global.Translations.Get("at62","Seasonal Access"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_63_1.png"), Global.Translations.Get("at63","Tourist Friendly"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_64_1.png"), Global.Translations.Get("at64","Tree Climbing"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_65_1.png"), Global.Translations.Get("at65","Front_Yard"), 0, false),
                new CellTristate(Global.LoadRessourceBitmap("Attributes.att_66_1.png"), Global.Translations.Get("at66","Teamwork_Required"), 0, false),
                };


        //Wireless_Beacon, Partnership_Cache, Seasonal_Access, Tourist_Friendly, Tree_Climbing, Front_Yard, Teamwork_Required,
        for (int ii = 0; ii < attributesFilter.Length; ii++)
        {
            attributesFilter[ii].State = filter.attributesFilter[ii];
            gAttributes.Rows.Insert(i);
            gAttributes[i++, 0] = attributesFilter[ii];
        }
        gAttributes.AutoSizeCells();



        string[] selectedCountries = filter.filterCountry.Split(';');
        bool addAllAsMarked = filter.filterCountry.Length == 0;
        CBCommand command = Database.Data.CreateCommand("SELECT DISTINCT country FROM Caches ORDER BY country ASC ");
        System.Data.Common.DbDataReader reader = command.ExecuteReader();
        command.Dispose();
        while (reader.Read())
        {
            string country = Global.Translations.Get("empty");
            if (!reader.IsDBNull(0))
                country = reader.GetString(0);

            if (addAllAsMarked || selectedCountries.Contains(country))
                lstCountries.Items.Add(country, true);
            else
                lstCountries.Items.Add(country);
        };

        if (lstCountries.CheckedItems.Count == 0)
        {
            lstStates.Enabled = false;
        }
        else
        {
            lstStates.Enabled = true;

            bool empty = true;
            string countries = "";
            foreach(string item in lstCountries.CheckedItems)
            {
                if (item.Equals(Global.Translations.Get("empty")))
                    empty = true;
                else countries += "'" + item + "' ,";
            }
            if (countries.Length > 0)
                countries = countries.Substring(0, countries.Length - 2);

            string[] selectedStates = filter.filterState.Split(';');
            addAllAsMarked = filter.filterState.Length == 0;
            if (empty)
            {
                countries = "''";
                command = Database.Data.CreateCommand("SELECT DISTINCT state, country FROM Caches WHERE country IN (" + countries + ") OR country IS NULL ORDER BY country, state ASC ");
            }
            else
                command = Database.Data.CreateCommand("SELECT DISTINCT state, country FROM Caches WHERE country IN (" + countries + ") ORDER BY country, state ASC ");
            
            reader = command.ExecuteReader();
            command.Dispose();
            while (reader.Read())
            {
                string state;
                try
                {
                    state = reader.GetString(0);
                    if (state.Length == 0) state = Global.Translations.Get("empty");
                }
                catch
                {
                    state = Global.Translations.Get("empty");
                };
                if (!lstStates.Items.Contains(state))
                    if (addAllAsMarked || selectedStates.Contains(state))
                        lstStates.Items.Add(state, true);
                    else
                        lstStates.Items.Add(state);
            };
        }

        dgPresets.Rows.Clear();
        i = 0;
        dgPresets.Rows.Add(1);
        dgPresets.Rows[i].Cells[1].Value = Global.Translations.Get("all") + " " + Global.Translations.Get("Cache");
        dgPresets.Rows[i].Cells[0].Value = Global.Icons[18];
        if (filter.ToString() == FilterPresets.presets[i])
            dgPresets.Rows[i].Selected = true;
        i++;
        dgPresets.Rows.Add(1);
        dgPresets.Rows[i].Cells[1].Value = Global.Translations.Get("searchable");
        dgPresets.Rows[i].Cells[0].Value = Global.Icons[2];
        if (filter.ToString() == FilterPresets.presets[i])
            dgPresets.Rows[i].Selected = true;
        i++;
        dgPresets.Rows.Add(1);
        dgPresets.Rows[i].Cells[1].Value = Global.Translations.Get("quick") + " " + Global.Translations.Get("Cache");
        dgPresets.Rows[i].Cells[0].Value = Global.CacheIconsBig[0];
        if (filter.ToString() == FilterPresets.presets[i])
            dgPresets.Rows[i].Selected = true;
        i++;
        dgPresets.Rows.Add(1);
        dgPresets.Rows[i].Cells[1].Value = Global.Translations.Get("contains") + " " + Global.Translations.Get("travelbugs");
        dgPresets.Rows[i].Cells[0].Value = Global.Icons[15];
        if (filter.ToString() == FilterPresets.presets[i])
            dgPresets.Rows[i].Selected = true;
        i++;
        dgPresets.Rows.Add(1);
        dgPresets.Rows[i].Cells[1].Value = Global.Translations.Get("travelbugs") + " " + Global.Translations.Get("droppable");
        dgPresets.Rows[i].Cells[0].Value = Global.Icons[16];
        if (filter.ToString() == FilterPresets.presets[i])
            dgPresets.Rows[i].Selected = true;
        i++;
        dgPresets.Rows.Add(1);
        dgPresets.Rows[i].Cells[1].Value = Global.Translations.Get("highlights");
        dgPresets.Rows[i].Cells[0].Value = Global.Icons[17];
        if (filter.ToString() == FilterPresets.presets[i])
            dgPresets.Rows[i].Selected = true;
        i++;
        dgPresets.Rows.Add(1);
        dgPresets.Rows[i].Cells[1].Value = Global.Translations.Get("Favorite");
        dgPresets.Rows[i].Cells[0].Value = Global.Icons[19];
        if (filter.ToString() == FilterPresets.presets[i])
            dgPresets.Rows[i].Selected = true;
        i++;
        dgPresets.Rows.Add(1);
        dgPresets.Rows[i].Cells[1].Value = Global.Translations.Get("archivable");
        dgPresets.Rows[i].Cells[0].Value = Global.Icons[22];
        if (filter.ToString() == FilterPresets.presets[i])
            dgPresets.Rows[i].Selected = true;
        i++;
        dgPresets.Rows.Add(1);
        dgPresets.Rows[i].Cells[1].Value = Global.Translations.Get("listing") + " " + Global.Translations.Get("changed");
        dgPresets.Rows[i].Cells[0].Value = Global.Icons[26];
        if (filter.ToString() == FilterPresets.presets[i])
            dgPresets.Rows[i].Selected = true;

        if (dgPresets.SelectedRows.Count == 0)
            tabControl1.SelectedIndex = 1;

        numericUpDown1.Value = filter.gpxAge;
    }

    private EventHandler InvertDisabledCheckBox(int i)
    {
      return delegate
      {
        
      };
    }

    private void FilterForm_Load(object sender, EventArgs e)
    {
        Global.Categories.ReadFromFilter(filter);
        gCategory.SelectionMode = SourceGrid.GridSelectionMode.Row;
        gCategory.Selection.EnableMultiSelection = true;
        gCategory.Selection.SelectionChanged += new SourceGrid.RangeRegionChangedEventHandler(Selection_SelectionChanged);
        fillForm();
        gCategory.AutoSizeCells();
    }

    void Selection_SelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
    {
        if (e.AddedRange == null)
            return;
        if (e.AddedRange.Count <= 0)
            return;
        if (e.AddedRange[0].Start.Row < 1)
            return;
        aktCategory = (Category)(gCategory.Rows[e.AddedRange[0].Start.Row].Tag);
        fillGpxFilenames();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (tabControl1.SelectedTab == tabPage7)
        {
            DataGridViewRow row = dgPresets.CurrentRow;
            if (row == null)
                return;
            int index = row.Cells[0].RowIndex;
            if ((index >= 0) && (index < FilterPresets.presets.Length))
            {         
                filter = new FilterProperties(FilterPresets.presets[index]);
                DialogResult = System.Windows.Forms.DialogResult.OK;
                return;
            }
        }

        // Filter Cachename
        filter.filterName = tbName.Text;
        // GPXFilaname
        filter.GPXFilenameIds.Clear();
        Global.Categories.WriteToFilter(filter);
        // General
        filter.NotAvailable = NotAvailable.State;
        filter.Archived = Archived.State;
        filter.Finds = Finds.State;
        filter.Own = Own.State;
        filter.ContainsTravelbugs = ContainsTravelBugs.State;
        filter.Favorites = Favorites.State;
        filter.HasUserData = HasUserData.State;
        filter.ListingChanged = ListingChanged.State;
        filter.WithManualWaypoint = WithManualWaypoint.State;

        // D / T
        filter.MinDifficulty = minDifficulty.CurrentValue;
        filter.MaxDifficulty = maxDifficulty.CurrentValue;
        filter.MinTerrain = minTerrain.CurrentValue;
        filter.MaxTerrain = maxTerrain.CurrentValue;
        filter.MinContainerSize = minContainerSize.CurrentValue;
        filter.MaxContainerSize = maxContainerSize.CurrentValue;
        filter.MinRating = minRating.CurrentValue;
        filter.MaxRating = maxRating.CurrentValue;

        // Cache Type
        for (int ii = 0; ii < 11; ii++)
        {
            filter.cacheTypes[ii] = enableCacheType[ii].StateBool;
        }
        // Attribute
        for (int ii = 0; ii < attributesFilter.Length; ii++)
        {
            filter.attributesFilter[ii] = attributesFilter[ii].State;
        }

        string countries = "";
        string states = "";
        if (lstCountries.CheckedItems.Count != lstCountries.Items.Count || lstStates.CheckedItems.Count != lstStates.Items.Count)
        // only if not all is checked
        // !!! perhaps todo : looks like cancel, if another filter is already set
        // !!! perhaps todo : looks like filtered, even if all contries/states are shown
        {
            foreach (string item in lstCountries.CheckedItems)
            {
                countries += item + ";";
            }
            if (countries.Length > 0)
                countries = countries.Substring(0, countries.Length - 1);
            filter.filterCountry = countries;

            foreach (string item in lstStates.CheckedItems)
            {
                if (!item.Equals(Global.Translations.Get("empty")))
                    states += item + ";";
            }
            if (states.Length > 0)
                states = states.Substring(0, states.Length - 1);
            filter.filterState = states;
        }

        filter.gpxAge = Convert.ToInt32(numericUpDown1.Value);

        DialogResult = System.Windows.Forms.DialogResult.OK;
    }



    private void gGeneral_Resize(object sender, EventArgs e)
    {
      gGeneral.AutoSizeCells();
    }

    private void gCacheType_Resize(object sender, EventArgs e)
    {
      gCacheType.AutoSizeCells();
    }

    private void grid1_Resize(object sender, EventArgs e)
    {
      gAttributes.AutoSizeCells();
    }

    private void gDT_Resize(object sender, EventArgs e)
    {
      gDT.AutoSizeCells();
    }

    private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      checkAll(true);
    }

    private void unselectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      checkAll(false);
    }

    private void checkAll(bool value)
    {
      foreach (SourceGrid.GridRow row in gGPXFilenames.Rows)
      {
        if (!(gGPXFilenames[row.Index, 0] is CellTristate))
          continue;
        (gGPXFilenames[row.Index, 0] as CellTristate).setStateBool(value);
      }
      gGPXFilenames.Refresh();
    }

    private void checkSelectionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      checkSelection(true);
    }

    private void uncheckSelectionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      checkSelection(false);
    }

    private void checkSelection(bool value)
    {
      SourceGrid.RangeRegion region = gGPXFilenames.Selection.GetSelectionRegion();
      foreach (SourceGrid.Range range in region)
      {
        for (int row = range.Start.Row; row <= range.End.Row; row++)
        {
          if (!(gGPXFilenames[row, 0] is CellTristate))
            continue;
          (gGPXFilenames[row, 0] as CellTristate).setStateBool(value);
        }
      }
      gGPXFilenames.Refresh();
    }

    private void miPinned_Click(object sender, EventArgs e)
    {
        if (aktCategory == null)
            return;
        aktCategory.Pinned = !aktCategory.Pinned;



        foreach (SourceGrid.GridRow row in gCategory.Rows)
        {
            if (!(gCategory[row.Index, 0] is CellTristate))
                continue;
            Category cat = gCategory.Rows[row.Index].Tag as Category;
            if (cat == aktCategory)
            {
                (gCategory[row.Index, 0] as CellTristate).SetImage(cat.pinned ? Global.Icons[27] : Global.Icons[20]);
            }
        }
        gGPXFilenames.Refresh();
        gCategory.Refresh();
    }

    private void gCategory_Resize(object sender, EventArgs e)
    {
        gCategory.AutoSizeCells();
    }

    private void gGPXFilenames_Resize(object sender, EventArgs e)
    {
        gGPXFilenames.AutoSizeCells();
    }

    private void lstCountries_SelectedIndexChanged(object sender, EventArgs e)
    {
        lstStates.Items.Clear();

        if (lstCountries.CheckedItems.Count == 0)
        {
            lstStates.Enabled = false;
        }
        else
        {
            lstStates.Enabled = true;

            bool empty = false;
            string countries = "";
            foreach (string item in lstCountries.CheckedItems)
            {
                if (item.Equals(Global.Translations.Get("empty")))
                    empty = true;
                else countries += "'" + item + "' ,";
            }
            if (countries.Length > 0)
                countries = countries.Substring(0, countries.Length - 2);

            string[] selectedStates = filter.filterState.Split(';');
            bool addAllAsMarked = filter.filterState.Length == 0;
            CBCommand command;
            if (empty)
                command = Database.Data.CreateCommand("SELECT DISTINCT state, country FROM Caches WHERE country IN (" + countries + ") OR [country] IS NULL ORDER BY country, state ASC ");
            else
                command = Database.Data.CreateCommand("SELECT DISTINCT state, country FROM Caches WHERE country IN (" + countries + ") ORDER BY country, state ASC ");

            System.Data.Common.DbDataReader reader = command.ExecuteReader();
            command.Dispose();
            while (reader.Read())
            {
                string state;
                try
                {
                    state = reader.GetString(0);
                    if (state.Length == 0) state = Global.Translations.Get("empty");
                }
                catch
                {
                    state = Global.Translations.Get("empty");
                };
                if (!lstStates.Items.Contains(state))
                    if (addAllAsMarked || selectedStates.Contains(state))
                        lstStates.Items.Add(state, true);
                    else
                        lstStates.Items.Add(state);
            };
        }
    }

    private void lstCountries_ItemCheck(object sender, ItemCheckEventArgs e)
    {
    }


  }

  public class CellNumeric : SourceGrid.Cells.Cell
  {
    private CellClickEvent clickController;
    protected List<Bitmap> valueImages;
    private string text;
    float minValue;
    float maxValue;
    float currentValue;
    float stepsize;
    public CellNumeric(List<Bitmap> valueImages, string text, float minValue, float maxValue, float currentValue, float stepsize)
    {
      this.valueImages = valueImages;
      this.text = text;
      this.minValue = minValue;
      this.maxValue = maxValue;
      this.currentValue = currentValue;
      this.stepsize = stepsize;
      this.View = new CustomNumericView(this);
      clickController = new CellClickEvent();
      this.AddController(clickController);
    }
    public void Up()
    {
      currentValue += stepsize;
      if (currentValue > maxValue)
        currentValue -= (maxValue - minValue + stepsize);
    }
    public void Down()
    {
      currentValue -= stepsize;
      if (currentValue < minValue)
        currentValue += (maxValue - minValue + stepsize);
    }

    public float CurrentValue { get { return currentValue; } set { currentValue = value; } }

    public class CellClickEvent : SourceGrid.Cells.Controllers.ControllerBase
    {
      public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
      {
        if (!(e is MouseEventArgs))
          return;
        if (sender.Cell is CellNumeric)
        {
          if ((e as MouseEventArgs).X > sender.Grid.DisplayRectangle.Width - 32)
          {
            // +
            (sender.Cell as CellNumeric).Up();
          }
          else if ((e as MouseEventArgs).X > sender.Grid.DisplayRectangle.Width - 64)
          {
            // -
            (sender.Cell as CellNumeric).Down();
          }
        }
        base.OnClick(sender, e);
        sender.Grid.Refresh();
      }
    }

    public class CustomNumericView : SourceGrid.Cells.Views.Cell
    {

      CellNumeric cell;
      bool[] buttonsDown = { false, false };
      public CustomNumericView(CellNumeric cell)
        : base()
      {
        this.cell = cell;
      }

      protected override SizeF OnMeasureContent(DevAge.Drawing.MeasureHelper measure, SizeF maxSize)
      {
                SizeF result = new SizeF
                {
                    Width = cell.Grid.DisplayRectangle.Width,
                    Height = 32
                };


                return result;
      }
      protected override void OnDraw(DevAge.Drawing.GraphicsCache graphics, RectangleF area)
      {
        base.OnDraw(graphics, area);
        int rowHeight = 13;
        int intent = 3;

        graphics.Graphics.DrawLine(Pens.Black, area.X, area.Y + area.Height - 1, area.Width, area.Y + area.Height - 1);

        graphics.Graphics.FillRectangle(Global.whiteBrush, area.X, area.Y, area.Width, area.Height - 1);
        Global.PutImageTargetHeight(graphics.Graphics, cell.valueImages[(int)(cell.currentValue / cell.stepsize)], (int)area.X + intent, (int)area.Y + 16, rowHeight);
        graphics.Graphics.DrawString(cell.text + ": " + cell.currentValue.ToString(System.Globalization.NumberFormatInfo.InvariantInfo), Global.normalFont, Brushes.Black, new Rectangle((int)area.X + intent, (int)area.Y, (int)area.Width, (int)area.Height - 1));

        if (buttonsDown[1])
          paintButtonDown(graphics.Graphics, (int)area.Width - (int)area.Height, (int)area.Y - 1, (int)area.Height, (int)area.Height + 1, "+");
        else
          paintButtonUp(graphics.Graphics, (int)area.Width - (int)area.Height, (int)area.Y - 1, (int)area.Height, (int)area.Height + 1, "+");

        if (buttonsDown[0])
          paintButtonDown(graphics.Graphics, (int)area.Width - (int)area.Height - (int)area.Height + 1, (int)area.Y - 1, (int)area.Height, (int)area.Height + 1, "-");
        else
          paintButtonUp(graphics.Graphics, (int)area.Width - (int)area.Height - (int)area.Height + 1, (int)area.Y - 1, (int)area.Height, (int)area.Height + 1, "-");
      }
      protected void paintButtonUp(Graphics graphics, int x, int y, int Width, int Height, String caption)
      {
        int borderThickness = 1;

        int capX = x + (int)((Width - 5) / 2);
        int capY = y + (int)((Height - 5) / 2);

        for (int i = 0; i < borderThickness; i++)
          graphics.DrawRectangle(Global.blackPen, x + i, y + i, Width - i * 2 - 1, Height - i * 2 - 1);

        graphics.FillRectangle(Global.backBrushHead, x + borderThickness, y + borderThickness, Width - borderThickness - borderThickness, Height - borderThickness - borderThickness);

        graphics.DrawString(caption, Global.boldFont, Brushes.Black, capX, capY);
      }
      protected void paintButtonDown(Graphics graphics, int x, int y, int Width, int Height, String caption)
      {
        int capX = x + (int)((Width - 5) / 2);
        int capY = y + (int)((Height - 5) / 2);

        graphics.FillRectangle(Global.blackBrush, x, y, Width, Height);

        graphics.DrawString(caption, Global.boldFont, Global.whiteBrush, capX, capY);
      }
    }
  }
  public class CellTristate : SourceGrid.Cells.Cell
  {
    private CellClickEvent clickController;
    string text;
    // -1 -> crossed, 0 -> unchecked, 1 -> checked
    int state = 0;
    Bitmap image;
    bool onlyYes = true;
    public string string2;
    public string string3;
    public float height = 26;
    public CellTristate(Bitmap image, string text, int state, bool onlyYes)
    {
      this.onlyYes = onlyYes;
      this.text = text;
      this.state = state;
      this.image = image;
      clickController = new CellClickEvent();
      this.AddController(clickController);
      this.View = new CustomTristateView(this);
      string2 = "";
      string3 = "";
    }
    public void NextState()
    {
      state++;
      if (state > 1)
      {
        if (onlyYes)
          state = 0;
        else
          state = -1;
      }
      if (TargetChanged != null)
          TargetChanged.Invoke(this);
    }

    public bool getStateBool()
    {
      return state > 0;
    }
    public void setStateBool(bool value)
    {
      if (value)
        state = 1;
      else
        state = 0;
    }
    public void SetImage(Bitmap image)
    {
        this.image = image;
    }
    public delegate void TargetChangedHandler(CellTristate cellTristate);
    public event TargetChangedHandler TargetChanged;
    public int State { get { return state; } set { state = value; } }
    public bool StateBool { get { return getStateBool(); } set { setStateBool(value); } }

    public class CellClickEvent : SourceGrid.Cells.Controllers.ControllerBase
    {
      public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
      {
        if (!(e is MouseEventArgs))
          return;
        if ((sender.Cell is CellTristate) && ((e as MouseEventArgs).X > sender.Grid.DisplayRectangle.Width - (sender.Cell as CellTristate).height))
        {
          CellTristate tri = sender.Cell as CellTristate;
          tri.NextState();
        }
        base.OnClick(sender, e);
        sender.Grid.Refresh();
      }
    }

    public class CustomTristateView : SourceGrid.Cells.Views.Cell
    {
      protected static Image imageChecked = Global.LoadRessourceBitmap("checkbox-checked.png");
      protected static Image imageUnchecked = Global.LoadRessourceBitmap("checkbox-unchecked.png");
      protected static Image imageCrossed = Global.LoadRessourceBitmap("checkbox-crossed.png");

      CellTristate cell;
      public CustomTristateView(CellTristate cell)
        : base()
      {
        this.cell = cell;
      }

      protected override SizeF OnMeasureContent(DevAge.Drawing.MeasureHelper measure, SizeF maxSize)
      {
                SizeF result = new SizeF
                {
                    Width = cell.Grid.DisplayRectangle.Width,
                    Height = cell.height
                };


                return result;
      }
      protected override void OnDraw(DevAge.Drawing.GraphicsCache graphics, RectangleF area)
      {
        base.OnDraw(graphics, area);
        Font font = (cell.string2 == "") ? Global.normalFont : Global.boldFont;
        if (font.Equals(Global.normalFont)) {
            graphics.Graphics.DrawString(cell.text, font, Brushes.Black, area.Left + 30, area.Top + 4);
        }
        else graphics.Graphics.DrawString(cell.text, font, Brushes.Black, area.Left + 30, area.Top);
        if (cell.string2 != "")
          graphics.Graphics.DrawString(cell.string2, Global.normalFont, Brushes.Black, area.Left + 30, area.Top + 13);
        if (cell.string3 != "")
        {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Far
                    };
                    graphics.Graphics.DrawString(cell.string3, Global.normalFont, Brushes.Black, area.Right - area.Height, area.Top + 13, sf);
        }

        Image checkImage;
        if (cell.state == -1)
          checkImage = imageCrossed;
        else
          if (cell.state == 0)
            checkImage = imageUnchecked;
          else
            checkImage = imageChecked;

        Global.PutImageTargetHeight(graphics.Graphics, checkImage, (int)area.Right - (int)area.Height - 1, (int)area.Top, (int)area.Height-2);
        Global.PutImageTargetHeight(graphics.Graphics, cell.image, (int)area.Left+1, (int)area.Top, (int)area.Height-2);
      }
    }
  }
}
