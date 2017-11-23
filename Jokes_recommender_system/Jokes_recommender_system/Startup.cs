using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Jokes_recommender_system.Startup))]
namespace Jokes_recommender_system
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);            
        }
    }
}
