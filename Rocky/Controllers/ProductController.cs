using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Products.Include(u => u.Category).Include(u => u.FirtsWork);

            

            return View(objList);
        }
        // GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Categories.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.CategoryId.ToString()
            //});

            //ViewBag.CategoryDropDown = CategoryDropDown;

            //Product product = new Product();
            ProductVM productVm = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.CategoryId.ToString()
                }),
                WorkSelectList = _db.FirtsWorks.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),


            };
           
           
            if (id == null)
            {
                // create
                return View(productVm);
            }
            else
            {
                productVm.Product = _db.Products.Find(id);
                if (productVm.Product == null)
                {
                    return NotFound();
                }
                return View(productVm);
            }
            
        }
        // POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                
                string webRootPath = _webHostEnvironment.WebRootPath;
                
                if(productVM.Product.Id == 0)
                {
                    // creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;
                    _db.Products.Add(productVM.Product);
                    

                }
                else
                {
                    // updating
                    var objFromDb = _db.Products.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);
                    string test = productVM.Product.Image;
                    string name = productVM.Product.Name;

                    if (files.Count() > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;

                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }
                    _db.Products.Update(productVM.Product);
                    
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _db.Categories.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CategoryId.ToString()
            });
            productVM.WorkSelectList = _db.FirtsWorks.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(productVM);
            
        }

       
        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            
            var obj = _db.Products.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + WC.ImagePath;
            
       

            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _db.Products.Remove(obj);

            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}