using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Account
    {

    }

    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is Required")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password doesn't match")]
        public string cPassword { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contact is Required")]
        public Nullable<int> Contact { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is Required")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is Required")]

        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Postal is missing")]
        public string PostalAddress { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Age is missing")]
        public int AGE { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gender is missing")]
        public char Gender { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date of birth is missing")]
        public DateTime DOB { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "CNIC is missing")]
        public string CNIC { get; set; }
    }

    public class ResetPasswordViewModel
    {
        //[Required(AllowEmptyStrings = false, ErrorMessage = "New Password is required.")]
        //[DataType(DataType.Password)]
        //[MinLength(6, ErrorMessage = "At least 6 characters")]
        public string NewPassword { get; set; }
        //[Compare("NewPassword", ErrorMessage = "Password doesnot match")]
        //[DataType(DataType.Password)]
        //[MinLength(6, ErrorMessage = "At least 6 characters")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ResetCode { get; set; }
    }
}