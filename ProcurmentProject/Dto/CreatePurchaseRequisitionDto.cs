using System.ComponentModel.DataAnnotations;

namespace ProcurmentProject.Dto
{
    public class CreatePurchaseRequisitionDto
    {
        public PurchasedRequisitionDto PrRequest { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
