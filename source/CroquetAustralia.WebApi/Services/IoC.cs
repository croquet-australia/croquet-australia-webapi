using System;
using Anotar.NLog;
using CroquetAustralia.CQRS;
using CroquetAustralia.CQRS.AzurePersistence;
using CroquetAustralia.CQRS.AzurePersistence.Infrastructure;
using CroquetAustralia.Domain;
using CroquetAustralia.Domain.App;
using CroquetAustralia.WebApi.Settings;
using Ninject;
using Ninject.Extensions.Conventions;

namespace CroquetAustralia.WebApi.Services
{
    internal class IoC
    {
        internal static IKernel CreateKernel()
        {
            return CreateKernel(new AzureSettings());
        }

        internal static IKernel CreateKernel(AzureSettings azureSettings)
        { 
        var kernel = new StandardKernel();

            kernel.Bind(syntax =>
            {
                syntax
                    .FromAssemblyContaining(typeof(ICommandBus), typeof(AzureEventStore))
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });

            kernel.Rebind<ICommandBus>().To<DomainCommandBus>().InSingletonScope();
            kernel.Rebind<IEventStore>().To<CroquetAustraliaEventStore>();
            kernel.Rebind<IEventTypes>().ToMethod(ctx => EventTypes.FromAssemblyContaining(typeof(RanSetup))).InSingletonScope();
            kernel.Bind<IServiceProvider>().ToConstant(kernel);

#if DEBUG
            LogTo.Debug(() => $"TableNameFormat: {azureSettings.TableNameFormat}");
            kernel.Rebind<ITableNameResolver>().ToConstructor(ctx => new TableNameResolver(azureSettings.TableNameFormat));
#endif

            return kernel;
        }
    }
}