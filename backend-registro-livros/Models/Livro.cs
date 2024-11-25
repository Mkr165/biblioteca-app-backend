using System.Collections.Generic;

namespace backend_registro_livros.Models
{
    public class Livro
    {
        public int Codl { get; set; }           // Coluna Codl
        public string Titulo { get; set; }      // Coluna Titulo
        public string Editora { get; set; }     // Coluna Editora
        public int Edicao { get; set; }         // Coluna Edicao
        public string AnoPublicacao { get; set; } // Coluna AnoPublicacao
        public decimal Preco { get; set; }      // Coluna Preco
        public string FormaCompra { get; set; } // Coluna FormaCompra

        // Relacionamentos
        public ICollection<LivroAutor> LivrosAutores { get; set; }
        public ICollection<LivroAssunto> LivrosAssuntos { get; set; }
    }
    
}