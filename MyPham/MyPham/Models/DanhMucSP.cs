namespace MyPham.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhMucSP")]
    public partial class DanhMucSP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DanhMucSP()
        {
            SanPham = new HashSet<SanPham>();
        }

        [DisplayName("Mã danh mục")]
        [Key]
        public int MaDM { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống!")]
        [StringLength(80)]
        [DisplayName("Tên danh mục")]
        public string TenDM { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPham { get; set; }
    }
}
