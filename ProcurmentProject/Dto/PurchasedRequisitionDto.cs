using System.ComponentModel.DataAnnotations;

namespace ProcurmentProject.Dto
{
    public class PurchasedRequisitionDto
    {
        //[RegularExpression(@"^/d$",ErrorMessage = "Only number can accept")]
        [Required]
        public int Quantity { get; set; }
        public string Estimated_budget { get; set; } = default!;
        [Required]
        public string Title { get; set; } = default!;
        public DateOnly? DeliveryDate { get; set; }
        public string? Note { get; set; } = default!;
    }
}
