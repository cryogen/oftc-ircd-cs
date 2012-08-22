using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace oftc_ircd_cs
{
  public class Logging
  {
    private static TextWriter writer;
    public static LoggingSection Conf { get; set; }

    public static void Init()
    {
      Conf = new LoggingSection();
      Config.AddSection("logging", Conf);
    }

    public static void Start()
    {
      writer = new StreamWriter(Conf.Path, true);
    }
    
    private static void Log(LogLevel level, string format)
    {
      if (level < Conf.Level)
        return;

      writer.WriteLine("{0:u} - (core) [{1}] - {2}", DateTime.Now, level.ToString(), format);

      writer.Flush();
    }

    public static void Trace(string format)
    {
      Log(LogLevel.Trace, format);
    }

    public static void Debug(string format)
    {
      Log(LogLevel.Debug, format);
    }

    public static void Info(string format)
    {
      Log(LogLevel.Info, format);
    }

    public static void Notice(string format)
    {
      Log(LogLevel.Notice, format);
    }

    public static void Warning(string format)
    {
      Log(LogLevel.Warning, format);
    }

    public static void Error(string format)
    {
      Log(LogLevel.Error, format);
    }

    public static void Critical(string format)
    {
      Log(LogLevel.Critical, format);
    }
  }
}
