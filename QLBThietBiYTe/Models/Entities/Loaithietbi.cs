using System;
using System.Collections.Generic;

namespace QLBThietBiYTe.Models.Entities;

public partial class Loaithietbi
{
    public int Id { get; set; }

    public string Maloai { get; set; } = null!;

    public string Tenloaithietbi { get; set; } = null!;

    public virtual ICollection<Thietbi> Thietbis { get; set; } = new List<Thietbi>();
}
