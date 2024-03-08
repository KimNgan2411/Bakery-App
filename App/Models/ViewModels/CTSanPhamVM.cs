using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bakery.Models.ViewModels
{
	public class CTSanPhamVM
	{
		public sp_ChiTietSP_Result CTSP { get; set; }
		public List<sp_XemDanhGiaSP_Result> DanhGia { get; set; }
	}
}