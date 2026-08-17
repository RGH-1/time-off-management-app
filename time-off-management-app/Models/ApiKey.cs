namespace time_off_management_app.Models
{
    public class ApiKey
    {
        public int Id { get; set; }
        public String Name { get; set; } = string.Empty;
        public String KeyHash { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
