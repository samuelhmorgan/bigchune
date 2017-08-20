using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using Neo4j.Driver.V1; 

namespace BigChune.Scanner.Scan
{
    internal class MediaScanner
    {
       
      
        private IEntityManager<SoundFile> soundFileManager;
        private IDriver driver;
        public MediaScanner()
        {
            driver = DriverFactory.MakeDriver("bolt://localhost:7687");
           
        }

        public void TreeScan(string sDir)
        {
           
                foreach (string f in Directory.GetFiles(sDir))
                {
                using (var session = driver.Session())
                {
                    soundFileManager = new SoundFileManager(session);
                    try
                    {
                        SoundFile sf = null;
                        TagLib.File file = null;

                        Console.WriteLine("Processing " + f);

                        file = TagLib.File.Create(f);
                        double length = file.Properties.Duration.TotalSeconds;

                        sf = new SoundFile()
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
                            Duration = (int) length,
                            Mime = file.MimeType

                        };

                        if (file.Tag.Pictures.Any())
                        {
                            var artwork = file.Tag.Pictures.First();
                            using (MemoryStream m = new MemoryStream())
                            {
                                byte[] imageBytes = artwork.Data.Data;
                                // Convert byte[] to Base64 String
                                string base64String = Convert.ToBase64String(imageBytes);
                                sf.Artwork = base64String;
                                imageBytes = null;
                            }

                        }

                        soundFileManager.Add(sf);
                        file = null;
                        sf = null;
                    }
                    catch (Exception uex)
                    {
                        continue;

                        //todo - log and give report
                    }

                }

            }
            foreach (string d in Directory.GetDirectories(sDir))
            {
                TreeScan(d);
            }
        }
    }


    public class SoundFile
    {
        public SoundFile()
        {
             Hash = Guid.NewGuid().ToString();    
        }

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
        public string Hash { get; set; }
        public string Mime { get; set; }
        public string Artwork { get; set; }
    }

    public class SoundFileManager : IEntityManager<SoundFile>
    {
        private readonly Neo4JProxy _proxy;
        private readonly ISession _session;

        public SoundFileManager(ISession session)
        {
            // Console.WriteLine($"{record["people.name"].As<string>()}");
            _session = session;
            _proxy = new Neo4JProxy();
        }

        public void Add(SoundFile item)
        {
            var template = "(a:SoundFile {title:{title},artist:{artist},album:{album}, year:{year}, genre:{genre}, comments:{comments}, lyrics:{lyrics}, rating:{rating}, " +
                              " filename:{filename}, directory:{directory},filesize:{filesize}, duration:{duration}, hash:{hash}, albumart:{albumart}, mime:{mime}})";

            string title = string.IsNullOrEmpty(item.Title)?item.Filename: item.Title;
            var parameters = new Dictionary<string, object>();
            parameters.Add("title", title);
            parameters.Add("year", item.Year);
            parameters.Add("genre", item.Genre);
            parameters.Add("comments", item.Comment);
            parameters.Add("lyrics", item.Lyrics);
            parameters.Add("rating", item.Rating);
            parameters.Add("filename", item.Filename);
            parameters.Add("directory", item.Directory);
            parameters.Add("filesize", item.Filesize);
            parameters.Add("duration", item.Duration);
            parameters.Add("hash",item.Hash);
            parameters.Add("mime", item.Mime);
            parameters.Add("artist", item.Artist);
            parameters.Add("album", item.Album);
            parameters.Add("albumart",item.Artwork);

            _proxy.Create(template, parameters, _session);
            parameters = null;  
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
           session.Run($"CREATE {command}", parameters); 

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
     


}
