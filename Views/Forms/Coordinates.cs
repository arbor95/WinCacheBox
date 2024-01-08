using System;
using System.Windows.Forms;
using System.Globalization;

namespace WinCachebox.Views
{
    public partial class Coordinates : Form
    {
        Coordinate coord = new Coordinate();
        UTM.Convert convert = new UTM.Convert();
        private bool doNotCheckValid = true;
        public Coordinates(Coordinate coord)
        {
            this.coord = coord;
            InitializeComponent();

            this.Text = Global.Translations.Get("solverGroupCoordinates");
            this.pParse.Text = Global.Translations.Get("Parse");
            this.tabPage1.Text = Global.Translations.Get("deg");
            //this.label5.Text = "°";
            //this.label6.Text = "°";
            this.cbDLon.Items.Clear();
            this.cbDLon.Items.AddRange(new object[] {
            Global.Translations.Get("E"),
            Global.Translations.Get("W")});
            this.cbDLat.Items.Clear();
            this.cbDLat.Items.AddRange(new object[] {
            Global.Translations.Get("N"),
            Global.Translations.Get("S")});
            //this.tbDLon.Text = "0";
            //this.tbDLat.Text = "0";
            this.tabPage2.Text = Global.Translations.Get("degmin");
            //this.label3.Text = "\'";
            //this.label4.Text = "\'";
            //this.tbMLonMin.Text = "0";
            //this.tbMLatMin.Text = "0";
            //this.label2.Text = "°";
            //this.label1.Text = "°";
            this.cbMLon.Items.Clear();
            this.cbMLon.Items.AddRange(new object[] {
            Global.Translations.Get("E"),
            Global.Translations.Get("W")});
            this.cbMLat.Items.Clear();
            this.cbMLat.Items.AddRange(new object[] {
            Global.Translations.Get("N"),
            Global.Translations.Get("S")});
            //this.tbMLonDeg.Text = "0";
            //this.tbMLatDeg.Text = "0";
            this.tabPage3.Text = Global.Translations.Get("degminsec");
            //this.label11.Text = "\'\'";
            //this.label12.Text = "\'\'";
            //this.tbSLonSec.Text = "0";
            //this.tbSLatSec.Text = "0";
            //this.label7.Text = "\'";
            //this.label8.Text = "\'";
            //this.tbSLonMin.Text = "0";
            //this.tbSLatMin.Text = "0";
            //this.label9.Text = "°";
            //this.label10.Text = "°";
            this.cbSLon.Items.Clear();
            this.cbSLon.Items.AddRange(new object[] {
            Global.Translations.Get("E"),
            Global.Translations.Get("W")});
            this.cbSLat.Items.Clear();
            this.cbSLat.Items.AddRange(new object[] {
            Global.Translations.Get("N"),
            Global.Translations.Get("S")});
            //this.tbSLonDeg.Text = "0";
            //this.tbSLatDeg.Text = "0";
            this.tabPage4.Text = Global.Translations.Get("UTM");
            //this.tbZone.Text = "0";
            //this.tbEasting.Text = "0";
            //this.tbNording.Text = "0";
            this.cbULon.Items.Clear();
            this.cbULon.Items.AddRange(new object[] {
            Global.Translations.Get("E"),
            Global.Translations.Get("W")});
            this.cbULat.Items.Clear();
            this.cbULat.Items.AddRange(new object[] {
            Global.Translations.Get("N"),
            Global.Translations.Get("S")});
            this.button1.Text = "&" + Global.Translations.Get("ok");
            this.button2.Text = "&" + Global.Translations.Get("cancel");

            tabControl1.SelectedIndex = 1;

            cbDLat.SelectedIndex = 0;
            cbDLon.SelectedIndex = 0;
            cbMLat.SelectedIndex = 0;
            cbMLon.SelectedIndex = 0;
            cbSLat.SelectedIndex = 0;
            cbSLon.SelectedIndex = 0;
            cbULat.SelectedIndex = 0;
            cbULon.SelectedIndex = 0;

            updateView();
            parseView();
        }

        public Coordinate Coord { get { return coord; } }
        private void pParse_Click(object sender, EventArgs e)
        {
            string text = tbText.Text;
            NumberFormatInfo ni = new NumberFormatInfo();
            text = text.Replace(".", Global.DecimalSeparator);
            text = text.Replace(",", Global.DecimalSeparator);

            coord = new Coordinate(text);
            if (coord.Valid)
            {
                updateView();
                this.Text = Global.FormatLatitudeDM(coord.Latitude) + " - " + Global.FormatLongitudeDM(coord.Longitude);
                return;
            }
        }

