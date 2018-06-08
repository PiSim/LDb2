//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class Test
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Test()
        {
            this.SubTests = new HashSet<SubTest>();
            this.TaskItems = new HashSet<TaskItem>();
        }
    
        public int ID { get; set; }
        public int MethodID { get; set; }
        public Nullable<int> TBD { get; set; }
        public string Notes { get; set; }
        public Nullable<int> operatorID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public bool IsComplete { get; set; }
        public Nullable<int> instrumentID { get; set; }
        public Nullable<int> RequirementID { get; set; }
        public double Duration { get; set; }
        public int TestRecordID { get; set; }
    
        public virtual Person Person { get; set; }
        public virtual Report TBD2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTest> SubTests { get; set; }
        public virtual Method Method { get; set; }
        public virtual Instrument Instrument { get; set; }
        public virtual Requirement Requirement { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskItem> TaskItems { get; set; }
        public virtual TestRecord TestRecord { get; set; }
    }
}
