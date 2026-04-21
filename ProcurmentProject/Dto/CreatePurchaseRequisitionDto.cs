using System.ComponentModel.DataAnnotations;

namespace ProcurmentProject.Dto
{
    public class CreatePurchaseRequisitionDto
    {
        public PurchasedRequisitionDto prRequest { get; set; }
        public List<ProductDto> products { get; set; }
    }
}
