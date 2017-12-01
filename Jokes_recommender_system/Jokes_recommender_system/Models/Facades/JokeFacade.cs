using Jokes_recommender_system.DbContexts;
using Jokes_recommender_system.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
<<<<<<< HEAD
                IEnumerable<Joke> notRatedJokes = jokesFromCategory.Where(joke => ratingFacade.RatedByUser(joke.Id, userName) == null).ToList();
=======
                IEnumerable<Joke> notRatedJokes = jokesFromCategory.Where(joke => ratingFacade.ratedByUser(joke.Id, userName) == null).ToList();
>>>>>>> b52a7b3bce14381e2eb31d08e9fc14a37e399517
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
<<<<<<< HEAD
                IEnumerable<Joke> notRatedJokes = jokesFromCategory.Where(joke => ratingFacade.RatedByUser(joke.Id, userName) == null).ToList();
=======
                IEnumerable<Joke> notRatedJokes = jokesFromCategory.Where(joke => ratingFacade.ratedByUser(joke.Id, userName) == null).ToList();
>>>>>>> b52a7b3bce14381e2eb31d08e9fc14a37e399517
                IEnumerable<Joke> filteredByLength = notRatedJokes.Where(joke => GetJokeById(jokeId).IsLong != joke.IsLong);
                IEnumerable<Joke> orderedJokes = filteredByLength.OrderBy(joke => getJaccardIndex(jokeId, joke.Id));

                if (orderedJokes.FirstOrDefault() != null) return orderedJokes.First().Id;
            }

            return GetRandomJoke().Id; // happens when user had rated all jokes from his preferenced categories
        }

        public int GetRecommendedJoke(string userName, int id)
        {
            Random rand = new Random();
            int modulo = rand.Next();
            if (modulo % 2 == 0)
                 return TfIdtRecommendation(userName, id);
            else  return CollaborativeFilteringRecommendation(userName, id);
            
        }

        public int TfIdtRecommendation(string userName, int id)
        {
            var joke = GetJokeById(id);
            string[] kw = joke.Keywords.Split(',');

            // tf of same category
            var jokesInCategory = GetJokesFromCategory(joke.Category);
            var tf = FindKeywords(jokesInCategory, kw);

            // idf for all jokes
            var rnd = new Random();
            var allJokes = db.Jokes.ToList();
            var jokes = allJokes.OrderByDescending(r => rnd.Next()).ToList();
            var idf = FindKeywords(jokes, kw);

            Dictionary<string, double> keywords = new Dictionary<string, double>();
            foreach (var key in kw)
            {
                tf[key] /= jokesInCategory.Count();
                idf[key] = Math.Log(jokes.Count() / idf[key]);
                keywords.Add(key, tf[key] * idf[key]);
            }

            var orderedKeywords = keywords.OrderByDescending(k => k.Value);

            foreach (var j in jokes)
            {
                if (j.Keywords.Contains(orderedKeywords.First().Key) && ratingFacade.RatedByUser(j.Id, userName) == null && j.Id != id)
                    return j.Id;
            }
            return GetRandomJoke().Id; 
        }
        

        public int CollaborativeFilteringRecommendation(string userName, int id)
        {
            var user = db.Users.Where(u => u.UserName == userName).First();
            var otherUsers = db.Users.Where(u => u.UserName != userName).ToList();
            if (otherUsers.Count() >= 2)
            {
                Dictionary<string, double> counter = new Dictionary<string, double>();
                if (user.CategoryPreference != null)
                {
                    var prefCat = user.CategoryPreference.Replace(" ", "").Split(',');
                    {
                        for (int j = 0; j < prefCat.Count(); j++)
                        {
                            foreach (var u in otherUsers)
                            {
                                if (u.CategoryPreference != null)
                                {
                                    if (u.CategoryPreference.Contains(prefCat[j]))
                                    {
                                        var categories = u.CategoryPreference.Replace(" ", "").Split(',');
                                        for (int i = 0; i < categories.Count(); i++)
                                        {
                                            var c = categories[i];
                                            if (!counter.ContainsKey(c))
                                                counter.Add(c, 0);
                                            counter[c] += 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    var orderedCounter = counter.OrderByDescending(c => c.Value);
                    string category = null;
                    foreach (var c in orderedCounter)
                    {
                        if (c.Key != prefCat[0])
                        {
                            category = c.Key;
                            break;
                        }
                    }
                    if (category != null)
                    {
                        var jokes = db.Jokes.ToList();
                        foreach (var j in jokes)
                        {
                            if (j.Category.Equals(category) && ratingFacade.RatedByUser(j.Id, userName) == null && j.Id != id)
                                return j.Id;
                        }
                    }
                }
            }
            return TfIdtRecommendation(userName, id);
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

        public IEnumerable<string> GetCategories()
        {
            return db.Jokes.Select(joke => joke.Category).Distinct().ToList();
        }

        public Dictionary<string, double> FindKeywords(IEnumerable<Joke> jokes, string[] kw)
        {
            Dictionary<string, double> keywords = new Dictionary<string, double>
            {
                { kw[0], 0 },
                { kw[1], 0 },
                { kw[2], 0 },
                { kw[3], 0 }
            };
            foreach (var j in jokes)
            {
                if (Regex.IsMatch(j.Keywords, string.Format(@"\b{0}\b", Regex.Escape(kw[0]))))
                {
                    keywords[kw[0]] += 1;
                }
                if (Regex.IsMatch(j.Keywords, string.Format(@"\b{0}\b", Regex.Escape(kw[1]))))
                {
                    keywords[kw[1]] += 1;
                }
                if (Regex.IsMatch(j.Keywords, string.Format(@"\b{0}\b", Regex.Escape(kw[2]))))
                {
                    keywords[kw[2]] += 1;
                }
                if (Regex.IsMatch(j.Keywords, string.Format(@"\b{0}\b", Regex.Escape(kw[3]))))
                {
                    keywords[kw[3]] += 1;
                }
            }
            return keywords;
        }
    }
}