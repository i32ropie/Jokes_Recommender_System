using Jokes_recommender_system.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jokes_recommender_system
{
    public class JokesDb : DbContext
    {
        public JokesDb() : base("DefaultConnection")
        {
        }
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
