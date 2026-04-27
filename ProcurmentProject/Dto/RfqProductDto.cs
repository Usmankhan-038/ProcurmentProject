namespace ProcurmentProject.Dto
{
    public class RfqProductDto
    {
        public string ProductName { get; set; } = default!;
        public string? ProductCompany { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductUPC { get; set; }
    }
}
