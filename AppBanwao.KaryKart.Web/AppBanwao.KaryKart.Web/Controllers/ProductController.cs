using AppBanwao.KarryKart.Model;
using AppBanwao.KaryKart.Web.Helpers;
using AppBanwao.KaryKart.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBanwao.KaryKart.Web.Controllers
{
    public class ProductController : BaseController
    {
        //
        // GET: /Product/
        string _productImages = ConfigurationManager.AppSettings["ProductDirectory"].ToString();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create() {
            CreateViewBagForProduct();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                using (_dbContext = new karrykartEntities())
                {
                    var product = new Product()
                    {
                        Active = model.Active,
                        CategoryID = model.CategoryID,
                        CreatedBy = User.Identity.Name,
                        UpdatedBy = User.Identity.Name,
                        Description = model.Description,
                        Name = model.Name,
                        ProductID = Guid.NewGuid(),
                        SubCategoryID = model.SubCategoryID,
                        BrandID = model.BrandID,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now

                    };
                    _dbContext.Products.Add(product);
                    _dbContext.SaveChanges();
                    _logger.WriteLog(CommonHelper.MessageType.Success, "Product created successfully with name=" + product.ProductID, "Create", "ProductController", User.Identity.Name);
                    return RedirectToAction("AddImageFeatureDetails", "Product", new { id = product.ProductID });
                }
            
            }
            CreateViewBagForProduct();
            return View();
        }

        public ActionResult AddImageFeatureDetails(Guid id)
        {
            using (_dbContext = new karrykartEntities())
            {
                var product = _dbContext.Products.Find(id);
                return View(new ProductModel() { ProductID = product.ProductID, Name = product.Name });
            
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddImageFeatureDetails(ProductModel model)
        {
            
            using (_dbContext = new karrykartEntities())
            {
                    if (!(String.IsNullOrEmpty(model.Features)))
                    {
                        foreach (var featureText in model.Features.Split(';'))
                        {
                            _dbContext.ProductFeatures.Add(new ProductFeature() { Feature = featureText, ProductID = model.ProductID });
                        }
                    }

                    var lstImages = UploadImage(model);

                    foreach (var image in lstImages)
                    {
                        if (!String.IsNullOrEmpty(image))
                        {
                            _dbContext.ProductImages.Add(new ProductImage() {ImageID=Guid.NewGuid(), ImageLink = image, ProductID = model.ProductID });
                            _dbContext.SaveChanges();
                        }
                    }
                    _logger.WriteLog(CommonHelper.MessageType.Success, "Product imgaes and features added successfully with name=" + model.ProductID, "Create", "ProductController", User.Identity.Name);
                    return RedirectToAction("AddStockPrice", "Product", new { id = model.ProductID });
            }

            return View(model);
        }
        
        public ActionResult AddStockPrice(Guid id)
        {
            using (_dbContext = new karrykartEntities())
            {
                var product = _dbContext.Products.Find(id);
                return View(new ProductStockPriceModel() { ProductID = product.ProductID, Name = product.Name });
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetSubCategories(int id) // its a GET, not a POST
        {
            _dbContext = new karrykartEntities();
            var subcategories = _dbContext.Subcategories.Where(x => x.CategoryID == id).Select(x => new { x.SCategoryID,x.Name }).ToList();
            _dbContext = null;
            return Json(subcategories, JsonRequestBehavior.AllowGet);
        }       
        void CreateViewBagForProduct()
        {
            _dbContext = new karrykartEntities();
            ViewBag.CategoryID = new SelectList(_dbContext.Categories.ToList(), "CategoryID", "Name");
            ViewBag.SubcategoryID = new SelectList(_dbContext.Subcategories.ToList(), "SCategoryID", "Name");
            ViewBag.BrandID = new SelectList(_dbContext.Brands.ToList(), "BrandID", "Name");

            _dbContext = null;

        }

        List<string> UploadImage(ProductModel model)
        {
            List<string> lstImageLink = new List<string>();
            if (model.Image1 != null)
                lstImageLink.Add( CommonHelper.UploadFile(model.Image1, _productImages));

            if (model.Image2 != null)
                lstImageLink.Add(CommonHelper.UploadFile(model.Image2, _productImages));

            if (model.Image3 != null)
                lstImageLink.Add(CommonHelper.UploadFile(model.Image3, _productImages));
            
            return lstImageLink;
        }
    }

}
