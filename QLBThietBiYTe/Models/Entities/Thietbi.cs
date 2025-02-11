using System;
using System.Collections.Generic;

namespace QLBThietBiYTe.Models.Entities;

public partial class Thietbi
{
    public int Id { get; set; }

    public string Mathietbi { get; set; } = null!;

    public string? Tenthietbi { get; set; }

    public string Maloai { get; set; } = null!;

    public int? Giamua { get; set; }

    public int? Giaban { get; set; }

    public string Mancc { get; set; } = null!;

    public DateOnly? Namsanxuat { get; set; }

    public virtual ICollection<Chitiethoadon> Chitiethoadons { get; set; } = new List<Chitiethoadon>();

    public virtual ICollection<Kho> Khos { get; set; } = new List<Kho>();

    public virtual Loaithietbi MaloaiNavigation { get; set; } = null!;

    public virtual Nhacungcap ManccNavigation { get; set; } = null!;
}
