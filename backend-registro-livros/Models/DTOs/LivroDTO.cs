
using System.Collections.Generic;

namespace backend_registro_livros.Models.DTOs
{
    public class LivroDTO
    {
        public int Codl { get; set; }
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public string AnoPublicacao { get; set; }
        public decimal Preco { get; set; }
        public string FormaCompra { get; set; }
        public List<AutorDTO> Autores { get; set; }
        public List<AssuntoDTO> Assuntos { get; set; }
    }
}