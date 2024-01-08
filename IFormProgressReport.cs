using System;

namespace WinCachebox
{
    public interface IFormProgressReport
  {
    bool Cancel
    {
      get;
    }
    void ReportUncriticalError(string error);
    void setTitle(String text);
    void ProgressChanged(string activity, int processed, int total);
    void ProgressChangedTotal(string activity, int processed, int total);
    bool PerformMemoryTest(String path, int neededKb);
  }
}
