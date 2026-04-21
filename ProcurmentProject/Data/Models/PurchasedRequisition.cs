using System;
using System.Collections.Generic;

namespace ProcurmentProject.Models;

public partial class PurchasedRequisition
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public string? EstimatedBudget { get; set; }

    public string Title { get; set; } = null!;

    public DateOnly? DeliveryDate { get; set; }

    public string? Note { get; set; }

    public byte? Deleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<PrProduct> PrProducts { get; set; } = new List<PrProduct>();

    public virtual ICollection<Rfq> Rfqs { get; set; } = new List<Rfq>();

    public virtual User? User { get; set; }
}
