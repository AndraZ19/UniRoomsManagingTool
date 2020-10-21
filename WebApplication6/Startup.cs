using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WebApplication6.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication6.Startup))]
namespace WebApplication6
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
        
    }
}
