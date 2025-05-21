using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizadorEventos.Data.Migrations
{
    /// <inheritdoc />
    public partial class StatusPagamentoParticipante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aceito",
                table: "PagamentoParticipantes");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PagamentoParticipantes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "PagamentoParticipantes");

            migrationBuilder.AddColumn<bool>(
                name: "Aceito",
                table: "PagamentoParticipantes",
                type: "boolean",
                nullable: true);
        }
    }
}
