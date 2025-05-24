namespace WebApp.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsFilled { get; set; } = false;
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;

        // Σχέση με την εταιρεία
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
    }

}
