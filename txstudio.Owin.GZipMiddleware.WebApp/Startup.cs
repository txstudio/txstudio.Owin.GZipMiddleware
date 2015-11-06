using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(txstudio.Owin.GZipMiddleware.WebApp.Startup))]

namespace txstudio.Owin.GZipMiddleware.WebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration _config;

            _config = new HttpConfiguration();
            _config.MapHttpAttributeRoutes();
            _config.Formatters.Remove(_config.Formatters.XmlFormatter);

            app.UseGZipRequest();
            app.UseWebApi(_config);
        }
    }
}
