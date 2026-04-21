using ProcurmentProject.Models;

namespace ProcurmentProject.Dto
{
    public class SuppliersDto
    {
        public SignUpDto userData { get; set; } = default!;
        public string CompanyName { get; set; } = default!;
        public string NtnTaxNumber { get; set; } = default!;

    }
}
