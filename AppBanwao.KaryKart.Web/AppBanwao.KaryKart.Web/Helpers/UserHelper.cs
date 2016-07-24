﻿using AppBanwao.KarryKart.Model;
using AppBanwao.KaryKart.Web.Models;
using DA.EncryptionManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

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
                var userDet = new UserDetail()
                {
                    UserID = user.UserID
                };

                var userAddress = new UserAddressDetail()
                {
                    UserID = user.UserID
                };


                _context.Users.Add(user);
                _context.UserDetails.Add(userDet);
                _context.UserAddressDetails.Add(userAddress);
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

        public bool VerifyOtp(OtpModel model,bool isOtp)
        {
            _context = new karrykartEntities();

            var otp = _context.OTPHolders.Where(x => x.OTPAssignedTo == model.UserIdentifier && x.OTPValue == model.Userotp).FirstOrDefault();

            if (otp != null)
            {
                CommonHelper.RemoveOTP(model.UserIdentifier);
                return true;
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

        public User IsAuthenticatedUser(LoginModel model)
        {
            _context = new karrykartEntities();
            model.UserPwd = EncryptionManager.ConvertToUnSecureString(EncryptionManager.EncryptData(model.UserPwd));
            var user = _context.Users.Where(x => x.EmailAddress == model.UserID || x.Mobile == model.UserID).FirstOrDefault();
            _context = null;
            return (user.Active.Value && user.Password == model.UserPwd) ? user : null;
        }

        public HttpCookie CreateAuthenticationTicket(User authUser)
        {
            _context = new karrykartEntities();
            CustomPrincipalSerialize serializeModel = new CustomPrincipalSerialize();

            serializeModel.UserName = string.IsNullOrEmpty(authUser.EmailAddress) ? authUser.Mobile : authUser.EmailAddress;
            serializeModel.FirstName = (!string.IsNullOrEmpty(authUser.UserDetails.FirstOrDefault().FirstName)) ? authUser.UserDetails.FirstOrDefault().FirstName : serializeModel.UserName;
            serializeModel.LastName = (!string.IsNullOrEmpty(authUser.UserDetails.FirstOrDefault().LastName)) ? authUser.UserDetails.FirstOrDefault().LastName : string.Empty;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string userData = serializer.Serialize(serializeModel);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, authUser.EmailAddress, DateTime.Now, DateTime.Now.AddHours(8), false, userData);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            return faCookie;
        }

        public void SignoffUser()
        {
            FormsAuthentication.SignOut();
        }

        public bool UserExists(string userID)
        {

            _context = new karrykartEntities();

            if (_context.Users.Where(x => x.EmailAddress == userID || x.Mobile == userID).Count() > 0)
                return true;

            _context = null;

            return false;

        }

        public bool SendPasswordResetOtp(string userID)
        {
            if (!(string.IsNullOrEmpty(userID)))
            {
                if (CommonHelper.IsMobile(userID))
                {
                    string otp = userID.Substring(0, 2) + CommonHelper.GenerateOTP();
                    CommonHelper.SaveOTP(otp, userID);
                    SmsHelper.SendOtpMessage(userID,otp);
                    return true;
                }
                else
                {
                    _emailHelper = new EmailHelper();

                    string otp = userID.Substring(0, 2) + CommonHelper.GenerateOTP();
                    CommonHelper.SaveOTP(otp, userID);
                    if (_emailHelper.SendOtpEmail(userID,otp))
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
                
            }
            return false;
        }

        public bool SetPassword(LoginModel model)
        { 
        _context = new karrykartEntities();

        var user = _context.Users.Where(x => x.EmailAddress == model.UserID).FirstOrDefault();

        if (user != null)
        {
            user.Password = EncryptionManager.ConvertToUnSecureString(EncryptionManager.EncryptData(model.UserPwd));
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        _context = null;
        _emailHelper = new EmailHelper();
        
            var IsMessageSent = SendChangePasswordMessage(model.UserID);
            _emailHelper = null;
            return IsMessageSent;

        
        }

        bool SendChangePasswordMessage(string userID)
        {
            if (!(string.IsNullOrEmpty(userID)))
            {
                if (CommonHelper.IsMobile(userID))
                {
                    SmsHelper.SendChangePasswordMessage(userID);
                    return true;
                }
                else
                {
                    _emailHelper = new EmailHelper();
                    
                    if (_emailHelper.SendPasswordChangeEmail(userID))
                    {
                        CommonHelper.RemoveOTP(userID);
                        _emailHelper = null;
                        return true;
                    }
                    else
                    {
                        _emailHelper = null;
                        return false;
                    }

                }

            }
            return false;
        }

    }
}