using System.Collections.Generic;

namespace backend_registro_livros.Models.DTOs
{
    public class LivroAssuntoDTO
    {
        public int Livro_Codl { get; set; }
        public int Assunto_CodAs { get; set; }
        public LivroDTO Livro { get; set; }
        public AssuntoDTO Assunto { get; set; }
    }
}