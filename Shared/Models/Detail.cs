namespace Shared.Models
{
    public abstract class Detail
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public string UdatedAt { get; set; } = string.Empty;
        public bool DomainStatus { get; set; } = false;

    }
}