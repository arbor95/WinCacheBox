using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace WinCachebox.Geocaching
{
    public class GpxFilename
    {
        public long Id;
        public string GpxFileName;
        public DateTime Imported;
        public int CacheCount;
        public long CategoryId;
        public bool Checked;

        public GpxFilename(DbDataReader reader)
        {
            Id = reader.GetInt64(0);
            GpxFileName = reader.GetString(1);
            if (reader.IsDBNull(2))
                Imported = DateTime.Now;
            else
                Imported = reader.GetDateTime(2);
            if (reader.IsDBNull(3))
                CacheCount = 0;
            else
                CacheCount = reader.GetInt32(3);
        }
        public GpxFilename(long Id, string GpxFileName, long categoryId)
        {
            this.Id = Id;
            this.GpxFileName = GpxFileName;
            this.Imported = DateTime.Now;
            this.CategoryId = categoryId;
        }
    }

    public class Category : SortedList<long, GpxFilename>
    {
        public long Id;
        public string GpxFilename;
        public bool pinned;
        public bool Checked;
        public Category(DbDataReader reader)
        {
            Id = reader.GetInt64(0);
            GpxFilename = reader.GetString(1);
            pinned = reader.GetBoolean(2);

            // alle GpxFilenames einlesen
            CBCommand query = Database.Data.CreateCommand("select ID, GPXFilename, Imported, CacheCount from GpxFilenames where CategoryId=@CategoryId");
            query.ParametersAdd("@CategoryId", DbType.Int64, Id);
            DbDataReader reader2 = query.ExecuteReader();
            while (reader2.Read())
            {
                GpxFilename gpx = new GpxFilename(reader2);
                this.Add(gpx.Id, gpx);
            }
            reader2.Dispose();
            query.Dispose();
        }
        public Category(string filename)
        {
            // neue Category in DB anlegen
            CBCommand GPXcommand;
            GPXcommand = Database.Data.CreateCommand("insert into Category(GPXFilename) values (@GPXFilename)");
            GPXcommand.ParametersAdd("@GPXFilename", DbType.String, System.IO.Path.GetFileName(filename));
            GPXcommand.ExecuteNonQuery();
            GPXcommand.Dispose();
            int Category_ID = 0;
            GPXcommand = Database.Data.CreateCommand("Select max(ID) from Category");
            Category_ID = Convert.ToInt32(GPXcommand.ExecuteScalar().ToString());
            GPXcommand.Dispose();
            this.Id = Category_ID;
            this.GpxFilename = System.IO.Path.GetFileName(filename);
            this.Checked = true;
            this.pinned = false;
        }
        
        public GpxFilename NewGpxFilename(string fileName)
        {
            fileName = System.IO.Path.GetFileName(fileName);
            CBCommand GPXcommand;
            GPXcommand = Database.Data.CreateCommand("insert into GPXFilenames(GPXFilename, CategoryId, Imported) values (@GPXFilename, @CategoryId, @Imported)");
            GPXcommand.ParametersAdd("@GPXFilename", DbType.String, fileName);
            GPXcommand.ParametersAdd("@CategoryId", DbType.Int64, this.Id);
            GPXcommand.ParametersAdd("@Imported", DbType.DateTime, DateTime.Now);
            GPXcommand.ExecuteNonQuery();
            GPXcommand.Dispose();
            int GPXFilename_ID = 0;
            GPXcommand = Database.Data.CreateCommand("Select max(ID) from GPXFilenames");
            GPXFilename_ID = Convert.ToInt32(GPXcommand.ExecuteScalar().ToString());
            GPXcommand.Dispose();
            GpxFilename result = new GpxFilename(GPXFilename_ID, fileName, Id);
            this.Add(GPXFilename_ID, result);
            return result;
        }

        public bool Pinned
        {
            get { return pinned; }
            set
            {
                pinned = value;
                CBCommand command = Database.Data.CreateCommand("update [Category] set Pinned=@Pinned where Id=@Id");
                command.ParametersAdd("@Id", DbType.Int64, Id);
                command.ParametersAdd("@Pinned", DbType.Boolean, value);
                command.ExecuteNonQuery();
                command.Dispose();
            }
        }
        public int CacheCount()
        {
            int result = 0;
            foreach (GpxFilename gpx in this.Values)
                result += gpx.CacheCount;
            return result;
        }
        public DateTime LastImported()
        {
            if (Count == 0) return DateTime.Now;
            return this.Values[this.Count - 1].Imported;
        }
        public string GpxFilenameWoNumber()
        {
            // Nummer der PQ weglassen, wenn dahinter noch eine Bezeichnung kommt.
            string name = GpxFilename;
            int pos = name.IndexOf('_');
            if (pos < 0)
                return name;
            string part = name.Substring(0, pos);
            if (part.Length < 7)
                return name;
            try
            {
                // Vorderen Teil nur dann weglassen, wenn dies eine Zahl ist.
                Convert.ToInt32(part);
            }
            catch (Exception)
            {
                return name;
            }

            name = name.Substring(pos + 1, name.Length - pos - 1);

            if (name.ToLower().IndexOf(".gpx") == name.Length - 4)
                name = name.Substring(0, name.Length - 4);
            return name;
        }
    }

    public class Categories : SortedList<long, Category>
    {
        public Categories()
        {
            // alle Categories einlesen
            CBCommand query = Database.Data.CreateCommand("select ID, GPXFilename, Pinned from Category");
            DbDataReader reader = query.ExecuteReader();
            while (reader.Read())
            {
                Category cat = new Category(reader);
                this.Add(cat.Id, cat);
            }
            reader.Dispose();
            query.Dispose();
        }

        private void checkAll()
        {
            foreach (Category cat in this.Values)
            {
                cat.Checked = false;
                foreach (GpxFilename gpx in cat.Values)
                {
                    gpx.Checked = true;
                }
            }
        }
        public void ReadFromFilter(FilterProperties filter)
        {
            checkAll();
            bool foundOne = false;
            foreach (long id in filter.Categories)
            {
                if (this.ContainsKey(id))
                {
                    this[id].Checked = true;
                    foundOne = true;
                }
            }
            if (!foundOne)
            {
                // Wenn gar keine Category aktiv -> alle aktivieren!
                foreach (Category cat in this.Values)
                {
                    cat.Checked = true;
                }
            }
            foreach (long id in filter.GPXFilenameIds)
            {
                foreach (Category cat in this.Values)
                {
                    if (cat.ContainsKey(id))
                        cat[id].Checked = false;
                }
            }
            foreach (Category cat in this.Values)
            {
              // wenn Category nicht checked ist -> alle GpxFilenames deaktivieren
              if (cat.Checked) continue;
              foreach (GpxFilename gpx in cat.Values)
              {
                gpx.Checked = false;
              }
            }
        }
        public void WriteToFilter(FilterProperties filter)
        {
            filter.GPXFilenameIds.Clear();
            filter.Categories.Clear();
            foreach (Category cat in this.Values)
            {
                if (cat.Checked)
                {
                    // GpxFilename Filter nur setzen, wenn die Category aktiv ist!
                    filter.Categories.Add(cat.Id);
                    foreach (GpxFilename gpx in cat.Values)
                    {
                        if (!gpx.Checked)
                            filter.GPXFilenameIds.Add(gpx.Id);
                    }
                }
                else
                {
                    // Category ist nicht aktiv -> alle GpxFilenames in Filter aktivieren
                    foreach (GpxFilename gpx in cat.Values)
                        filter.GPXFilenameIds.Add(gpx.Id);
                }
            }
        }
        public Category GetCategory(string filename)
        {
            filename = System.IO.Path.GetFileName(filename);
            foreach (Category category in this.Values)
            {
                if (filename.ToUpper() == category.GpxFilename.ToUpper())
                {
                    return category;
                }
            }
            Category cat = new Category(filename);
            this.Add(cat.Id, cat);
            return cat;
        }
        public Category GetCategoryByGpxFilenameId(long gpxFilenameId)
        {
            Category result = null;
            foreach (Category category in this.Values)
            {
                if (category.ContainsKey(gpxFilenameId))
                {
                    result = category;
                }
            }
            return result;
        }
        public GpxFilename GetGpxFilename(long gpxFilenameId)
        {
          GpxFilename result = null;
          foreach (Category category in this.Values)
          {
            if (category.ContainsKey(gpxFilenameId))
            {
              result = category[gpxFilenameId];
            }
          }
          return result;
        }
        public DateTime GetLastImported(long gpxFilename_Id)
        {
          GpxFilename gpx = GetGpxFilename(gpxFilename_Id);
          if (gpx == null)
            return DateTime.Now;
          return gpx.Imported;
        }

        public void DeleteEmptyCategories()
        {
            List<Category> delete = new List<Category>();
            foreach (Category cat in this.Values)
            {
                if (cat.CacheCount() == 0)
                {
                    CBCommand command = Database.Data.CreateCommand("delete from [Category] where Id=@Id");
                    command.ParametersAdd("@Id", DbType.Int64, cat.Id);
                    command.ExecuteNonQuery();
                    command.Dispose();
                    delete.Add(cat);
                }
            }
            foreach (Category cat in delete)
                this.Remove(cat.Id);
        }
    }
}
