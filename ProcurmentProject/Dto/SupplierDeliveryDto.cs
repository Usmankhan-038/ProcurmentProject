namespace ProcurmentProject.Dto
{
    public class SupplierDeliveryDto
    {
        public string RecivingDateTime { get; set; } = default!;
        public string RecivedBy { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string Note { get; set; } = default!;
    }
}
