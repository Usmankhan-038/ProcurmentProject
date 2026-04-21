using System.ComponentModel.DataAnnotations;

namespace ProcurmentProject.Dto
{
    public class PurchasedRequisitionDto
    {
        //[RegularExpression(@"^/d$",ErrorMessage = "Only number can accept")]
        [Required]
        public int quantity { get; set; }
        public string estimated_budget { get; set; } = default!;
        [Required]
        public string title { get; set; } = default!;
        public DateOnly? deliveryDate { get; set; }
        public string? note { get; set; } = default!;
    }
}
