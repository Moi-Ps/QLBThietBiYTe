using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLBThietBiYTe.Models.Entities;

public partial class ThietBiYTeContext : DbContext
{
    public ThietBiYTeContext()
    {
    }

    public ThietBiYTeContext(DbContextOptions<ThietBiYTeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chitiethoadon> Chitiethoadons { get; set; }

    public virtual DbSet<Hoadon> Hoadons { get; set; }

    public virtual DbSet<Kho> Khos { get; set; }

    public virtual DbSet<Loaithietbi> Loaithietbis { get; set; }

    public virtual DbSet<Nhacungcap> Nhacungcaps { get; set; }

    public virtual DbSet<Taikhoan> Taikhoans { get; set; }

    public virtual DbSet<Thietbi> Thietbis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Connection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chitiethoadon>(entity =>
        {
            entity.HasKey(e => e.Machitiet).HasName("PK__CHITIETH__3006A9DEC7AB5EF1");

            entity.ToTable("CHITIETHOADON");

            entity.Property(e => e.Machitiet)
                .HasMaxLength(10)
                .HasColumnName("MACHITIET");
            entity.Property(e => e.Giatien).HasColumnName("GIATIEN");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Mahoadon)
                .HasMaxLength(10)
                .HasColumnName("MAHOADON");
            entity.Property(e => e.Mathietbi)
                .HasMaxLength(10)
                .HasColumnName("MATHIETBI");
            entity.Property(e => e.Soluong).HasColumnName("SOLUONG");
            entity.Property(e => e.Thanhtien)
                .HasComputedColumnSql("([SOLUONG]*[GIATIEN])", true)
                .HasColumnName("THANHTIEN");

            entity.HasOne(d => d.MahoadonNavigation).WithMany(p => p.Chitiethoadons)
                .HasForeignKey(d => d.Mahoadon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHITIETHOADON_HOADON");

            entity.HasOne(d => d.MathietbiNavigation).WithMany(p => p.Chitiethoadons)
                .HasForeignKey(d => d.Mathietbi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHITIETHOADON_THIETBI");
        });

        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.Mahoadon).HasName("PK__HOADON__A4999DF57F538E27");

            entity.ToTable("HOADON");

            entity.Property(e => e.Mahoadon)
                .HasMaxLength(10)
                .HasColumnName("MAHOADON");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Ngaylap)
                .HasColumnType("datetime")
                .HasColumnName("NGAYLAP");
            entity.Property(e => e.Tenkhachhang)
                .HasMaxLength(50)
                .HasColumnName("TENKHACHHANG");
            entity.Property(e => e.Tongtien)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("TONGTIEN");
        });

        modelBuilder.Entity<Kho>(entity =>
        {
            entity.HasKey(e => e.Makho).HasName("PK__KHO__7AFB3D161DD50BB8");

            entity.ToTable("KHO");

            entity.Property(e => e.Makho)
                .HasMaxLength(10)
                .HasColumnName("MAKHO");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Mathietbi)
                .HasMaxLength(10)
                .HasColumnName("MATHIETBI");
            entity.Property(e => e.Soluong).HasColumnName("SOLUONG");

            entity.HasOne(d => d.MathietbiNavigation).WithMany(p => p.Khos)
                .HasForeignKey(d => d.Mathietbi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KHO_THIETBI");
        });

        modelBuilder.Entity<Loaithietbi>(entity =>
        {
            entity.HasKey(e => e.Maloai).HasName("PK__LOAITHIE__2F633F23401B5F87");

            entity.ToTable("LOAITHIETBI");

            entity.Property(e => e.Maloai)
                .HasMaxLength(10)
                .HasColumnName("MALOAI");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Tenloaithietbi)
                .HasMaxLength(50)
                .HasColumnName("TENLOAITHIETBI");
        });

        modelBuilder.Entity<Nhacungcap>(entity =>
        {
            entity.HasKey(e => e.Mancc).HasName("PK__NHACUNGC__7ABEA5824DEB7D87");

            entity.ToTable("NHACUNGCAP");

            entity.Property(e => e.Mancc)
                .HasMaxLength(10)
                .HasColumnName("MANCC");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Tennhacungcap)
                .HasMaxLength(50)
                .HasColumnName("TENNHACUNGCAP");
        });

        modelBuilder.Entity<Taikhoan>(entity =>
        {
            entity.HasKey(e => e.Mataikhoan).HasName("PK__TAIKHOAN__2ED8B517E25C5487");

            entity.ToTable("TAIKHOAN");

            entity.Property(e => e.Mataikhoan)
                .HasMaxLength(10)
                .HasColumnName("MATAIKHOAN");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.PassWord)
                .HasMaxLength(100)
                .HasColumnName("PASS_WORD");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .HasDefaultValue("Admin")
                .HasColumnName("ROLE");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .HasColumnName("USERNAME");
        });

        modelBuilder.Entity<Thietbi>(entity =>
        {
            entity.HasKey(e => e.Mathietbi).HasName("PK__THIETBI__AF9850ED6494B5F5");

            entity.ToTable("THIETBI");

            entity.Property(e => e.Mathietbi)
                .HasMaxLength(10)
                .HasColumnName("MATHIETBI");
            entity.Property(e => e.Giaban).HasColumnName("GIABAN");
            entity.Property(e => e.Giamua).HasColumnName("GIAMUA");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Maloai)
                .HasMaxLength(10)
                .HasColumnName("MALOAI");
            entity.Property(e => e.Mancc)
                .HasMaxLength(10)
                .HasColumnName("MANCC");
            entity.Property(e => e.Namsanxuat).HasColumnName("NAMSANXUAT");
            entity.Property(e => e.Tenthietbi)
                .HasMaxLength(100)
                .HasColumnName("TENTHIETBI");

            entity.HasOne(d => d.MaloaiNavigation).WithMany(p => p.Thietbis)
                .HasForeignKey(d => d.Maloai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_THIETBI_LOAITHIETBI");

            entity.HasOne(d => d.ManccNavigation).WithMany(p => p.Thietbis)
                .HasForeignKey(d => d.Mancc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_THIETBI_NHACUNGCAP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
