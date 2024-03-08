using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bakery.Models.ViewModels
{
    public class ThemHDVM
    {
        public int id { get; set; }
        public List<sp_ChiTietSP_Result> sps { get; set; }
    }
}