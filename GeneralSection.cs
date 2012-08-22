using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oftc_ircd_cs
{
  public class GeneralSection : ConfigSection
  {
    public bool Daemon { get; set; }
    public string SSLCertificate { get; set; }
    public string SSLPrivateKey { get; set; }
    public string MessagesFile { get; set; }
    public string ServerName { get; set; }
    public string ServerInfo { get; set; }
    public string MotdFile { get; set; }
    public int Nicklen { get; set; }

    #region ConfigSection Members

    public void SetDefaults()
    {
      Daemon = true;
      ServerName = "IRC Server";
      Nicklen = 30;
    }

    public void Process(object o)
    {
      Dictionary<string, object> section = o as Dictionary<string, object>;

      if (section == null)
        throw new Exception("config element is not an object as expected");

      object tmp;

      if (section.TryGetValue("daemon", out tmp))
        Daemon = (bool)tmp;
      if (section.TryGetValue("ssl_certificate", out tmp))
        SSLCertificate = (string)tmp;
      if (section.TryGetValue("ssl_privatekey", out tmp))
        SSLPrivateKey = (string)tmp;
      if (section.TryGetValue("messages_file", out tmp))
        MessagesFile = (string)tmp;
      if (section.TryGetValue("server_name", out tmp))
        ServerName = (string)tmp;
      if (section.TryGetValue("server_info", out tmp))
        ServerInfo = (string)tmp;
      if (section.TryGetValue("nicklen", out tmp))
        Nicklen = (int)tmp;
    }

    public void Verify()
    {
    }

    #endregion
  }
}
