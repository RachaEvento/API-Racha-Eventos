using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizadorEventos.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConcertadoRelacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PagamentoParticipantes_ParticipanteId",
                table: "PagamentoParticipantes");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoParticipantes_ParticipanteId",
                table: "PagamentoParticipantes",
                column: "ParticipanteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PagamentoParticipantes_ParticipanteId",
                table: "PagamentoParticipantes");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoParticipantes_ParticipanteId",
                table: "PagamentoParticipantes",
                column: "ParticipanteId",
                unique: true);
        }
    }
}
