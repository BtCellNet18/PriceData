using Microsoft.EntityFrameworkCore;

namespace PriceData.Models
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//modelBuilder.Entity<Price>()
			//	.HasIndex(p => p.Date)
			//	.IsUnique();

			modelBuilder.Entity<Price>()
				.Property(p => p.Value)
				.HasColumnType("decimal(18,8)");
		}

		public DbSet<Price> Prices { get; set; }
	}
}
