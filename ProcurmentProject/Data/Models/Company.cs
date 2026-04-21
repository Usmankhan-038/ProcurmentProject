using System;
using System.Collections.Generic;

namespace ProcurmentProject.Models;

public partial class Company
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? NtnNumber { get; set; }

    public string? RegisterIn { get; set; }

    public byte? Deleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
}
