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
    
    public partial class AuthorPaper
    {
        public int people_id { get; set; }
        public int paper_id { get; set; }
        public string current_mssv_msnv { get; set; }
        public int money_reward { get; set; }
    
        public virtual Person Person { get; set; }
        public virtual Paper Paper { get; set; }
    }
}
