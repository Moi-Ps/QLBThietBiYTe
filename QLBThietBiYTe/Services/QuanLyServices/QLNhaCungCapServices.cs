using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;
using System.Threading.Tasks;

namespace QLBThietBiYTe.Services.QuanLyServices
{
    public interface IQlNhaCungCapServices
    {
        Task<dynamic> DanhSachNhaCungCap();
        Task<dynamic> GetMaNCC(string mancc);
        Task<dynamic> CreateNhaCungCap(NhaCungCapMap modelMap);
        Task<dynamic> UpdateNhaCungCap(NhaCungCapMap modelMap);
        Task<dynamic> deleteNhaCungCap(string mancc);
    }
    public class QLNhaCungCapServices : IQlNhaCungCapServices
    {
        private ThietBiYTeContext _context;
        private readonly IMapper _mapper;
        public QLNhaCungCapServices(ThietBiYTeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<dynamic> DanhSachNhaCungCap()
        {
            var data = await _context.Nhacungcaps.
            Select(x => new
            {
                Id = x.Id,
                MaNCC = x.Mancc,
                TenNhaCungCap = x.Tennhacungcap
            }).ToListAsync();
            return data;
        }
        public async Task<dynamic> CreateNhaCungCap(NhaCungCapMap modelMap)
        {
            Nhacungcap model = _mapper.Map<Nhacungcap>(modelMap);
            try
            {
                var data = await _context.Nhacungcaps.AddAsync(model);
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
        public async Task<dynamic> GetMaNCC(string mancc)
        {
            var data = await _context.Nhacungcaps
                .Select(x => new
                {
                    MaNCC = x.Mancc,
                    TenNhaCungCap = x.Tennhacungcap
                })
                .FirstOrDefaultAsync(x => x.MaNCC == mancc);
            return data;
        }
        public async Task<dynamic> UpdateNhaCungCap(NhaCungCapMap modelMap)
        {
            Nhacungcap model = _mapper.Map<Nhacungcap>(modelMap);

            try
            {
                var existingModel = await _context.Nhacungcaps.FindAsync(model.Mancc);
                if (existingModel == null)
                {
                    return new
                    {
                        statusCode = 404,
                        message = "Không tìm thấy nhà cung cấp!",
                    };
                }

                existingModel.Tennhacungcap = model.Tennhacungcap;
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
        public async Task<dynamic> deleteNhaCungCap(string mancc)
        {
            try
            {
                var nhaCC = await _context.Nhacungcaps.FindAsync(mancc);
                _context.Nhacungcaps.Remove(nhaCC);

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
