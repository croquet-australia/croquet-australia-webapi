using System;
using CroquetAustralia.WebApi.Services;
using CroquetAustralia.WebApi.Settings;
using Ninject;

namespace CroquetAustralia.WebApi.Specifications.Services
{
    public class TestContext : IDisposable
    {
        public readonly IKernel Kernel;
        private bool _disposeApiWebsite;
        private bool _isDisposed;

        public TestContext(ActualData actual)
        {
            Actual = actual;
            Kernel = CreateKernel();
        }

        public IApiWebsite ApiWebsite => Kernel.Get<IApiWebsite>();
        public ActualData Actual { get; }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        private IKernel CreateKernel()
        {
            var kernel = IoC.CreateKernel();

            kernel.Rebind<IApiWebsite>().To<ApiWebsite>().OnActivation(website => _disposeApiWebsite = true);

            return kernel;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (_disposeApiWebsite)
                {
                    ApiWebsite.Dispose();
                }
            }

            _isDisposed = true;
        }

        public TService Get<TService>()
        {
            return Kernel.Get<TService>();
        }
    }
}