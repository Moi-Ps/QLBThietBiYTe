using System;
using System.Collections.Generic;

namespace QLBThietBiYTe.Models.Entities;

public partial class Kho
{
    public int Id { get; set; }

    public string Makho { get; set; } = null!;

    public string Mathietbi { get; set; } = null!;

    public int? Soluong { get; set; }

    public virtual Thietbi MathietbiNavigation { get; set; } = null!;
}
