using System;
using System.Drawing;
using System.Windows.Forms;
using WinCachebox.Geocaching;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace WinCachebox.Views
{
    public partial class SpoilerView : DockContent
  {
    public static SpoilerView View = null;
    public Cache aktCache = null;

    private Bitmap offScreenBmp = null;
    private Bitmap picture = null;

    private string filename = "";
    public SpoilerView()
    {
      View = this;
      InitializeComponent();
      Global.TargetChanged += new Global.TargetChangedHandler(OnTargetChanged);
    }

    void OnTargetChanged(Cache cache, Waypoint waypoint)
    {
      if (cache == aktCache)
        return;  // nur der Waypoint hat sich geändert...
      aktCache = cache;
      fillSpoilerList();
      SelectedCacheChanged();
    }

    private void SelectedCacheChanged()
    {
      if (filename != "")
      {
        try
        {
            using (var tmpBmp = new Bitmap(filename))
            {
                // make copy of Bitmap because new Bitmap(filename) will lock file on HD. This causes Problems with Spoiler update
                picture = new Bitmap(tmpBmp);
            }
        }
        catch (Exception)
        {
          picture = new Bitmap(200, 200);
          Graphics graphics = Graphics.FromImage(picture);
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    graphics.DrawString("File ERROR", Global.normalFont, Brushes.Black, 100, 100, sf);
          graphics.Dispose();
        }
        String name = Path.GetFileNameWithoutExtension(filename);
          // remove Hash at the end of spoiler filenames
          if (name.EndsWith("@")) {
              int pos = name.IndexOf("@");
              if (pos > 0) {
                  name = name.Substring(0, pos);
              }
          }
        label1.Text = name;
        zoom = 1;
        center = new SizeF(50, 50);
        if (picture != null)
          center = new SizeF(picture.Width / 2, picture.Height / 2);
        createOffScreenBitmap();
        checkZoomPan();
        drawImage();
//        Refresh();
      }
      else
      {
        picture = null;
        label1.Text= "No Spoiler";
        createOffScreenBitmap();
        Refresh();
      }
    }
    
    private void SpoilerView_Paint(object sender, PaintEventArgs e)
    {
      if (offScreenBmp == null)
        return;
      e.Graphics.DrawImage(offScreenBmp, 0, 0);
    }

    private void SpoilerView_Resize(object sender, EventArgs e)
    {
      createOffScreenBitmap();
      checkZoomPan();
      drawImage();
    }

    private void createOffScreenBitmap()
    {
      offScreenBmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
      if (picture == null)
        return;
      //      drawImage();
    }

    float zoom = 1;
    float zoomFaktor = 1;

    SizeF center = new SizeF(0, 0);
    private void drawImage()
    {
      if (picture == null)
        return;
      if ((offScreenBmp.Width == 0) || (offScreenBmp.Height == 0))
        return;
      Graphics graphics = Graphics.FromImage(offScreenBmp);
      graphics.FillRectangle(Brushes.White, 0, 0, offScreenBmp.Width, offScreenBmp.Height);
            PointF centerOffset = new PointF
            {
                X = offScreenBmp.Width / zoomFaktor / 2,
                Y = offScreenBmp.Height / zoomFaktor / 2
            };
            RectangleF dest = new RectangleF(0, 0, offScreenBmp.Width, offScreenBmp.Height);
      RectangleF source = new RectangleF(/*picture.Width * pan.Width*/center.Width - centerOffset.X, /*picture.Height * pan.Height*/center.Height - centerOffset.Y, picture.Width / zoom, picture.Height / zoom);
      float faktorD = dest.Width / dest.Height;
      float faktorS = source.Width / source.Height;
      if (faktorD > faktorS)
      {
        source.Width = source.Height * faktorD;
      }
      else
      {
        source.Height = source.Width / faktorD;
      }
  /*    if (source.Right > picture.Width)
      {
        if (source.Width > picture.Width)
          source.X -= (source.Right - picture.Width) / 2;
        else
          source.X -= source.Right - picture.Width;
      }
/*
      if (source.Bottom > picture.Height)
        if (source.Height > picture.Height)
          source.Y -= (source.Bottom - picture.Height) / 2;
        else
          source.Y -= source.Bottom - picture.Height;
      */

      graphics.DrawImage(picture, dest, source, GraphicsUnit.Pixel);
      graphics.Dispose();
      pictureBox1.CreateGraphics().DrawImage(offScreenBmp, 0, 0);
    }

    private void SpoilerView_KeyPress(object sender, KeyPressEventArgs e)
    {

    }

    private void SpoilerView_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.Add:
          zoom = zoom * 1.333f;
          checkZoomPan();
          drawImage();
//          this.Refresh();
          break;
        case Keys.Subtract:
          zoom = zoom / 1.333f;
          checkZoomPan();
          drawImage();
//          this.Refresh();
          break;
      }
    }

    private void checkZoomPan()
    {
      if (picture == null)
        return;
      if (zoom < 1)
        zoom = 1;
      float zoomFX = (float)offScreenBmp.Width / (float)picture.Width;
      float zoomFY = (float)offScreenBmp.Height / (float)picture.Height;
      zoomFaktor = Math.Min(zoomFX, zoomFY) * zoom;

            // größe des Anzeigefensters in Bildkoordinaten
            PointF viewSize = new PointF
            {
                X = offScreenBmp.Width / zoomFaktor,
                Y = offScreenBmp.Height / zoomFaktor
            };

            if (center.Width < viewSize.X / 2)
        center.Width = viewSize.X / 2;
      if (center.Width > picture.Width - viewSize.X / 2)
        center.Width = picture.Width - viewSize.X / 2;
      if (center.Height < viewSize.Y / 2)
        center.Height = viewSize.Y / 2;
      if (center.Height > picture.Height - viewSize.Y / 2)
        center.Height = picture.Height - viewSize.Y / 2;

      if (picture.Width < viewSize.X)
        center.Width = picture.Width / 2;
      if (picture.Height < viewSize.Y)
        center.Height = picture.Height / 2;
    }

    private Point mouseDownPos = new Point(0, 0);
    private bool mouseDown = false;
    private void SpoilerView_MouseDown(object sender, MouseEventArgs e)
    {
      mouseDown = true;
      mouseDownPos = new Point(e.X, e.Y);
    }

    private void SpoilerView_MouseMove(object sender, MouseEventArgs e)
    {
      if (!mouseDown) return;
      Point delta = new Point(e.X - mouseDownPos.X, e.Y - mouseDownPos.Y);
      center.Width -= (float)delta.X / zoomFaktor;
      center.Height -= (float)delta.Y / zoomFaktor;

      checkZoomPan();
      drawImage();
      mouseDownPos = new Point(e.X, e.Y);
//      this.Refresh();
    }

    private void SpoilerView_MouseUp(object sender, MouseEventArgs e)
    {
      mouseDown = false;
    }

    private void fillSpoilerList()
    {
        for (int ii = 0; ii < gSpoiler.Columns.Count; ii++)
        {
            CustomSpoilerView csv = gSpoiler[0, ii].View as CustomSpoilerView;
            if (csv != null)
                csv.bitmap.Dispose();
        }
      filename = "";
      gSpoiler.RowsCount = 1;
      gSpoiler.ColumnsCount = 0;
      if (Global.SelectedCache == null)
        return;
      int i = 0;
      foreach (string spoiler in Global.SelectedCache.SpoilerRessources)
      {
        gSpoiler.Columns.Insert(i);
        SourceGrid.Cells.Cell cell;
                cell = new SourceGrid.Cells.Cell
                {
                    Tag = spoiler
                };
                gSpoiler[0, i] = cell;
        cell.View = new CustomSpoilerView(spoiler, cell);
        cell.ToolTipText = Path.GetFileNameWithoutExtension(spoiler);
        if (filename == "")
          filename = spoiler;
        i++;
      }
      gSpoiler.AutoSizeCells();
      splitContainer1.Panel1Collapsed = Global.SelectedCache.SpoilerRessources.Count <= 1;
    }

    void Selection_SelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
    {
      if (e.AddedRange == null)
        return;
      if (e.AddedRange.Count <= 0)
        return;
      if (e.AddedRange[0].Start.Column < 0)
        return;
      filename = (string)gSpoiler[0, e.AddedRange[0].Start.Column].Tag;
      SelectedCacheChanged();
    }

    private void SpoilerView_Load(object sender, EventArgs e)
    {
      gSpoiler.Selection.EnableMultiSelection = false;
      gSpoiler.Selection.SelectionChanged += new SourceGrid.RangeRegionChangedEventHandler(Selection_SelectionChanged);
    }

    private void button2_Click(object sender, EventArgs e)
    {
      zoom = zoom * 1.333f;
      checkZoomPan();
      drawImage();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      zoom = zoom / 1.333f;
      checkZoomPan();
      drawImage();
    }

    private class CustomSpoilerView : SourceGrid.Cells.Views.Cell
    {
      SourceGrid.Cells.Cell cell;
      string filename;
      public Bitmap bitmap;
      public CustomSpoilerView(string filename, SourceGrid.Cells.Cell cell)
      {
        this.filename = filename;
        try
        {
            if (bitmap != null)
                bitmap.Dispose();
            // first load the Bitmap into a temp Object and then create the new from the temp
            // because loading a Bitmap directly from file will lock this file for Spoiler Update
            using (var tmpBmp = new Bitmap(filename))
            {
                bitmap = new Bitmap(tmpBmp);
            }
        }
        catch (Exception)
        {
          bitmap = null;
        }
        this.cell = cell;

        TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
        //        base.Background = mBackground;
        Border = DevAge.Drawing.RectangleBorder.NoBorder;
        this.WordWrap = true;
      }
      protected override SizeF OnMeasureContent(DevAge.Drawing.MeasureHelper measure, SizeF maxSize)
      {
        int cellHeight = cell.Grid.DisplayRectangle.Height;
                SizeF sizeF = new SizeF
                {
                    Height = cellHeight,
                    Width = cellHeight
                };
                return sizeF;
      }
      protected override void OnDraw(DevAge.Drawing.GraphicsCache graphics, RectangleF area)
      {
        base.OnDraw(graphics, area);
        if (Global.SelectedCache == null)
          return;
        float faktorW = area.Width / bitmap.Width;
        float faktorH = area.Height / bitmap.Height;
        float x = area.X;
        float y = area.Y;
        float width = area.Width;
        float height = area.Height;
        if (faktorW > faktorH)
        {
          // change width
          float diff = bitmap.Width - bitmap.Width / faktorW * faktorH;
          diff = diff / bitmap.Width * area.Width;
          x += diff / 2;
          width -= diff;
        }
        else
        {
          // change height
          float diff = bitmap.Height - bitmap.Height / faktorH * faktorW;
          diff = diff / bitmap.Height * area.Height;
          y += diff / 2;
          height -= diff;
        }
        if (bitmap != null)
          graphics.Graphics.DrawImage(bitmap, x, y, width, height);
        else
          graphics.Graphics.DrawString("FileNotFound!", Global.normalFont, Brushes.Black, x, y);
/*
        graphics.Graphics.FillRectangle(Global.backBrushHead, area.X, area.Y, area.Width, headerHeight);
        int space = (logEntry.TypeIcon >= 0) ? Global.PutImageTargetHeight(graphics.Graphics, Global.LogIcons[logEntry.TypeIcon], 0, (int)area.Y, headerHeight) + 4 : 0;

        String dateString = logEntry.Timestamp.ToShortDateString();
        int dateWidth = (int)graphics.Graphics.MeasureString(dateString, font).Width;
        graphics.Graphics.DrawString(logEntry.Finder, Global.boldFont, Global.blackBrush, new RectangleF(space, area.Y, area.Width - space - dateWidth - 5, headerHeight));
        graphics.Graphics.DrawString(dateString, font, Global.blackBrush, area.Width - dateWidth, area.Y);
        graphics.Graphics.DrawLine(Global.blackPen, 0, area.Y + area.Height - 1, area.Width, area.Y + area.Height - 1);


        RectangleF rect = area;

        rect.Height -= headerHeight;
        rect.Offset(0, headerHeight);
        graphics.Graphics.DrawString(logEntry.Comment, font, Brushes.Black, rect);*/
      }
    }

    private void gSpoiler_Resize(object sender, EventArgs e)
    {
      gSpoiler.AutoSizeCells();
    }

    private void downloadSpoilersToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (Config.GetBool("AllowInternetAccess"))
      {
        if (Global.SelectedCache != null)
        {
          if (MessageBox.Show("Really download spoiler?", "Download Spoiler", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
          {
            Views.Forms.FormDownloadCacheImages formDownloadCacheImages = new Views.Forms.FormDownloadCacheImages(Global.SelectedCache.Id);
            formDownloadCacheImages.ShowDialog();

            // reload Spoilers in SelectedCache object.
            Global.SelectedCache.ReloadSpoilerRessources();

            if (Global.SelectedCache.SpoilerExists)
            {
              fillSpoilerList();
              SelectedCacheChanged();
            }
            else
            {
              MessageBox.Show("No Spoilers available.", "Download Spoiler");
            }
          }
        }
        else
        {
          MessageBox.Show("No cache selected.", "Download Spoiler");
        }
      }
      else
      {
        MessageBox.Show("Internet access not allowed! Enable it in the settings to download spoilers.", "Download Spoiler");
      }

    }

    private void importSpoilerFromURLToolStripMenuItem_Click(object sender, EventArgs e)
    {
      string url = Clipboard.GetText();

      if (Config.GetBool("AllowInternetAccess"))
      {
        if (aktCache != null)
        {
          string message = "Really download spoiler picture from URL:";
          message += Environment.NewLine;
          message += Environment.NewLine;
          message += url;
          if (MessageBox.Show(message, "Download Spoiler", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
          {
            try
            {
              String local = "";
              int i = 0;
              do
              {
                string name = "Spoiler";
                if (i > 0)
                  name += "_" + i.ToString();
                local = DescriptionImageGrabber.BuildAdditionalImageFilename(aktCache.GcCode, name, new Uri(url));
                if ((i == 0) && (File.Exists(local)))
                {
                  if (MessageBox.Show("Spoiler [" + Path.GetFileName(local) + "] already exists! Download Spoiler nevertheless?", "Download Spoiler", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;
                }
                i++;
              } while (File.Exists(local));
              if (DescriptionImageGrabber.Download(url, local))
              {
                // reload Spoilers in SelectedCache object.
                Global.SelectedCache.ReloadSpoilerRessources();

                if (Global.SelectedCache.SpoilerExists)
                {
                  fillSpoilerList();
                  SelectedCacheChanged();
                }
                MessageBox.Show("Spoiler Download OK!", "Spoiler Download");
              }
              else
              {
                MessageBox.Show("Image" + Environment.NewLine + url + Environment.NewLine + "could not be loaded!", "Download Spoiler");
              }
            }
            catch (Exception)
            {
              MessageBox.Show(url + Environment.NewLine + "is not a valid Image-URL!", "Download Spoiler");
            }
          }
        }
        else
        {
          MessageBox.Show("No cache selected.", "Download Spoiler");
        }
      }
      else
      {
        MessageBox.Show("Internet access not allowed! Enable it in the settings to download spoilers.", "Download Spoiler");
      }
    }
  }
}
