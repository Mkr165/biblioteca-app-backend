using System.Collections.Generic;

namespace backend_registro_livros.Models.DTOs
{
    public class LivroAutorDTO
    {
        public int Livro_Codl { get; set; }
        public int Autor_CodAu { get; set; }
        public LivroDTO Livro { get; set; }
        public AutorDTO Autor { get; set; }
    }
}
