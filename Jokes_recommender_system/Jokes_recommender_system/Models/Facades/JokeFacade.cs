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
        public JokeFacade()
        {
        }
        public Joke GetJokeById(int id)
        {
            return db.Jokes.Find(id);
        }


        public int GetSimilarRecommendedJoke(string userName)
        {
            // apply similar recommendations here
            // prefered category + similar keywords + wasnt rated by the user
            return GetRandomJoke().Id; // will be changed after implementation
        }

        public int GetDifferentRecommendedJoke(string userName)
        {
            // apply different recommendations here
            // prefered category + different keywords + wasnt rated by the user
            return GetRandomJoke().Id; // will be changed after implementation
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

        public void AddJokesToDb(List<Joke> jokes)
        {
            db.Jokes.AddRange(jokes);
            db.SaveChanges();
        }
    }
}