using System.Collections.Generic;
using System.IO; 
using BigChune.Data.Neo4J;
using BigChune.Data.Templates;
using BigChune.Models;
using BigChune.Models.View;
using BigChune.Models.Web; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options; 

namespace BigChune.Controllers
{
    public class HomeController : BaseController
    {
       
        public HomeController(IOptions<AppSettings> appSettings) : base(appSettings)
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Library(string a="")
        {
           
            AllMusicTemplate template = new AllMusicTemplate().StartsWith(a);
            Neo4JProxy proxy = new Neo4JProxy();
            var result  = proxy.Query(template, GraphDriver.Session());

            var library = new List<MusicFileViewModel>();

            foreach (var record in result)
            {
                MusicFileViewModel model = new MusicFileViewModel()
                {
                    Title = record["title"] as string,
                    Artist = record["artist"] as string,
                    Album = record["album"] as string,
                    Genre = record["genre"] as string,
                    Directory = record["directory"] as string,
                    Filename = record["filename"] as string,
                    Hash= record["hash"] as string
                };
                library.Add(model);
            }

            return View(library);
        }

        public IActionResult Grab(string id)
        {
            AllMusicTemplate template = new AllMusicTemplate().Find(id.Replace(".mp3",""));
            Neo4JProxy proxy = new Neo4JProxy();
            var result = proxy.Query(template, GraphDriver.Session());
             
            foreach (var record in result)
            {
                MusicFileViewModel model = new MusicFileViewModel()
                {
                    Title = record["title"] as string,
                    Artist = record["artist"] as string,
                    Album = record["album"] as string,
                    Genre = record["genre"] as string,
                    Directory = record["directory"] as string,
                    Filename = record["filename"] as string,
                    Hash = record["hash"] as string,
                    Mime= record["mime"] as string
                };

                string filepath = model.Filename;
                byte[] filedata = System.IO.File.ReadAllBytes(filepath);

                string filename = "song"+Path.GetExtension(model.Filename);
                return File(filedata, "audio/mpeg3", filename);
            }

            return NotFound();
        }



        public IActionResult AlbumArt(string hash)
        {
            AllMusicTemplate template = new AllMusicTemplate().Find(hash);
            Neo4JProxy proxy = new Neo4JProxy();
            var result = proxy.Query(template, GraphDriver.Session());

            foreach (var record in result)
            {
                MusicFileViewModel model = new MusicFileViewModel()
                {
                    Title = record["title"] as string,
                    Artist = record["artist"] as string,
                    Album = record["album"] as string,
                    Genre = record["genre"] as string,
                    Hash = record["hash"] as string,
                    Mime = record["mime"] as string,
                     AlbumArt= record["albumart"] as string
                };

                return Json(model);
            }

            return NotFound();
        }

        public IActionResult Error()
        {
            return View();
        }

        
    }
}
