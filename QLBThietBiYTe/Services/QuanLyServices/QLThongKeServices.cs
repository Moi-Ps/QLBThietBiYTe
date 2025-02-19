using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;
using System.Globalization;

namespace QLBThietBiYTe.Services.QuanLyServices
{
    public interface IQlThongKeServices
    {
        /*Task<dynamic> tkKho();
        Task<dynamic> tkHoaDon();
        Task<dynamic> tkLoaiThietBi();
        Task<dynamic> tkThietBi();
        Task<dynamic> tkNhaCungCap();*/
        Task<dynamic> tkDoanhThu(string tuNgay, string denNgay);
    }
    public class QLThongKeServices : IQlThongKeServices
    {
        private ThietBiYTeContext _context;
        private readonly IMapper _mapper;
        public QLThongKeServices(ThietBiYTeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<dynamic> tkDoanhThu(string tuNgay, string denNgay)
        {
            
            
            DateTime fromDate = DateTime.ParseExact(tuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            DateTime toDate = DateTime.ParseExact(denNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);

            var data = await _context.Hoadons
                .Where(hd => hd.Ngaylap.HasValue &&
                             hd.Ngaylap.Value >= fromDate &&
                             hd.Ngaylap.Value <= toDate)
                .Select(hd => new {
                    hd.Mahoadon,
                    hd.Tenkhachhang,
                    NgayLap = hd.Ngaylap.Value.ToString("dd/MM/yyyy"),
                    hd.Tongtien
                })
                .ToListAsync();

            var tongHoaDon = data.Sum(i => i.Tongtien);

            return new
            {
                HoaDon = data,
                tongHoaDon = tongHoaDon
            };
        }


    }
}
