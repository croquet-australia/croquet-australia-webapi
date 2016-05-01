using CroquetAustralia.CQRS.AzurePersistence;
using CroquetAustralia.CQRS.AzurePersistence.Infrastructure;
using CroquetAustralia.WebApi.Settings;

namespace CroquetAustralia.WebApi.Services
{
    internal class CroquetAustraliaEventStore : AzureEventStore
    {
        public CroquetAustraliaEventStore(AzureSettings azureSettings, ITableNameResolver tableNameResolver, IEventSerializer eventSerializer, IEventDeserializer eventDeserializer) 
            : base(azureSettings.StorageConnectionString, tableNameResolver, eventSerializer, eventDeserializer)
        {
        }
    }
}