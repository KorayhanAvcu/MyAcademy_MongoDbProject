namespace Travel.Web.Entities.Common
{
    public class LocalizedText
    {
        public string Tr { get; set; } = string.Empty;
        public string En { get; set; } = string.Empty;

        public string Get(string lang) => lang?.ToLower() switch
        {
            "en" => En,
            _ => Tr
        };
    }
}
