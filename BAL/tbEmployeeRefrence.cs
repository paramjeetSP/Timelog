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
    
    public partial class tbEmployeeRefrence
    {
        public int ID { get; set; }
        public int FKEmployee { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Designation { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
