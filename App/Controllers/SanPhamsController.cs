using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bakery.Models;
using Bakery.Models.ViewModels;

namespace Bakery.Controllers
{
    public class SanPhamsController : Controller
    {
        private BakeryStoreDBEntities db = new BakeryStoreDBEntities();

        // GET: SanPhams
		public ActionResult Index(bool? tinhtrang, string keyword, int? maloai, int? page, int? pagelength, int? orderOpt)
		{
			if (!pagelength.HasValue) pagelength = 9;
			ObjectParameter count = new ObjectParameter("totalPage", typeof(Int32));
			var danhsach = db.sp_DSSP(count, tinhtrang, keyword, maloai, orderOpt, page, pagelength).ToList();

			ViewBag.PageCount = Convert.ToInt32(count.Value);
			var SelectOrderOptions = new SelectList(new[] {
				new Tuple<string, int>("Đánh giá tốt nhất", 4),
				new Tuple<string, int>("Giá thấp nhất", 1),
				new Tuple<string, int>("Giá cao nhất", 2)
			}, "Item2", "Item1");
			ViewBag.orderOpt = SelectOrderOptions;
			ViewBag.maLoai = new SelectList(db.sp_ds_loaisp(), "MaLoai", "TenLoai");
            ViewBag.loai = db.sp_ds_loaisp().ToList();
			return View(danhsach);
        }

		// GET: SanPhams/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var makh = (int?)Session["CustomerID"];
            var sanPham = db.sp_ChiTietSP(id, makh).SingleOrDefault();
            if (sanPham == null)
            {
                return HttpNotFound();
            }

			var danhgia = db.sp_XemDanhGiaSP(id).ToList();
			CTSanPhamVM cTSanPhamVM = new CTSanPhamVM();
			cTSanPhamVM.CTSP = sanPham;
			cTSanPhamVM.DanhGia = danhgia;

            return View(cTSanPhamVM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
