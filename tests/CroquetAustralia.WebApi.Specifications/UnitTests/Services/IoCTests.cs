using System;
using System.Linq;
using System.Web.Http;
using CroquetAustralia.WebApi.Controllers;
using CroquetAustralia.WebApi.Services;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.WebApi.Specifications.UnitTests.Services
{
    public class IoCTests
    {
        public class CreateKernel : IoCTests
        {
            [Fact]
            public void ShouldReturnConfiguredKernel()
            {
                // Given

                // When
                var kernel = IoC.CreateKernel();

                // Then
                kernel.Should().NotBeNull();
            }

            [Fact]
            public void ShouldReturnKernelThatCanCreateControllers()
            {
                // Given
                var kernel = IoC.CreateKernel();
                var controllerBaseType = typeof(ApiController);
                var controllerTypes =
                    from type in typeof(ApplicationController).Assembly.GetTypes()
                    where controllerBaseType.IsAssignableFrom(type)
                    select type;

                // When
                foreach (var controllerType in controllerTypes)
                {
                    // Then
                    kernel.GetService(controllerType).Should().NotBeNull();
                }
            }
        }
    }
}
