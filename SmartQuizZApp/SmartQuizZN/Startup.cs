using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartQuizZN.Startup))]
namespace SmartQuizZN
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
