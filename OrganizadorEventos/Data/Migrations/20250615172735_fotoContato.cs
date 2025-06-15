using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizadorEventos.Data.Migrations
{
    /// <inheritdoc />
    public partial class fotoContato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Foto",
                table: "Contatos",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Contatos");
        }
    }
}
