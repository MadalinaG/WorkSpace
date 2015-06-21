using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartQuizZApp.Startup))]
namespace SmartQuizZApp
{
    public partial class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}