using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Runtime;

namespace oftc_ircd_cs
{
  public enum AccessLevel
  {
    Unregistered = 0,
    Registered
  }

  public class Parser
  {
    private static Dictionary<string, Command> commands = new Dictionary<string, Command>(StringComparer.InvariantCultureIgnoreCase);
    public static Parser Default { get; private set; }

    static Parser()
    {
      Default = new Parser();
    }

    public void Parse(BaseClient client, string line)
    {
      string[] tokens = line.Split(" \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
      string commandArg;
      Command command;
      object[] args;
      int argIndex;

      if (tokens[0][0] == ':')
      {
        commandArg = tokens[1];
        argIndex = 2;
      }
      else
      {
        commandArg = tokens[0];
        argIndex = 1;
      }

      if (!commands.TryGetValue(commandArg, out command))
      {
        if (client.is_registered() && client is Client)
        {
          Client c = client as Client;

          c.numeric(421, commandArg);
        }
        return;
      }

      if (command.MinAccess > AccessLevel.Unregistered && !client.is_registered())
        return;

      args = new object[tokens.Length - argIndex + 1];
      args[0] = client;
      Array.Copy(tokens, argIndex, args, 1, tokens.Length - argIndex);

      try
      {
        Module.Runtime.Operations.Invoke(command.Handler, args);
      }
      catch (Exception ex)
      {
      }
    }

    public static void Register(string name, dynamic callback, uint min_args=0, uint max_args=0, int access=(int)AccessLevel.Unregistered, uint rate_control=0)
    {
      commands.Add(name, new Command(callback, name, (AccessLevel)access, min_args, max_args, rate_control));
    }
  }
}
