using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizadorEventos.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_AspNetUsers_UsuarioId1",
                table: "Contatos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_AspNetUsers_UsuarioId1",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Locais_AspNetUsers_UsuarioId",
                table: "Locais");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_UsuarioId1",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Contatos_UsuarioId1",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "Contatos");

            migrationBuilder.RenameColumn(
                name: "Rua",
                table: "Locais",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "Pais",
                table: "Locais",
                newName: "Endereco");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Locais",
                newName: "LocalId");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Locais",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Locais",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Locais",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Locais",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Eventos",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Contatos",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_UsuarioId",
                table: "Eventos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_UsuarioId",
                table: "Contatos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_AspNetUsers_UsuarioId",
                table: "Contatos",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_AspNetUsers_UsuarioId",
                table: "Eventos",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locais_AspNetUsers_UsuarioId",
                table: "Locais",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_AspNetUsers_UsuarioId",
                table: "Contatos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_AspNetUsers_UsuarioId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Locais_AspNetUsers_UsuarioId",
                table: "Locais");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_UsuarioId",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Contatos_UsuarioId",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Locais");

            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Locais");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Locais",
                newName: "Rua");

            migrationBuilder.RenameColumn(
                name: "Endereco",
                table: "Locais",
                newName: "Pais");

            migrationBuilder.RenameColumn(
                name: "LocalId",
                table: "Locais",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Locais",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Locais",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "UsuarioId",
                table: "Eventos",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId1",
                table: "Eventos",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UsuarioId",
                table: "Contatos",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId1",
                table: "Contatos",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_UsuarioId1",
                table: "Eventos",
                column: "UsuarioId1");

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_UsuarioId1",
                table: "Contatos",
                column: "UsuarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_AspNetUsers_UsuarioId1",
                table: "Contatos",
                column: "UsuarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_AspNetUsers_UsuarioId1",
                table: "Eventos",
                column: "UsuarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locais_AspNetUsers_UsuarioId",
                table: "Locais",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
