using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
    public class FirstWorkController : Controller
    {
        private readonly ApplicationDbContext _db;
        public FirstWorkController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<FirtsWork> objList = _db.FirtsWorks;
            return View(objList);
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FirtsWork obj)
        {
            if (ModelState.IsValid)
            {
                _db.FirtsWorks.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.FirtsWorks.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(FirtsWork obj)
        {
            if (ModelState.IsValid)
            {
                _db.FirtsWorks.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            
            var obj = _db.FirtsWorks.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.FirtsWorks.Remove(obj);
            _db.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}