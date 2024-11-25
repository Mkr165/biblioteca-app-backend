using System.Collections.Generic;

namespace backend_registro_livros.Models
{
    
        public class LivroAutor
        {
            public int Livro_Codl { get; set; }   // FK para Livro
            public Livro Livro { get; set; }      // Referência para o Livro

            public int Autor_CodAu { get; set; }  // FK para Autor
            public Autor Autor { get; set; }      // Referência para o Autor
        }
    
}