using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;

namespace QLBThietBiYTe.Services.QuanLyServices
{
    public interface IQlLoaiThietBiServices
    {
        Task<dynamic> DanhSachLoaiThietBi();
        Task<dynamic> GetMaLoai(string maLTB);
        Task<dynamic> CreateLoaiThietBi(LoaiThietBiMap modelMap);
        Task<dynamic> DeleteLoaiThietBi(string maLTB);
        Task<dynamic> UpdateLoaiThietBi(LoaiThietBiMap modelMap);
    }
    public class QLLoaiThietBiServices : IQlLoaiThietBiServices
    {
        private ThietBiYTeContext _context;
        private readonly IMapper _mapper;
        public QLLoaiThietBiServices(ThietBiYTeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<dynamic> DanhSachLoaiThietBi()
        {
            var data = await _context.Loaithietbis.
            Select(x => new
            {
                Id = x.Id,
                MaLoai = x.Maloai,
                TenLoaiThietBi = x.Tenloaithietbi
            }).ToListAsync();
            return data;
        }
        public async Task<dynamic> GetMaLoai(string maLTB)
        {
            var data = await _context.Loaithietbis
                .Select(x => new
                {
                    MaLoai = x.Maloai,
                    TenLoaiThietBi = x.Tenloaithietbi
                })
                .FirstOrDefaultAsync(x => x.MaLoai == maLTB);
            return data;
        }
        public async Task<dynamic> CreateLoaiThietBi(LoaiThietBiMap modelMap)
        {
            Loaithietbi model = _mapper.Map<Loaithietbi>(modelMap);
            try
            {
                var data = await _context.Loaithietbis.AddAsync(model);
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
        public async Task<dynamic> UpdateLoaiThietBi(LoaiThietBiMap modelMap)
        {
            Loaithietbi model = _mapper.Map<Loaithietbi>(modelMap);

            try
            {
                var existingModel = await _context.Loaithietbis.FindAsync(model.Maloai);
                if (existingModel == null)
                {
                    return new
                    {
                        statusCode = 404,
                        message = "Không tìm thấy!",
                    };
                }
                existingModel.Tenloaithietbi = model.Tenloaithietbi;
                await _context.SaveChangesAsync();
                return new
                {
                    statusCode = 200,
                    message = "Cập nhật thành công!",
                    data = existingModel
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
        public async Task<dynamic> DeleteLoaiThietBi(string maLTB)
        {
            try
            {
                var dlt = await _context.Loaithietbis.FindAsync(maLTB);

                if (dlt == null)
                {
                    return new
                    {
                        statusCode = 404,
                        message = "Không tìm thấy loại thiết bị!"
                    };
                }
                _context.Loaithietbis.Remove(dlt);

                await _context.SaveChangesAsync();
                return new
                {
                    statusCode = 200,
                    message = "Thành công!"
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    statusCode = 500,
                    message = "Thất bại!",
                };
            }

        }
    }
}
