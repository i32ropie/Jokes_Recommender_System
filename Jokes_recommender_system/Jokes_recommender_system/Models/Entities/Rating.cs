using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jokes_recommender_system.Models.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int JokeId { get; set; }
        public string UserName { get; set; }
        public bool Liked { get; set; }
    }
}