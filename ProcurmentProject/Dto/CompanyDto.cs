using System.ComponentModel.DataAnnotations;

namespace ProcurmentProject.Dto
{
    public class CompanyDto
    {
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; } = default!;

        [Required(ErrorMessage = "NTN Number is required")]
        [RegularExpression(@"^[a-zA-z0-9]*$",ErrorMessage = "Only Number and Letter is required")]
        public string NTNNumber { get; set; } = default!;

        [RegularExpression(@"^[a-zA-Z]+$",ErrorMessage = "Only Letter are Allowed")]
        public string RegisterIn { get; set; } = default!;
    }
}
