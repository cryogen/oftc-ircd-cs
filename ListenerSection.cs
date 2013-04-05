using System;
using System.Collections.Generic;

namespace oftc_ircd_cs
{
  public class ListenerSection : IConfigSection
  {
    #region ConfigSection Members

    public void SetDefaults()
    {
    }

    public void Process(object o)
    {
      var section = o as List<object>;

      if (section == null)
        throw new Exception("config section was not an array as expected");

      foreach (Dictionary<string, object> element in section)
      {
        ListenerFlag flags = 0;
        string host = "";
        long port = 6667;
        object tmp;

        if (element.TryGetValue("host", out tmp))
          host = (string)tmp;
        if (element.TryGetValue("port", out tmp))
          port = (long)tmp;
        if (element.ContainsKey("ssl"))
          flags |= ListenerFlag.SSL;

        if (port < 1024 || port > ushort.MaxValue)
          continue;

        Listener.Create(host, (ushort)port, flags);
      }
    }

    public void Verify()
    {
    }

    #endregion
  }
}
