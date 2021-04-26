using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCShop.Startup))]
namespace MVCShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
