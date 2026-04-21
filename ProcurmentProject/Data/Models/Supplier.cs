using System;
using System.Collections.Generic;

namespace ProcurmentProject.Models;

public partial class Supplier
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? NtnTaxNumber { get; set; }

    public byte? Deleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<SupplierQuotation> SupplierQuotations { get; set; } = new List<SupplierQuotation>();

    public virtual ICollection<SuppliesDelivery> SuppliesDeliveries { get; set; } = new List<SuppliesDelivery>();

    public virtual User User { get; set; } = null!;
}
