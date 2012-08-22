using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oftc_ircd_cs
{
  public class ModuleSection : ConfigSection
  {
    public List<string> Paths { get; private set; }
    #region ConfigSection Members

    public void SetDefaults()
    {
      Paths = new List<string>();
    }

    public void Process(object o)
    {
      Dictionary<string, object> section = o as Dictionary<string, object>;

      if (section == null)
        throw new Exception("config element is not an object as expected");

      List<object> paths = (List<object>)section["paths"];

      foreach (string s in paths)
      {
        Paths.Add(s);
      }

      Dictionary<string, object> modules = (Dictionary<string, object>)section["load"];

      foreach (KeyValuePair<string, object> module in modules)
      {
        Module.Create(module.Key, (string)module.Value);
      }
    }

    public void Verify()
    {
    }

    #endregion
  }
}
