using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bakery.Models;

namespace Bakery.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KhuyenMaiController : Controller
    {
        private BakeryStoreDBEntities db = new BakeryStoreDBEntities();

        // GET: Admin/KhuyenMais
        public ActionResult Index()
        {
            ViewBag.ToastHeader = TempData["ToastHeader"];
            ViewBag.ToastBody = TempData["ToastBody"];
            ViewBag.ToastTheme = TempData["ToastTheme"];
            return View(db.sp_ds_khuyenMai(null).ToList());
        }

        // GET: Admin/KhuyenMais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMai khuyenMai = db.KhuyenMais.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        // GET: Admin/KhuyenMais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhuyenMais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KhuyenMai km)
        {
            try
            {
                db.sp_add_khuyenMai(km.TenKM, km.TiLeKM, km.NgayBD, km.NgayKT, km.pr_img, km.MoTa);
                TempData["ToastHeader"] = "Đã thêm khuyến mãi";
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                ViewBag.ErrorMsg = ex.InnerException.Message.Split('\r')[0];
                return View(km);
            }
        }

        // GET: Admin/KhuyenMais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var khuyenMai = db.sp_detail_khuyenMai(id).FirstOrDefault();
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }

            var sps = db.sp_dssp_khuyenMai(id).ToList();
            ViewBag.sps = sps;
            return View(khuyenMai);
        }

        // POST: Admin/KhuyenMais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(sp_detail_khuyenMai_Result km)
        {
            try
            {
                db.sp_update_khuyenMai(km.MaKM, km.TenKM, km.TiLeKM, km.NgayBD, km.NgayKT, km.MoTa, km.pr_img);
                TempData["ToastHeader"] = "Đã cập nhật khuyến mãi";
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                var sps = db.sp_dssp_khuyenMai(km.MaKM).ToList();
                ViewBag.sps = sps;
                ViewBag.ErrorMsg = ex.InnerException.Message.Split('\r')[0];
                return View(km);
            }
            
        }

        // GET: Admin/KhuyenMais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMai khuyenMai = db.KhuyenMais.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        // POST: Admin/KhuyenMais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                db.sp_delete_khuyenMai(id);
                TempData["ToastHeader"] = "Đã xóa khuyến mãi";
                return RedirectToAction("Index");
            }
            catch {
                return RedirectToAction("Index");
            }
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
