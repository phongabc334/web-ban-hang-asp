using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MyPham.Models
{
    public partial class MyPhamDB : DbContext
    {
        public MyPhamDB()
            : base("name=MyPhamDB")
        {
        }

        public virtual DbSet<Chi_Tiet_Gio_Hang> Chi_Tiet_Gio_Hang { get; set; }
        public virtual DbSet<DanhMucSP> DanhMucSP { get; set; }
        public virtual DbSet<GioHang> GioHang { get; set; }
        public virtual DbSet<HoaDon> HoaDon { get; set; }
        public virtual DbSet<PhanQuyen> PhanQuyen { get; set; }
        public virtual DbSet<SanPham> SanPham { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DanhMucSP>()
                .HasMany(e => e.SanPham)
                .WithRequired(e => e.DanhMucSP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GioHang>()
                .HasMany(e => e.Chi_Tiet_Gio_Hang)
                .WithRequired(e => e.GioHang)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GioHang>()
                .HasMany(e => e.HoaDon)
                .WithRequired(e => e.GioHang)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<PhanQuyen>()
                .HasMany(e => e.TaiKhoan)
                .WithRequired(e => e.PhanQuyen)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.Gia)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.Chi_Tiet_Gio_Hang)
                .WithRequired(e => e.SanPham)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.SoDienThoai)
                .IsFixedLength();

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.GioHang)
                .WithRequired(e => e.TaiKhoan)
                .WillCascadeOnDelete(false);
        }
    }
}
