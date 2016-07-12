using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBanwao.KaryKart.Web.Helpers
{
    public class MessageHelper
    {

        public const string TempDataKey = "TempDataAlerts";

        public string AlertStyle { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }

    }   
    public static class AlertStyles
        {
            public const string Success = "success";
            public const string Information = "info";
            public const string Warning = "warning";
            public const string Danger = "danger";
            public const string Default = "default";
        }

    public class UserRegisterationType
    {
        public const string MOBILE = "mobile";
        public const string EMAIL = "email";
    }

    
}