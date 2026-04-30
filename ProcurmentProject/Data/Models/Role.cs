using System;
using System.Collections.Generic;

namespace ProcurmentProject.Data.Models;

public partial class Role
{
    public string Name { get; set; } = null!;

    public string Permission { get; set; } = null!;

    public byte? Deleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long Id { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
