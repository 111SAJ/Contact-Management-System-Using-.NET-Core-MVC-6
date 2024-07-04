using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactManagement.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50)]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "User name is required")]
        [MaxLength(50)]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(50)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [NotMapped]
        public string? ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        [MaxLength(10, ErrorMessage = "Invalid phone number, phone number should be 10 digits")]
        [MinLength(10, ErrorMessage = "Invalid phone number, phone number should be 10 digits")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }
        public bool? isFavorite { get; set; } = false;
    }
}
