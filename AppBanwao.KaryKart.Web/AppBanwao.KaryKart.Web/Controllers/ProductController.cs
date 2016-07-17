﻿using AppBanwao.KarryKart.Model;
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
            using (_dbContext = new karrykartEntities())
            {
          //      return View(ProductHelper.GetAllProducts(_dbContext)); 
            }
            return View();
        }

        public ActionResult Details(Guid id)
        {
            using (_dbContext = new karrykartEntities())
            {
         //       return View(ProductHelper.GetProduct(_dbContext, id));
            }
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
                CreateViewBagForStockPrice();
                return View(new ProductStockPriceModel() { ProductID = product.ProductID, Name = product.Name });
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStockPrice(ProductStockPriceModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    using (_dbContext = new karrykartEntities())
            //    {
            //        var productSizeMapping = new ProductSizeMapping() { ProductID = model.ProductID, SizeID = model.SizeID, UnitID = model.UnitID, Stock = model.Stock };
            //        _dbContext.ProductSizeMappings.Add(productSizeMapping);
                    
            //        var productprice = new ProductPrice() { CurrencyID = model.CurrencyID, ProductID = model.ProductID, SizeID = model.SizeID, Price = Convert.ToDecimal(model.Price) };
            //        _dbContext.ProductPrices.Add(productprice);
                    
            //        var productShipping = new ProductShipping() { ProductID = model.ProductID, SizeID = model.SizeID, Cost = Convert.ToDecimal(model.ShippingCost) };
            //        _dbContext.ProductShippings.Add(productShipping);
            //        _dbContext.SaveChanges();
            //        _logger.WriteLog(CommonHelper.MessageType.Success, "Product Stock and price added successfully with ID=" + model.Name, "AddStockPrice", "ProductController", User.Identity.Name);
            //        return RedirectToAction("Index", "Product");
            //    }
            //}
            //CreateViewBagForStockPrice();
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

        [HttpGet]
        public JsonResult GetSizes(int id) // its a GET, not a POST
        {
            _dbContext = new karrykartEntities();
            var sizes = _dbContext.Sizes.Where(x => x.SizeTypeID == id).Select(x => new { x.SizeID, x.Name }).ToList();
            _dbContext = null;
            return Json(sizes, JsonRequestBehavior.AllowGet);
        }       

        
        void CreateViewBagForProduct()
        {
            _dbContext = new karrykartEntities();
            ViewBag.CategoryID = new SelectList(_dbContext.Categories.ToList(), "CategoryID", "Name");
            ViewBag.SubcategoryID = new SelectList(_dbContext.Subcategories.ToList(), "SCategoryID", "Name");
            ViewBag.BrandID = new SelectList(_dbContext.Brands.ToList(), "BrandID", "Name");

            _dbContext = null;

        }

        void CreateViewBagForStockPrice()
        {
            _dbContext = new karrykartEntities();
            ViewBag.UnitID = new SelectList(_dbContext.Units.ToList(), "UnitID", "Name");
            ViewBag.SizeTypeID = new SelectList(_dbContext.SizeTypes.ToList(), "SizeTypeID", "Name");
            ViewBag.CurrencyID = new SelectList(_dbContext.Currencies.ToList(), "CurrencyID", "Shortform");
            ViewBag.SizeID = new SelectList(_dbContext.Sizes.ToList(), "SizeID", "Name");
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
