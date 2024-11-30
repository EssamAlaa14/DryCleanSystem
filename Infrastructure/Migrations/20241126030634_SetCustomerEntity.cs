using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetCustomerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondaryPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_AppartmentNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address_FloorNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address_BuildingName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Address_District = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
