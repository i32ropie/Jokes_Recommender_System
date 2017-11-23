using Jokes_recommender_system.Models;
using Jokes_recommender_system.Models.Entities;
using Jokes_recommender_system.Models.Facades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jokes_recommender_system.Controllers
{
    public class HomeController : Controller
    {
        private readonly JokeFacade jokeFacade = new JokeFacade();
        
        public ActionResult Index()
        {
            //jokeFacade.AddJokesToDb(ImportJokes.Import());
            
            Joke randomJoke = jokeFacade.GetRandomJoke();
            return View(randomJoke);
        }

        public ActionResult Category(string category)
        {
            var jokesFromCategory = jokeFacade.GetJokesFromCategory(category);
            ViewBag.Title = category;
            return View(jokesFromCategory);
        }
    }
}