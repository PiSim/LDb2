//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LabDB2
{
    using System;
    using System.Collections.Generic;
    
    public partial class Test
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Test()
        {
            this.SubTests = new HashSet<SubTest>();
        }
    
        public long ID { get; set; }
        public string deletion_flag { get; set; }
        public long batchID { get; set; }
        public long methodID { get; set; }
        public long reportID { get; set; }
        public long external_reportID { get; set; }
        public string notes { get; set; }
        public long operatorID { get; set; }
        public string date { get; set; }
        public string meets_requirements { get; set; }
        public string stat_skip { get; set; }
    
        public virtual Report Report { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTest> SubTests { get; set; }
    }
}
