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
    
    public partial class AcademicDegree
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AcademicDegree()
        {
            this.AcademicDegreeLanguages = new HashSet<AcademicDegreeLanguage>();
            this.Profiles = new HashSet<Profile>();
            this.ProfileAcademicDegrees = new HashSet<ProfileAcademicDegree>();
        }
    
        public int academic_degree_id { get; set; }
        public Nullable<int> type_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcademicDegreeLanguage> AcademicDegreeLanguages { get; set; }
        public virtual AcademicDegreeType AcademicDegreeType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profile> Profiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfileAcademicDegree> ProfileAcademicDegrees { get; set; }
    }
}
