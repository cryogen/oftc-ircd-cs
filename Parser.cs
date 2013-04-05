using System;
using System.Collections.Generic;

namespace oftc_ircd_cs
{
    public enum AccessLevel
    {
        Unregistered = 0,
        Registered
    }

    public class Parser
    {
        private static readonly Dictionary<string, Command> Commands =
                    new Dictionary<string, Command>(StringComparer.InvariantCultureIgnoreCase);

        static Parser()
        {
            Default = new Parser();
        }

        public static Parser Default { get; private set; }

        public void Parse(BaseClient client, string line)
        {
            string[] tokens = line.Split(" \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string commandArg;
            Command command;
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

            if (!Commands.TryGetValue(commandArg, out command))
            {
                if (client.IsRegistered() && client is Client)
                {
                    var c = client as Client;

                    c.Numeric(421, commandArg);
                }
                return;
            }

            if (command.MinAccess > AccessLevel.Unregistered && !client.IsRegistered())
                return;

            var args = new object[tokens.Length - argIndex + 1];
            args[0] = client;
            Array.Copy(tokens, argIndex, args, 1, tokens.Length - argIndex);
        }

        public static void Register(string name,
                                    dynamic callback,
                                    uint minArgs = 0,
                                    uint maxArgs = 0,
                                    AccessLevel access = AccessLevel.Unregistered,
                                    uint rateControl = 0)
        {
            Commands.Add(name, new Command(callback, name, access, minArgs, maxArgs, rateControl));
        }
    }
}