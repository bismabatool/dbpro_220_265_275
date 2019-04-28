using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class FeedbackView
    {
        public int Counter { get; set; }
        public string UserId { get; set; }
        public string Details { get; set; }
        public string PackageId { get; set; }
    }
}