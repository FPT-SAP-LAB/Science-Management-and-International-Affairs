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
    
    public partial class CollaborationTypeDirection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CollaborationTypeDirection()
        {
            this.CollaborationTypeDirectionLanguages = new HashSet<CollaborationTypeDirectionLanguage>();
        }
    
        public int collab_type_direction_id { get; set; }
        public int direction_id { get; set; }
        public int collab_type_id { get; set; }
    
        public virtual AcademicCollaborationType AcademicCollaborationType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollaborationTypeDirectionLanguage> CollaborationTypeDirectionLanguages { get; set; }
        public virtual Direction Direction { get; set; }
    }
}
