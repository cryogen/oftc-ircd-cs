using System;
using System.Collections.Generic;

namespace oftc_ircd_cs
{
    public class Client : BaseClient
    {
        private static readonly List<Client> Clients = new List<Client>();
        public string Username { get; set; }
        public string Realname { get; set; }

        public void Numeric(int numeric, params object[] args)
        {
            var num = (string) oftc_ircd_cs.Numeric.Messages[numeric.ToString("D3")];

            Send(String.Format(":{0} {1:D3} {2} {3}", Server.Me, numeric, String.IsNullOrEmpty(Name) ? "*" : Name, String.Format(num, args)));
        }

        public override string ToString()
        {
            return String.IsNullOrEmpty(Name) ? "*" : String.Format("{0}!{1}@{2}", Name, Username, Host);
        }

        public static void Add(Client client)
        {
            RemoveUnregistered(client);
            client.Level = AccessLevel.Registered;
            Clients.Add(client);

            client.Numeric(001, client.ToString());
        }
    }
}