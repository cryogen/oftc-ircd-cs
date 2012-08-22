using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oftc_ircd_cs
{
  public class Server : BaseClient
  {
    public static Server Me { get; set; }
    public static List<Server> servers = new List<Server>();

    public string Info { get; set; }
  }
}
