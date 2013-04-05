using System;
using System.Collections.Generic;

namespace oftc_ircd_cs
{
    public delegate bool ClientStringEventHandler(BaseClient client, string str);

    public class ClientStringEventArgs : EventArgs
    {
        public BaseClient Client { get; private set; }
        public string Str { get; private set; }

        public ClientStringEventArgs(BaseClient client, string str)
        {
            Client = client;
            Str = str;
        }
    }

    public class BaseClient
    {
        private static readonly Dictionary<string, BaseClient> Names =
                    new Dictionary<string, BaseClient>(StringComparer.InvariantCultureIgnoreCase);

        private static readonly List<BaseClient> Unregistered = new List<BaseClient>();

        public string Name { get; set; }
        public string Host { get; set; }
        public Connection Connection { get; set; }
        public AccessLevel Level { get; set; }
        public DateTime FirstSeen { get; set; }
        public DateTime LastData { get; set; }
        public DateTime PingSent { get; set; }
        public Server Server { get; set; }
        public event ClientStringEventHandler NickChanging;

        public static void Init()
        {
            var me = new Server { Name = General.Conf.ServerName, Info = General.Conf.ServerInfo };

            Server.Me = me;
        }

        public static BaseClient FindByName(string name)
        {
            if (Names.ContainsKey(name))
                return Names[name];

            return null;
        }

        public static void AddName(BaseClient client)
        {
            Names.Add(client.Name, client);
        }

        public static void DeleteName(BaseClient client)
        {
            if (!String.IsNullOrEmpty(client.Name))
                Names.Remove(client.Name);
        }

        public static void AddUnregistered(BaseClient client)
        {
            Unregistered.Add(client);
        }

        public static void RemoveUnregistered(BaseClient client)
        {
            Unregistered.Remove(client);
        }

        public static bool nick_changing(BaseClient client, string str)
        {
            return client.NickChanging == null || client.NickChanging(client, str);
        }

        public void Send(string message)
        {
            string str = message.Length > 510 ? message.Substring(0, 510) : message;

            str += "\r\n";

            Connection.Send(str);
        }

        public bool IsRegistered()
        {
            return Level > AccessLevel.Unregistered;
        }

        public override string ToString()
        {
            return String.IsNullOrEmpty(Name) ? "*" : Name;
        }
    }
}