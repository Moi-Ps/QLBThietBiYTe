using System;
using System.Collections.Generic;

namespace QLBThietBiYTe.Models.Entities;

public partial class Chitiethoadon
{
    public int Id { get; set; }

    public string Machitiet { get; set; } = null!;

    public string Mahoadon { get; set; } = null!;

    public string Mathietbi { get; set; } = null!;

    public int? Soluong { get; set; }

    public int? Giatien { get; set; }

    public int? Thanhtien { get; set; }

    public virtual Hoadon MahoadonNavigation { get; set; } = null!;

    public virtual Thietbi MathietbiNavigation { get; set; } = null!;
}
