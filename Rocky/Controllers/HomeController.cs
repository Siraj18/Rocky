using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;
using Rocky.Utility;

namespace Rocky.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _db;

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
		{
			_db = db;
			_logger = logger;
		}

		public IActionResult Index()
		{
			HomeVM homeVM = new HomeVM()
			{
				Products = _db.Products.Include(u => u.Category).Include(u => u.FirtsWork),
				Categories = _db.Categories

			};
			return View(homeVM);
		}

		public IActionResult Details(int id)
		{
			List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
			if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
				&& HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
			{
				shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
			}



			DetailsVM detailsVM = new DetailsVM()
			{
				Product = _db.Products.Include(u => u.Category).Include(u => u.FirtsWork)
				.Where(u => u.Id == id).FirstOrDefault(),
				ExistInCart = false
			};

			foreach (var item in shoppingCartsList)
			{
				if (item.ProductId == id)
				{
					detailsVM.ExistInCart = true;
				}
			}

			return View(detailsVM);
		}
		[HttpPost, ActionName("Details")]
		public IActionResult DetailsPost(int id)
		{
			List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
			if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
				&& HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
			{
				shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
			}
			shoppingCartsList.Add(new ShoppingCart { ProductId = id });
			HttpContext.Session.Set(WC.SessionCart, shoppingCartsList);
			return RedirectToAction(nameof(Index));
		}
		public IActionResult RemoveFromCart(int id)
		{
			List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
			if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
				&& HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
			{
				shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
			}
			var itemToDelete = shoppingCartsList.SingleOrDefault(u => u.ProductId == id);
			if (itemToDelete != null) shoppingCartsList.Remove(itemToDelete);

			HttpContext.Session.Set(WC.SessionCart, shoppingCartsList);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
