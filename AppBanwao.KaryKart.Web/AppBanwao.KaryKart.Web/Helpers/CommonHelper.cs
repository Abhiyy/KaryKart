using AppBanwao.KarryKart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;
namespace AppBanwao.KaryKart.Web.Helpers
{
    public class CommonHelper
    {
        public class MessageType
        {
            public const string Error = "ERROR";
            public const string Success = "SUCCESS";
            public const string Warning = "WARNING";
            public const string Infotmation = "INFORMATION";
            public const string Exception = "EXCEPTION";
        }

        public class Logger
        {
            karrykartEntities _dbContext = null;
            public void WriteLog(string messageType, string message, string methodName, string fileName, string userName)
            {
                var log = new Log(){
                EventTimeStamp = DateTime.UtcNow,
                FileName = fileName,
                IpAddress = GetIPAddress(),
                Message = message,
                MessageType = messageType,
                MethodName = methodName,
                UserName = userName,
                Browser = HttpContext.Current.Request.UserAgent};
                try
                {
                    _dbContext = new karrykartEntities();
                    _dbContext.Logs.Add(log);
                    _dbContext.SaveChanges();

                }
                catch (Exception ex)
                {

                }
                finally {
                    _dbContext = null;
                }
            }
        }
        
        public static string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public static string UploadFile(HttpPostedFileBase file, string uploadDirectory)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/" + uploadDirectory), fileName);
                file.SaveAs(path);
                return "~/" + uploadDirectory + "/" + fileName;
            }

            return string.Empty;
        }
        
    }
}