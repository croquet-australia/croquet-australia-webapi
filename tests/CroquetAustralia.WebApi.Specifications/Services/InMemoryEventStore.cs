using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CroquetAustralia.CQRS;

namespace CroquetAustralia.WebApi.Specifications.Services
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly List<IAggregateEvents> _aggregateEventsCollection = new List<IAggregateEvents>();

        public Task AddEventsAsync(IEnumerable<IAggregateEvents> aggregateEventsCollection)
        {
            _aggregateEventsCollection.AddRange(aggregateEventsCollection);
            return Task.FromResult(0);
        }

        public Task<IEnumerable<IAggregateEvents>> GetAllAsync<TAggregate>() where TAggregate : IAggregate
        {
            return GetAllAsync(typeof(TAggregate));
        }

        public Task<IEnumerable<IAggregateEvents>> GetAllAsync(Type aggregateType)
        {
            var query =
                from aggregateEvents in _aggregateEventsCollection
                where aggregateEvents.AggregateType == aggregateType
                group aggregateEvents by aggregateEvents.AggregateId
                into grp
                select new AggregateEvents(aggregateType, grp.Key, grp.SelectMany(x => x.Events));

            return Task.FromResult<IEnumerable<IAggregateEvents>>(query);
        }

        public Task<IEnumerable<IEvent>> GetEventsAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEvent>> GetEventsAsync(Type aggregateType, Guid aggregateId)
        {
            throw new NotImplementedException();
        }
    }
}