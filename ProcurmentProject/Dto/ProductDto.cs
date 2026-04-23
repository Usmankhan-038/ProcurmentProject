using System.ComponentModel.DataAnnotations;

namespace ProcurmentProject.Dto
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; } = default!;
        public string? Company { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public string? Upc { get; set; } = default!;
    }
}
