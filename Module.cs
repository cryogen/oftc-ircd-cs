using System.Collections.Generic;

namespace oftc_ircd_cs
{
    public class Module
    {
        private static readonly List<Module> Modules = new List<Module>();

        public Module(string name, string filename)
        {
            Name = name;
            Filename = filename;
        }

        public string Name { get; private set; }
        public string Filename { get; private set; }

        public static ModuleSection Conf { get; set; }

        public void Load()
        {
        }

        public static void Init()
        {
            Conf = new ModuleSection();
            Config.AddSection("module", Conf);
        }

        public static void Create(string name, string filename)
        {
            Modules.Add(new Module(name, filename));
        }

        public static void LoadAll()
        {
            foreach (Module module in Modules)
            {
                module.Load();
            }
        }
    }
}