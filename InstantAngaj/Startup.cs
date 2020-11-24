using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InstantAngaj.Startup))]
namespace InstantAngaj
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
