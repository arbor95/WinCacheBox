using GeocachingAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace WinCachebox
{
    public partial class FormDownloadPocketQuery : Form
    {
        public FormDownloadPocketQuery()
        {
            InitializeComponent();
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public void InitListOfAvailablePQs(IList<GeocacheList> pocketQueries)
        {
            int PocketQueryListValue = Config.GetInt("PocketQueryListValue");

            if (PocketQueryListValue < 0)
            {
                PocketQueryListValue = 0;
            }

            int lastNumOfPQs = PocketQueryListValue >> 24;
            int checkedValues = PocketQueryListValue & 0x00FFFFFF;

            if (lastNumOfPQs != pocketQueries.Count)
            {
                PocketQueryListValue = 0;
                checkedValues = 0;
            }

            int i = 0;

            foreach (GeocacheList pocketQuery in pocketQueries)
            {
                CBCommand PQcommand;

                PQcommand = Database.Data.CreateCommand("select max(CreationTimeOfPQ) from PocketQueries where PQName=@PQName");
                PQcommand.ParametersAdd("@PQName", DbType.String, pocketQuery.Name);

                DateTime lastImportedCreationTimeOfPQ = new DateTime();
                string dateString = PQcommand.ExecuteScalar().ToString();
                bool PQwasImported = false;

                if (!dateString.Equals(""))
                {
                    PQwasImported = true;
                    lastImportedCreationTimeOfPQ = Convert.ToDateTime(dateString);
                }

                PQcommand.Dispose();

                try
                {
                    DateTime creationDateTime = pocketQuery.LastUpdatedDateUtc.Value; //.DateLastGenerated;

                    ListViewItem item = new ListViewItem(new string[] { pocketQuery.Name, creationDateTime.ToString("dd-MM-yyyy HH:mm:ss") })
                    {
                        Checked = ((checkedValues >> i) & 0x00000001) > 0
                    };

                    if (PQwasImported)
                    {
                        if (lastImportedCreationTimeOfPQ == creationDateTime)
                        {
                            item.Checked = false;
                            item.ForeColor = Color.Gray;
                        }
                        else
                        {
                            item.Checked = true;
                        }
                    }

                    item.SubItems.Add(creationDateTime.ToString("dd-MM-yyyy HH:mm:ss"));
                    item.Tag = (object)pocketQuery;
                    listViewAvailablePQs.Items.Add(item);
                }
                catch (System.FormatException)
                {
                    // Date format does not match, ignore item.
                }

                i++;
            }
        }

        public List<GeocacheList> GetListOfSelectedPQs()
        {
            List<GeocacheList> listOfSelectedPQs = new List<GeocacheList>();

            int i = 0;
            int checkedValues = 0;

            foreach (ListViewItem item in listViewAvailablePQs.Items)
            {
                if (item.Checked == true)
                {
                    listOfSelectedPQs.Add((GeocacheList)(item.Tag));
                    checkedValues |= (1 << i);
                }

                i++;
            }

            checkedValues |= listViewAvailablePQs.Items.Count << 24;

            Config.Set("PocketQueryListValue", checkedValues);

            return listOfSelectedPQs;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

    }
}
