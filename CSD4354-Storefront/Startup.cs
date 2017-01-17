using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CSD4354_Storefront.Startup))]
namespace CSD4354_Storefront
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
