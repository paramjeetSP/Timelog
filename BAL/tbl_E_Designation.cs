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
    
    public partial class tbl_E_Designation
    {
        public tbl_E_Designation()
        {
            this.Employees = new HashSet<Employee>();
            this.tb_ChangeDesignation = new HashSet<tb_ChangeDesignation>();
            this.tbEmployeeStatusRecords = new HashSet<tbEmployeeStatusRecord>();
            this.tbNoticePeriods = new HashSet<tbNoticePeriod>();
        }
    
        public int ID { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<tb_ChangeDesignation> tb_ChangeDesignation { get; set; }
        public virtual ICollection<tbEmployeeStatusRecord> tbEmployeeStatusRecords { get; set; }
        public virtual ICollection<tbNoticePeriod> tbNoticePeriods { get; set; }
    }
}
