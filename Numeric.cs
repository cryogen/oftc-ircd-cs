using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fastJSON;
using System.IO;

namespace oftc_ircd_cs
{
  public class Numeric
  {
    public static Dictionary<string, object> Messages { get; private set; }

    public static void LoadMessages(string path)
    {
      string json;

      using (TextReader reader = File.OpenText(path))
      {
        json = reader.ReadToEnd();
      }

      Messages = (Dictionary<string, object>)((Dictionary<string, object>)JSON.Instance.Parse(json)).Values.First();
    }
  }
}
