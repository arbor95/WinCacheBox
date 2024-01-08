using System.Drawing;

namespace DevAge.Windows.Forms
{
    public class Resources
	{
        static Resources()
        {
            using (System.IO.MemoryStream mem = new System.IO.MemoryStream(WinCachebox.Libs.SourceGrid.DevAge.Windows.Forms.Properties.Resource1.CURSOR_Right))
            {
                mRightArrow = new System.Windows.Forms.Cursor(mem);
            }
            using (System.IO.MemoryStream mem = new System.IO.MemoryStream(WinCachebox.Libs.SourceGrid.DevAge.Windows.Forms.Properties.Resource1.CURSOR_Left))
            {
                mLeftArrow = new System.Windows.Forms.Cursor(mem);
            }
        }

        public static Icon IconSortDown
		{
			get{return WinCachebox.Libs.SourceGrid.DevAge.Windows.Forms.Properties.Resource1.ICO_SortDown;}
		}
        public static Icon IconSortUp
		{
            get { return WinCachebox.Libs.SourceGrid.DevAge.Windows.Forms.Properties.Resource1.ICO_SortUp; }
		}

        private static System.Windows.Forms.Cursor mRightArrow;
        public static System.Windows.Forms.Cursor CursorRightArrow
        {
            get { return mRightArrow; }
        }
        private static System.Windows.Forms.Cursor mLeftArrow;
        public static System.Windows.Forms.Cursor CursorLeftArrow
        {
            get { return mLeftArrow; }
        }
	}
}
