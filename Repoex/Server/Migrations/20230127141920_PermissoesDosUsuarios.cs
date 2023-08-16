using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repoex.Server.Migrations
{
    /// <inheritdoc />
    public partial class PermissoesDosUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cotacao",
                table: "Usuarios");

            migrationBuilder.CreateTable(
                name: "Permissoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Relatorio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissoesUsuario",
                columns: table => new
                {
                    PermissoesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuariosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissoesUsuario", x => new { x.PermissoesId, x.UsuariosId });
                    table.ForeignKey(
                        name: "FK_PermissoesUsuario_Permissoes_PermissoesId",
                        column: x => x.PermissoesId,
                        principalTable: "Permissoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissoesUsuario_Usuarios_UsuariosId",
                        column: x => x.UsuariosId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesUsuario_UsuariosId",
                table: "PermissoesUsuario",
                column: "UsuariosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissoesUsuario");

            migrationBuilder.DropTable(
                name: "Permissoes");

            migrationBuilder.AddColumn<string>(
                name: "Cotacao",
                table: "Usuarios",
                type: "varchar(10)",
                nullable: false,
                defaultValue: "");
        }
    }
}
