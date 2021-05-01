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
        public IActionResult Create(FirtsWork obj)
        {
            _db.FirtsWorks.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}