using System;
using System.Collections.Generic;

namespace QLBThietBiYTe.Models.Entities;

public partial class Hoadon
{
    public int Id { get; set; }

    public string Mahoadon { get; set; } = null!;

    public string? Tenkhachhang { get; set; }

    public DateTime? Ngaylap { get; set; }

    public decimal? Tongtien { get; set; }

    public virtual ICollection<Chitiethoadon> Chitiethoadons { get; set; } = new List<Chitiethoadon>();
}
