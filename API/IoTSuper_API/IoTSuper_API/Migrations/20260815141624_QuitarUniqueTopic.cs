using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTSuper_API.Migrations
{
    /// <inheritdoc />
    public partial class QuitarUniqueTopic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Componentes_topic",
                table: "Componentes");

            migrationBuilder.CreateIndex(
                name: "IX_Componentes_topic",
                table: "Componentes",
                column: "topic");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Componentes_topic",
                table: "Componentes");

            migrationBuilder.CreateIndex(
                name: "IX_Componentes_topic",
                table: "Componentes",
                column: "topic",
                unique: true);
        }
    }
}
