using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using backend_registro_livros.Data;
using backend_registro_livros.Models;
using backend_registro_livros.Models.DTOs; // Importando os DTOs

namespace backend_registro_livros.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LivroAssuntoController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public LivroAssuntoController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/v1/LivroAssunto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LivroAssuntoDTO>>> GetLivrosAssuntos()
        {
            var livrosAssuntos = await _context.LivrosAssuntos
                .Include(la => la.Livro)
                .Include(la => la.Assunto)
                .Select(la => new LivroAssuntoDTO
                {
                    Livro_Codl = la.Livro_Codl,
                    Assunto_CodAs = la.Assunto_CodAs,
                    Livro = new LivroDTO
                    {
                        Codl = la.Livro.Codl,
                        Titulo = la.Livro.Titulo,
                        Editora = la.Livro.Editora,
                        Edicao = la.Livro.Edicao,
                        AnoPublicacao = la.Livro.AnoPublicacao,
                        Preco = la.Livro.Preco,
                        FormaCompra = la.Livro.FormaCompra
                    },
                    Assunto = new AssuntoDTO
                    {
                        CodAs = la.Assunto.CodAs,
                        Descricao = la.Assunto.Descricao
                    }
                })
                .ToListAsync();

            return Ok(livrosAssuntos);
        }

        // GET: api/v1/LivroAssunto/{Livro_Codl}/{Assunto_CodAs}
        [HttpGet("{livroCodl:int}/{assuntoCodAs:int}")]
        public async Task<ActionResult<LivroAssuntoDTO>> GetLivroAssunto(int livroCodl, int assuntoCodAs)
        {
            var livroAssunto = await _context.LivrosAssuntos
                .Include(la => la.Livro)
                .Include(la => la.Assunto)
                .Where(la => la.Livro_Codl == livroCodl && la.Assunto_CodAs == assuntoCodAs)
                .Select(la => new LivroAssuntoDTO
                {
                    Livro_Codl = la.Livro_Codl,
                    Assunto_CodAs = la.Assunto_CodAs,
                    Livro = new LivroDTO
                    {
                        Codl = la.Livro.Codl,
                        Titulo = la.Livro.Titulo,
                        Editora = la.Livro.Editora,
                        Edicao = la.Livro.Edicao,
                        AnoPublicacao = la.Livro.AnoPublicacao,
                        Preco = la.Livro.Preco,
                        FormaCompra = la.Livro.FormaCompra
                    },
                    Assunto = new AssuntoDTO
                    {
                        CodAs = la.Assunto.CodAs,
                        Descricao = la.Assunto.Descricao
                    }
                })
                .FirstOrDefaultAsync();

            if (livroAssunto == null)
            {
                return NotFound("A relação entre o Livro e o Assunto não foi encontrada.");
            }

            return Ok(livroAssunto);
        }

        // POST: api/v1/LivroAssunto
        [HttpPost]
        public async Task<ActionResult<LivroAssuntoDTO>> PostLivroAssunto(LivroAssuntoDTO livroAssuntoDTO)
        {
            if (livroAssuntoDTO == null)
            {
                return BadRequest("Os dados do LivroAssunto não foram fornecidos.");
            }

            var livroExistente = await _context.Livros.FindAsync(livroAssuntoDTO.Livro_Codl);
            var assuntoExistente = await _context.Assuntos.FindAsync(livroAssuntoDTO.Assunto_CodAs);

            if (livroExistente == null)
            {
                return NotFound($"Livro com ID {livroAssuntoDTO.Livro_Codl} não encontrado.");
            }

            if (assuntoExistente == null)
            {
                return NotFound($"Assunto com ID {livroAssuntoDTO.Assunto_CodAs} não encontrado.");
            }

            var relacaoExistente = await _context.LivrosAssuntos
                                                 .AnyAsync(la => la.Livro_Codl == livroAssuntoDTO.Livro_Codl && la.Assunto_CodAs == livroAssuntoDTO.Assunto_CodAs);

            if (relacaoExistente)
            {
                return Conflict("A relação entre o Livro e o Assunto já existe.");
            }

            var livroAssunto = new LivroAssunto
            {
                Livro_Codl = livroAssuntoDTO.Livro_Codl,
                Assunto_CodAs = livroAssuntoDTO.Assunto_CodAs
            };

            try
            {
                _context.LivrosAssuntos.Add(livroAssunto);
                await _context.SaveChangesAsync();

                livroAssuntoDTO.Livro = new LivroDTO
                {
                    Codl = livroExistente.Codl,
                    Titulo = livroExistente.Titulo,
                    Editora = livroExistente.Editora,
                    Edicao = livroExistente.Edicao,
                    AnoPublicacao = livroExistente.AnoPublicacao
                };

                livroAssuntoDTO.Assunto = new AssuntoDTO
                {
                    CodAs = assuntoExistente.CodAs,
                    Descricao = assuntoExistente.Descricao
                };

                return CreatedAtAction(nameof(GetLivroAssunto), new { livroCodl = livroAssuntoDTO.Livro_Codl, assuntoCodAs = livroAssuntoDTO.Assunto_CodAs }, livroAssuntoDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar os dados no banco: {ex.Message}");
            }
        }

        [HttpPut("{livroCodl:int}/{assuntoCodAs:int}")]
        public async Task<IActionResult> PutLivroAssunto(int livroCodl, int assuntoCodAs, LivroAssuntoDTO livroAssuntoDTO)
        {

            // Busca o registro existente
            var livroAssuntoExistente = await _context.LivrosAssuntos
                .FirstOrDefaultAsync(la => la.Livro_Codl == livroCodl && la.Assunto_CodAs == assuntoCodAs);

            if (livroAssuntoExistente == null)
            {
                return NotFound("A relação entre o Livro e o Assunto não foi encontrada.");
            }

            // Valida se o novo Assunto existe no banco
            var novoAssuntoExistente = await _context.Assuntos.FindAsync(livroAssuntoDTO.Assunto_CodAs);
            if (novoAssuntoExistente == null)
            {
                return NotFound($"Assunto com ID {livroAssuntoDTO.Assunto_CodAs} não encontrado.");
            }

            // Remove o registro antigo
            _context.LivrosAssuntos.Remove(livroAssuntoExistente);

            // Cria um novo registro com os dados atualizados
            var novoLivroAssunto = new LivroAssunto
            {
                Livro_Codl = livroAssuntoDTO.Livro_Codl,
                Assunto_CodAs = livroAssuntoDTO.Assunto_CodAs
            };

            try
            {
                _context.LivrosAssuntos.Add(novoLivroAssunto);
                await _context.SaveChangesAsync();
                return Ok("Registros editados com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar os dados no banco: {ex.Message}");
            }
        }

        [HttpDelete("{Livro_Codl:int}/{Assunto_CodAs:int}")]
        public async Task<IActionResult> DeleteLivroAutor(int Livro_Codl, int Assunto_CodAs)
        {
            var LivroAssunto= await _context.LivrosAssuntos
                                           .FirstOrDefaultAsync(la => la.Livro_Codl == Livro_Codl && la.Assunto_CodAs == Assunto_CodAs);

            if (LivroAssunto== null)
            {
                return NotFound();
            }

            _context.LivrosAssuntos.Remove(LivroAssunto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
