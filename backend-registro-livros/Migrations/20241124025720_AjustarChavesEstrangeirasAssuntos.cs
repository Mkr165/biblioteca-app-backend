using Microsoft.EntityFrameworkCore.Migrations;

namespace backend_registro_livros.Migrations
{
    public partial class AjustarChavesEstrangeirasAssuntos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livro_Assunto_Assunto_AssuntoCodAs",
                table: "Livro_Assunto");

            migrationBuilder.DropForeignKey(
                name: "FK_Livro_Assunto_Livro_LivroCodl",
                table: "Livro_Assunto");

            migrationBuilder.DropIndex(
                name: "IX_Livro_Assunto_AssuntoCodAs",
                table: "Livro_Assunto");

            migrationBuilder.DropIndex(
                name: "IX_Livro_Assunto_LivroCodl",
                table: "Livro_Assunto");

            migrationBuilder.DropColumn(
                name: "AssuntoCodAs",
                table: "Livro_Assunto");

            migrationBuilder.DropColumn(
                name: "LivroCodl",
                table: "Livro_Assunto");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_Assunto_Assunto_CodAs",
                table: "Livro_Assunto",
                column: "Assunto_CodAs");

            migrationBuilder.AddForeignKey(
                name: "FK_Livro_Assunto_Assunto_Assunto_CodAs",
                table: "Livro_Assunto",
                column: "Assunto_CodAs",
                principalTable: "Assunto",
                principalColumn: "CodAs",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Livro_Assunto_Livro_Livro_Codl",
                table: "Livro_Assunto",
                column: "Livro_Codl",
                principalTable: "Livro",
                principalColumn: "Codl",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livro_Assunto_Assunto_Assunto_CodAs",
                table: "Livro_Assunto");

            migrationBuilder.DropForeignKey(
                name: "FK_Livro_Assunto_Livro_Livro_Codl",
                table: "Livro_Assunto");

            migrationBuilder.DropIndex(
                name: "IX_Livro_Assunto_Assunto_CodAs",
                table: "Livro_Assunto");

            migrationBuilder.AddColumn<int>(
                name: "AssuntoCodAs",
                table: "Livro_Assunto",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LivroCodl",
                table: "Livro_Assunto",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Livro_Assunto_AssuntoCodAs",
                table: "Livro_Assunto",
                column: "AssuntoCodAs");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_Assunto_LivroCodl",
                table: "Livro_Assunto",
                column: "LivroCodl");

            migrationBuilder.AddForeignKey(
                name: "FK_Livro_Assunto_Assunto_AssuntoCodAs",
                table: "Livro_Assunto",
                column: "AssuntoCodAs",
                principalTable: "Assunto",
                principalColumn: "CodAs",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Livro_Assunto_Livro_LivroCodl",
                table: "Livro_Assunto",
                column: "LivroCodl",
                principalTable: "Livro",
                principalColumn: "Codl",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
