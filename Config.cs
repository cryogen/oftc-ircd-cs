using System;
using System.Collections.Generic;
using System.IO;
using fastJSON;

namespace oftc_ircd_cs
{
    [Serializable]
    public class Config
    {
        private static readonly Dictionary<string, IConfigSection> ConfigSections =
                    new Dictionary<string, IConfigSection>();

        public static void Load()
        {
            string json;

            foreach (var section in ConfigSections.Values)
            {
                section.SetDefaults();
            }

            using (TextReader reader = File.OpenText("ircd.conf"))
            {
                json = reader.ReadToEnd();
            }

            var tmp = JSON.Instance.Parse(json);
            var root = tmp as Dictionary<string, object>;

            if (root == null)
                throw new Exception("Config root is not an object");

            foreach (var element in root)
            {
                IConfigSection section;

                if (!ConfigSections.TryGetValue(element.Key, out section))
                    continue;

                section.Process(element.Value);
            }
        }

        public static void AddSection(string name, IConfigSection section)
        {
            ConfigSections.Add(name, section);
        }
    }
}