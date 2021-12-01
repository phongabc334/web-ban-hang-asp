namespace MyPham.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            Chi_Tiet_Gio_Hang = new HashSet<Chi_Tiet_Gio_Hang>();
        }


        [Key]
        [DisplayName("Mã sản phẩm")]
        public int MaSP { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống!")]

        [DisplayName("Tên sản phẩm")]
        public string TenSP { get; set; }

        [Required(ErrorMessage = "Giá không được để trống!")]
        [Column(TypeName = "money")]
        [DisplayName("Giá")]
        public decimal Gia { get; set; }

        [DisplayName("Thương hiệu")]
        [StringLength(80)]
        public string ThuongHieu { get; set; }

        [DisplayName("Số lượng")]
        [Required(ErrorMessage = "Số lượng không được để trống!")]
        public int SoLuongTon { get; set; }

        [DisplayName("Xuất Xứ")]
        [StringLength(80)]
        public string XuatXu { get; set; }

        [DisplayName("Dung tích")]
        public int? DungTich { get; set; }

        [StringLength(80)]
        [DisplayName("Hãng sản xuất.")]
        public string HangSX { get; set; }

        [DisplayName("Trọng lương")]
        public int? TrongLuong { get; set; }

        [DisplayName("Chất liệu")]
        [StringLength(60)]
        public string ChatLieu { get; set; }

        [Column(TypeName = "ntext")]
        [DisplayName("Công dụng")]
        public string CongDung { get; set; }

        [Column(TypeName = "ntext")]
        [DisplayName("Thành phần")]
        public string ThanhPhan { get; set; }

        [Column(TypeName = "ntext")]
        [DisplayName("Hướng dẫn sử dụng")]
        public string HuongDanSD { get; set; }

        [Column(TypeName = "ntext")]
        [DisplayName("Quy cách đóng gói")]
        public string QuyCachDongGoi { get; set; }

        [DisplayName("Giảm giá")]
        public double? GiamGia { get; set; }

        [Required(ErrorMessage = "Mã danh mục không được để trống!")]
        [DisplayName("Mã danh mục")]
        public int MaDM { get; set; }


        [StringLength(100)]
        [DisplayName("Ảnh")]
        public string AnhSP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chi_Tiet_Gio_Hang> Chi_Tiet_Gio_Hang { get; set; }

        public virtual DanhMucSP DanhMucSP { get; set; }
    }
}
