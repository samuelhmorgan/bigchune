using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BigChune.Data;
using BigChune.Data.Neo4J;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Neo4j.Driver.V1;

namespace BigChune.Models.Web
{
    public class BaseController : Controller
    {
        public IDatabaseContext DbContext { get; private set; }
        public IDriver GraphDriver { get; set; }
        public BaseController(IOptions<AppSettings> appSettings)
        {
            GraphDriver = DriverFactory.MakeDriver(appSettings.Value.Data.ConnectionStrings.LocalGraph);
            DbContext = new Neo4jDbContext(GraphDriver);
        }
    }
}
