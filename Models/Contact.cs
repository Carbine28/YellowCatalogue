using System.ComponentModel.DataAnnotations;

namespace PhoneBookApp.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string? Name { get; set; }
        [Required (ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [MinLength(9, ErrorMessage = "Phone number must be at least 9 characaters long") ]
        public string? PhoneNumber { get; set; }
    }
}
