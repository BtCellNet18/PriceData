using System;

namespace PriceData.Models
{
	public class Price
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public decimal Value { get; set; }
	}
}
