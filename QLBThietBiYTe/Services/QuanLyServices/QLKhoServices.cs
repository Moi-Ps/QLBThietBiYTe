using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QLBThietBiYTe.Services.QuanLyServices
{
    public interface IQlKhoServices
    {
        Task<dynamic> DanhSachKho();
        Task<dynamic> CreateKho(KhoMap modelMap);
        Task<dynamic> PDFKho();

    }
    public class QLKhoServices : IQlKhoServices
    {
        private ThietBiYTeContext _context;
        private readonly IMapper _mapper;

        public QLKhoServices(ThietBiYTeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<dynamic> DanhSachKho()
        {
            var data = _context.Khos
                .GroupBy(x => x.MathietbiNavigation.Tenthietbi)
                .Select(y => new
                {
                    TenThietBi = y.Key,
                    TongSoLuong = y.Sum(y => y.Soluong)
                }).OrderBy(x => x.TenThietBi);
            return data;
        }
        public async Task<dynamic> CreateKho(KhoMap modelMap)
        {
            Kho model = _mapper.Map<Kho>(modelMap);
            try
            {
                var data = await _context.Khos.AddAsync(model);
                await _context.SaveChangesAsync();
                return new
                {
                    statusCode = 200,
                    message = "Thành công!",
                    data = model
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    statusCode = 500,
                    message = "Thất bại!",
                    error = ex.Message
                };
            }
        }
        public async Task<dynamic> PDFKho()
        {
            var query = await _context.Khos
                .GroupBy(x => x.MathietbiNavigation.Tenthietbi)
                .Select(y => new
                {
                    TenThietBi = y.Key,
                    TongSoLuong = y.Sum(y => y.Soluong)
                })
                .OrderBy(x => x.TenThietBi)
                .ToListAsync();

            return query;
        }
    }
}
