using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication3;

namespace WebApplication3.Controllers
{
    [Authorize(Users ="admin@gmail.com")]
    public class adminLoanApprovalController : Controller
    {
        DB58Entities db = new DB58Entities();
        
        public ActionResult LoanApplications()
        {
            var loanApplications = db.LoanApplications.Include(l => l.Loan);
            return View(loanApplications.ToList());
        }

        public ActionResult ApprovedLoans()
        {
            var loanApplications = db.LoanApplications.Include(l => l.Loan);
            return View(loanApplications.ToList());
        }

        public ActionResult VerifiedLoans()
        {
            var loanApplications = db.LoanApplications.Include(l => l.Employee).Include(l => l.Loan);
            return View(loanApplications.ToList());
        }

        public ActionResult VerifyLoan(int? id)
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
            LoanStatu loanStatus = new LoanStatu();
            loanStatus.ApplicationID = loanApplication.ApplicationID;
            loanStatus.PrincipleAmount = loanApplication.PrincipalAmount;
            return View(loanStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyLoan([Bind(Include = "ApplicationID,PrincipleAmount,Tenure,EMI")]LoanStatu loanStatus)
        {
            db.Entry(loanStatus).State = EntityState.Modified;
            int appId = loanStatus.ApplicationID;
            LoanApplication application = db.LoanApplications.Find(loanStatus.ApplicationID);
            if (application != null)
            {
                Loan loan = db.Loans.Find(application.LoanID);
                if (loan.MaxAmount > loanStatus.PrincipleAmount)
                {
                    loanStatus.EMI = (loanStatus.PrincipleAmount * (1 + ((loan.InterestRate / 100) * loanStatus.Tenure))) / (12 * loanStatus.Tenure);
                    db.LoanStatus.Add(loanStatus);
                    application.Verified = true;
                    db.SaveChanges();
                    return RedirectToAction("VerifiedLoans");
                }

            }

            return View();
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
            else
            {
                db.LoanApplications.Remove(loanApplication);
                db.SaveChanges();
                return Redirect("LoanApplication");
            }
        }

        public ActionResult Reject(int? id)
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
            loanApplication.Verified = false;
            db.SaveChanges();
            return RedirectToAction("VerifiedLoans");
        }
    }
}