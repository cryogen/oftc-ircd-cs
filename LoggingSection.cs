using System;
using System.Collections.Generic;

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

    public class LoggingSection : IConfigSection
    {
        public LogLevel Level;
        public string Path { get; set; }

        #region ConfigSection Members

        public void SetDefaults()
        {
            Level = LogLevel.Info;
            Path = "ircd.log";
        }

        public void Process(object o)
        {
            var section = o as Dictionary<string, object>;

            if (section == null)
                throw new Exception("config element is not an object as expected");

            object tmp;
            string tmpLevel = "info";

            if (section.TryGetValue("log_level", out tmp))
                tmpLevel = (string) tmp;
            if (section.TryGetValue("log_path", out tmp))
                Path = (string) tmp;

            Level = (LogLevel) Enum.Parse(typeof (LogLevel), tmpLevel, true);
        }

        public void Verify()
        {
        }

        #endregion
    }
}