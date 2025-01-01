using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _23TH2508_Final_Project.Models;

namespace _23TH2508_Final_Project.Controllers
{
    public class SanPhams_23TH2508Controller : Controller
    {
        private QuanLyQuanCafeEntities db = new QuanLyQuanCafeEntities();

        // GET: SanPhams_23TH2508
        public ActionResult Index(int page = 1)
        {
            int pageSize = 8; // Số lượng sản phẩm trên mỗi trang
            var sanPhams = db.SanPhams.OrderBy(s => s.SanPhamID);

            var pagedSanPhams = sanPhams.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalRecords = sanPhams.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.CurrentPage = page;

            return View(pagedSanPhams);
        }

        // GET: SanPhams_23TH2508/Details/5
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

        // GET: SanPhams_23TH2508/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SanPhams_23TH2508/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SanPhamID,TenSanPham,Gia,MoTa,HinhAnh")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sanPham);
        }

        // GET: SanPhams_23TH2508/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: SanPhams_23TH2508/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SanPhamID,TenSanPham,Gia,MoTa,HinhAnh")] SanPham sanPham, HttpPostedFileBase HinhAnh, string GiaInput)
        {
            if (ModelState.IsValid)
            {
                // Chuyển đổi giá trị Gia từ chuỗi có dấu phẩy thành số nguyên
                sanPham.Gia = decimal.Parse(GiaInput.Replace(".", ""));

                // Kiểm tra xem người dùng có chọn ảnh mới không
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    // Lưu ảnh vào thư mục /Images/products/
                    var fileName = Path.GetFileName(HinhAnh.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/products/"), fileName);
                    HinhAnh.SaveAs(path);

                    // Cập nhật đường dẫn ảnh trong model
                    sanPham.HinhAnh = fileName;
                }

                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sanPham);
        }

        // GET: SanPhams_23TH2508/Delete/5
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

        // POST: SanPhams_23TH2508/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
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
    }
}
