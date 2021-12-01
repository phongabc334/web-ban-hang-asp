using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPham.Models
{
    [Serializable]
    public class Gio
    {
        public SanPham sanPham { get; set; }
        public int soLuong{ get; set; }
    }
}