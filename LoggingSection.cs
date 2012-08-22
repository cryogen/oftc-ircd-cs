using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oftc_ircd_cs
{
  public enum LogLevel
  {
    Trace = 0,
    Debug,
    Info,
    Notice,
    Warning,
    Error,
    Critical
  }

  public class LoggingSection : ConfigSection
  {
    public string Path { get; set; }
    public LogLevel Level;

    #region ConfigSection Members

    public void SetDefaults()
    {
      Level = LogLevel.Info;
      Path = "ircd.log";
    }

    public void Process(object o)
    {
      Dictionary<string, object> section = o as Dictionary<string, object>;

      if (section == null)
        throw new Exception("config element is not an object as expected");

      object tmp;
      string tmp_level = "info";

      if (section.TryGetValue("log_level", out tmp))
        tmp_level = (string)tmp;
      if (section.TryGetValue("log_path", out tmp))
        Path = (string)tmp;

      Level = (LogLevel)Enum.Parse(typeof(LogLevel), tmp_level, true);
    }

    public void Verify()
    {
    }

    #endregion
  }
}
