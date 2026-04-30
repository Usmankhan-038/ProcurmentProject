using System.Diagnostics;

namespace ProcurmentProject.Dto
{
    public class SupplierViewDto
    {
        public int QuotationId { get; set; }
        public string Title { get; set; } = default!;
        public string RfqStatus { get; set; } = default!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal FinalPrice { get; set; }
        public string RecivedByName { get; set; } = default!;
        public string DeliveryStatus { get; set; } = default!;
        public string DeliveryNote { get; set; } = default!;
        public DateTime RecevingDatetime { get; set; }
    }
}
