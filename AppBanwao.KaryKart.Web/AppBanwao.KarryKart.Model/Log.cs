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
    
    public partial class Log
    {
        public int LogId { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
        public string MethodName { get; set; }
        public string FileName { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> EventTimeStamp { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
    }
}
