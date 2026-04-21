using System;
using System.Collections.Generic;

namespace ProcurmentProject.Models;

public partial class Document
{
    public int Id { get; set; }

    public int BelongId { get; set; }

    public string BelongName { get; set; } = null!;

    public string EncodedFileName { get; set; } = null!;

    public string OriginalFileName { get; set; } = null!;

    public string Url { get; set; } = null!;

    public byte? Deleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
