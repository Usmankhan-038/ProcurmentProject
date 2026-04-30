using System;
using System.Collections.Generic;

namespace ProcurmentProject.Data.Models;

public partial class SupplierQuotation
{
    public int Id { get; set; }

    public int? SupplierId { get; set; }

    public int? ProductId { get; set; }

    public int? RfqId { get; set; }

    public string? UnitPrice { get; set; }

    public int? Quantity { get; set; }

    public string? FinalPrice { get; set; }

    public byte? Deleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Rfq? Rfq { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
