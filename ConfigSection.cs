using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oftc_ircd_cs
{
  public interface ConfigSection
  {
    void SetDefaults();
    void Process(object o);
    void Verify();
  }
}
