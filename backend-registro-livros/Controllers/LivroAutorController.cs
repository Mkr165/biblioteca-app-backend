using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_registro_livros.Data;
using backend_registro_livros.Models;
using backend_registro_livros.Models.DTOs;  // Importando o namespace dos DTOs

namespace backend_registro_livros.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LivroAutorController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public LivroAutorController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/v1/LivroAutor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> GetLivroAutores()
        {
            // Projeção para evitar ciclos de referência
            var livroAutores = await _context.LivrosAutores
                .Include(la => la.Livro)
                .Include(la => la.Autor)
                .Select(la => new LivroDTO
                {
                    Codl = la.Livro.Codl,
                    Titulo = la.Livro.Titulo,
                    Editora = la.Livro.Editora,
                    Edicao = la.Livro.Edicao,
                    AnoPublicacao = la.Livro.AnoPublicacao,
                    Preco = la.Livro.Preco,
                    FormaCompra = la.Livro.FormaCompra,
                    Autores = la.Livro.LivrosAutores.Select(la => new AutorDTO
                    {
                        CodAu = la.Autor.CodAu,
                        Nome = la.Autor.Nome
                    }).ToList()
                })
                .ToListAsync();

            return Ok(livroAutores);
        }

        // GET: api/v1/LivroAutor/5/10
        [HttpGet("{Livro_Codl:int}/{Autor_CodAu:int}")]
        public async Task<ActionResult<LivroDTO>> GetLivroAutor(int Livro_Codl, int Autor_CodAu)
        {
            var livroAutor = await _context.LivrosAutores
                                           .Include(la => la.Livro)
                                           .Include(la => la.Autor)
                                           .Where(la => la.Livro_Codl == Livro_Codl && la.Autor_CodAu == Autor_CodAu)
                                           .Select(la => new LivroDTO
                                           {
                                               Codl = la.Livro.Codl,
                                               Titulo = la.Livro.Titulo,
                                               Editora = la.Livro.Editora,
                                               Edicao = la.Livro.Edicao,
                                               AnoPublicacao = la.Livro.AnoPublicacao,
                                               Preco = la.Livro.Preco,
                                               FormaCompra = la.Livro.FormaCompra,
                                               Autores = new List<AutorDTO>
                                               {
                                                   new AutorDTO
                                                   {
                                                       CodAu = la.Autor.CodAu,
                                                       Nome = la.Autor.Nome
                                                   }
                                               }
                                           })
                                           .FirstOrDefaultAsync();

            if (livroAutor == null)
            {
                return NotFound();
            }

            return Ok(livroAutor);
        }


        // POST: api/v1/LivroAutor
        [HttpPost]
        public async Task<ActionResult<LivroAutorDTO>> PostLivroAutor(LivroAutor livroAutor)
        {
            // Verifica se as chaves estrangeiras Livro e Autor existem antes de adicionar
            var livroExistente = await _context.Livros.FindAsync(livroAutor.Livro_Codl);
            var autorExistente = await _context.Autores.FindAsync(livroAutor.Autor_CodAu);

            if (livroExistente == null || autorExistente == null)
            {
                return BadRequest("Livro ou Autor não encontrado.");
            }

            try
            {
                _context.LivrosAutores.Add(livroAutor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Em caso de duplicidade de chaves compostas
                if (LivroAutorExists(livroAutor.Livro_Codl, livroAutor.Autor_CodAu))
                {
                    return Conflict("A relação entre Livro e Autor já existe.");
                }
                else
                {
                    throw;
                }
            }

            // Criar o DTO para o LivroAutor
            var livroAutorDTO = new LivroAutorDTO
            {
                Livro_Codl = livroAutor.Livro_Codl,
                Autor_CodAu = livroAutor.Autor_CodAu,
                Livro = new LivroDTO
                {
                    Codl = livroAutor.Livro.Codl,
                    Titulo = livroAutor.Livro.Titulo,
                    Editora = livroAutor.Livro.Editora,
                    Edicao = livroAutor.Livro.Edicao,
                    AnoPublicacao = livroAutor.Livro.AnoPublicacao,
                    Preco = livroAutor.Livro.Preco,
                    FormaCompra = livroAutor.Livro.FormaCompra
                },
                Autor = new AutorDTO
                {
                    CodAu = livroAutor.Autor.CodAu,
                    Nome = livroAutor.Autor.Nome
                }
            };

            return CreatedAtAction(nameof(GetLivroAutor), new { Livro_Codl = livroAutor.Livro_Codl, Autor_CodAu = livroAutor.Autor_CodAu }, livroAutorDTO);
        }

        // PUT: api/v1/LivroAutor/5/10
        [HttpPut("{Livro_Codl:int}/{Autor_CodAu:int}")]
        public async Task<IActionResult> PutLivroAutor(int Livro_Codl, int Autor_CodAu, LivroAutor livroAutor)
        {
            if (Livro_Codl != livroAutor.Livro_Codl || Autor_CodAu != livroAutor.Autor_CodAu)
            {
                return BadRequest("Os parâmetros fornecidos não correspondem ao objeto.");
            }

            var livroExistente = await _context.Livros.FindAsync(livroAutor.Livro_Codl);
            var autorExistente = await _context.Autores.FindAsync(livroAutor.Autor_CodAu);

            if (livroExistente == null || autorExistente == null)
            {
                return BadRequest("Livro ou Autor não encontrado.");
            }

            _context.Entry(livroAutor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LivroAutorExists(Livro_Codl, Autor_CodAu))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/v1/LivroAutor/5/10
        [HttpDelete("{Livro_Codl:int}/{Autor_CodAu:int}")]
        public async Task<IActionResult> DeleteLivroAutor(int Livro_Codl, int Autor_CodAu)
        {
            var livroAutor = await _context.LivrosAutores
                                           .FirstOrDefaultAsync(la => la.Livro_Codl == Livro_Codl && la.Autor_CodAu == Autor_CodAu);

            if (livroAutor == null)
            {
                return NotFound();
            }

            _context.LivrosAutores.Remove(livroAutor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LivroAutorExists(int Livro_Codl, int Autor_CodAu)
        {
            return _context.LivrosAutores.Any(e => e.Livro_Codl == Livro_Codl && e.Autor_CodAu == Autor_CodAu);
        }
    }
}
