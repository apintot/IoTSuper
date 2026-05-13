using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTSuper_API.Migrations
{
    /// <inheritdoc />
    public partial class creacionTablasClienteTipoLoca6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Centros_Localizaciones_LocalizacionIdLocalizacion",
                table: "Centros");

            migrationBuilder.DropForeignKey(
                name: "FK_Centros_Tipologias_TipologiaIdTipologia",
                table: "Centros");

            migrationBuilder.DropIndex(
                name: "IX_Centros_LocalizacionIdLocalizacion",
                table: "Centros");

            migrationBuilder.DropIndex(
                name: "IX_Centros_TipologiaIdTipologia",
                table: "Centros");

            migrationBuilder.DropColumn(
                name: "LocalizacionIdLocalizacion",
                table: "Centros");

            migrationBuilder.DropColumn(
                name: "TipologiaIdTipologia",
                table: "Centros");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocalizacionIdLocalizacion",
                table: "Centros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TipologiaIdTipologia",
                table: "Centros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Centros_LocalizacionIdLocalizacion",
                table: "Centros",
                column: "LocalizacionIdLocalizacion");

            migrationBuilder.CreateIndex(
                name: "IX_Centros_TipologiaIdTipologia",
                table: "Centros",
                column: "TipologiaIdTipologia");

            migrationBuilder.AddForeignKey(
                name: "FK_Centros_Localizaciones_LocalizacionIdLocalizacion",
                table: "Centros",
                column: "LocalizacionIdLocalizacion",
                principalTable: "Localizaciones",
                principalColumn: "id_localizacion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Centros_Tipologias_TipologiaIdTipologia",
                table: "Centros",
                column: "TipologiaIdTipologia",
                principalTable: "Tipologias",
                principalColumn: "id_tipologia",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
