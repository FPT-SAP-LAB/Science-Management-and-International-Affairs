//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ENTITIES
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaperWithCriteria
    {
        public int criteria_id { get; set; }
        public int paper_id { get; set; }
        public string link { get; set; }
        public Nullable<bool> check { get; set; }
        public Nullable<bool> manager_check { get; set; }
        public int paperwithcriteria_id { get; set; }
    
        public virtual Paper Paper { get; set; }
        public virtual PaperCriteria PaperCriteria { get; set; }
    }
}
