using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bakery.Models.ViewModels
{
    public class HomeVM
    {
        public List<sp_DSSP_Result> sanphams { get; set; }
        public List<sp_ds_khuyenMai_Result> khuyenmais { get; set; }
    }
}