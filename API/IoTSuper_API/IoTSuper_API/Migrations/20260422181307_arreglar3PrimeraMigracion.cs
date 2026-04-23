using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTSuper_API.Migrations
{
    /// <inheritdoc />
    public partial class arreglar3PrimeraMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "esAdmin",
                table: "Clientes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "esAdmin",
                table: "Clientes");
        }
    }
}
