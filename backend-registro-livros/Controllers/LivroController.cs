using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_registro_livros.Data;
using backend_registro_livros.Models;
using backend_registro_livros.Models.DTOs;
using System.IO;
using FastReport;
using FastReport.Export.PdfSimple;

using System;
namespace backend_registro_livros.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public LivroController(BibliotecaContext context)
        {
            _context = context;
        }

        // Rota de pesquisa de livros por título
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> SearchLivros([FromQuery] string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                return BadRequest("O parâmetro de pesquisa não pode estar vazio.");
            }

            var livros = await _context.Livros
                .Where(l => l.Titulo.Contains(titulo))
                .Select(l => new LivroDTO
                {
                    Codl = l.Codl,
                    Titulo = l.Titulo,
                    Editora = l.Editora,
                    Edicao = l.Edicao,
                    AnoPublicacao = l.AnoPublicacao,
                    Preco = l.Preco,
                    FormaCompra = l.FormaCompra
                })
                .ToListAsync();

            return Ok(livros);
        }

        // Rota para obter todos os livros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> GetLivros()
        {
            var livros = await _context.Livros
                .Select(l => new LivroDTO
                {
                    Codl = l.Codl,
                    Titulo = l.Titulo,
                    Editora = l.Editora,
                    Edicao = l.Edicao,
                    AnoPublicacao = l.AnoPublicacao,
                    Preco = l.Preco,
                    FormaCompra = l.FormaCompra
                })
                .ToListAsync();

            return Ok(livros);
        }

        // Rota para obter um livro específico por ID
        [HttpGet("{Codl}")]
        public async Task<ActionResult<LivroDTO>> GetLivro(int Codl)
        {
            var livro = await _context.Livros
                .Where(l => l.Codl == Codl)
                .Select(l => new LivroDTO
                {
                    Codl = l.Codl,
                    Titulo = l.Titulo,
                    Editora = l.Editora,
                    Edicao = l.Edicao,
                    AnoPublicacao = l.AnoPublicacao,
                    Preco = l.Preco,
                    FormaCompra = l.FormaCompra
                })
                .FirstOrDefaultAsync();

            if (livro == null)
            {
                return NotFound("Livro não encontrado.");
            }

            return Ok(livro);
        }

        // Rota para cadastrar um novo livro
        [HttpPost]
        public async Task<ActionResult<Livro>> PostLivro(Livro livro)
        {
            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLivro), new { Codl = livro.Codl }, livro);
        }

        // Rota para atualizar um livro existente
        [HttpPut("{Codl}")]
        public async Task<IActionResult> PutLivro(int Codl, Livro livro)
        {
            if (Codl != livro.Codl)
            {
                return BadRequest("O código fornecido não corresponde ao código do livro.");
            }

            _context.Entry(livro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LivroExists(Codl))
                {
                    return NotFound("Livro não encontrado para atualização.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Rota para deletar um livro
        [HttpDelete("{Codl}")]
        public async Task<IActionResult> DeleteLivro(int Codl)
        {
            var livro = await _context.Livros.FindAsync(Codl);
            if (livro == null)
            {
                return NotFound("Livro não encontrado para exclusão.");
            }

            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método auxiliar para verificar se um livro existe
        private bool LivroExists(int Codl)
        {
            return _context.Livros.Any(e => e.Codl == Codl);
        }

        [HttpGet("gerar-relatorio")]
        public IActionResult GerarRelatorio()
        {
            try
            {
                // Caminho do template
                var caminhoTemplate = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "RelatorioLivrosAutores.frx");

                // Carrega o relatório
                using var relatorio = new FastReport.Report();
                relatorio.Load(caminhoTemplate);

                // Conecta ao banco de dados
                relatorio.Dictionary.Connections[0].ConnectionString = _context.Database.GetDbConnection().ConnectionString;

                // Preenche os dados
                relatorio.Prepare();

                // Exporta para PDF
                using var memoryStream = new MemoryStream();
                var pdfExport = new PDFSimpleExport();
                relatorio.Export(pdfExport, memoryStream);

                // Certifique-se de que o stream está na posição inicial
                memoryStream.Position = 0;

                // Retorna o arquivo como resposta
                return File(memoryStream.ToArray(), "application/pdf", "RelatorioLivrosAutores.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao gerar relatório: {ex.Message}");
            }
        } 

    }
}
