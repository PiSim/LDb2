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
    
    public partial class UserRoleMapping
    {
        public int ID { get; private set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public bool IsSelected { get; set; }
    
        public virtual UserRole UserRole { get; set; }
        public virtual User User { get; set; }
    }
}
