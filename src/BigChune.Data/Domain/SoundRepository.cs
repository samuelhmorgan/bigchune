using System;
using System.Collections.Generic; 
using BigChune.Data.Neo4J;
using Neo4j.Driver.V1;

namespace BigChune.Data.Domain
{
    public class SoundRepository : IRepository<SoundFile>
    {
        private readonly Neo4JProxy _proxy;
        private readonly IDriver _driver;

        public SoundRepository(IDriver driver)
        {
            _driver = driver;
            _proxy = new Neo4JProxy();
        }

        public void Add(SoundFile item)
        {
            var template = "(a:SoundFile {title:{title}, year:{year}, genre:{genre}, comments:{comments}, lyrics:{lyrics}, rating:{rating}, " +
                              " filename:{filename}, directory:{directory},filesize:{filesize}, duration:{duration}})";

            var parameters = new Dictionary<string, object>();
            parameters.Add("title", item.Title);
            parameters.Add("year", item.Year);
            parameters.Add("genre", item.Genre);
            parameters.Add("comments", item.Comment);
            parameters.Add("lyrics", item.Lyrics);
            parameters.Add("rating", item.Rating);
            parameters.Add("filename", item.Filename);
            parameters.Add("directory", item.Directory);
            parameters.Add("filesize", item.Filesize);
            parameters.Add("filesize", item.Duration);

            _proxy.Create(template, parameters, _driver.Session());
            
        }

        public void Remove(SoundFile item)
        {
            throw new NotImplementedException();
        }

        public void Update(SoundFile item)
        {
            throw new NotImplementedException();
        }
    }
}
