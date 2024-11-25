using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_registro_livros.Data;
using backend_registro_livros.Models;
using backend_registro_livros.Models.DTOs;
namespace backend_registro_livros.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public AutorController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/Autor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutores()
        {
            return await _context.Autores.ToListAsync();
        }

        // GET: api/Autor/5
        [HttpGet("{CodAu}")]
        public async Task<ActionResult<Autor>> GetAutor(int CodAu)
        {
            var autor = await _context.Autores.FindAsync(CodAu);

            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        // GET: api/Autor/search?nome=SAM
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutoresPorNome(string nome)
        {
            // Valida se o parâmetro foi fornecido
            if (string.IsNullOrWhiteSpace(nome))
            {
                return BadRequest("O parâmetro 'nome' deve ser fornecido.");
            }

            // Filtra os autores com base no nome fornecido (ignora maiúsculas/minúsculas)
            var autores = await _context.Autores
                .Where(a => EF.Functions.Like(a.Nome, $"%{nome}%"))
                .ToListAsync();

            if (autores == null || !autores.Any())
            {
                return NotFound($"Nenhum autor encontrado com o nome: '{nome}'.");
            }

            return Ok(autores);
        }


        // POST: api/Autor
        [HttpPost]
        public async Task<ActionResult<Autor>> PostAutor(Autor autor)
        {
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAutor), new { CodAu = autor.CodAu }, autor);
        }

        // PUT: api/Autor/5
        [HttpPut("{CodAu}")]
        public async Task<IActionResult> PutAutor(int CodAu, Autor autor)
        {
            if (CodAu != autor.CodAu)
            {
                return BadRequest();
            }

            _context.Entry(autor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutorExists(CodAu))
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

        // DELETE: api/Autor/5
        [HttpDelete("{CodAu}")]
        public async Task<IActionResult> DeleteAutor(int CodAu)
        {
            var autor = await _context.Autores.FindAsync(CodAu);
            if (autor == null)
            {
                return NotFound();
            }

            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AutorExists(int CodAu)
        {
            return _context.Autores.Any(e => e.CodAu == CodAu);
        }
    }
}
