using System.ComponentModel.DataAnnotations;

namespace ProcurmentProject.Dto
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; } = default!;
        public string? company { get; set; } = default!;
        public string? description { get; set; } = default!;
        public string? upc { get; set; } = default!;
    }
}
