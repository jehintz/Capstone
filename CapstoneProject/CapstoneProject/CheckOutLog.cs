//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CapstoneProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class CheckOutLog
    {
        public int CheckOutLogID { get; set; }
        public int CardholderID { get; set; }
        public int BookID { get; set; }
        public System.DateTime CheckOutDate { get; set; }
        public Nullable<System.DateTime> CheckInDate { get; set; }
    
        public virtual Books Books { get; set; }
        public virtual Cardholders Cardholders { get; set; }
    }
}
