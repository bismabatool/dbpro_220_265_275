using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class UserView
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Nullable<int> Contact { get; set; }
        public string PackageId { get; set; }
    }
}