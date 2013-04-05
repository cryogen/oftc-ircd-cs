using System.Collections.Generic;

namespace oftc_ircd_cs
{
    public class Server : BaseClient
    {
        public static List<Server> Servers = new List<Server>();
        public static Server Me { get; set; }

        public string Info { get; set; }
    }
}