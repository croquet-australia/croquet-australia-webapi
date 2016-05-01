using System;
using System.Threading.Tasks;
using System.Web.Http;
using CroquetAustralia.CQRS;
using CroquetAustralia.Domain.App;
using CroquetAustralia.WebApi.Settings;

namespace CroquetAustralia.WebApi.Controllers
{
    [RoutePrefix("application")]
    public class ApplicationController : ApiController
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly ICommandBus _commandBus;

        public ApplicationController(ApplicationSettings applicationSettings, ICommandBus commandBus)
        {
            _applicationSettings = applicationSettings;
            _commandBus = commandBus;
        }

        [Route("setup"), HttpGet]
        public async Task SetupAsync()
        {
            var aggregateId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var initialAdministratorEmailAddress = _applicationSettings.InitialAdministratorEmailAddress;
            var command = new RunSetup(aggregateId, initialAdministratorEmailAddress);

            await _commandBus.SendCommandAsync(command);
        }
    }
}
