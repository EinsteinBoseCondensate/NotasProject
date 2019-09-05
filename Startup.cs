using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NotasProject.Startup))]
namespace NotasProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
