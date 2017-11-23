using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jokes_recommender_system.Models.Entities
{
    public class Joke
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public bool IsLong { get; set; }
        //keywords split by ','
        public string Keywords { get; set; }
    }
}