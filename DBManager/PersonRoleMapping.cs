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
    
    public partial class PersonRoleMapping
    {
        public int ID { get; set; }
        public int personID { get; set; }
        public int roleID { get; set; }
        public bool IsSelected { get; set; }
    
        public virtual Person Person { get; set; }
        public virtual PersonRole Role { get; set; }
    }
}