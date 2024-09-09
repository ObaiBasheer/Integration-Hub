namespace Integration_Hub.Models
{
    public class Source
    {
        public Guid Id { get; set; }
        public SourceType Type { get; set; }
        public string? Configuration { get; set; } // JSON string for config (e.g., API URL, auth details)
        public string? DataStructure { get; set; } // JSON schema for the incoming data
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

}
