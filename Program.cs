using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace oftc_ircd_cs
{
  class Program
  {
    public static ManualResetEvent allDone = new ManualResetEvent(false);

    static void Main(string[] args)
    {
      General.Init();
      Listener.Init();
      Logging.Init();
      Module.Init();
      Config.Load();

      Numeric.LoadMessages(General.Conf.MessagesFile);
      BaseClient.Init();
      Logging.Start();

      Logging.Info("oftc-ircd-cs starting up");

      Module.LoadAll();
      Listener.StartListeners();

      while (true)
      {
      }
    }
  }
}
