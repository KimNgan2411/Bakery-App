using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bakery.Models.ViewModels
{
	public class SignUpVM
	{
		public string TenKH { get; set; }
		public Nullable<int> GioiTinh { get; set; }
		public string SoDienThoai { get; set; }
		public Nullable<System.DateTime> NgaySinh { get; set; }
		public string TaiKhoan { get; set; }
		public string MatKhau { get; set; }
	}
}