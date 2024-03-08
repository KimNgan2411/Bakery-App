using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bakery.Models.ViewModels
{
	public class CTHDVM
	{
		public sp_dshoadon_Result hd { get; set; }
		public List<sp_ds_cthd_Result> cthds { get; set; }
	}
}