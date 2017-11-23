using Jokes_recommender_system.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Jokes_recommender_system.Models.Facades
{
    public static class ImportJokes
    {
        public static List<Joke> Import()
        {
            using (StreamReader r = new StreamReader(System.Web.HttpContext.Current.Request.MapPath("jokes.json")))
            {
                string json = r.ReadToEnd();
                List<JokeImport> jokes = JsonConvert.DeserializeObject<List<JokeImport>>(json);
                List<Joke> readyForDbJokes = new List<Joke>();
                foreach(var jokeImport in jokes)
                {
                    string keywordsString = string.Empty;
                    foreach(string word in jokeImport.Keywords)
                    {
                        keywordsString += word + ',';
                    }
                    keywordsString = keywordsString.TrimEnd(',');

                    readyForDbJokes.Add(new Joke()
                    {
                        Title = jokeImport.Title,
                        Text = jokeImport.Text,
                        Category = jokeImport.Category,
                        IsLong = jokeImport.Length > 24,
                        Keywords = keywordsString,
                    });
                }
                return readyForDbJokes;
            }
        }

        private class JokeImport
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public string Category { get; set; }
            public int Length { get; set; }
            public List<string> Keywords { get; set; }
        }
    }   
}