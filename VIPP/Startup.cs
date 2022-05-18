using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VIPP.Startup))]
namespace VIPP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
