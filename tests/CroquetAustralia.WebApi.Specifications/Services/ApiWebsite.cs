using CroquetAustralia.CQRS;
using CroquetAustralia.CQRS.AzurePersistence.Infrastructure;
using CroquetAustralia.WebApi.Services;
using CroquetAustralia.WebApi.Settings;
using CroquetAustralia.WebApi.Specifications.Configuration;

namespace CroquetAustralia.WebApi.Specifications.Services
{
    public class ApiWebsite : WebsiteBase, IApiWebsite
    {
        private readonly ITableNameResolver _tableNameResolver;
        private readonly IEventSerializer _eventSerializer;
        private readonly IEventDeserializer _eventDeserializer;

        public ApiWebsite(SolutionConfiguration solutionConfiguration, AzureSettings azureSettings, ITableNameResolver tableNameResolver, IEventSerializer eventSerializer, IEventDeserializer eventDeserializer) 
            : base(solutionConfiguration.Directory, "CroquetAustralia.WebApi", 44301, azureSettings)
        {
            _tableNameResolver = tableNameResolver;
            _eventSerializer = eventSerializer;
            _eventDeserializer = eventDeserializer;
        }

        public IEventStore EventStore => new CroquetAustraliaEventStore(AzureSettings, _tableNameResolver, _eventSerializer, _eventDeserializer);
    }
}