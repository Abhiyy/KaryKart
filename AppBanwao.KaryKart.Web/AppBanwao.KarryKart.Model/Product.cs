//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppBanwao.KarryKart.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public System.Guid ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> BrandID { get; set; }
        public Nullable<int> SubCategoryID { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
