using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace oftc_ircd_cs
{
    public class Connection
    {
        private static readonly List<Connection> Connections = new List<Connection>();

        private readonly byte[] buffer = new byte[4096];
        private readonly Parser parser;
        private string dnsIp;
        private Socket socket;
        private StringBuilder stringBuffer;
        private StringWriter writer;

        public Connection()
        {
            parser = Parser.Default;
        }

        public Client Client { get; set; }
        public string Host { get; private set; }

        public void OnSend(IAsyncResult ar)
        {
            socket.EndSend(ar);
        }

        public void Send(string message)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);

            socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, SendCallback, this);
        }

        public void OnDns(IAsyncResult ar)
        {
            IPHostEntry result = Dns.EndGetHostEntry(ar);

            bool found = result.AddressList.Any(addr => addr.ToString() == dnsIp);

            if (!found)
                Client.Send(String.Format(":{0} NOTICE {1} :*** Your forward and reverse DNS don't match, ignoring.",
                                          Server.Me,
                                          Client));
            else
                Client.Send(String.Format(":{0} NOTICE {1} :*** Found your hostname.", Server.Me, Client));

            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReadCallback, this);
        }

        public void Read(IAsyncResult ar)
        {
            socket.EndReceive(ar);

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
                string[] lines = tmp.Split(new[] {"\r", "\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    parser.Parse(Client, line);
                }
            }

            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReadCallback, this);
        }

        public void Accept(Socket socket)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            this.socket = socket;

            var endpoint = (IPEndPoint) this.socket.RemoteEndPoint;

            Host = endpoint.Address.ToString();
            Client.Host = Host;

            Client.Send(String.Format(":{0} NOTICE {1} :*** Looking up your hostname", Server.Me, Client));

            dnsIp = Host;
            Dns.BeginGetHostEntry(dnsIp, DnsCallback, this);

            stringBuffer = new StringBuilder();
            writer = new StringWriter(stringBuffer);
        }

        public static void SendCallback(IAsyncResult ar)
        {
            var connection = (Connection) ar.AsyncState;

            connection.OnSend(ar);
        }

        public static void DnsCallback(IAsyncResult ar)
        {
            ((Connection) ar.AsyncState).OnDns(ar);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            ((Connection) ar.AsyncState).Read(ar);
        }

        public static void Add(Connection connection)
        {
            Connections.Add(connection);
        }
    }
}