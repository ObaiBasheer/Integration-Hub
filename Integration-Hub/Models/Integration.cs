namespace Integration_Hub.Models
{
    public class Integration
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid SourceId { get; set; }
        public Guid DestinationId { get; set; }
        public Guid MappingId { get; set; }
        public string? Schedule { get; set; }
        public IntegrationStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime LastRunTime { get; set; }

        // Navigation Properties
        public Source? Source { get; set; }
        public Destination? Destination { get; set; }
        public Mapping? Mapping { get; set; }
    }

}
