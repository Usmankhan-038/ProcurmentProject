using System.ComponentModel.DataAnnotations;

namespace ProcurmentProject.Dto
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "Name is Required")]
        public string name { get; set; } = default!;
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Please Enter correct email")]
        public string email { get; set; } = default!;
        [StringLength(11, MinimumLength = 11, ErrorMessage = "The Phone number must be 11 digit")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "The Number contain only digits")]
        public string phone { get; set; } = default!;
        public string companyName { get; set; } = default!;
        [StringLength(100,MinimumLength = 6,ErrorMessage = "Minimum Length should be 6 character")]
        public string password { get; set; } = default!;
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Minimum Length should be 6 character")]
        public string confirmpassword { get; set; } = default!;

    }
}
