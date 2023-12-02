using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using recordBook.Models;

namespace recordBook.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}


		[HttpPost]
		public IActionResult Index(UserViewModel user)
		{
			if (ModelState.IsValid)
			{
				return Content(user.Login + " " + user.Password);
			}
			else return View(user);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		
	}
}