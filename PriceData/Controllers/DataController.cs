using Microsoft.AspNetCore.Mvc;
using PriceData.Models;
using System.Collections.Generic;
using System.Linq;

namespace PriceData.Controllers
{
	public class DataController : Controller
	{
		private readonly DataContext _context;

		public DataController(DataContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IEnumerable<Price> GetPrices()
		{
			return _context.Prices.ToList();
		}
	}
}