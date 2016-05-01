namespace CroquetAustralia.WebApi.Settings
{
    public class AzureSettings : BaseAppSettings
    {
        public AzureSettings() 
            : base("Azure")
        {
        }

        public string StorageConnectionString => Get(nameof(StorageConnectionString));
        public string TableNameFormat => Get(nameof(TableNameFormat));
    }
}