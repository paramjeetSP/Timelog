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
    
    public partial class tbl_E_AppraisalRating
    {
        public tbl_E_AppraisalRating()
        {
            this.PerformanceAppraisals = new HashSet<PerformanceAppraisal>();
            this.PerformanceEvaluations = new HashSet<PerformanceEvaluation>();
            this.PerformanceEvaluations1 = new HashSet<PerformanceEvaluation>();
        }
    
        public int ID { get; set; }
        public string AppraisalMetric { get; set; }
    
        public virtual ICollection<PerformanceAppraisal> PerformanceAppraisals { get; set; }
        public virtual ICollection<PerformanceEvaluation> PerformanceEvaluations { get; set; }
        public virtual ICollection<PerformanceEvaluation> PerformanceEvaluations1 { get; set; }
    }
}