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
    
    public partial class tbTemplate
    {
        public int ID { get; set; }
        public string TemplatePath { get; set; }
        public int EventID { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual tbApplicationEvent tbApplicationEvent { get; set; }
    }
}