using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementCafe.Models;

namespace ManagementCafe.Controllers
{
    public class ChiTietHoaDonsController : Controller
    {
        private QuanLyQuanCafeEntities db = new QuanLyQuanCafeEntities();

        // GET: ChiTietHoaDons
        public ActionResult Index(int page = 1)
        {
            int pageSize = 10; // Số lượng chi tiết hóa đơn trên mỗi trang
            var chiTietHoaDons = db.ChiTietHoaDons.Include(c => c.HoaDon).Include(c => c.SanPham);

            var pagedChiTietHoaDons = chiTietHoaDons.OrderBy(c => c.ChiTietHoaDonID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalRecords = chiTietHoaDons.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.CurrentPage = page;

            return View(pagedChiTietHoaDons);
        }


        // GET: ChiTietHoaDons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietHoaDon chiTietHoaDon = db.ChiTietHoaDons.Find(id);
            if (chiTietHoaDon == null)
            {
                return HttpNotFound();
            }
            return View(chiTietHoaDon);
        }

        // GET: ChiTietHoaDons/Create
        public ActionResult Create()
        {
            // Set default value for SanPhamID = 1
            ViewBag.SanPhamID = new SelectList(db.SanPhams, "SanPhamID", "TenSanPham", 1);
            ViewBag.HoaDonID = new SelectList(db.HoaDons, "HoaDonID", "HoaDonID");
            return View();
        }


        // POST: ChiTietHoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChiTietHoaDonID,HoaDonID,SanPhamID,SoLuong,DonGia,ThanhTien")] ChiTietHoaDon chiTietHoaDon, string DonGiaInput, string ThanhTienInput)
        {
            if (ModelState.IsValid)
            {
                // Chuyển đổi giá trị DonGia và ThanhTien thành số nguyên
                chiTietHoaDon.DonGia = decimal.Parse(Request.Form["DonGiaInput"].Replace(".", ""));
                chiTietHoaDon.ThanhTien = decimal.Parse(Request.Form["ThanhTienInput"].Replace(".", ""));

                db.ChiTietHoaDons.Add(chiTietHoaDon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HoaDonID = new SelectList(db.HoaDons, "HoaDonID", "HoaDonID", chiTietHoaDon.HoaDonID);
            ViewBag.SanPhamID = new SelectList(db.SanPhams, "SanPhamID", "TenSanPham", chiTietHoaDon.SanPhamID);
            return View(chiTietHoaDon);
        }

        // GET: ChiTietHoaDons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietHoaDon chiTietHoaDon = db.ChiTietHoaDons.Find(id);
            if (chiTietHoaDon == null)
            {
                return HttpNotFound();
            }
            ViewBag.HoaDonID = new SelectList(db.HoaDons, "HoaDonID", "HoaDonID", chiTietHoaDon.HoaDonID);
            ViewBag.SanPhamID = new SelectList(db.SanPhams, "SanPhamID", "TenSanPham", chiTietHoaDon.SanPhamID);
            return View(chiTietHoaDon);
        }

        // POST: ChiTietHoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChiTietHoaDonID,HoaDonID,SanPhamID,SoLuong,DonGia,ThanhTien")] ChiTietHoaDon chiTietHoaDon, string DonGiaInput, string ThanhTienInput)
        {
            if (ModelState.IsValid)
            {
                // Chuyển đổi giá trị DonGia và ThanhTien từ chuỗi có dấu phẩy thành số nguyên
                chiTietHoaDon.DonGia = decimal.Parse(DonGiaInput.Replace(".", ""));
                chiTietHoaDon.ThanhTien = decimal.Parse(ThanhTienInput.Replace(".", ""));

                db.Entry(chiTietHoaDon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HoaDonID = new SelectList(db.HoaDons, "HoaDonID", "HoaDonID", chiTietHoaDon.HoaDonID);
            ViewBag.SanPhamID = new SelectList(db.SanPhams, "SanPhamID", "TenSanPham", chiTietHoaDon.SanPhamID);
            return View(chiTietHoaDon);
        }

        // GET: ChiTietHoaDons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietHoaDon chiTietHoaDon = db.ChiTietHoaDons.Find(id);
            if (chiTietHoaDon == null)
            {
                return HttpNotFound();
            }
            return View(chiTietHoaDon);
        }

        // POST: ChiTietHoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietHoaDon chiTietHoaDon = db.ChiTietHoaDons.Find(id);
            db.ChiTietHoaDons.Remove(chiTietHoaDon);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Phương thức để lấy giá trị DonGia từ bảng SanPham
        public JsonResult GetDonGia(int sanPhamID)
        {
            var donGia = db.SanPhams.Where(s => s.SanPhamID == sanPhamID).Select(s => s.Gia).FirstOrDefault();
            System.Diagnostics.Debug.WriteLine("DonGia: " + donGia);
            return Json(donGia, JsonRequestBehavior.AllowGet);
        }

    }
}
