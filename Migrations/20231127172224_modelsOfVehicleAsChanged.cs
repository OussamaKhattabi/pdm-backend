using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PremiumDeluxeMotorSports_v1.Migrations
{
    public partial class modelsOfVehicleAsChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NbrSiege",
                table: "Vehicule");

            migrationBuilder.AddColumn<string>(
                name: "Marque",
                table: "Vehicule",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Marque",
                table: "Vehicule");

            migrationBuilder.AddColumn<int>(
                name: "NbrSiege",
                table: "Vehicule",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
