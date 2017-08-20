using BigChune.Data.Domain;
using Neo4j.Driver.V1;

namespace BigChune.Data
{
    public class Neo4jDbContext : IDatabaseContext
    {
        public IRepository<SoundFile> SoundRepository { get; set; }

        public Neo4jDbContext(IDriver driver)
        {
            SoundRepository = new SoundRepository(driver);
        }
    }
}
