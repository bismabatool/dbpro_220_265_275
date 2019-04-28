﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class PackageView
    {
        public string Name { get; set; }
        public Nullable<int> Price { get; set; }
        public string Details { get; set; }
        public string Category { get; set; }
        public string CompanyId { get; set; }
    }
}