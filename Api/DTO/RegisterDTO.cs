using System.ComponentModel.DataAnnotations;

namespace Api.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(11)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please provide a value for the role name field")]
        public string NameRole { get; set; }
    }
}
