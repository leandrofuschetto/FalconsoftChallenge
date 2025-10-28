namespace RecruitingChallenge.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
    }
}
