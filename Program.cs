using System;
using System.Windows.Forms;

namespace WinCachebox
{
    static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        try
        {
            Form1 form = new Form1();
            Global.Initialized = true;
            Application.Run(form);
        }
        catch (Exception exc)
        {

#if DEBUG
            Global.AddLog("Main: " + exc.ToString());
            MessageBox.Show("Main: " + exc.ToString(), "Sorry!");
#endif
        }
        finally
        {
            if (Database.Data.Connection != null)
            {
                Database.Data.Connection.Close();
                Database.Data.Connection.Dispose();
            }
        }
    }
  }
}
