using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigChune.Models
{
    public class AppSettings
    {
        public DataSettings Data { get; set; } 
    }

    public class DataSettings
    {
        public ConnectionStringSettings ConnectionStrings { get; set; }
      
    }

    public class ConnectionStringSettings
    {
       public string LocalGraph { get; set; }
    }
}
