using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace oftc_ircd_cs
{
  [Flags]
  public enum ListenerFlag
  {
    SSL = 0x1
  }

  public class Listener
  {
    private static List<Listener> listeners = new List<Listener>();
    private Socket listener;
    private ListenerFlag flags;
    
    public static ListenerSection Conf { get; set; }
    public string Host { get; private set; }
    public ushort Port { get; private set; }

    public Listener(string host, ushort port, ListenerFlag _flags)
    {
      Host = host;
      Port = port;
      flags = _flags;
    }

    public void Start()
    {
      IPAddress address;

      listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

      if (String.IsNullOrEmpty(Host))
        address = IPAddress.Any;
      else
        address = IPAddress.Parse(Host);

      listener.Bind(new IPEndPoint(address, Port));
      listener.Listen(100);

      listener.BeginAccept(new AsyncCallback(AcceptCallback), this);
    }

    public void Connected(IAsyncResult ar)
    {
      Socket handler = listener.EndAccept(ar);

      Connection connection = new Connection();

      Connection.Add(connection);

      connection.Client = new Client();
      connection.Client.Connection = connection;
      connection.Client.FirstSeen = DateTime.Now;
      connection.Client.Server = Server.Me;

      BaseClient.AddUnregistered(connection.Client);

      connection.Accept(handler);
    }

    public static void AcceptCallback(IAsyncResult ar)
    {
      Listener listener = (Listener)ar.AsyncState;

      listener.Connected(ar);
    }

    public static void Init()
    {
      Conf = new ListenerSection();

      Config.AddSection("listeners", Conf);
    }

    public static Listener Create(string host, ushort port, ListenerFlag flags)
    {
      Listener listener = new Listener(host, port, flags);

      listeners.Add(listener);

      return listener;
    }

    public static void StartListeners()
    {
      foreach (Listener listener in listeners)
      {
        listener.Start();
      }
    }
  }
}
