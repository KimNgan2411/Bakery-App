using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bakery.Models.ViewModels
{
    public class CTKMVM
    {
        public sp_detail_khuyenMai_Result km { get; set; }
        public List<sp_dssp_khuyenMai_Result> sps { get; set; }
    }
}