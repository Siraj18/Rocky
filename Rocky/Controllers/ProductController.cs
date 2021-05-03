using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Products;

            foreach(var obj in objList)
            {
                obj.Category = _db.Categories.FirstOrDefault(u => u.CategoryId == obj.CategoryId);
            }

            return View(objList);
        }
        // GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            Product product = new Product();
            if (id == null)
            {
                // create
                return View(product);
            }
            else
            {
                product = _db.Products.Find(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            return View();
        }
        // POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
            
        }

       
        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            
            var obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);

            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}