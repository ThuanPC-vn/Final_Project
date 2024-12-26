using System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementCafe.Models;

namespace ManagementCafe.Controllers
{
    public class HoaDonsController : Controller
    {
        private QuanLyQuanCafeEntities db = new QuanLyQuanCafeEntities();

        // GET: HoaDons
        public ActionResult Index(string searchTerm, int page = 1)
        {
            int pageSize = 10; // Số lượng hóa đơn trên mỗi trang
            var hoaDons = db.HoaDons.Include(h => h.KhachHang).Include(h => h.NhanVien);

            if (!String.IsNullOrEmpty(searchTerm))
            {
                hoaDons = hoaDons.Where(h => h.HoaDonID.Contains(searchTerm));
            }

            var pagedHoaDons = hoaDons.OrderBy(h => h.HoaDonID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalRecords = hoaDons.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.CurrentPage = page;

            return View(pagedHoaDons);
        }


        // GET: HoaDons/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Sử dụng Include để tải dữ liệu liên quan từ bảng ChiTietHoaDon
            HoaDon hoaDon = db.HoaDons
                .Include(h => h.ChiTietHoaDons.Select(ct => ct.SanPham))
                .Include(h => h.KhachHang)
                .Include(h => h.NhanVien)
                .FirstOrDefault(h => h.HoaDonID == id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }


        // GET: HoaDons/Create
        public ActionResult Create()
        {
            ViewBag.KhachHangID = new SelectList(db.KhachHangs, "KhachHangID", "HoTen");
            ViewBag.NhanVienID = new SelectList(db.NhanViens, "NhanVienID", "HoTen");

            // Generate new HoaDonID
            ViewBag.NewHoaDonID = GenerateNewHoaDonID();

            return View();
        }

        // POST: HoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HoaDonID,NgayLap,ThoiGianLap,NhanVienID,KhachHangID,TongTien")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                DateTime ngayLap;
                if (DateTime.TryParseExact(Request.Form["NgayLap"], "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayLap))
                {
                    hoaDon.NgayLap = ngayLap;

                    // Chuyển đổi giá trị ThoiGianLap thành TimeSpan
                    TimeSpan thoiGianLap;
                    if (TimeSpan.TryParse(Request.Form["ThoiGianLap"], out thoiGianLap))
                    {
                        hoaDon.ThoiGianLap = thoiGianLap;
                    }
                    else
                    {
                        ModelState.AddModelError("ThoiGianLap", "Thời gian lập không hợp lệ.");
                    }

                    // Chuyển đổi giá trị TongTien thành số nguyên
                    hoaDon.TongTien = decimal.Parse(Request.Form["TongTienInput"].Replace(".", ""));

                    db.HoaDons.Add(hoaDon);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                            }
                        }
                        ViewBag.KhachHangID = new SelectList(db.KhachHangs, "KhachHangID", "HoTen", hoaDon.KhachHangID);
                        ViewBag.NhanVienID = new SelectList(db.NhanViens, "NhanVienID", "HoTen", hoaDon.NhanVienID);
                        return View(hoaDon);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("NgayLap", "Ngày lập không hợp lệ.");
                }
            }

            ViewBag.KhachHangID = new SelectList(db.KhachHangs, "KhachHangID", "HoTen", hoaDon.KhachHangID);
            ViewBag.NhanVienID = new SelectList(db.NhanViens, "NhanVienID", "HoTen", hoaDon.NhanVienID);
            return View(hoaDon);
        }

