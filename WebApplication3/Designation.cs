//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication3
{
    using System;
    using System.Collections.Generic;
    
    public partial class Designation
    {
        public int DesignationID { get; set; }
        public string EmployeeID { get; set; }
        public string PostTitle { get; set; }
        public int Salary { get; set; }
        public System.DateTime JoiningDate { get; set; }
        public System.DateTime ToDate { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
