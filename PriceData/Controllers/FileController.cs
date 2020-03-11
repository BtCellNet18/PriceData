using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PriceData.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Z.EntityFramework.Extensions;

namespace PriceData.Controllers
{
	public class FileController : Controller
	{
		private readonly DataContext _context;
		private readonly IConfiguration _config;
		private readonly string _connectionString;

		public FileController(
			DataContext context,
			IConfiguration config)
		{
			_context = context;
			_config = config;
			_connectionString = _config.GetConnectionString("DefaultConnection");
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(UploadFile uploadFile)
		{
			if (ModelState.IsValid)
			{
				await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Prices]");
				var transaction = await _context.Database.BeginTransactionAsync();

				try
				{
					EntityFrameworkManager.ContextFactory = _context => GetContext();

					using (var context = GetContext())
					{
						var prices = await ReadFileAsync(uploadFile.File);
						await context.BulkInsertAsync(prices);
					}

					await transaction.CommitAsync();
				}
				catch
				{
					transaction.Rollback();
				}
			}

			return View();
		}

		private async Task<List<Price>> ReadFileAsync(IFormFile file)
		{
			var culture = new CultureInfo("en-GB");
			List<Price> prices = new List<Price>();
			using (var reader = new StreamReader(file.OpenReadStream()))
			{
				int row = 0;

				while (reader.Peek() >= 0)
				{
					row += 1;
					var line = await reader.ReadLineAsync();
					// Skip headers
					if (row == 1) continue;

					string[] parts = line.Split(',');

					prices.Add(new Price()
					{
						Date = DateTime.Parse(parts[0], culture, DateTimeStyles.None),
						Value = decimal.Parse(parts[1])
					});
				}
			}
			return prices;
		}

		private DataContext GetContext()
		{
			var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
			optionsBuilder.UseSqlServer(_connectionString);
			return new DataContext(optionsBuilder.Options);
		}
	}
}