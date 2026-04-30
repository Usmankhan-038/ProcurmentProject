using System;

namespace ProcurmentProject.Models;

public partial class SupplierDeliveryView
{
    public string Title { get; set; } = null!;

    public string? RfqStatus { get; set; }

    public string? UnitPrice { get; set; }

    public int? Quantity { get; set; }

    public string? FinalPrice { get; set; }

    public string? RecivedByName { get; set; }

    public string? DeliveryStatus { get; set; }

    public string? DeliveryNote { get; set; }

    public DateTime? RecevingDatetime { get; set; }
}
