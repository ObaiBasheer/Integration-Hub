namespace Integration_Hub.viewModel
{
    public class IntegrationViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }  // Example: "API to Database"
        public bool IsActive { get; set; }
        public DateTime? LastRunTime { get; set; }  // Last time this integration was executed
    }

}
