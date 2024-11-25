using System.Collections.Generic;

namespace backend_registro_livros.Models
{
    public class LivroAssunto
    {
        public int Livro_Codl { get; set; }   // FK para Livro
        public Livro Livro { get; set; }      // Referência para o Livro

        public int Assunto_CodAs { get; set; } // FK para Assunto
        public Assunto Assunto { get; set; }  // Referência para o Assunto
    }
}
