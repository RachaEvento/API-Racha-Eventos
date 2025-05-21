using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizadorEventos.Data.Migrations
{
    /// <inheritdoc />
    public partial class DataPagamentoParticipante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataPagamento",
                table: "PagamentoParticipantes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataPagamento",
                table: "PagamentoParticipantes");
        }
    }
}
