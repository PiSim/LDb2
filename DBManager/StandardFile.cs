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
    
    public partial class StandardFile
    {
        public int ID { get; set; }
        public int standardID { get; set; }
        public string path { get; set; }
        public string description { get; set; }
    
        public virtual Std standard { get; set; }
    }
}