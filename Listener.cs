using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace oftc_ircd_cs
{
    [Flags]
    public enum ListenerFlag
    {
        SSL = 0x1
    }

    public class Listener
    {
        private static readonly List<Listener> Listeners = new List<Listener>();
        private ListenerFlag flags;
        private Socket listener;

        public Listener(string host, ushort port, ListenerFlag flags)
        {
            Host = host;
            Port = port;
            this.flags = flags;
        }

        public static ListenerSection Conf { get; set; }
        public string Host { get; private set; }
        public ushort Port { get; private set; }

        public void Start()
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress address = String.IsNullOrEmpty(Host) ? IPAddress.Any : IPAddress.Parse(Host);

            listener.Bind(new IPEndPoint(address, Port));
            listener.Listen(100);

            listener.BeginAccept(AcceptCallback, this);
        }

        public void Connected(IAsyncResult ar)
        {
            Socket handler = listener.EndAccept(ar);

            var connection = new Connection();

            Connection.Add(connection);

            connection.Client = new Client { Connection = connection, FirstSeen = DateTime.Now, Server = Server.Me };

            BaseClient.AddUnregistered(connection.Client);

            connection.Accept(handler);
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            var listener = (Listener) ar.AsyncState;

            listener.Connected(ar);
        }

        public static void Init()
        {
            Conf = new ListenerSection();

            Config.AddSection("listeners", Conf);
        }

        public static Listener Create(string host, ushort port, ListenerFlag flags)
        {
            var listener = new Listener(host, port, flags);

            Listeners.Add(listener);

            return listener;
        }

        public static void StartListeners()
        {
            foreach (Listener listener in Listeners)
            {
                listener.Start();
            }
        }
    }
}