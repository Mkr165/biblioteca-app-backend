using System.Collections.Generic;

namespace backend_registro_livros.Models
{
    public class Autor
    {
        public int CodAu { get; set; }  // Essa propriedade se tornará uma coluna chamada CodAu
        public string Nome { get; set; } // Essa propriedade se tornará uma coluna chamada Nome

        // Relacionamento: Um autor pode ter vários livros
        public ICollection<LivroAutor> LivrosAutores { get; set; }
    }

}