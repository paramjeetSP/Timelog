//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmpExperience
    {
        public int ID { get; set; }
        public Nullable<int> fkEmployee { get; set; }
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public string CompanyAddress { get; set; }
        public string NatureofDuties { get; set; }
        public string LeavingReason { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}