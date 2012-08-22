using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Hosting.Providers;
using System.Reflection;
using IronPython.Runtime.Types;

namespace oftc_ircd_cs
{
  public class Module
  {
    public string Name { get; private set; }
    public string Filename { get; private set; }

    private static List<Module> modules = new List<Module>();
    public static ScriptRuntime Runtime { get; set; }
    
    public static ModuleSection Conf { get; set; }

    public Module(string name, string filename)
    {
      Name = name;
      Filename = filename;
    }

    public void Load()
    {
      Runtime.UseFile(Filename);
    }

    public static void Init()
    {
      Conf = new ModuleSection();
      Config.AddSection("module", Conf);

      Runtime = Python.CreateRuntime();

//      runtime.LoadAssembly(Assembly.GetExecutingAssembly());

      ScriptScope inner = Runtime.CreateScope();
      inner.SetVariable("Parser", DynamicHelpers.GetPythonTypeFromType(typeof(Parser)));
      inner.SetVariable("Client", DynamicHelpers.GetPythonTypeFromType(typeof(Client))); 
      inner.SetVariable("Channel", DynamicHelpers.GetPythonTypeFromType(typeof(Channel)));

      Scope scope = HostingHelpers.GetScope(inner);

      Runtime.Globals.SetVariable("pythonwrap", inner);
    }

    public static void Create(string name, string filename)
    {
      modules.Add(new Module(name, filename));
    }

    public static void LoadAll()
    {
      dynamic sys = Runtime.GetSysModule();

      foreach (string path in Conf.Paths)
      {
        sys.path.append(path);
      }
      sys.path.append("C:\\Program Files (x86)\\IronPython 2.7\\Lib");

      foreach (Module module in modules)
      {
        module.Load();
      }
    }
  }
}
