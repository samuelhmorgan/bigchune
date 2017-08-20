using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;

namespace BigChune.Scanner.Scan
{
    internal class MediaScanner
    {
       
        private IRepository repository;
        private IEntityManager<SoundFile> soundFileManager;
        public MediaScanner()
        {
            var driver = DriverFactory.MakeDriver("bolt://localhost:7687");
            repository = new Repository(driver);
            soundFileManager = repository.SoundManager;
        }

        public void TreeScan(string sDir)
        {
            foreach (string f in Directory.GetFiles(sDir))
            {
                Console.WriteLine("Processing "+ f);
                //Save f :)
                TagLib.File file = TagLib.File.Create(f);
                double length = file.Properties.Duration.TotalSeconds;

                SoundFile sf = new SoundFile()
                {
                    Title = file.Tag.Title,
                    Filename = f,
                    Directory = sDir,
                    Comment = file.Tag.Comment,
                    Rating = 0,
                    Filesize = "",
                    Year = (int) file.Tag.Year,
                    Genre = file.Tag.JoinedGenres,
                    Lyrics = file.Tag.Lyrics,
                    Album = file.Tag.Album,
                    Artist = file.Tag.JoinedAlbumArtists,
                    Duration = (int) length
                };

                soundFileManager.Add(sf);
                 
            }
            foreach (string d in Directory.GetDirectories(sDir))
            {
                TreeScan(d);
            }
        }
    }


    public class SoundFile
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int Year { get; set; }
        public string Comment { get; set; }
        public string Genre { get; set; }
        public int Rating { get; set; }
        public string Lyrics { get; set; }
        public string Directory { get; set; }
        public string Filename { get; set; }
        public string Filesize { get; set; }
        public int Duration { get; set; }
    }

    public class SoundFileManager : IEntityManager<SoundFile>
    {
        private readonly Neo4JProxy _proxy;
        private readonly IDriver _driver;

        public SoundFileManager(IDriver driver)
        {
            // Console.WriteLine($"{record["people.name"].As<string>()}");
            _driver = driver;
            _proxy = new Neo4JProxy();
        }

        public void Add(SoundFile item)
        {
            var template = "(a:SoundFile {title:{title}, year:{year}, genre:{genre}, comments:{comments}, lyrics:{lyrics}, rating:{rating}, " +
                              " filename:{filename}, directory:{directory},filesize:{filesize}, duration:{duration})";

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

            //publisher
            //album
            //
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


    public class DriverFactory
    {
        public static IDriver MakeDriver(string url)
        {
            return GraphDatabase.Driver(url, AuthTokens.Basic("app", "app"));
        }
    }

    public class Neo4JProxy
    {
        public void Create(string command, Dictionary<string, object> parameters, ISession session)
        {
            session.Run(command, parameters);
        }

        public IStatementResult Query(string command, Dictionary<string, object> parameters, ISession session)
        {
            var result = session.Run(command, parameters);
            return result;
        }
    }


    public interface IEntityManager<in T>
    {
        void Add(T item);
        void Remove(T item);
        void Update(T item);
    }
    public interface IRepository
    {
        IEntityManager<SoundFile> SoundManager { get; set; }
    }

    public class Repository : IRepository
    {
        public IEntityManager<SoundFile> SoundManager { get; set; }

        public Repository(IDriver driver)
        {
            SoundManager = new SoundFileManager(driver);
        }
    }


}
