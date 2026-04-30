using System;
using System.Collections.Generic;

namespace ProcurmentProject.Data.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public long RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
