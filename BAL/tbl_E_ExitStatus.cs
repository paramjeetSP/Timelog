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
    
    public partial class tbl_E_ExitStatus
    {
        public tbl_E_ExitStatus()
        {
            this.Employees = new HashSet<Employee>();
            this.tbEmployeeExitRecords = new HashSet<tbEmployeeExitRecord>();
        }
    
        public int ID { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<tbEmployeeExitRecord> tbEmployeeExitRecords { get; set; }
    }
}
