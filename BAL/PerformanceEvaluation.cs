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
    
    public partial class PerformanceEvaluation
    {
        public int ID { get; set; }
        public Nullable<int> fkPerformanceAppraisal { get; set; }
        public Nullable<int> fkAssessmentCriteria { get; set; }
        public Nullable<int> SelfRating { get; set; }
        public string SelfComment { get; set; }
        public Nullable<int> ReviewerRating { get; set; }
        public string ReviewerComment { get; set; }
    
        public virtual AssessmentCriteria AssessmentCriteria { get; set; }
        public virtual PerformanceAppraisal PerformanceAppraisal { get; set; }
        public virtual tbl_E_AppraisalRating tbl_E_AppraisalRating { get; set; }
        public virtual tbl_E_AppraisalRating tbl_E_AppraisalRating1 { get; set; }
    }
}
