using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Anotar.NLog;
using CroquetAustralia.WebApi.Settings;
using IISExpressMagic;
using Microsoft.WindowsAzure.Storage;
using OpenMagic.Extensions;

namespace CroquetAustralia.WebApi.Specifications.Services
{
    public abstract class WebsiteBase
    {
        // todo: read port from config file
        // todo: validate websiteName is in config file

        private readonly FileInfo _configFile;
        private readonly string _websiteName;

        protected readonly AzureSettings AzureSettings;

        private IISExpress _iisExpress;
        private bool _isDisposed;

        protected WebsiteBase(string solutionDirectory, string websiteName, int port, AzureSettings azureSettings)
            : this(new DirectoryInfo(solutionDirectory), websiteName, port, azureSettings)
        {
        }

        protected WebsiteBase(DirectoryInfo solutionDirectory, string websiteName, int port, AzureSettings azureSettings)
            : this(GetConfigFile(solutionDirectory), websiteName, port, azureSettings)
        {
        }

        protected WebsiteBase(FileInfo configFile, string websiteName, int port, AzureSettings azureSettings)
        {
            if (!configFile.Exists)
            {
                throw new ArgumentException($"Cannot find config file '{configFile.FullName}'.", nameof(configFile));
            }

            _configFile = configFile;
            _websiteName = websiteName;
            AzureSettings = azureSettings;

            Port = port;
        }

        public int Port { get; }
        public Uri Uri => new Uri($"https://localhost:{Port}/");

        // ReSharper disable once SuggestBaseTypeForParameter
        private static FileInfo GetConfigFile(DirectoryInfo solutionDirectory)
        {
            if (!solutionDirectory.Exists)
            {
                throw new ArgumentException($"Cannot find solution directory '{solutionDirectory.FullName}'.", nameof(solutionDirectory));
            }

            return new FileInfo(Path.Combine(solutionDirectory.FullName, @".vs\config\applicationhost.config"));
        }

        public void StartIfNotRunning()
        {
            StartIfNotRunning(true);
        }

        public void StartIfNotRunning(bool runSetup)
        {
            ClearAll();

            if (runSetup)
            {
                RunSetup();
            }

            var sw = Stopwatch.StartNew();


            if (IsRunning())
            {
                return;
            }

            Start();

            while (!IsRunning() && sw.ElapsedMilliseconds < 2000)
            {
                Thread.Sleep(1);
            }

            if (!IsRunning())
            {
                throw new Exception($"Cannot start website '{Uri}'.");
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing && _iisExpress != null)
            {
                _iisExpress.Dispose();
                _iisExpress = null;
            }
            _isDisposed = true;
        }

        private void ClearAll()
        {
            LogTo.Trace($"{nameof(ClearAll)}()");

            var storageAccount = CloudStorageAccount.Parse(AzureSettings.StorageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var tableNamePrefix = AzureSettings.TableNameFormat.Replace("{0}", "");
            var tables = tableClient.ListTables(tableNamePrefix).ToArray();

            LogTo.Debug($"Found {tables.Length} tables to delete.");
            foreach (var table in tables)
            {
                LogTo.Debug($"Delete table '{table.Name}'");
                table.Delete();
            }
        }

        private bool IsRunning()
        {
            return Uri.IsResponding();
        }

        private static void RunSetup()
        {
            // todo: Nothing to do until setup has been written.
        }

        private void Start()
        {
            _iisExpress = new IISExpress(Arguments.UseConfigFile(_configFile.FullName, _websiteName));
        }
    }
}