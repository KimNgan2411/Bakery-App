using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bakery.Models.ViewModels
{
	public class CTKHVM
	{
		public sp_profilekhachhang_Result kh { get; set; }
		public List<CTHDVM> hds {get;set;}
	}
}