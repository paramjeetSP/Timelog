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
    
    public partial class LeaveStatu
    {
        public LeaveStatu()
        {
            this.ReminderLeaves = new HashSet<ReminderLeave>();
        }
    
        public int ID { get; set; }
        public string Emp_id { get; set; }
        public Nullable<int> fkLeaveType { get; set; }
        public string Department { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string LeaveReason { get; set; }
        public string FirstLineManager_id { get; set; }
        public Nullable<int> FirstLineManagerStatus { get; set; }
        public string FirstLineMangerComment { get; set; }
        public string SecondLineManager_id { get; set; }
        public Nullable<int> SecondLineManagerStatus { get; set; }
        public string SecondLineManagerComment { get; set; }
        public string Hr_id { get; set; }
        public string Hr_Comment { get; set; }
        public Nullable<int> Hr_Status { get; set; }
        public Nullable<int> EmpLeaveStatus { get; set; }
        public Nullable<System.DateTime> LeaveAppliedDate { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string Admin_id { get; set; }
        public string Admin_Comment { get; set; }
        public Nullable<System.DateTime> FLDecisiondate { get; set; }
        public Nullable<System.DateTime> SLDecisiondate { get; set; }
        public Nullable<System.DateTime> HRRDecisiondate { get; set; }
        public Nullable<bool> IsHalfDay { get; set; }
        public bool IsProbationLeave { get; set; }
    
        public virtual tbl_E_LeaveType tbl_E_LeaveType { get; set; }
        public virtual ICollection<ReminderLeave> ReminderLeaves { get; set; }
    }
}