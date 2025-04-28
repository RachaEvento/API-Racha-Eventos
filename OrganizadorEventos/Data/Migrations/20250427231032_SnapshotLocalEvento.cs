using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizadorEventos.Data.Migrations
{
    /// <inheritdoc />
    public partial class SnapshotLocalEvento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalBairro",
                table: "Eventos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalCidade",
                table: "Eventos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalDescricaoLocal",
                table: "Eventos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalEndereco",
                table: "Eventos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalEstado",
                table: "Eventos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalNome",
                table: "Eventos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalBairro",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "LocalCidade",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "LocalDescricaoLocal",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "LocalEndereco",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "LocalEstado",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "LocalNome",
                table: "Eventos");
        }
    }
}
