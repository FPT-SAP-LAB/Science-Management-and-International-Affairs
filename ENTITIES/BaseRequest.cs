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
    
    public partial class BaseRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BaseRequest()
        {
            this.CommentBases = new HashSet<CommentBase>();
            this.Decisions = new HashSet<Decision>();
        }
    
        public int request_id { get; set; }
        public Nullable<int> account_id { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.DateTime> finished_date { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual RequestCitation RequestCitation { get; set; }
        public virtual RequestConference RequestConference { get; set; }
        public virtual RequestInvention RequestInvention { get; set; }
        public virtual RequestPaper RequestPaper { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentBase> CommentBases { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Decision> Decisions { get; set; }
    }
}
