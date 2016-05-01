using System;
using System.Web.Http;
using CroquetAustralia.Logging;
using CroquetAustralia.WebApi.Services;
using CroquetAustralia.WebApi.Settings;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

namespace CroquetAustralia.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            LoggerConfiguration.StartLogging(new ApplicationSettings().IsDeveloperMachine);

            appBuilder
                .UseNinjectMiddleware(IoC.CreateKernel)
                .UseNinjectWebApi(CreateHttpConfiguration());
        }

        private static HttpConfiguration CreateHttpConfiguration()
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            return config;
        }
    }
}