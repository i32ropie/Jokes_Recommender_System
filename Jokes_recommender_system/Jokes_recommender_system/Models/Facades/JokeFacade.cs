using Jokes_recommender_system.DbContexts;
using Jokes_recommender_system.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jokes_recommender_system.Models.Facades
{
    public class JokeFacade
    {
        private readonly JokeDbContext db = new JokeDbContext();
        private readonly RatingFacade ratingFacade = new RatingFacade();
        public JokeFacade()
        {
        }
        public Joke GetJokeById(int id)
        {
            return db.Jokes.Find(id);
        }


        public int GetSimilarRecommendedJoke(string userName, int jokeId)
        {
            string preferencedCategories = db.Users.Where(user => user.UserName == userName).Select(user => user.CategoryPreference).FirstOrDefault();

            IEnumerable<string> categories = (preferencedCategories == null) ? categories = GetCategories() : categories = preferencedCategories.Split(',');

            foreach (string category in categories)
            {
                IEnumerable<Joke> jokesFromCategory = GetJokesFromCategory(category);
                IEnumerable<Joke> notRatedJokes = jokesFromCategory.Where(joke => ratingFacade.ratedByUser(joke.Id, userName) == null).ToList();
                IEnumerable<Joke> filteredByLength = notRatedJokes.Where(joke => GetJokeById(jokeId).IsLong == joke.IsLong);
                IEnumerable<Joke> orderedJokes = filteredByLength.OrderByDescending(joke => getJaccardIndex(jokeId, joke.Id));

                if (orderedJokes.FirstOrDefault() != null) return orderedJokes.First().Id;
            }

            return GetRandomJoke().Id; // happens when user had rated all jokes from his preferenced categories
        }

        private double getJaccardIndex(int thisJoke, int otherJoke)
        {
            List<string> thisKeywords = db.Jokes.Where(joke => joke.Id == thisJoke).Select(joke => joke.Keywords).ToString().Split(',').ToList();
            List<string> otherKeywords = db.Jokes.Where(joke => joke.Id == otherJoke).Select(joke => joke.Keywords).ToString().Split(',').ToList();

            double unionCount = thisKeywords.Union(otherKeywords).Count();
            double intersectCount = thisKeywords.Intersect(otherKeywords).Count();

            return intersectCount / unionCount;
        }

        public int GetDifferentRecommendedJoke(string userName, int jokeId)
        {
            string preferencedCategories = db.Users.Where(user => user.UserName == userName).Select(user => user.CategoryPreference).FirstOrDefault();

            IEnumerable<string> categories = (preferencedCategories == null) ? categories = GetCategories() : categories = preferencedCategories.Split(',');

            foreach (string category in categories)
            {
                IEnumerable<Joke> jokesFromCategory = GetJokesFromCategory(category);
                IEnumerable<Joke> notRatedJokes = jokesFromCategory.Where(joke => ratingFacade.ratedByUser(joke.Id, userName) == null).ToList();
                IEnumerable<Joke> filteredByLength = notRatedJokes.Where(joke => GetJokeById(jokeId).IsLong != joke.IsLong);
                IEnumerable<Joke> orderedJokes = filteredByLength.OrderBy(joke => getJaccardIndex(jokeId, joke.Id));

                if (orderedJokes.FirstOrDefault() != null) return orderedJokes.First().Id;
            }

            return GetRandomJoke().Id; // happens when user had rated all jokes from his preferenced categories
        }

        public Joke GetRecommendedJoke(string userName)
        {
            
            // apply full recommendations here
            // preferred category + a lot of likes on the joke + wasnt rated by the user
            return GetRandomJoke(); // will be changed after implementation
        }
        public Joke GetRandomJoke()
        {
            Random rand = new Random();
            int toSkip = rand.Next(0, db.Jokes.Count());

            return db.Jokes.OrderBy(r => Guid.NewGuid()).Skip(toSkip).Take(1).First();
        }

        public IEnumerable<Joke> GetJokesFromCategory(string category)
        {
            return db.Jokes.ToList().Where(joke => joke.Category == category);
        }

        public IEnumerable<string> GetCategories()
        {
            return db.Jokes.Select(joke => joke.Category).Distinct().ToList();
        }

        public void AddJokesToDb(List<Joke> jokes)
        {
            db.Jokes.AddRange(jokes);
            db.SaveChanges();
        }
    }
}