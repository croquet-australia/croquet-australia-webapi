using System;
using CroquetAustralia.CQRS;

namespace CroquetAustralia.WebApi.Specifications.Services
{
    public interface IApiWebsite : IDisposable
    {
        IEventStore EventStore { get; }
        int Port { get; }
        Uri Uri { get; }

        void StartIfNotRunning();
        void StartIfNotRunning(bool runSetup);
    }
}