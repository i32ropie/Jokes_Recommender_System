using Jokes_recommender_system.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jokes_recommender_system.Models.Facades
{
    public class RatingFacade
    {
        private readonly JokeDbContext db = new JokeDbContext();

        public void SaveRating(int jokeId, string userName, bool liked)
        {
            db.Ratings.Add(new Entities.Rating()
            {
                JokeId = jokeId,
                UserName = userName,
                Liked = liked,
            });
            db.SaveChanges();
        }

        public bool? ratedByUser(int jokeId, string userName)
        {
            var r = db.Ratings.Where(rating => rating.JokeId == jokeId && rating.UserName == userName).FirstOrDefault();
            if (r != null)
                return r.Liked;
            return null;
        }
    }
}