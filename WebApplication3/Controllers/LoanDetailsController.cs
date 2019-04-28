using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3;

namespace WebApplication3.Controllers
{
    public class LoanDetailsController : Controller
    {
        private DB58Entities db = new DB58Entities();

        // GET: LoanDetails
        public ActionResult Index()
        {
            var loanDetails = db.LoanDetails.Include(l => l.Loan).Include(l => l.Lookup);
            return View(loanDetails.ToList());
        }

        // GET: LoanDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanDetail loanDetail = db.LoanDetails.Find(id);
            if (loanDetail == null)
            {
                return HttpNotFound();
            }
            return View(loanDetail);
        }

        // GET: LoanDetails/Create
        public ActionResult Create()
        {
            ViewBag.LoanID = new SelectList(db.Loans, "LoanID", "LoanType");
            ViewBag.Category = new SelectList(db.Lookups, "LookupID", "Title");
            return View();
        }

        // POST: LoanDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoanDetailID,LoanID,Detail,Category")] LoanDetail loanDetail)
        {
            if (ModelState.IsValid)
            {
                db.LoanDetails.Add(loanDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LoanID = new SelectList(db.Loans, "LoanID", "LoanType", loanDetail.LoanID);
            ViewBag.Category = new SelectList(db.Lookups.Where(a=>a.LookupID <= 3), "LookupID", "Title", loanDetail.Category);
            return View(loanDetail);
        }

        // GET: LoanDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanDetail loanDetail = db.LoanDetails.Find(id);
            if (loanDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoanID = new SelectList(db.Loans, "LoanID", "LoanType", loanDetail.LoanID);
            ViewBag.Category = new SelectList(db.Lookups.Where(a => a.LookupID <= 3), "LookupID", "Title", loanDetail.Category);
            return View(loanDetail);
        }

        // POST: LoanDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoanDetailID,LoanID,Detail,Category")] LoanDetail loanDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loanDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoanID = new SelectList(db.Loans, "LoanID", "LoanType", loanDetail.LoanID);
            ViewBag.Category = new SelectList(db.Lookups, "LookupID", "Title", loanDetail.Category);
            return View(loanDetail);
        }

        // GET: LoanDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanDetail loanDetail = db.LoanDetails.Find(id);
            if (loanDetail == null)
            {
                return HttpNotFound();
            }
            return View(loanDetail);
        }

        // POST: LoanDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoanDetail loanDetail = db.LoanDetails.Find(id);
            db.LoanDetails.Remove(loanDetail);
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
