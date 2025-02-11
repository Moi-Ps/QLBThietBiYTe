using System;
using System.Collections.Generic;

namespace QLBThietBiYTe.Models.Entities;

public partial class Nhacungcap
{
    public int Id { get; set; }

    public string Mancc { get; set; } = null!;

    public string Tennhacungcap { get; set; } = null!;

    public virtual ICollection<Thietbi> Thietbis { get; set; } = new List<Thietbi>();
}
