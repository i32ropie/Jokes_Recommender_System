using Jokes_recommender_system.DbContexts;
using Jokes_recommender_system.Models.Entities;
using System.Data.Entity.Migrations;
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

            SaveCategoriesInOrder(userName);           
        }

        public bool? RatedByUser(int jokeId, string userName)
        {
            var r = db.Ratings.Where(rating => rating.JokeId == jokeId && rating.UserName == userName).FirstOrDefault();
            if (r != null)
                return r.Liked;
            return null;
        }

        public List<Joke> RatedJokesByUser(string userName)
        {
            var ratings = db.Ratings.Where(rating => rating.UserName == userName).ToList();
            if (ratings != null)
            {
                List<Joke> jokes = new List<Joke>();
                foreach (var r in ratings)
                {
                    jokes.Add(db.Jokes.Where(joke => joke.Id == r.JokeId).First());
                }
                return jokes.OrderBy(joke => joke.Category).ToList();
            }
            return null;
        }

        public void SaveCategoriesInOrder(string userName)
        {
            var jokes = RatedJokesByUser(userName);
            var ratings = db.Ratings.Where(rating => rating.UserName == userName).ToList();
            var user = db.Users.Where(u => u.UserName == userName).First();

            string categories = null;
            if (jokes.Count() >= 5)
            {
                Dictionary<string, int> counter = new Dictionary<string, int>();
                int length = 0;
                foreach (var j in jokes)
                {
                    if (!counter.ContainsKey(j.Category))
                    {
                        counter.Add(j.Category, 0);
                    }
                    var rating = ratings.Find(r => r.JokeId == j.Id);
                    if (rating.Liked)
                        counter[j.Category] += 1;
                    else counter[j.Category] -= 1;

                    if (j.IsLong)
                        length++;
                    else length--;
                }

                if (length > 0)
                    user.PreferLong = true;
                else user.PreferLong = false;

                var orderedCounter = counter.OrderByDescending(v => v.Value).ToList();   
                foreach (var c in orderedCounter)
                {
                    if (c.Value >= 5)
                    {
                        if (categories == null)
                        {
                            categories += c.Key;
                        }
                        else if (!categories.Contains(c.Key))
                        {
                            List<String> str = categories.Split(',').Select(s => s.Trim()).ToList();
                            str.Add(c.Key);
                            categories = String.Join(", ", str.ToArray());
                        }
                    }
                }
            }
            user.CategoryPreference = categories;
            db.Users.AddOrUpdate(user);
            db.SaveChanges();
        }
    }
}