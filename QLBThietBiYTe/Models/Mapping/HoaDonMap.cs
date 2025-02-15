namespace QLBThietBiYTe.Models.Mapping
{
    public class HoaDonMap
    {
        public int Id { get; set; }
        public string Mahoadon { get; set; } = null!;

        public string? Tenkhachhang { get; set; }

        public DateTime? Ngaylap { get; set; }

        public string? Tongtien { get; set; }
        
    }
}
