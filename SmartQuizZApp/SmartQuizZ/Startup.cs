using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartQuizZ.Startup))]
namespace SmartQuizZ
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
