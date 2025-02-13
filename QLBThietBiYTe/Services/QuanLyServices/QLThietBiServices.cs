using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;
using System;

namespace QLBThietBiYTe.Services.QuanLyServices
{
    public interface IQlThietBiServices
    {
        Task<dynamic> DanhSachThietBi();
        Task<dynamic> GetMaThietBi(string matb);
        Task<dynamic> CreateThietBi(ThietBiMap modelMap);
        Task<dynamic> UpdateThietBi(ThietBiMap modelMap);
        Task<dynamic> DeleteThietBi(string matb);
    }
    public class QLThietBiServices : IQlThietBiServices
    {
        private ThietBiYTeContext _context;
        private readonly IMapper _mapper;
        public QLThietBiServices(ThietBiYTeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<dynamic> DanhSachThietBi()
        {
            var data = await _context.Thietbis.
            Select(x => new
            {
                Id = x.Id,
                MaThietBi = x.Mathietbi,
                TenThietBi = x.Tenthietbi,
                TenLoaiThietBi = x.MaloaiNavigation.Tenloaithietbi,
                TenNhaCungCap = x.ManccNavigation.Tennhacungcap,
                GiaMua = x.Giamua,
                GiaBan = x.Giaban,
                NamSanXuat = x.Namsanxuat
            }).ToListAsync();
            return data;
        }
        public async Task<dynamic> CreateThietBi(ThietBiMap modelMap)
        {
            Thietbi model = _mapper.Map<Thietbi>(modelMap);
            try
            {
                var data = await _context.Thietbis.AddAsync(model);
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
        public async Task<dynamic> GetMaThietBi(string matb)
        {
            var data = await _context.Thietbis
                .Select(x => new
                {
                    Id = x.Id,
                    MaThietBi = x.Mathietbi,
                    TenThietBi = x.Tenthietbi,
                    TenLoaiThietBi = x.MaloaiNavigation.Tenloaithietbi,
                    TenNhaCungCap = x.ManccNavigation.Tennhacungcap,
                    GiaMua = x.Giamua,
                    GiaBan = x.Giaban,
                    NamSanXuat = x.Namsanxuat
                })
                .FirstOrDefaultAsync(x => x.MaThietBi == matb);
            return data;
        }
        public async Task<dynamic> UpdateThietBi(ThietBiMap modelMap)
        {
            Thietbi model = _mapper.Map<Thietbi>(modelMap);

            try
            {
                var existingModel = await _context.Thietbis.FindAsync(model.Mathietbi);
                if (existingModel == null)
                {
                    return new
                    {
                        statusCode = 404,
                        message = "Không tìm thấy!",
                    };
                }

                existingModel.Tenthietbi = model.Tenthietbi;
                existingModel.Maloai = model.Maloai;
                existingModel.Mancc = model.Mancc;
                existingModel.Giamua = model.Giamua;
                existingModel.Giaban = model.Giaban;
                existingModel.Namsanxuat = model.Namsanxuat;

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
        public async Task<dynamic> DeleteThietBi(string matb)
        {
            try
            {
                var thietBi = await _context.Thietbis.FindAsync(matb);
                _context.Thietbis.Remove(thietBi);

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
