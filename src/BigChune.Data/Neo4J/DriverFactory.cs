using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Neo4j.Driver.V1;

namespace BigChune.Data.Neo4J
{
   public class DriverFactory
    {
        public static IDriver MakeDriver(string url)
        {
            return  GraphDatabase.Driver(url, AuthTokens.Basic("app", "app"));
        }
    }

    
}
