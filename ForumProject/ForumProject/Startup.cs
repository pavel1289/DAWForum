using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ForumProject.Startup))]
namespace ForumProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
