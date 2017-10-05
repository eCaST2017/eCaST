using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CTL.Startup))]
namespace CTL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            //app.UseSignInCookies();

            ConfigureAuth(app);
        }
    }


   

}
