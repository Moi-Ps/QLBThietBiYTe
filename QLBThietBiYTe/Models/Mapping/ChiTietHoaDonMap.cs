using QLBThietBiYTe.Models.Entities;

namespace QLBThietBiYTe.Models.Mapping
{
    public class ChiTietHoaDonMap
    {
        public string Machitiet { get; set; } = null!;
        public string Mahoadon { get; set; } = null!;
        public string Mathietbi { get; set; } = null!;
        public int? Soluong { get; set; }
        public int? Giatien { get; set; }
        public int? Thanhtien { get; set; }
    }

}
