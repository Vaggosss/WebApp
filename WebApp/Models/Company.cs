namespace WebApp.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;

        // Λίστα αγγελιών της εταιρείας
        public List<Job> Jobs { get; set; } = new();
    }
}