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

namespace WebApplication3.Controllers
{
    public class AccountController : Controller
    {
        const string connectionString = "Data Source=DESKTOP-2FD4D2N;Initial Catalog=DB58;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        SqlConnection connection = new SqlConnection(connectionString);
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string ReturnUrl="")
        {
            string msg = "",a=" ll";
            int i=0;
            if (ModelState.IsValid)
            {
                connection.Open();
                string q = "SELECT * FROM tbl_Login WHERE Email='" + model.Email + "' AND Password='" + model.Password + "'";
                SqlCommand command = new SqlCommand(q, connection);
                SqlDataReader data = command.ExecuteReader();
                if (data.Read())
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
                        return RedirectToAction("AccountApproval", "Admin");
                    }
                }
                else
                {
                    i = 1;
                    msg = "Invalid email and password";
                }
                ViewBag.Status = i;
                connection.Close();
            }
            else
            {
                i = 1;
                msg = "Invalid Information";
            }
            ViewBag.Message = msg + " "+ a;
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

        public ActionResult ResetPassword()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View();
        }
    }
}