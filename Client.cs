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

    public void numeric(int numeric, params object[] args)
    {
      string num = (string)Numeric.Messages[numeric.ToString("D3")];

      send(String.Format(":{0} {1:D3} {2} {3}", Server.Me, numeric, String.IsNullOrEmpty(Name) ? "*" : Name, String.Format(num, args)));
    }

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

      client.numeric(001, client.ToString());
    }
  }
}
