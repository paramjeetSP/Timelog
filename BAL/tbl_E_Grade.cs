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
    
    public partial class tbl_E_Grade
    {
        public tbl_E_Grade()
        {
            this.AssessmentCriterias = new HashSet<AssessmentCriteria>();
            this.Employees = new HashSet<Employee>();
            this.tbEmployeeStatusRecords = new HashSet<tbEmployeeStatusRecord>();
        }
    
        public int ID { get; set; }
        public string Description { get; set; }
    
        public virtual AppraisalInterval AppraisalInterval { get; set; }
        public virtual ICollection<AssessmentCriteria> AssessmentCriterias { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<tbEmployeeStatusRecord> tbEmployeeStatusRecords { get; set; }
    }
}
