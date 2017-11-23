using Jokes_recommender_system.Models;
using Jokes_recommender_system.Models.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jokes_recommender_system.DbContexts
{
    public class JokeDbContext : IdentityDbContext<User>
    {
        public JokeDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static JokeDbContext Create()
        {
            return new JokeDbContext();
        }

        public DbSet<Joke> Jokes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
