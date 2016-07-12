using AppBanwao.KarryKart.Model;
using AppBanwao.KaryKart.Web.Models;
using DA.EncryptionManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBanwao.KaryKart.Web.Helpers
{
    public class UserHelper
    {
        karrykartEntities _context = null;
        public User RegisterUser(RegisterModel model)
        {
            _context = new karrykartEntities();

            var user = new User()
            {
                DateCreated = DateTime.Now,
                EmailAddress = CommonHelper.IsEmail(model.UserIdentifier) ? model.UserIdentifier : string.Empty,
                Mobile = CommonHelper.IsMobile(model.UserIdentifier) ? model.UserIdentifier : string.Empty,
                Password = EncryptionManager.ConvertToUnSecureString(EncryptionManager.EncryptData(model.UserPwd)),
                UserID = Guid.NewGuid(),
                LastUpdated = DateTime.Now
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            _context = null;

            return user;
        }

        public string SendRegisterUserConfirmation(User user)
        {
            string userRegisterWith = null;

            if (!(string.IsNullOrEmpty(user.Mobile)))
            {
                SmsHelper.SendRegisterMessage(user.Mobile);
                userRegisterWith = UserRegisterationType.MOBILE;
            }
            if (!(string.IsNullOrEmpty(user.EmailAddress)))
            {
                EmailHelper.SendRegisterEmail(user.EmailAddress);
                userRegisterWith = UserRegisterationType.EMAIL;
            }
            return userRegisterWith;
        }
    }
}