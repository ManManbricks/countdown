using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Countdown.Startup))]
namespace Countdown
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
