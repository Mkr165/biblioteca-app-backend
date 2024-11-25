using System.Collections.Generic;

namespace backend_registro_livros.Models
{
   
        public class Assunto
        {
            public int CodAs { get; set; } // Coluna CodAs
            public string Descricao { get; set; } // Coluna Descricao

            // Relacionamento: Um assunto pode estar em vários livros
            public ICollection<LivroAssunto> LivrosAssuntos { get; set; }
        }
    
}