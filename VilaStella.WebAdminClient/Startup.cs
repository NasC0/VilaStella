using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VilaStella.WebAdminClient.Startup))]

namespace VilaStella.WebAdminClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
