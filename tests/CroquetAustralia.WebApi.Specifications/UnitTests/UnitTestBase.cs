using System;
using CroquetAustralia.CQRS;
using CroquetAustralia.WebApi.Controllers;
using CroquetAustralia.WebApi.Services;
using CroquetAustralia.WebApi.Settings;
using CroquetAustralia.WebApi.Specifications.Services;
using Ninject;

namespace CroquetAustralia.WebApi.Specifications.UnitTests
{
    public abstract class UnitTestBase
    {
        private readonly Lazy<IKernel> _kernel = new Lazy<IKernel>(IoC.CreateKernel);

        protected ApplicationController ApplicationController => Kernel.Get<ApplicationController>();
        protected ApplicationSettings ApplicationSettings => Kernel.Get<ApplicationSettings>();
        protected IEventStore EventStore => Kernel.Get<IEventStore>();
        protected IKernel Kernel => _kernel.Value;

        protected void UseInMemoryEventStore()
        {
            Kernel.Rebind<IEventStore>().To<InMemoryEventStore>().InSingletonScope();
        }

    }
}