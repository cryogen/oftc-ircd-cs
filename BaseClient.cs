using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    private static Dictionary<string, BaseClient> names = new Dictionary<string, BaseClient>(StringComparer.InvariantCultureIgnoreCase);
    private static List<BaseClient> unregistered = new List<BaseClient>();

    public event ClientStringEventHandler NickChanging;
    public string Name { get; set; }
    public string Host { get; set; }
    public Connection Connection { get; set; }
    public AccessLevel Level { get; set; }
    public DateTime FirstSeen { get; set; }
    public DateTime LastData { get; set; }
    public DateTime PingSent { get; set; }
    public Server Server { get; set; }

    public static void Init()
    {
      Server me = new Server();
      me.Name = General.Conf.ServerName;
      me.Info = General.Conf.ServerInfo;

      Server.Me = me;
    }

    public static BaseClient find_by_name(string name)
    {
      if (names.ContainsKey(name))
        return names[name];
      else
        return null;
    }

    public static void add_name(BaseClient client)
    {
      names.Add(client.Name, client);
    }

    public static void del_name(BaseClient client)
    {
      if(!String.IsNullOrEmpty(client.Name))
        names.Remove(client.Name);
    }

    public static void AddUnregistered(BaseClient client)
    {
      unregistered.Add(client);
    }

    public static void RemoveUnregistered(BaseClient client)
    {
      unregistered.Remove(client);
    }

    public static bool nick_changing(BaseClient client, string str)
    {
      if (client.NickChanging != null)
        return client.NickChanging(client, str);
      else
        return true;
    }

    public void Send(string message)
    {
      string str;

      if (message.Length > 510)
        str = message.Substring(0, 510);
      else
        str = message;

      str += "\r\n";

      Connection.Send(str);
    }

    public bool is_registered()
    {
      return Level > AccessLevel.Unregistered;
    }

    public override string ToString()
    {
      if (String.IsNullOrEmpty(Name))
        return "*";

      return Name;
    }
  }
}
