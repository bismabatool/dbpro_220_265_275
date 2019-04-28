using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class CompanyView
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Nullable<int> Contact { get; set; }
        public string Ceo { get; set; }
        public string Address { get; set; }
        public Nullable<bool> Isapproved { get; set; }
    }
}