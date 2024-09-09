namespace Integration_Hub.Config
{
    public class IntegrationSettings
    {
        public Dictionary<string, ApiSourceConfig>? Sources { get; set; }
        public Dictionary<string, DatabaseDestinationConfig>? Destinations { get; set; }
    }
}
