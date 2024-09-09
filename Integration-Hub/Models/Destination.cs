namespace Integration_Hub.Models
{
    public class Destination
    {
        public Guid Id { get; set; }
        public DestinationType Type { get; set; }
        public string? Configuration { get; set; } // JSON string for config (e.g., connection string)
        public string? DataStructure { get; set; } // JSON schema for the outgoing data
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

}
