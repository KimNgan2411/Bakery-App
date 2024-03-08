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
    public class LoaiSanPhamsController : Controller
    {
        private BakeryStoreDBEntities db = new BakeryStoreDBEntities();

        // GET: Admin/LoaiSanPhams
        public ActionResult Index()
        {
            var list = db.sp_ds_loaisp().ToList();

            ViewBag.ToastHeader = TempData["ToastHeader"];
            ViewBag.ToastBody = TempData["ToastBody"];
            ViewBag.ToastTheme = TempData["ToastTheme"];
            return View(list);
        }

        // GET: Admin/LoaiSanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham loaiSanPham = db.LoaiSanPhams.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        // GET: Admin/LoaiSanPhams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/LoaiSanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LoaiSanPham lsp)
        {
            try
            {
                db.sp_them_loaisp(lsp.TenLoai, lsp.cate_img);

                TempData["ToastHeader"] = "Đã thêm loại sản phẩm";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.InnerException.Message.Split('\r')[0];
                return View(lsp);
            }
        }

        // GET: Admin/LoaiSanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lsp = db.sp_detail_loaisp(id).FirstOrDefault();
            if (lsp == null)
            {
                return HttpNotFound();
            }
            return View(lsp);
        }

        // POST: Admin/LoaiSanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLoai,TenLoai,cate_img")] sp_detail_loaisp_Result lsp)
        {
            try
            {
                db.sp_update_loaisp(lsp.MaLoai, lsp.TenLoai, lsp.cate_img);
                TempData["ToastHeader"] = "Đã cập nhật loại sản phẩm";
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                ViewBag.ErrorMsg = ex.InnerException.Message.Split('\r')[0];
                return View(lsp);
            }
        }

        // GET: Admin/LoaiSanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham loaiSanPham = db.LoaiSanPhams.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        // POST: Admin/LoaiSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                db.sp_delete_loaisp(id);
                TempData["ToastHeader"] = "Đã xóa loại sản phẩm";
                return RedirectToAction("Index");
            }
            catch {
                return RedirectToAction("Edit", "LoaiSanPhams", new { id = id });
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
