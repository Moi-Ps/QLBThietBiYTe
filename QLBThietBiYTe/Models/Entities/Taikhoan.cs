using System;
using System.Collections.Generic;

namespace QLBThietBiYTe.Models.Entities;

public partial class Taikhoan
{
    public int Id { get; set; }

    public string Mataikhoan { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? PassWord { get; set; }

    public string? Role { get; set; }
}
