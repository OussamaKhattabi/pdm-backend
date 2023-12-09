using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pdm.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleName = table.Column<string>(type: "TEXT", nullable: false),
                    RoleDescription = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Vehicule",
                columns: table => new
                {
                    VehiculeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Marque = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Prix = table.Column<int>(type: "INTEGER", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    ReservationId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicule", x => x.VehiculeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserFirstName = table.Column<string>(type: "TEXT", nullable: false),
                    UserLastName = table.Column<string>(type: "TEXT", nullable: false),
                    UserEmail = table.Column<string>(type: "TEXT", nullable: false),
                    UserPassword = table.Column<string>(type: "TEXT", nullable: false),
                    RoleID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Custom",
                columns: table => new
                {
                    CustomId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Couleur = table.Column<string>(type: "TEXT", nullable: false),
                    Stage = table.Column<int>(type: "INTEGER", nullable: false),
                    PrixCstm = table.Column<double>(type: "REAL", nullable: false),
                    VehiculeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Custom", x => x.CustomId);
                    table.ForeignKey(
                        name: "FK_Custom_Vehicule_VehiculeId",
                        column: x => x.VehiculeId,
                        principalTable: "Vehicule",
                        principalColumn: "VehiculeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    IdReservation = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateReservation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    VehiculeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.IdReservation);
                    table.ForeignKey(
                        name: "FK_Reservation_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_Vehicule_VehiculeId",
                        column: x => x.VehiculeId,
                        principalTable: "Vehicule",
                        principalColumn: "VehiculeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commande",
                columns: table => new
                {
                    CmdId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date_Cmd = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CustomId = table.Column<int>(type: "INTEGER", nullable: false),
                    VehiculeId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commande", x => x.CmdId);
                    table.ForeignKey(
                        name: "FK_Commande_Custom_CustomId",
                        column: x => x.CustomId,
                        principalTable: "Custom",
                        principalColumn: "CustomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commande_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commande_Vehicule_VehiculeId",
                        column: x => x.VehiculeId,
                        principalTable: "Vehicule",
                        principalColumn: "VehiculeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commande_CustomId",
                table: "Commande",
                column: "CustomId");

            migrationBuilder.CreateIndex(
                name: "IX_Commande_UserId",
                table: "Commande",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Commande_VehiculeId",
                table: "Commande",
                column: "VehiculeId");

            migrationBuilder.CreateIndex(
                name: "IX_Custom_VehiculeId",
                table: "Custom",
                column: "VehiculeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_UserId",
                table: "Reservation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_VehiculeId",
                table: "Reservation",
                column: "VehiculeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicule_ReservationId",
                table: "Vehicule",
                column: "ReservationId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commande");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Custom");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Vehicule");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
