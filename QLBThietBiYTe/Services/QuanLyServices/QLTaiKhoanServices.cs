using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;

namespace QLBThietBiYTe.Services.QuanLyServices
{
    public interface IQlTaiKhoanServices
    {
        Task<dynamic> read();
        Task<dynamic> getTaiKhoan(string maTK);
        Task<dynamic> createTaiKhoan(TaiKhoanMap modelMap);
        Task<dynamic> updateTaiKhoan(TaiKhoanMap modelMap);
        Task<dynamic> deleteTaiKhoan(string maTK);
    }
    public class QLTaiKhoanServices : IQlTaiKhoanServices
    {
        private ThietBiYTeContext _context;
        private readonly IMapper _mapper;
        public QLTaiKhoanServices(ThietBiYTeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<dynamic> read()
        {
            var data = await _context.Taikhoans.
            Select(x => new
            {
                Id = x.Id,
                MaTaiKhoan = x.Mataikhoan,
                UserName = x.Username,
                PassWord = x.PassWord,
                Role = x.Role
            }).ToListAsync();
            return data;
        }
        public async Task<dynamic> getTaiKhoan(string maTK)
        {
            var data = await _context.Taikhoans
                .Select(x => new
                {
                    Id = x.Id,
                    MaTaiKhoan = x.Mataikhoan,
                    UserName = x.Username,
                    PassWord = x.PassWord,
                    Role = x.Role
                })
                .FirstOrDefaultAsync(x => x.MaTaiKhoan == maTK);
            return data;
        }
        public async Task<dynamic> createTaiKhoan(TaiKhoanMap modelMap)
        {
            Taikhoan model = _mapper.Map<Taikhoan>(modelMap);
            try
            {
                var data = await _context.Taikhoans.AddAsync(model);
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
        public async Task<dynamic> updateTaiKhoan(TaiKhoanMap modelMap)
        {
            Taikhoan model = _mapper.Map<Taikhoan>(modelMap);

            try
            {
                var existingModel = await _context.Taikhoans.FindAsync(model.Mataikhoan);
                if (existingModel == null)
                {
                    return new
                    {
                        statusCode = 404,
                        message = "Không tìm thấy!",
                    };
                }

                existingModel.Username = model.Username;
                existingModel.PassWord = model.PassWord;
                existingModel.Role = model.Role;
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
        public async Task<dynamic> deleteTaiKhoan(string maTK)
        {
            try
            {
                var tk = await _context.Taikhoans.FindAsync(maTK);
                _context.Taikhoans.Remove(tk);

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
