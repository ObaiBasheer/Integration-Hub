namespace Integration_Hub.Models
{
    public class Mapping
    {
        public Guid Id { get; set; }
        public string? SourceDataStructure { get; set; } // JSON schema for the source data
        public string? DestinationDataStructure { get; set; } // JSON schema for the destination data
        public string? FieldMappings { get; set; } // JSON mapping rules
        public string? Transformations { get; set; } // Optional transformation rules
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

}
