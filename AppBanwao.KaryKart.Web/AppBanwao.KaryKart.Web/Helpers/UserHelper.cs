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
        EmailHelper _emailHelper = null;
        public User RegisterUser(RegisterModel model)
        {
            _context = new karrykartEntities();
            if (!(_context.Users.Where(x => x.EmailAddress == model.UserIdentifier || x.Mobile == model.UserIdentifier).Count() > 0))
            {
                var user = new User()
                {
                    DateCreated = DateTime.Now,
                    EmailAddress = CommonHelper.IsEmail(model.UserIdentifier) ? model.UserIdentifier : string.Empty,
                    Mobile = CommonHelper.IsMobile(model.UserIdentifier) ? model.UserIdentifier : string.Empty,
                    Password = EncryptionManager.ConvertToUnSecureString(EncryptionManager.EncryptData(model.UserPwd)),
                    UserID = Guid.NewGuid(),
                    LastUpdated = DateTime.Now,
                    RoleID = CommonHelper.CustomerType.Customer,
                    Active = false
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                return user;
            }
           
            _context = null;

            return null;
        }

        public string SendRegisterUserConfirmation(User user)
        {
            string userRegisterWith = null;
            if (user != null)
            {
                if (!(string.IsNullOrEmpty(user.Mobile)))
                {
                    SmsHelper.SendRegisterMessage(user.Mobile);
                    userRegisterWith = ApplicationMessages.UserRegisterationType.MOBILE;
                }
                if (!(string.IsNullOrEmpty(user.EmailAddress)))
                {
                    _emailHelper = new EmailHelper();

                    if (!_emailHelper.SendRegisterEmail(user.EmailAddress))
                        userRegisterWith = ApplicationMessages.UserRegisterationType.EMAILWITHERROR;
                    else
                        userRegisterWith = ApplicationMessages.UserRegisterationType.EMAIL;

                    _emailHelper = null;
                }
            }
            else
            {
                userRegisterWith = ApplicationMessages.UserRegisterationType.USEREXIST;
            }
            return userRegisterWith;
        }

        public bool VerifyOtp(OtpModel model)
        {
            _context = new karrykartEntities();

            var otp = _context.OTPHolders.Where(x => x.OTPAssignedTo == model.UserIdentifier && x.OTPValue == model.Userotp).FirstOrDefault();

            if (otp != null)
            {
                var user = _context.Users.Where(u => u.EmailAddress == model.UserIdentifier || u.Mobile == model.UserIdentifier).FirstOrDefault();
                user.LastUpdated = DateTime.Now;
                user.Active = true;
                _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                CommonHelper.RemoveOTP(model.UserIdentifier);
                return SendOtpVerificationToUser(user);
            }

            return false;
       
        }

        bool SendOtpVerificationToUser(User user)
        {
            if (!(string.IsNullOrEmpty(user.Mobile)))
            {
                SmsHelper.SendVerificationMessage(user.Mobile);
                return true;
            }
            if (!(string.IsNullOrEmpty(user.EmailAddress)))
            {
                _emailHelper = new EmailHelper();

                if (_emailHelper.SendVerificationEmail(user.EmailAddress))
                {
                    _emailHelper = null;
                    return true;
                }
                else
                {
                    _emailHelper = null;
                    return false;
                }
               
            }
            return false;
        }

    }
}