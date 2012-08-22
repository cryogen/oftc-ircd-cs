using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace oftc_ircd_cs
{

  public class Connection
  {
    public Client Client { get; set; }
    public string Host { get; private set; }

    private static List<Connection> connections = new List<Connection>();

    private Socket socket;
    private string dnsIp;
    private byte[] buffer = new byte[4096];
    private StringBuilder stringBuffer;
    private StringWriter writer;
    private Parser parser;

    public Connection()
    {
      parser = Parser.Default;
    }

    public void OnSend(IAsyncResult ar)
    {
      socket.EndSend(ar);
    }

    public void Send(string message)
    {
      byte[] buffer = Encoding.ASCII.GetBytes(message);

      socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), this);
    }

    public void OnDns(IAsyncResult ar)
    {
      IPHostEntry result = Dns.EndGetHostEntry(ar);

      bool found = false;

      foreach(IPAddress addr in result.AddressList)
      {
        if (addr.ToString() == dnsIp)
        {
          found = true;
          break;
        }
      }

      if (!found)
        Client.Send(String.Format(":{0} NOTICE {1} :*** Your forward and reverse DNS don't match, ignoring.", Server.Me, Client));
      else
        Client.Send(String.Format(":{0} NOTICE {1} :*** Found your hostname.", Server.Me, Client));

      socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), this);
    }

    public void Read(IAsyncResult ar)
    {
      int bytes = socket.EndReceive(ar);

      string buf = Encoding.ASCII.GetString(buffer).Trim('\0');

      writer.Write(buf);

      string tmp = stringBuffer.ToString();

      int index = tmp.LastIndexOfAny("\r\n".ToCharArray());

      stringBuffer.Clear();

      int lenLeft = tmp.Length - index - 1;

      if (lenLeft > 0 && index < tmp.Length - 1)
        writer.Write(tmp.Substring(index + 1, lenLeft));

      if (index != -1)
      {
        string[] lines = tmp.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
          parser.Parse(Client, line);
        }
      }

      socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), this);
    }

    public void Accept(Socket _socket)
    {
      socket = _socket;

      IPEndPoint endpoint = (IPEndPoint)socket.RemoteEndPoint;

      Host = endpoint.Address.ToString();
      Client.Host = Host;

      Client.Send(String.Format(":{0} NOTICE {1} :*** Looking up your hostname", Server.Me, Client));

      dnsIp = Host;
      Dns.BeginGetHostEntry(dnsIp, new AsyncCallback(DnsCallback), this);

      stringBuffer = new StringBuilder();
      writer = new StringWriter(stringBuffer);
    }

    public static void SendCallback(IAsyncResult ar)
    {
      Connection connection = (Connection)ar.AsyncState;

      connection.OnSend(ar);
    }

    public static void DnsCallback(IAsyncResult ar)
    {
      ((Connection)ar.AsyncState).OnDns(ar);
    }

    public static void ReadCallback(IAsyncResult ar)
    {
      ((Connection)ar.AsyncState).Read(ar); ;
    }

    public static void Add(Connection connection)
    {
      connections.Add(connection);
    }
  }
}
