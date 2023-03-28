namespace ITRootsTask.Models
{
    public class UserVerification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Email { get; set; }
        public string VerificationToken { get; set; }
        public DateTime VerificationTokenExpiry { get; set; }

    }
}
