namespace Integration_Hub.Models
{
    public class Log
    {
        public Guid Id { get; set; }
        public Guid IntegrationId { get; set; }
        public string? Message { get; set; }
        public LogLevels LogLevel { get; set; }
        public DateTime Timestamp { get; set; }

        // Navigation Property
        public Integration? Integration { get; set; }
    }

}
