namespace DevAge.Shell
{
    /// <summary>
    /// Shell utilities
    /// </summary>
    public class Utilities
	{
		public static void OpenFile(string p_File)
		{
			ExecCommand(p_File);
		}

		public static void ExecCommand(string p_Command)
		{
            System.Diagnostics.ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo(p_Command)
            {
                UseShellExecute = true
            };
            System.Diagnostics.Process process = new System.Diagnostics.Process
            {
                StartInfo = p
            };
            process.Start();
		}
	}
}
