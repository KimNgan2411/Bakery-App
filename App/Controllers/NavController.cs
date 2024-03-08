using Bakery.Models;
using Bakery.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace Bakery.Controllers
{
    public class NavController : Controller
    {
        private BakeryStoreDBEntities db = new BakeryStoreDBEntities();

        int? getCustomerId(HttpSessionStateBase Session) {
            int? id = Session["CustomerID"] == null ? null : (int?)Session["CustomerID"];
            return id;
        }

        // GET: Nav
        [ChildActionOnly]
        public ActionResult GetUsername()
        {
            int? id = getCustomerId(Session);
            if (id == null) return null;

            var khachHang = db.sp_profilekhachhang(id).SingleOrDefault();
            if (khachHang == null) return null;

            return PartialView("~/Views/Shared/Partials/Account.cshtml", khachHang.TenKH);
        }

        [ChildActionOnly]
        public ActionResult GetCartInfo()
        {
            int? id = getCustomerId(Session);
            if (id == null) return null;
            var gioHangs = db.sp_ds_gioHang(id).ToList();
            if (gioHangs == null) return null;


            return PartialView("~/Views/Shared/Partials/Cart.cshtml", gioHangs.Count());
        }
    }
}