//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppBanwao.KarryKart.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Coupon
    {
        public System.Guid CouponID { get; set; }
        public string DisplayName { get; set; }
        public Nullable<int> CouponTypeID { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedBy { get; set; }
    }
}
