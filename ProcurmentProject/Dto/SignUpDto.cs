using System.ComponentModel.DataAnnotations;

namespace ProcurmentProject.Dto
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Please Enter correct email")]
        public string Email { get; set; } = default!;
        [StringLength(11, MinimumLength = 11, ErrorMessage = "The Phone number must be 11 digit")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "The Number contain only digits")]
        public string Phone { get; set; } = default!;
        public string CompanyName { get; set; } = default!;
        [StringLength(100,MinimumLength = 6,ErrorMessage = "Minimum Length should be 6 character")]
        public string Password { get; set; } = default!;
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Minimum Length should be 6 character")]
        public string Confirmpassword { get; set; } = default!;

    }
}
