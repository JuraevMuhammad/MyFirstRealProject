using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class reference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Rentals_RentalId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Cars_CarId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Rentals_RentalId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CarId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RentalId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Cars_RentalId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RentalId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RentalId",
                table: "Cars");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CarId",
                table: "Rentals",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_UserId",
                table: "Rentals",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Cars_CarId",
                table: "Rentals",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Users_UserId",
                table: "Rentals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Cars_CarId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Users_UserId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_CarId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_UserId",
                table: "Rentals");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentalId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentalId",
                table: "Cars",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CarId",
                table: "Users",
                column: "CarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RentalId",
                table: "Users",
                column: "RentalId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_RentalId",
                table: "Cars",
                column: "RentalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Rentals_RentalId",
                table: "Cars",
                column: "RentalId",
                principalTable: "Rentals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Cars_CarId",
                table: "Users",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Rentals_RentalId",
                table: "Users",
                column: "RentalId",
                principalTable: "Rentals",
                principalColumn: "Id");
        }
    }
}
