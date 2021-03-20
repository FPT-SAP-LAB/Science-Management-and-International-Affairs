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
    
    public partial class Profile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Profile()
        {
            this.ProfileAcademicDegrees = new HashSet<ProfileAcademicDegree>();
        }
    
        public int people_id { get; set; }
        public Nullable<System.DateTime> birth_date { get; set; }
        public string working_address { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public string google_scholar { get; set; }
        public string cv { get; set; }
        public Nullable<int> avatar_id { get; set; }
        public Nullable<int> current_academic_degree_id { get; set; }
        public Nullable<int> country_id { get; set; }
        public Nullable<long> bank_number { get; set; }
        public string bank_branch { get; set; }
        public Nullable<long> tax_code { get; set; }
        public string identification_number { get; set; }
        public Nullable<int> identification_file_id { get; set; }
        public int office_id { get; set; }
        public string mssv_msnv { get; set; }
        public Nullable<int> account_id { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Country Country { get; set; }
        public virtual File File { get; set; }
        public virtual File File1 { get; set; }
        public virtual Office Office { get; set; }
        public virtual Person Person { get; set; }
        public virtual AcademicDegree AcademicDegree { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfileAcademicDegree> ProfileAcademicDegrees { get; set; }
    }
}
