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
    
    public partial class ControlPlanItems
    {
        public int ID { get; private set; }
        public int ControlPlanID { get; private set; }
        public int MethodID { get; private set; }
    
        public virtual ControlPlan ControlPlans { get; set; }
        public virtual Method method { get; set; }
    }
}