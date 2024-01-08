using System;
using System.IO;
using System.Windows.Forms;

namespace WinCachebox.Views.Forms
{
    public partial class OpenDB : Form
    {
        private ListViewItem _SelectedDB;
        public ListViewItem SelectedDB {
            get
            {
                return _SelectedDB;
            }
        }

        public OpenDB()
        {
            InitializeComponent();
        }

        private void OpenDB_Load(object sender, EventArgs e)
        {
            Text = Global.Translations.Get("openDB");
            cancel.Text= Global.Translations.Get("cancel");
            ok.Text= Global.Translations.Get("ok");

            DBList.FullRowSelect = true;
            DBList.GridLines = true;
            DBList.Items.Clear();

            DBList.View = View.Details;
            DBList.Columns.Add(Global.Translations.Get("Database Name"));
            DBList.Columns.Add(Global.Translations.Get("Database Type"));
            DBList.Columns.Add(Global.Translations.Get("Own Repository"));

            AddFiles("*.sdf");
            AddFiles("*.db3");
            // for size adjustment of columns
            ListViewItem lvi = DBList.Items.Add(Global.Translations.Get("Database Name") + "     ");
            lvi.SubItems.Add(Global.Translations.Get("Database Type"));
            lvi.SubItems.Add(Global.Translations.Get("Own Repository"));
            DBList.Columns[0].Width = -1;
            DBList.Columns[1].Width = -1;
            DBList.Columns[2].Width = -1;
            DBList.Items.Remove(lvi);
        }

        private void AddFiles(string extension)
        {
            String DataPath = Path.GetDirectoryName(Global.databaseName); //Config.GetString("DatabasePath")
            DirectoryInfo d = new DirectoryInfo(DataPath);
            FileInfo[] Files = d.GetFiles(extension);
            foreach (FileInfo file in Files)
            {
                String DBName = Path.GetFileNameWithoutExtension(file.Name);
                if (!DBName.Equals(Path.GetFileNameWithoutExtension(Global.databaseName)))
                {
                    ListViewItem lvi = DBList.Items.Add(DBName);
                    String DBType = file.Extension.Substring(1);
                    lvi.SubItems.Add(DBType);
                    if (Directory.Exists(DataPath + "\\Repositories\\" + DBName))
                    {
                        lvi.SubItems.Add("true");
                    }
                    else
                    {
                        lvi.SubItems.Add("false");
                    }
                }
            }
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (DBName.Text.Length > 0)
            {
                _SelectedDB = new ListViewItem(DBName.Text);
                _SelectedDB.SubItems.Add(DBType.Text);
                _SelectedDB.SubItems.Add(OwnRepository.Text);
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }

            if (DialogResult == DialogResult.OK)
            {
                if (Database.Data.Connection != null)
                {
                    Database.Data.Connection.Close();
                    Database.Data.Connection.Dispose();
                }
            }

        }

        private void DBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DBList.SelectedItems.Count == 1)
            {
                _SelectedDB = DBList.SelectedItems[0];
                DBName.Text = _SelectedDB.SubItems[0].Text;
                DBType.Text = _SelectedDB.SubItems[1].Text;
                OwnRepository.Text = _SelectedDB.SubItems[2].Text;
            }
            else
            {
                DBName.Text = "";
            }
        }
    }
}
