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

namespace Bakery.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SanPhamController : Controller
    {
        private BakeryStoreDBEntities db = new BakeryStoreDBEntities();

        // GET: Admin/SanPhams
        public ActionResult Index(string keyword, int? cate, int? page = 1, bool? active = true)
        {
            ObjectParameter count = new ObjectParameter("totalPage", typeof(Int32));
            var list = db.sp_DSSP(count, active, keyword, cate, null, page, 20).ToList();

            var cates = db.sp_ds_loaisp().ToList();

            ViewBag.cates = cates;
            ViewBag.PageCount = Convert.ToInt32(count.Value);
            ViewBag.ToastHeader = TempData["ToastHeader"];
            ViewBag.ToastBody = TempData["ToastBody"];
            ViewBag.ToastTheme = TempData["ToastTheme"];
            return View(list);
        }

        public ActionResult GetHints(string keyword)
        {
            var sps = from sp in db.SanPhams
                      where sp.tinhTrang == 1
                      from km in db.KhuyenMais.Where(x => x.MaKM == sp.MaKM && x.NgayKT.HasValue && x.NgayKT.Value > DateTime.Now).DefaultIfEmpty()
                      select new { sp.MaSP, sp.TenSP, sp.SoluongSP, sp.img, sp.GiaSP, km.TiLeKM };
            var list = sps.Where(x => x.MaSP.ToString().StartsWith(keyword) || x.TenSP.ToLower().Contains(keyword.ToLower()))
                .Take(20).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaKM = new SelectList(db.KhuyenMais, "MaKM", "TenKM");
            ViewBag.maLoai = new SelectList(db.LoaiSanPhams, "MaLoai", "TenLoai");
            return View();
        }

        // POST: Admin/SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham sanPham)
        {
            try
            {
                db.sp_ThemSP(sanPham.TenSP, sanPham.GiaSP, sanPham.MotaSP, sanPham.img, sanPham.maLoai, sanPham.SoluongSP);

                TempData["ToastHeader"] = "Đã thêm sản phẩm";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.InnerException.Message.Split('\r')[0];
                if (sanPham.TenSP == null) ViewBag.ErrorMsg = "Tên không được bỏ trống";
                ViewBag.MaKM = new SelectList(db.KhuyenMais, "MaKM", "TenKM", sanPham.MaKM);
                ViewBag.maLoai = new SelectList(db.LoaiSanPhams, "MaLoai", "TenLoai", sanPham.maLoai);
                return View(sanPham);
            }
        }

        // GET: Admin/SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sanPham = db.sp_ChiTietSP(id, null).FirstOrDefault();
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            var tinhtrangs = new SelectList(new[] {
                new Tuple<string, int>("Đang bán", 1),
                new Tuple<string, int>("Tạm ẩn", 0)
            }, "Item2", "Item1", sanPham.tinhTrang);

            ViewBag.tinhTrang = tinhtrangs;
            ViewBag.MaKM = new SelectList(db.KhuyenMais, "MaKM", "TenKM", sanPham.MaKM);
            ViewBag.maLoai = new SelectList(db.LoaiSanPhams, "MaLoai", "TenLoai", sanPham.MaLoai);
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(sp_ChiTietSP_Result sanPham)
        {
            try
            {
                db.sp_SuaSP(sanPham.MaSP, sanPham.TenSP, sanPham.GiaSP, sanPham.MotaSP, sanPham.img, sanPham.Sao, sanPham.SoLuotDanhGia, sanPham.MaLoai, sanPham.SoluongSP, sanPham.MaKM, sanPham.tinhTrang);
                TempData["ToastHeader"] = "Đã cập nhật sản phẩm";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.InnerException.Message.Split('\r')[0];
                var tinhtrangs = new SelectList(new[] {
                        new Tuple<string, int>("Đang bán", 1),
                        new Tuple<string, int>("Tạm ẩn", 0)
                    }, "Item2", "Item1", sanPham.tinhTrang);

                ViewBag.tinhTrang = tinhtrangs;
                ViewBag.MaKM = new SelectList(db.KhuyenMais, "MaKM", "TenKM", sanPham.MaKM);
                ViewBag.maLoai = new SelectList(db.LoaiSanPhams, "MaLoai", "TenLoai", sanPham.MaLoai);
                return View(sanPham);
            }
        }

        // GET: Admin/SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                SanPham sanPham = db.SanPhams.Find(id);
                db.SanPhams.Remove(sanPham);
                db.SaveChanges();
                TempData["ToastHeader"] = "Đã xóa sản phẩm";
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
