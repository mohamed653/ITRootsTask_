using System.ComponentModel.DataAnnotations;

namespace ITRootsTask.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z]).+$", 
        ErrorMessage = "Password must contain at least one uppercase letter and one lowercase letter")]
        public string Password { get; set; }
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        public UserType UserType { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public UserVerification? UserVerification { get; set; }
        public bool IsActive { get; set; } = false;
 
    }
}
