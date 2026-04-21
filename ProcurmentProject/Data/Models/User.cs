using System;
using System.Collections.Generic;

namespace ProcurmentProject.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public byte? Deleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<PurchasedRequisition> PurchasedRequisitions { get; set; } = new List<PurchasedRequisition>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public virtual ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
