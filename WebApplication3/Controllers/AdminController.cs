using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using WebApplication3.Models;
using System.Threading.Tasks;
using System.Web.Security;
using System.Net;
using System.Net.Mail;

namespace WebApplication3.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        const string connectionString = "Data Source=DESKTOP-2FD4D2N;Initial Catalog=DB58;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        SqlConnection connection = new SqlConnection(connectionString);
        DB58Entities db = new DB58Entities();
        // GET: Admin
        [Authorize]
        public ActionResult ApprovedEmployee()
        {
            if (Session["AdminEmail"]==null)
            {
                return View("Login", "Account");
            }
            List<currentEmployeeView> el = db.currentEmployeeViews.ToList();
            return View(el);
        }

        [Authorize]
        public ActionResult Delete(string email)
        {
            if (email == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Login loanDetail = db.tbl_Login.Find(email);
            if (loanDetail == null)
            {
                return HttpNotFound();
            }
            else
            {
                loanDetail.isActive = false;
                db.SaveChanges();
                return RedirectToAction("ApprovedEmployee");
            }
        }

        [Authorize]
        public ActionResult PendingEmployeeRequest()
        {
            if (Session["AdminEmail"] == null)
            {
                return View("Login", "Account");
            }
            List<employeeApproveView> el = db.employeeApproveViews.ToList();
            return View(el);
        }
        [Authorize]
        public ActionResult ApproveEmployee(string email)
        {
            if (email == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Login loanDetail = db.tbl_Login.Find(email);
            if (loanDetail == null)
            {
                return HttpNotFound();
            }
            else
            {
                loanDetail.isActive = true;
                db.SaveChanges();
                return RedirectToAction("PendingEmployeeRequest");
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult AccountApproval()
        {
            List<employeeApproveView> el = db.employeeApproveViews.ToList();
            return View(el);
        }
        [Authorize]
        [HttpGet]
        public ActionResult ApproveAccount(string email)
        {
            if (email == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Login model = db.tbl_Login.Find(email);
            if (model == null)
            {
                return HttpNotFound();
            }
            else
            {
                DesignationViewModel designation = new DesignationViewModel();
                designation.Email = model.Email;
                designation.Cnic = model.UserID;
                return View(designation);
            }
        }
        [HttpPost]
        public ActionResult ApproveAccount(DesignationViewModel designation)
        {
            tbl_Login login = db.tbl_Login.Find(designation.Email);
            if (login != null)
            {
                login.isActive = true;
                login.isApproved = true;
                Designation des = new Designation();
                des.EmployeeID = designation.Cnic;
                des.Salary = Convert.ToInt32(designation.Salary);
                des.PostTitle = designation.Designation;
                des.JoiningDate = DateTime.Now;
                des.ToDate = DateTime.Now;
                db.Designations.Add(des);
                db.SaveChanges();
                return RedirectToAction("AccountApproval");
            }
            return View(designation);
        }




    }
}