using System;
using System.Collections.Generic;

namespace ProcurmentProject.Data.Models;

public partial class Rfq
{
    public int Id { get; set; }

    public int? PrId { get; set; }

    public string? Status { get; set; }

    public byte? Deleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual PurchasedRequisition? Pr { get; set; }

    public virtual ICollection<SupplierQuotation> SupplierQuotations { get; set; } = new List<SupplierQuotation>();

    public virtual ICollection<SuppliesDelivery> SuppliesDeliveries { get; set; } = new List<SuppliesDelivery>();
}
