using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTSuper_API.Migrations
{
    /// <inheritdoc />
    public partial class eliminarTelefonos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "telefono_emergencia",
                table: "Termometros");

            migrationBuilder.DropColumn(
                name: "telefono_emergencia",
                table: "Stocks");

            migrationBuilder.AddColumn<string>(
                name: "email_emergencia",
                table: "Etiquetas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email_emergencia",
                table: "Etiquetas");

            migrationBuilder.AddColumn<string>(
                name: "telefono_emergencia",
                table: "Termometros",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "telefono_emergencia",
                table: "Stocks",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
