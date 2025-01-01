using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _23TH2508_Final_Project.Models;

namespace _23TH2508_Final_Project.Controllers
{
    public class QuanTris_23TH2508Controller : Controller
    {
        private QuanLyQuanCafeEntities db = new QuanLyQuanCafeEntities();

        // GET: QuanTris_23TH2508
        public ActionResult Index()
        {
            return View(db.QuanTris.ToList());
        }

        // GET: QuanTris_23TH2508/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }

        // GET: QuanTris_23TH2508/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuanTris_23TH2508/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,Admin,HoTen,Password")] QuanTri quanTri)
        {
            if (ModelState.IsValid)
            {
                db.QuanTris.Add(quanTri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quanTri);
        }

        // GET: QuanTris_23TH2508/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }

        // POST: QuanTris_23TH2508/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,Admin,HoTen,Password")] QuanTri quanTri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quanTri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quanTri);
        }

        // GET: QuanTris_23TH2508/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }

        // POST: QuanTris_23TH2508/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            QuanTri quanTri = db.QuanTris.Find(id);
            db.QuanTris.Remove(quanTri);
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
