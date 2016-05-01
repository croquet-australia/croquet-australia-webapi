using System.Linq;
using CroquetAustralia.Domain.App;
using CroquetAustralia.WebApi.Settings;
using CroquetAustralia.WebApi.Specifications.Services;
using FluentAssertions;
using Ninject;
using TechTalk.SpecFlow;

namespace CroquetAustralia.WebApi.Specifications.Steps
{
    [Binding]
    public class SetupSteps
    {
        private readonly IApiWebsite _website;

        public SetupSteps(TestContext context)
            : this(context.ApiWebsite)
        {
        }

        private SetupSteps(IApiWebsite website)
        {
            _website = website;
        }

        [Given(@"the website is running and the setup procedure has not been run")]
        public void GivenTheWebsiteIsRunningAndTheSetupProcedureHasNotBeenRun()
        {
            _website.StartIfNotRunning();
        }

        [Given(@"the website is running and the setup procedure has been run")]
        public void GivenTheWebsiteIsRunningAndTheSetupProcedureHasBeenRun()
        {
            _website.StartIfNotRunning();
        }

        [Then(@"the setup procedure should run")]
        public void ThenTheSetupProcedureShouldRun()
        {
            _website.EventStore.GetAllAsync<Application>().Result.Count().Should().Be(1);
        }
    }
}