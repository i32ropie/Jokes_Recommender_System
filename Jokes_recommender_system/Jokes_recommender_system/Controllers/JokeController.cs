using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jokes_recommender_system.Models;
using Jokes_recommender_system.Models.Entities;
using Jokes_recommender_system.Models.Facades;

namespace Jokes_recommender_system.Controllers
{
    public class JokeController : Controller
    {
        private readonly JokeFacade jokeFacade = new JokeFacade();
        private readonly RatingFacade ratingFacade = new RatingFacade();

        // GET: Joke
        public ActionResult Index()
        {
            return View();
        }


        // GET: Joke/Details/5
        public ActionResult Details(int id)
        {
            Joke joke = jokeFacade.GetJokeById(id);
            if (joke == null)
            {
                return HttpNotFound();
            }

            if(User.Identity.IsAuthenticated)
            {
                ViewBag.Liked = ratingFacade.ratedByUser(id, User.Identity.Name);
            }
            else
                ViewBag.Liked = null;

            return View(joke);
        }

        public ActionResult Back(string category)
        {
            return RedirectToAction("Category", "Home", new { category = category});
        }

        public ActionResult Similar()
        {
            int similarId = jokeFacade.GetSimilarRecommendedJoke(User.Identity.Name);
            return RedirectToAction("Details",new { id = similarId });
        }

        public ActionResult Different()
        {
            int differentId = jokeFacade.GetDifferentRecommendedJoke(User.Identity.Name);
            return RedirectToAction("Details", new { id = differentId });
        }

        public ActionResult Recommended()
        {
            Joke recommendedJoke = jokeFacade.GetRecommendedJoke(User.Identity.Name);
            return View("~/Views/Home/Index.cshtml", recommendedJoke);
        }

        public ActionResult RatingClicked(bool liked, int jokeId)
        {
            ViewBag.Liked = liked;
            var joke = jokeFacade.GetJokeById(jokeId);
           
            ratingFacade.SaveRating(jokeId, User.Identity.Name, liked);
            return View("Details", joke);
        }


    }
}
