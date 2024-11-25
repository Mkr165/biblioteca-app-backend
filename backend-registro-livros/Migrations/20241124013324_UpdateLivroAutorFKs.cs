using Microsoft.EntityFrameworkCore.Migrations;

namespace backend_registro_livros.Migrations
{
    public partial class UpdateLivroAutorFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livro_Autor_Autor_AutorCodAu",
                table: "Livro_Autor");

            migrationBuilder.DropForeignKey(
                name: "FK_Livro_Autor_Livro_LivroCodl",
                table: "Livro_Autor");

            migrationBuilder.DropIndex(
                name: "IX_Livro_Autor_AutorCodAu",
                table: "Livro_Autor");

            migrationBuilder.DropIndex(
                name: "IX_Livro_Autor_LivroCodl",
                table: "Livro_Autor");

            migrationBuilder.DropColumn(
                name: "AutorCodAu",
                table: "Livro_Autor");

            migrationBuilder.DropColumn(
                name: "LivroCodl",
                table: "Livro_Autor");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_Autor_Autor_CodAu",
                table: "Livro_Autor",
                column: "Autor_CodAu");

            migrationBuilder.AddForeignKey(
                name: "FK_Livro_Autor_Autor_Autor_CodAu",
                table: "Livro_Autor",
                column: "Autor_CodAu",
                principalTable: "Autor",
                principalColumn: "CodAu",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Livro_Autor_Livro_Livro_Codl",
                table: "Livro_Autor",
                column: "Livro_Codl",
                principalTable: "Livro",
                principalColumn: "Codl",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livro_Autor_Autor_Autor_CodAu",
                table: "Livro_Autor");

            migrationBuilder.DropForeignKey(
                name: "FK_Livro_Autor_Livro_Livro_Codl",
                table: "Livro_Autor");

            migrationBuilder.DropIndex(
                name: "IX_Livro_Autor_Autor_CodAu",
                table: "Livro_Autor");

            migrationBuilder.AddColumn<int>(
                name: "AutorCodAu",
                table: "Livro_Autor",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LivroCodl",
                table: "Livro_Autor",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Livro_Autor_AutorCodAu",
                table: "Livro_Autor",
                column: "AutorCodAu");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_Autor_LivroCodl",
                table: "Livro_Autor",
                column: "LivroCodl");

            migrationBuilder.AddForeignKey(
                name: "FK_Livro_Autor_Autor_AutorCodAu",
                table: "Livro_Autor",
                column: "AutorCodAu",
                principalTable: "Autor",
                principalColumn: "CodAu",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Livro_Autor_Livro_LivroCodl",
                table: "Livro_Autor",
                column: "LivroCodl",
                principalTable: "Livro",
                principalColumn: "Codl",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
