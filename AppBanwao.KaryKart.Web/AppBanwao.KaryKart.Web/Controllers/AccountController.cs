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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {

            try
            {
                _userHelper = new UserHelper();
                
                var userRegType = _userHelper.SendRegisterUserConfirmation(_userHelper.RegisterUser(model));

                

            }
            catch (Exception ex)
            { 
            
            }

            return View();
        }

       
    }
}