        private void updateView()
        {
            // aktuelle Koordinate ins aktuelle Register eintragen
            doNotCheckValid = true;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    // Decimalgrad
                    double deg = Math.Abs(coord.Latitude);
                    if (coord.Latitude < 0)
                        cbDLat.SelectedIndex = 1;
                    else if (coord.Latitude > 0)
                        cbDLat.SelectedIndex = 0;
                    tbDLat.Text = String.Format(Global.NumberFormatInfo, "{0:0.00000}", deg);
                    deg = Math.Abs(coord.Longitude);
                    if (coord.Longitude < 0)
                        cbDLon.SelectedIndex = 1;
                    else if (coord.Longitude > 0)
                        cbDLon.SelectedIndex = 0;
                    tbDLon.Text = String.Format(Global.NumberFormatInfo, "{0:0.00000}", deg);

                    break;
                case 1:
                    // Grad-Minuten
                    deg = Math.Abs((int)coord.Latitude);
                    double frac = Math.Abs(coord.Latitude) - deg;
                    double min = frac * 60;

                    if (coord.Latitude < 0)
                        cbMLat.SelectedIndex = 1;
                    else if (coord.Latitude > 0)
                        cbMLat.SelectedIndex = 0;
                    tbMLatDeg.Text = deg.ToString();
                    tbMLatMin.Text = String.Format(Global.NumberFormatInfo, "{0:0.000}", min);

                    deg = Math.Abs((int)coord.Longitude);
                    frac = Math.Abs(coord.Longitude) - deg;
                    min = frac * 60;

                    if (coord.Longitude < 0)
                        cbMLon.SelectedIndex = 1;
                    else if (coord.Longitude > 0)
                        cbMLon.SelectedIndex = 0;

                    tbMLonDeg.Text = deg.ToString();
                    tbMLonMin.Text = String.Format(Global.NumberFormatInfo, "{0:0.000}", min);
                    break;
                case 2:
                    // Grad-Minuten-Sekunden
                    deg = Math.Abs((int)coord.Latitude);
                    frac = Math.Abs(coord.Latitude) - deg;
                    min = frac * 60;
                    int imin = (int)min;
                    frac = min - imin;
                    double sec = frac * 60;

                    if (coord.Latitude < 0)
                        cbSLat.SelectedIndex = 1;
                    else if (coord.Latitude > 0)
                        cbSLat.SelectedIndex = 0;
                    tbSLatDeg.Text = deg.ToString();
                    tbSLatMin.Text = imin.ToString();
                    tbSLatSec.Text = String.Format(Global.NumberFormatInfo, "{0:0.00}", sec);

                    deg = Math.Abs((int)coord.Longitude);
                    frac = Math.Abs(coord.Longitude) - deg;
                    min = frac * 60;
                    imin = (int)min;
                    frac = min - imin;
                    sec = frac * 60;

                    if (coord.Longitude < 0)
                        cbSLon.SelectedIndex = 1;
                    else if (coord.Longitude > 0)
                        cbSLon.SelectedIndex = 0;
                    tbSLonDeg.Text = deg.ToString();
                    tbSLonMin.Text = imin.ToString();
                    tbSLonSec.Text = String.Format(Global.NumberFormatInfo, "{0:0.00}", sec);
                    break;
                case 3:
                    double nording = 0;
                    double easting = 0;
                    string zone = "";
                    convert.iLatLon2UTM(coord.Latitude, coord.Longitude, ref nording, ref easting, ref zone);
                    tbNording.Text = String.Format(NumberFormatInfo.InvariantInfo, "{0:0}", Math.Floor(nording));
                    tbEasting.Text = String.Format(NumberFormatInfo.InvariantInfo, "{0:0}", Math.Floor(easting));
                    tbNording.Text = Math.Round(nording, 1).ToString();
                    tbEasting.Text = Math.Round(easting, 1).ToString();
                    tbZone.Text = zone;
                    if (coord.Latitude > 0)
                        cbULat.Text = "N";
                    else if (coord.Latitude < 0)
                        cbULat.Text = "S";
                    if (coord.Longitude > 0)
                        cbULon.Text = "E";
                    else if (coord.Longitude < 0)
                        cbULon.Text = "W";
                    break;
            }
            doNotCheckValid = false;
        }

        private bool parseView()
        {
            string scoord = "";
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    scoord += cbDLat.Text + " " + tbDLat.Text + "°";
                    scoord += " " + cbDLon.Text + " " + tbDLon.Text + "°";
                    break;
                case 1:
                    scoord += cbMLat.Text + " " + tbMLatDeg.Text + "° " + tbMLatMin.Text + "'";
                    scoord += " " + cbMLon.Text + " " + tbMLonDeg.Text + "° " + tbMLonMin.Text + "'";
                    break;
                case 2:
                    scoord += cbSLat.Text + " " + tbSLatDeg.Text + "° " + tbSLatMin.Text + "' " + tbSLatSec.Text + "''";
                    scoord += " " + cbSLon.Text + " " + tbSLonDeg.Text + "° " + tbSLonMin.Text + "' " + tbSLonSec.Text + "''";
                    break;
                case 3:
                    scoord += tbZone.Text + " " + tbEasting.Text + " " + tbNording.Text;
                    break;
            }
            Coordinate tmpCoord = new Coordinate(scoord);
            if (tmpCoord.Valid)
            {
                coord = tmpCoord;
                tbText.Text = tmpCoord.FormatCoordinate();
                return true;
            }
            else
                return false;
        }

        private void Coordinates_Load(object sender, EventArgs e)
        {
            //      tbText.Text = Clipboard.GetText();
            doNotCheckValid = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tbMLonMin_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateView();
            parseView();
        }

        private void tbMLatMin_Leave(object sender, EventArgs e)
        {
            if (doNotCheckValid) return;
            if (!parseView())
            {
                if (this.ActiveControl == button2)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    return;
                }
                MessageBox.Show("Not a valid Coordinate");
                (sender as Control).Focus();
                if (sender is TextBox)
                    (sender as TextBox).SelectAll();
            }
            else
                updateView();
        }

        private void tbText_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!parseView())
            {
                tbMLatMin_Leave(this.ActiveControl, e);
                DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }
    }
}
