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
    [Authorize]
    public class EnmployeeLoanApplicationsController : Controller
    {
        private DB58Entities db = new DB58Entities();
        public ActionResult Index()
        {
            string email = Session["EmployeeEmail"].ToString();
            var loanApplications = db.LoanApplications.Include(l => l.Employee).Include(l => l.Loan);
            var e = db.currentEmployeeViews.Where(x => x.Email == email).FirstOrDefault();
            ViewBag.Id = e.CNIC;
            return View(loanApplications.ToList());
        }

        public ActionResult Verified()
        {
            string email = Session["EmployeeEmail"].ToString();
            return View(db.Verifiedloans.ToList());
        }

        public ActionResult Approved()
        {
            string email = Session["EmployeeEmail"].ToString();
            var loanApplications = db.LoanApplications.Include(l => l.Employee).Include(l => l.Loan);
            var e = db.currentEmployeeViews.Where(x => x.Email == email).FirstOrDefault();
            ViewBag.Id = e.CNIC;
            return View(loanApplications.ToList());
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanApplication loanApplication = db.LoanApplications.Find(id);
            if (loanApplication == null)
            {
                return HttpNotFound();
            }
            return View(loanApplication);
        }

        public ActionResult Create(int? id)
        {
            string email = Session["EmployeeEmail"].ToString();
            currentEmployeeView employee = db.currentEmployeeViews.Where(x => x.Email == email).FirstOrDefault();
            LoanApplication loan = db.LoanApplications.Where(x => x.LoanID == id && x.EmployeeID == employee.CNIC).FirstOrDefault();
            if (loan != null)
            {
                return Redirect("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "CNIC", "FirstName");
            ViewBag.LoanID = new SelectList(db.Loans, "LoanID", "LoanType");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LoanApplication loanApplication,string ReturnUrl="")
        {
            if (ModelState.IsValid)
            {
                if (Session["EmployeeEmail"] == null)
                {
                    return Redirect(ReturnUrl);
                }
                string email = Session["EmployeeEmail"].ToString();
                currentEmployeeView employee = db.currentEmployeeViews.Where(x => x.Email == email).FirstOrDefault();
                if (employee != null)
                {
                    Loan loan = db.Loans.Find(loanApplication.LoanID);
                    if (loan != null && loan.MaxAmount <= loanApplication.PrincipalAmount)
                    {
                        ModelState.AddModelError("PrincipalAmount", "Your amount exceeds companies maximum loan amunt limit");
                        return View(loanApplication);
                    }
                    else
                    {
                        loanApplication.EmployeeID = employee.CNIC;
                        loanApplication.LoanID = 1;
                        loanApplication.AppliedDate = DateTime.Now;
                        db.LoanApplications.Add(loanApplication);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    
                }
            }
            return View(loanApplication);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanApplication loanApplication = db.LoanApplications.Find(id);
            if (loanApplication == null)
            {
                return HttpNotFound();
            }
            if (loanApplication.Verified == true)
            {
                return Redirect("Approved");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "CNIC", "FirstName", loanApplication.EmployeeID);
            ViewBag.LoanID = new SelectList(db.Loans, "LoanID", "LoanType", loanApplication.LoanID);
            return View(loanApplication);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicationID,LoanID,EmployeeID,PrincipalAmount,Verified,Reason,AppliedDate")] LoanApplication loanApplication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loanApplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "CNIC", "FirstName", loanApplication.EmployeeID);
            ViewBag.LoanID = new SelectList(db.Loans, "LoanID", "LoanType", loanApplication.LoanID);
            return View(loanApplication);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanApplication loanApplication = db.LoanApplications.Find(id);
            if (loanApplication == null)
            {
                return HttpNotFound();
            }
            else if (loanApplication.Verified == true)
            {
                return Redirect("Approved");
            }
            else
            {
                db.LoanApplications.Remove(loanApplication);
                db.SaveChanges();
                return RedirectToAction("Approved");
            }
            
        }

        public ActionResult AcceptLoan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanApplication loanApplication = db.LoanApplications.Find(id);
            if (loanApplication == null)
            {
                return HttpNotFound();
            }
            else if (loanApplication.Verified == true)
            {
                loanApplication.Accepted = true;
                db.SaveChanges();
                return RedirectToAction("Approved");
            }
            return View();
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
