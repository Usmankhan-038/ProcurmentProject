using System;
using System.Collections.Generic;

namespace ProcurmentProject.Models;

public partial class SuppliesDelivery
{
    public int Id { get; set; }

    public int? RfqId { get; set; }

    public int? SuplierId { get; set; }

    public DateTime? RecevingDatetime { get; set; }

    public string? RecivedBy { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public byte? Deleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Rfq? Rfq { get; set; }

    public virtual Supplier? Suplier { get; set; }
}
