using System.Threading;

namespace oftc_ircd_cs
{
    internal class Program
    {
        public static ManualResetEvent AllDone = new ManualResetEvent(false);

        private static void Main()
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