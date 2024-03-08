using Bakery.Models;
using Bakery.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using System.Web.UI;

namespace Bakery.Controllers
{
	public class HomeController : Controller
	{
        private BakeryStoreDBEntities db = new BakeryStoreDBEntities();
        public ActionResult Index()
		{
            ObjectParameter count = new ObjectParameter("totalPage", typeof(Int32));
			var sps = db.sp_DSSP(count, null, null, null, 4, null, 8).ToList();
			var kms = db.sp_ds_khuyenMai(false).ToList();
			var model = new HomeVM();
			model.sanphams = sps;
			model.khuyenmais = kms;

			return View(model);
		}

		[Authorize]
		public ActionResult About()
		{
			ViewBag.Message = "Đã đăng nhập";

			return View();
		}

		[Authorize(Roles = "Admin")]
		public ActionResult Contact()
		{
			ViewBag.Message = "Admin";
			return View();
		}
	}
}