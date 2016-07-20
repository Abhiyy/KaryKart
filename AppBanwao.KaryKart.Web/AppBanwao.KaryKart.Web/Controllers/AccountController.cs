using AppBanwao.KarryKart.Model;
using AppBanwao.KaryKart.Web.Helpers;
using AppBanwao.KaryKart.Web.Models;
using DA.EncryptionManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBanwao.KaryKart.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        UserHelper _userHelper = null;

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                _userHelper = new UserHelper();
                var user = _userHelper.IsAuthenticatedUser(model);

                if (user != null)
                {
                    Response.Cookies.Add(_userHelper.CreateAuthenticationTicket(user));
                    return Json(new { messagetype = ApplicationMessages.UserLogin.VALIDUSER }, JsonRequestBehavior.AllowGet); 
                }
                else
                {
                    return Json(new { messagetype = ApplicationMessages.UserLogin.INVALIDUSER, message = "Invalid username/password. Please try again." }, JsonRequestBehavior.AllowGet); 
                }
            }
            catch (Exception ex)
            { 
            
            }
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {

            try
            {
                _userHelper = new UserHelper();
                
                var userRegType = _userHelper.SendRegisterUserConfirmation(_userHelper.RegisterUser(model));

                switch(userRegType)
                {
                    case ApplicationMessages.UserRegisterationType.EMAILWITHERROR:
                    return Json(new { messagetype = ApplicationMessages.UserRegisterationType.EMAILWITHERROR, message = "Unable to deliver the email to your mailbox. Please contact administrator to know your OTP." }, JsonRequestBehavior.AllowGet); 
                
                    case  ApplicationMessages.UserRegisterationType.EMAIL:
                    return Json(new { messagetype = ApplicationMessages.UserRegisterationType.EMAIL, message = "Your account has been created successfully. Please check your email for otp and use it to activate your account." }, JsonRequestBehavior.AllowGet);

                    case ApplicationMessages.UserRegisterationType.MOBILE:
                        return Json(new { messagetype = ApplicationMessages.UserRegisterationType.MOBILE, message = "success" }, JsonRequestBehavior.AllowGet);

                    case ApplicationMessages.UserRegisterationType.MOBILEWITHERROR:
                        return Json(new { messagetype = ApplicationMessages.UserRegisterationType.MOBILEWITHERROR, message = "success" }, JsonRequestBehavior.AllowGet);

                    case ApplicationMessages.UserRegisterationType.USEREXIST:
                        return Json(new { messagetype = ApplicationMessages.UserRegisterationType.USEREXIST, message = "The user already exists. Please try with different email." }, JsonRequestBehavior.AllowGet);
                }
             

            }
            catch (Exception ex)
            { 
            
            }

            return View();
        }
        
        [HttpPost]
        public ActionResult Verifyotp(OtpModel model)
        {
            try
            {
                _userHelper = new UserHelper();
                if (_userHelper.VerifyOtp(model))
                {
                    return Json(new { messagetype = "success", message = "Your account is verified and active. Please login using your credentials." }, JsonRequestBehavior.AllowGet); 
                }
                else {
                    return Json(new { messagetype = "error", message = "Your account is active now. Unable to deliver email to your mailbox. Please contact administrator for verification purpose" }, JsonRequestBehavior.AllowGet); 
                }


            }
            catch (Exception ex)
            { 
            
            }
            return View();
        }

        public ActionResult Logout()
        {
            _userHelper = new UserHelper();
            _userHelper.SignoffUser();
            _userHelper=null;
            return RedirectToAction("Index", "Home");
        }
       
    }
}
