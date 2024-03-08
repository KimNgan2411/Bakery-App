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

namespace Bakery.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HoaDonsController : Controller
    {
        private BakeryStoreDBEntities db = new BakeryStoreDBEntities();

        // GET: Admin/HoaDons
        public ActionResult Index(int page = 1, bool? active = null)
        {
            ObjectParameter count = new ObjectParameter("totalPage", typeof(Int32));
            var hoaDons = db.sp_dshoadon(count, page, 20, null, null, active).ToList();

            ViewBag.PageCount = Convert.ToInt32(count.Value);
            ViewBag.ToastHeader = TempData["ToastHeader"];
            ViewBag.ToastBody = TempData["ToastBody"];
            ViewBag.ToastTheme = TempData["ToastTheme"];
            return View(hoaDons);
        }

        // GET: Admin/HoaDons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        // GET: Admin/HoaDons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/HoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<CTHD> model)
        {
            using (var tran = db.Database.BeginTransaction()) {
                try
                {
                    var hd = db.HoaDons.Add(new HoaDon() { NgayHD = DateTime.Now, TinhTrangGiao = true });
                    db.SaveChanges();
                    int id = hd.MaHD;
                    //db.sp_createhoadon(null, null);
                    //var hd = db.HoaDons.OrderByDescending(x => x.MaHD).FirstOrDefault();
                    //int id = (hd == null) ? 1 : hd.MaHD;
                    foreach (var x in model) {
                        db.sp_add_CTHD(id, x.MaSP, x.SoLuong);
                    }
                    db.sp_tongHoaDon(id);

                    tran.Commit();
                    TempData["ToastHeader"] = "Đã tạo hóa đơn mới";
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.ToastHeader = "Có lỗi xảy ra";
                    ViewBag.ToastBody = "Vui lòng thử lại";
                    ViewBag.ToastTheme = "Danger";
                    return View(model);
                }
                finally {
                    tran.Dispose();
                }
            }
              
        }

        // GET: Admin/HoaDons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var hoaDon = db.sp_detailhoadon(id).FirstOrDefault();
            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            var tinhtrangs = new SelectList(new[] {
                new Tuple<string, bool>("Đã giao", true),
                new Tuple<string, bool>("Chưa giao", false)
            }, "Item2", "Item1", hoaDon.TinhTrangGiao);

            var cthds = db.sp_ds_cthd(id).ToList();

            ViewBag.cthds = cthds;
            ViewBag.TinhTrangGiao = tinhtrangs;
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", hoaDon.MaKH);
            return View(hoaDon);
        }

        // POST: Admin/HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(sp_detailhoadon_Result hoaDon)
        {
            try
            {
                db.sp_update_hoadon(hoaDon.MaHD, hoaDon.TinhTrangGiao, hoaDon.DiaChiGiao);
                TempData["ToastHeader"] = "Đã cập nhật hóa đơn";
                return RedirectToAction("Index");
            }
            catch {
                var tinhtrangs = new SelectList(new[] {
                    new Tuple<string, bool>("Đã giao", true),
                    new Tuple<string, bool>("Chưa giao", false)
                }, "Item2", "Item1", hoaDon.TinhTrangGiao);

                ViewBag.TinhTrangGiao = tinhtrangs;
                ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", hoaDon.MaKH);
                return View(hoaDon);
            }
        }

        // GET: Admin/HoaDons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        // POST: Admin/HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                HoaDon hoaDon = db.HoaDons.Find(id);
                db.HoaDons.Remove(hoaDon);
                db.SaveChanges();
                TempData["ToastHeader"] = "Đã xóa hóa đơn";
                return RedirectToAction("Index");
            }
            catch {
                TempData["ToastHeader"] = "Có lỗi xảy ra";
                TempData["ToastTheme"] = "Danger";
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
