using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartQA.Startup))]
namespace SmartQA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
