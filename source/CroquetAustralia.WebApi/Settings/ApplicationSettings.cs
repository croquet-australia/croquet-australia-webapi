namespace CroquetAustralia.WebApi.Settings
{
    public class ApplicationSettings : BaseAppSettings
    {
        public ApplicationSettings()
            : base("Application")
        {
        }

        public string InitialAdministratorEmailAddress => Get(nameof(InitialAdministratorEmailAddress));
        public bool IsDeveloperMachine => GetBoolean(nameof(IsDeveloperMachine));
    }
}