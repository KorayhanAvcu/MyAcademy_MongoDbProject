namespace Travel.Web.Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string BannerCollectionName { get; set; }
        public string RouteCollectionName { get; set; }
        public string DestinationCollectionName { get; set; }
        public string WhyChooseUsCollectionName { get; set; }
        public string TourCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }

    }
}