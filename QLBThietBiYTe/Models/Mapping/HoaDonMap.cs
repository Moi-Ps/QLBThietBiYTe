using QLBThietBiYTe.Models.Entities;

namespace QLBThietBiYTe.Models.Mapping
{
    public class HoaDonMap
    {
        public string Mahoadon { get; set; } = null!;
        public string? Tenkhachhang { get; set; }
        public DateTime? Ngaylap { get; set; }
        public decimal? Tongtien { get; set; }
        public virtual ICollection<Chitiethoadon> Chitiethoadons { get; set; } = new List<Chitiethoadon>();
        //public List<ChiTietHoaDonMap> Chitiethoadons { get; set; } = new();

    }

}
