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
    
    public partial class ModulePermission
    {
        public int ID { get; set; }
        public Nullable<int> fkModule { get; set; }
        public Nullable<int> fkRole { get; set; }
        public Nullable<bool> CanAdd { get; set; }
        public Nullable<bool> CanEdit { get; set; }
        public Nullable<bool> CanDelete { get; set; }
        public Nullable<bool> CanView { get; set; }
    
        public virtual tbl_E_Module tbl_E_Module { get; set; }
        public virtual tbl_E_Role tbl_E_Role { get; set; }
    }
}