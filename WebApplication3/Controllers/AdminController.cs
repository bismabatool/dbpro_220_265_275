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
    [Authorize(Users = "admin@gmail.com")]
    public class AdminController : Controller
    {
        const string connectionString = "Data Source=DESKTOP-2FD4D2N;Initial Catalog=DB58;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        SqlConnection connection = new SqlConnection(connectionString);
        DB58Entities db = new DB58Entities();
        // GET: Admin
        public ActionResult ApprovedEmployee()
        {
            List<currentEmployeeView> el = db.currentEmployeeViews.ToList();
            return View(el);
        }

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
        public ActionResult PendingEmployeeRequest()
        {
            List<employeeApproveView> el = db.employeeApproveViews.ToList();
            return View(el);
        }
        
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
        
        [HttpGet]
        public ActionResult AccountApproval()
        {
            List<employeeApproveView> el = db.employeeApproveViews.ToList();
            return View(el);
        }
        
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
                designation.Cnic = model.UserID;
                designation.Email = model.Email;
                return View(designation);
            }
        }
        [HttpPost]
        public ActionResult ApproveAccount(DesignationViewModel model)
        {
            Designation des = new Designation();
            tbl_Login login = db.tbl_Login.Find(model.Email);
            Employee emp = db.Employees.Where(x => x.CNIC == login.UserID).FirstOrDefault();
            if (login != null && emp!=null)
            {
                login.isActive = true;
                login.isApproved = true;
                des.EmployeeID = emp.CNIC;
                des.Salary = Convert.ToInt32(model.Salary);
                des.PostTitle = model.Designation;
                des.JoiningDate = DateTime.Now;
                des.ToDate = DateTime.Now;
                db.Designations.Add(des);
                db.SaveChanges();
                SendVerificationLinkEmail(model.Email);
                return RedirectToAction("ApprovedEmployee");
            }
            return View(model);
        }

        [NonAction]
        public void SendVerificationLinkEmail(string EmailId)
        {
            var fromEmail = new MailAddress("evento.managment@gmail.com", "Employee Loan Managerials");
            var toEmail = new MailAddress(EmailId);
            var fromEmailPassword = "evento12@#";
            string subject = "Account Approval";
            string Message = "Congragulations! your employee account has been approved. Now, you can login into your account";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = Message,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}