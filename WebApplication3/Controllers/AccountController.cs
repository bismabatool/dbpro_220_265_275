using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Data.Entity.Infrastructure;
using System.Net.Mail;
using System.Net;

namespace WebApplication3.Controllers
{
    public class AccountController : Controller
    {
        const string connectionString = "Data Source=DESKTOP-2FD4D2N;Initial Catalog=DB58;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        SqlConnection connection = new SqlConnection(connectionString);
        DB58Entities db = new DB58Entities();
        // GET: Account
        
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string ReturnUrl="")
        {
            string msg = "";
            int i=0;
            if (ModelState.IsValid)
            {
                var account = db.tbl_Login.Where(x => x.Email.ToString() == model.Email &&
                x.Password.ToString() == model.Password && x.isActive == true && x.isApproved == true);
                if (account!=null)
                {
                    
                    int timeout = model.RememberMe ? 525600 : 20;
                    var ticket = new FormsAuthenticationTicket(model.Email, model.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        tbl_Login acc = db.tbl_Login.Find(model.Email);
                        if (acc.UserID==null)
                        {
                            Session["AdminEmail"] = model.Email.ToString();
                            return RedirectToAction("AccountApproval", "Admin");
                        }
                        else
                        {
                            Session["EmployeeEmail"] = model.Email.ToString();
                            return RedirectToAction("Index", "loanViews");
                        }
                        
                        
                    }
                }
                else
                {
                    i = 1;
                    msg = "Invalid email and password";
                }
                ViewBag.Status = i;
            }
            else
            {
                i = 1;
                msg = "Invalid Information";
            }
            ViewBag.Message = msg;
            return View(model);
        }

        [HttpGet]
        public ActionResult Register(string returnUrl)
        {            
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel acc)
        {
            string msg = "", errMsg="";
            int i = 0;
            if (ModelState.IsValid)
            {
                connection.Open();
                string query = "Select * From Employee,tbl_Login  WHERE CNIC= '" + acc.CNIC + "' OR Email= '"+acc.Email+"'" ;
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                try
                {
                    string insertQuery = "INSERT INTO Employee(FirstName,LastName,AGE,CNIC,Gender,DOB,Contact,Postal_Address) " +
                        "Values('" + acc.FirstName + "','" + acc.LastName + "','" + Convert.ToInt32(acc.AGE) + "','" + acc.CNIC + "','" + acc.Gender + "'," +
                        "'" + acc.DOB + "','" + acc.Contact + "','" + acc.PostalAddress + "') ";
                    string insertQ = "INSERT INTO tbl_Login(Email,Password,UserID) " +
                        "VALUES('" + acc.Email + "','" + acc.Password + "','" + acc.CNIC + "')";
                    SqlCommand cmd1 = new SqlCommand(insertQuery, connection);
                    SqlCommand cmd2 = new SqlCommand(insertQ, connection);
                        cmd1.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();
                        msg = "You've been registered. Wait for approval";
                        i = 1;
                }
                catch (DbUpdateException ex) when ((ex.InnerException as SqlException)?.Number == 2627)
                {
                    i = 2;
                    errMsg = "Employee already exists";
                }
                catch (Exception e)
                {
                    i = 2;
                    errMsg = e.ToString();
                }
                connection.Close();
            }
            else
            {
                i = 2;
                errMsg = "Invalid Model";
            }
            ViewBag.Status = i;
            ViewBag.ErrMessage = errMsg;
            ViewBag.Message = msg;
            return View(acc);
        }
        public ActionResult reg()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(LoginViewModel model)
        {
            string message="";
            var account = db.tbl_Login.Where(a => a.Email.ToString().ToLower() == model.Email.ToString().ToLower()).FirstOrDefault();
            if (account != null)
            {
                if(account.isActive==true && account.isApproved == true)
                {
                    //Send email reset password mail
                    account.resetCode = Guid.NewGuid();
                    SendVerificationLinkEmail(model.Email, account.resetCode.ToString(), "ForgetPassword");
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    ModelState.Clear();
                    message = "Reset passord lsink has been sent to your account";
                    ViewBag.Status = true;
                }
            }
            else
            {
                ViewBag.Status = true;

                message = "Invalid Account";
            }
            ViewBag.Message = message;
            return View(model);
        }
        [HttpGet]
        public ActionResult ForgetPassword(string id)
        {
            var user = db.tbl_Login.Where(a => a.resetCode.ToString() == id).FirstOrDefault();
            if (user != null)
            {
                ResetPasswordViewModel model = new ResetPasswordViewModel();
                model.ResetCode = id;
                ViewBag.Message = model.ResetCode;
                ViewBag.Status = 2;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(ResetPasswordViewModel model)
        {
            int flag = 0;
            string msg = "";
            if (model.ResetCode == null)
            {
                msg = "No request found";
                flag = 2;
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var user = db.tbl_Login.Where(a => a.resetCode.ToString() == model.ResetCode.ToString()).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = model.NewPassword;
                        user.resetCode = null;
                        db.SaveChanges();
                        msg = "Your password has been changed sucessfully!";
                        flag = 1;
                    }
                }
                else
                {
                    msg = "Password doesn't match";
                    flag = 2;
                }
            }
            ViewBag.Message = msg;
            ViewBag.Status = flag;
            return View(model);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View();
        }

        [NonAction]
        public void SendVerificationLinkEmail(string EmailId, string activationCode, string emailfor)
        {

            var verifyUrl = "/Account/" + emailfor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("evento.managment@gmail.com", "Employee Loan Managerials");
            var toEmail = new MailAddress(EmailId);
            var fromEmailPassword = "evento12@#";
            string subject = "", Message = "";
                subject = "Reset Password";
                Message = "Hey, <br/><br/>We got request account reset password." + "Please click on the link below to reset your password" +
                    "<br/><br/><a href=" + link + ">" + link + "</a> <br/> <br/> Thankyou.";
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