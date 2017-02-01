//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class Report
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Report()
        {
            this.ReportFiles = new HashSet<ReportFile>();
            this.tasks = new HashSet<Task>();
            this.Tests = new HashSet<Test>();
        }
    
        public int ID { get; set; }
        public int authorID { get; set; }
        public int batchID { get; set; }
        public string category { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> end_date { get; set; }
        public int Number { get; set; }
        public Nullable<int> projectID { get; set; }
        public int specification_versionID { get; set; }
        public string start_date { get; set; }
        public string requestID { get; set; }
    
        public virtual Batch Batch { get; set; }
        public virtual Person Author { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportFile> ReportFiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> tasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
        public virtual SpecificationVersion specification_versions { get; set; }
    }
}
