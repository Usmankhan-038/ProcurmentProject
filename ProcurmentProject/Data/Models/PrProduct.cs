using System;
using System.Collections.Generic;

namespace ProcurmentProject.Models;

public partial class PrProduct
{
    public int Id { get; set; }

    public int? PrId { get; set; }

    public int? ProductId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte Deleted { get; set; }

    public virtual PurchasedRequisition? Pr { get; set; }

    public virtual Product? Product { get; set; }
}