        // GET: HoaDons/Edit/5
        public ActionResult Edit(string id)
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
            ViewBag.KhachHangID = new SelectList(db.KhachHangs, "KhachHangID", "HoTen", hoaDon.KhachHangID);
            ViewBag.NhanVienID = new SelectList(db.NhanViens, "NhanVienID", "HoTen", hoaDon.NhanVienID);
            return View(hoaDon);
        }

        // POST: HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HoaDonID,NgayLap,NhanVienID,KhachHangID,TongTien")] HoaDon hoaDon, string TongTienInput)
        {
            if (ModelState.IsValid)
            {
                // Chuyển đổi giá trị TongTien từ chuỗi có dấu phẩy thành số nguyên
                hoaDon.TongTien = decimal.Parse(TongTienInput.Replace(".", ""));

                DateTime ngayLap;
                if (DateTime.TryParseExact(Request.Form["NgayLap"], "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayLap))
                {
                    hoaDon.NgayLap = ngayLap;

                    // Chuyển đổi giá trị ThoiGianLap thành TimeSpan
                    TimeSpan thoiGianLap;
                    if (TimeSpan.TryParse(Request.Form["ThoiGianLap"], out thoiGianLap))
                    {
                        hoaDon.ThoiGianLap = thoiGianLap;
                    }
                    else
                    {
                        ModelState.AddModelError("ThoiGianLap", "Thời gian lập không hợp lệ.");
                    }

                    // Chuyển đổi giá trị TongTien thành số nguyên
                    hoaDon.TongTien = decimal.Parse(Request.Form["TongTienInput"].Replace(".", ""));

                    db.Entry(hoaDon).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("NgayLap", "Ngày lập không hợp lệ.");
                }
            }

            ViewBag.KhachHangID = new SelectList(db.KhachHangs, "KhachHangID", "HoTen", hoaDon.KhachHangID);
            ViewBag.NhanVienID = new SelectList(db.NhanViens, "NhanVienID", "HoTen", hoaDon.NhanVienID);
            return View(hoaDon);
        }

        // GET: HoaDons/Delete/5
        public ActionResult Delete(string id)
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

        // POST: HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HoaDon hoaDon = db.HoaDons.Find(id);
            db.HoaDons.Remove(hoaDon);
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

        private string GenerateNewHoaDonID()
        {
            var maxID = db.HoaDons
                .Select(h => h.HoaDonID)
                .ToList()// Load IDs into memory
                .Where(id => id.StartsWith("HD"))
                .Select(id => int.Parse(id.Substring(2)))
                .DefaultIfEmpty(0)
                .Max(); return "HD" + (maxID + 1)
                .ToString("D4");
        }

        // GET: HoaDons/TimKiem_HD
        public ActionResult TimKiem_HD()
        {
            var hoaDons = db.HoaDons.Include(h => h.KhachHang).Include(h => h.NhanVien).ToList();
            return View(hoaDons);
        }


        // GET: HoaDons/TimKiem_HD_KQ
        public ActionResult TimKiem_HD_KQ(string searchID, DateTime? searchDateFrom, DateTime? searchDateTo, TimeSpan? searchTimeFrom, TimeSpan? searchTimeTo, decimal? searchAmountFrom, decimal? searchAmountTo)
        {
            var hoaDons = db.Database.SqlQuery<HoaDon>("EXEC HoaDon_TimKiem @HoaDonID, @NgayLapFrom, @NgayLapTo, @ThoiGianLapFrom, @ThoiGianLapTo, @TongTienFrom, @TongTienTo",
                new SqlParameter("@HoaDonID", (object)searchID ?? DBNull.Value),
                new SqlParameter("@NgayLapFrom", (object)searchDateFrom ?? DBNull.Value),
                new SqlParameter("@NgayLapTo", (object)searchDateTo ?? DBNull.Value),
                new SqlParameter("@ThoiGianLapFrom", (object)searchTimeFrom ?? DBNull.Value),
                new SqlParameter("@ThoiGianLapTo", (object)searchTimeTo ?? DBNull.Value),
                new SqlParameter("@TongTienFrom", (object)searchAmountFrom ?? DBNull.Value),
                new SqlParameter("@TongTienTo", (object)searchAmountTo ?? DBNull.Value)
            ).ToList();

            return View("TimKiem_HD", hoaDons);
        }



    }
}
