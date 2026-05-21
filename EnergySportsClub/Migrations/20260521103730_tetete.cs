using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnergySportsClub.Migrations
{
    /// <inheritdoc />
    public partial class tetete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ReservationMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ReservationMaterials");
        }
    }
}
