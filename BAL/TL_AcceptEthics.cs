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
    
    public partial class TL_AcceptEthics
    {
        public int ID { get; set; }
        public string Emp_id { get; set; }
        public Nullable<System.DateTime> AcceptDate { get; set; }
        public Nullable<bool> IsAgree { get; set; }
        public Nullable<int> fkEthics { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual TL_Ethics TL_Ethics { get; set; }
    }
}
