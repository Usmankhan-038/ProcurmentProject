using System;
using System.Collections.Generic;

namespace ProcurmentProject.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Company { get; set; }

    public string? Description { get; set; }

    public string? Upc { get; set; }

    public byte? Deleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<PrProduct> PrProducts { get; set; } = new List<PrProduct>();

    public virtual ICollection<SupplierQuotation> SupplierQuotations { get; set; } = new List<SupplierQuotation>();
}
