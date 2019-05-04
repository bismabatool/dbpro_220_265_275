using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Authorize]
    public class loanViewsController : Controller
    {
        private DB58Entities db = new DB58Entities();

        // GET: loanViews
        public ActionResult Index()
        {
            var ll = db.Loans.Include(l => l.LoanDetails);
            return View(ll.ToList());
        }
        
        
        // GET: loanViews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loanView loanView = db.loanViews.Find(id);
            if (loanView == null)
            {
                return HttpNotFound();
            }
            return View(loanView);
        }

        // POST: loanViews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            loanView loanView = db.loanViews.Find(id);
            db.loanViews.Remove(loanView);
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
