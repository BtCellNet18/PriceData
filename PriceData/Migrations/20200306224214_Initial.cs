using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PriceData.Migrations
{
	public partial class Initial : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
					name: "Prices",
					columns: table => new
					{
						Id = table.Column<int>(nullable: false)
									.Annotation("SqlServer:Identity", "1, 1"),
						Date = table.Column<DateTime>(nullable: false),
						Value = table.Column<decimal>(type: "decimal(18,8)", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_Prices", x => x.Id);
					});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
					name: "Prices");
		}
	}
}
