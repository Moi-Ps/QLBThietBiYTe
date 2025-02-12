using Microsoft.EntityFrameworkCore;
using QLBThietBiYTe.Models.Entities;
using QLBThietBiYTe.Models.Mapping;
using QLBThietBiYTe.Services.QuanLyServices;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<ThietBiYTeContext>(c =>
        c.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//builder.Services.AddScoped<INhaCungCapServices, NhaCungCapServices>();
//builder.Services.AddScoped<IHoaDonCTServices, ChiTietHoaDonServices>();
builder.Services.AddScoped<IQlKhoServices, QLKhoServices>();
//builder.Services.AddScoped<ILoaiThietBiServices, LoaiThietBiServices>();
//builder.Services.AddScoped<ITaiKhoanServices, TaiKhoanServices>();
//builder.Services.AddScoped<IThietBiServices, ThietBiServices>();
//builder.Services.AddScoped<IHoaDonServices, HoaDonServices>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
