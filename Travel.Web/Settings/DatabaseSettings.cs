namespace Travel.Web.Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string BannerCollectionName { get; set; }
        public string RouteCollectionName { get; set; }
    }
}
