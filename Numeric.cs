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
    private static Dictionary<string, object> messages;

    public static void LoadMessages(string path)
    {
      string json;

      using (TextReader reader = File.OpenText(path))
      {
        json = reader.ReadToEnd();
      }

      messages = (Dictionary<string, object>)((Dictionary<string, object>)JSON.Instance.Parse(json)).Values.First();
    }
  }
}
