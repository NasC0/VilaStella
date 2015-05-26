using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VilaStella.WebAdminClient.Startup))]

namespace VilaStella.WebAdminClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            this.ConfigureAuth(app);
        }
    }
}
