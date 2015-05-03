using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WDPlatform.Startup))]
namespace WDPlatform
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
