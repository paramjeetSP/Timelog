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
    
    public partial class tb_ChangeDesignation
    {
        public int Id { get; set; }
        public int FKEmployeeId { get; set; }
        public int FKOldDesignation { get; set; }
        public int FKNewDesignation { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime ChangeDate { get; set; }
        public Nullable<int> FKOldGrade { get; set; }
        public Nullable<int> FKNewGrade { get; set; }
        public string Experience { get; set; }
    
        public virtual tbl_E_Designation tbl_E_Designation { get; set; }
    }
}