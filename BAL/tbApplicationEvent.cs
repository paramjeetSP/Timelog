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
    
    public partial class tbApplicationEvent
    {
        public tbApplicationEvent()
        {
            this.tbEmailQueues = new HashSet<tbEmailQueue>();
            this.tbTemplates = new HashSet<tbTemplate>();
        }
    
        public int ID { get; set; }
        public string EventName { get; set; }
    
        public virtual ICollection<tbEmailQueue> tbEmailQueues { get; set; }
        public virtual ICollection<tbTemplate> tbTemplates { get; set; }
    }
}