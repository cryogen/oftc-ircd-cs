using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oftc_ircd_cs
{
  public class Client : BaseClient
  {
    public string Username { get; set; }
    public string Realname { get; set; }

    private static List<Client> clients = new List<Client>();

    public override string ToString()
    {
      if (String.IsNullOrEmpty(Name))
        return "*";

      return String.Format("{0}!{1}@{2}", Name, Username, Host);
    }

    public static void add(Client client)
    {
      BaseClient.RemoveUnregistered(client);
      client.Level = AccessLevel.Registered;
      clients.Add(client);
    }
  }
}
