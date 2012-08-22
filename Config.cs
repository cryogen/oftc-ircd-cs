using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fastJSON;
using System.IO;

namespace oftc_ircd_cs
{
  [Serializable]
  public class Config
  {
    private static Dictionary<string, ConfigSection> configSections = new Dictionary<string, ConfigSection>();

    public static void Load()
    {
      string json;

      foreach (ConfigSection section in configSections.Values)
      {
        section.SetDefaults();
      }

      using(TextReader reader = File.OpenText("ircd.conf"))
      {
        json = reader.ReadToEnd();
      }

      var tmp = JSON.Instance.Parse(json);
      Dictionary<string, object> root = tmp as Dictionary<string, object>;

      if (root == null)
        throw new Exception("Config root is not an object");

      foreach (var element in root)
      {
        ConfigSection section;

        if (!configSections.TryGetValue(element.Key, out section))
          continue;

        section.Process(element.Value);
      }
    }

    public static void AddSection(string name, ConfigSection section)
    {
      configSections.Add(name, section);
    }
  }
}
