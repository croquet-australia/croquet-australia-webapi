using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using CroquetAustralia.Logging;
using CroquetAustralia.WebApi.Settings;
using CroquetAustralia.WebApi.Specifications.Services;
using FluentAssertions;
using FluentAssertions.Execution;
using TechTalk.SpecFlow;

namespace CroquetAustralia.WebApi.Specifications.Steps
{
    [Binding]
    public class CommonSteps
    {
        private readonly ActualData _actual;
        private readonly IApiWebsite _website;

        public CommonSteps(TestContext context) 
            : this(context.Actual, context.ApiWebsite)
        {
        }

        private CommonSteps(ActualData actual, IApiWebsite website)
        {
            _actual = actual;
            _website = website;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            LoggerConfiguration.StartLogging(new ApplicationSettings().IsDeveloperMachine);
        }

        [When(@"I call (.*)")]
        public void WhenICall(string resource)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = _website.Uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                _actual.Response = client.GetAsync(resource).Result;
            }
        }

        [Then(@"HTTP response status code should be (.*)")]
        public void ThenHTTPResponseStatusCodeShouldBe(string expectedResponseStatusCode)
        {
            var expected = (HttpStatusCode) Enum.Parse(typeof(HttpStatusCode), expectedResponseStatusCode);

            Execute.Assertion
                .ForCondition(_actual.Response.StatusCode.ToString().Equals(expectedResponseStatusCode))
                .FailWith($"Expected Response.StatusCode to be {expected}, but found {_actual.Response.StatusCode}.\n\n{_actual.Response.Content.ReadAsStringAsync().Result}");
        }

        [Then(@"HTTP response message should '(.*)'")]
        public void ThenHTTPResponseMessageShould(string expectedResponseMessage)
        {
            _actual.Response.ReasonPhrase.Should().Be(expectedResponseMessage);
        }
    }
}