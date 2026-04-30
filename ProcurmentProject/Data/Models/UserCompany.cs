using System;
using System.Collections.Generic;

namespace ProcurmentProject.Data.Models;

public partial class UserCompany
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? CompanyId { get; set; }

    public DateTime EffectToDatetime { get; set; }

    public DateTime? EffectFromDatetime { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual Company? Company { get; set; }

    public virtual User? User { get; set; }
}
