using System.Linq;
using CroquetAustralia.Domain.App;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.WebApi.Specifications.UnitTests.Controllers
{
    public class ApplicationControllerTests : UnitTestBase
    {
        public class Setup : ApplicationControllerTests
        {
            [Fact]
            public void ShouldRunSetup()
            {
                // Given
                //UseInMemoryEventStore();

                var controller = ApplicationController;

                // When
                controller.SetupAsync().Wait();

                // Then
                var applications = EventStore.GetAllAsync<Application>().Result;
                var application = applications.Single();
                var events = application.Events;
                var @event = events.Single();

                @event.Should().BeOfType<RanSetup>();
            }
        }
    }
}