using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BigChune.Data.Templates;
using Neo4j.Driver.V1;

namespace BigChune.Data.Neo4J
{
    public class Neo4JProxy
    {
        public void Create(string command, Dictionary<string,object> parameters, ISession session)
        {
            session.Run($"CREATE {command}", parameters);
        }

        public IStatementResult Query(string command, Dictionary<string, object> parameters, ISession session)
        {
            var result = session.Run(command,parameters);
            return result;
        }

        public IStatementResult Query(AbstractTemplate template, ISession session)
        {
            var result = session.Run(template.Query, template.Parameters);
            return result;
        }
    }
}
