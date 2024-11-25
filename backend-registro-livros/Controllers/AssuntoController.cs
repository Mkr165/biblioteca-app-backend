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
    public class AssuntoController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public AssuntoController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/Assunto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assunto>>> GetAssuntos()
        {
            return await _context.Assuntos.ToListAsync();
        }

         // GET: api/Autor/search?nome=SAM
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<AssuntoDTO>>> GetAssuntosPorDescricao(string descricao)
        {
            // Valida se o parâmetro foi fornecido
            if (string.IsNullOrWhiteSpace(descricao))
            {
                return BadRequest("O parâmetro 'descrição' deve ser fornecido.");
            }

            // Filtra os autores com base no nome fornecido (ignora maiúsculas/minúsculas)
            var assuntos = await _context.Assuntos
                .Where(a => EF.Functions.Like(a.Descricao, $"%{descricao}%"))
                .ToListAsync();

            if ( assuntos == null || !assuntos.Any())
            {
                return NotFound($"Nenhum assunto encontrado com a descricao '{descricao}'.");
            }

            return Ok(assuntos);
        }


        // GET: api/Assunto/5
        [HttpGet("{CodAs}")]
        public async Task<ActionResult<Assunto>> GetAssunto(int CodAs)
        {
            var assunto = await _context.Assuntos.FindAsync(CodAs);

            if (assunto == null)
            {
                return NotFound();
            }

            return assunto;
        }

        // POST: api/Assunto
        [HttpPost]
        public async Task<ActionResult<Assunto>> PostAssunto(Assunto assunto)
        {
            _context.Assuntos.Add(assunto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAssunto), new { CodAs = assunto.CodAs }, assunto);
        }

        // PUT: api/Assunto/5
        [HttpPut("{CodAs}")]
        public async Task<IActionResult> PutAssunto(int CodAs, Assunto assunto)
        {
            if (CodAs != assunto.CodAs)
            {
                return BadRequest();
            }

            _context.Entry(assunto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssuntoExists(CodAs))
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

        // DELETE: api/Assunto/5
        [HttpDelete("{CodAs}")]
        public async Task<IActionResult> DeleteAssunto(int CodAs)
        {
            var assunto = await _context.Assuntos.FindAsync(CodAs);
            if (assunto == null)
            {
                return NotFound();
            }

            _context.Assuntos.Remove(assunto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AssuntoExists(int CodAs)
        {
            return _context.Assuntos.Any(e => e.CodAs == CodAs);
        }
    }
}
