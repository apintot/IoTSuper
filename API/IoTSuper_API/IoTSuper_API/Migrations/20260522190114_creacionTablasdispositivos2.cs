using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTSuper_API.Migrations
{
    /// <inheritdoc />
    public partial class creacionTablasdispositivos2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "temperatura_actual",
                table: "Termometros");

            migrationBuilder.DropColumn(
                name: "stock_actual",
                table: "Stocks");

            migrationBuilder.AddColumn<string>(
                name: "Frase1",
                table: "Etiquetas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Frase2",
                table: "Etiquetas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Frase3",
                table: "Etiquetas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Frase4",
                table: "Etiquetas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Frase1",
                table: "Etiquetas");

            migrationBuilder.DropColumn(
                name: "Frase2",
                table: "Etiquetas");

            migrationBuilder.DropColumn(
                name: "Frase3",
                table: "Etiquetas");

            migrationBuilder.DropColumn(
                name: "Frase4",
                table: "Etiquetas");

            migrationBuilder.AddColumn<double>(
                name: "temperatura_actual",
                table: "Termometros",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "stock_actual",
                table: "Stocks",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
