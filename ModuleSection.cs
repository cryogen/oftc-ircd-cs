using System;
using System.Collections.Generic;

namespace oftc_ircd_cs
{
    public class ModuleSection : IConfigSection
    {
        public List<string> Paths { get; private set; }

        #region ConfigSection Members

        public void SetDefaults()
        {
            Paths = new List<string>();
        }

        public void Process(object o)
        {
            var section = o as Dictionary<string, object>;

            if (section == null)
                throw new Exception("config element is not an object as expected");

            var paths = (List<object>) section["paths"];

            foreach (string s in paths)
            {
                Paths.Add(s);
            }

            var modules = (Dictionary<string, object>) section["load"];

            foreach (var module in modules)
            {
                Module.Create(module.Key, (string) module.Value);
            }
        }

        public void Verify()
        {
        }

        #endregion
    }
}