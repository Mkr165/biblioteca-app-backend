using Microsoft.EntityFrameworkCore;
using backend_registro_livros.Models; // Importa o namespace das models

namespace backend_registro_livros.Data
{
    // Classe para mapear os dados da View
    public class RelatorioLivroAutor
    {
        public int AutorId { get; set; }
        public string NomeAutor { get; set; }
        public int LivroId { get; set; }
        public string TituloLivro { get; set; }
        public string Editora { get; set; }
        public string AnoPublicacao { get; set; }
        public string Assuntos { get; set; } // Concatenado no formato de lista (opcional)
    }

    public class BibliotecaContext : DbContext
    {
        // Construtor que recebe as opções do DbContext
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) { }

        // DbSets representam as tabelas no banco de dados
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Assunto> Assuntos { get; set; }
        public DbSet<LivroAutor> LivrosAutores { get; set; }
        public DbSet<LivroAssunto> LivrosAssuntos { get; set; }
        public DbSet<RelatorioLivroAutor> RelatorioLivrosAutores { get; set; } // DbSet para a View

        // Método para configurar o modelo (entidades e relacionamentos)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração para a View
            modelBuilder.Entity<RelatorioLivroAutor>()
                .HasNoKey()
                .ToView("vwrelatoriolivrosagrupados");

            // Configuração para a tabela Livro
            modelBuilder.Entity<Livro>(entity =>
            {
                entity.ToTable("Livro");
                entity.HasKey(l => l.Codl);

                entity.Property(l => l.Titulo)
                    .HasColumnType("VARCHAR(40)")
                    .IsRequired();

                entity.Property(l => l.Editora)
                    .HasColumnType("VARCHAR(40)")
                    .IsRequired();

                entity.Property(l => l.Edicao)
                    .HasColumnType("INTEGER")
                    .IsRequired();

                entity.Property(l => l.AnoPublicacao)
                    .HasColumnType("VARCHAR(4)")
                    .IsRequired();

                entity.Property(l => l.Preco)
                    .HasColumnType("DECIMAL(10, 2)")
                    .IsRequired();

                entity.Property(l => l.FormaCompra)
                    .HasColumnType("VARCHAR(40)")
                    .IsRequired();
            });

            // Configuração para a tabela Autor
            modelBuilder.Entity<Autor>(entity =>
            {
                entity.ToTable("Autor");
                entity.HasKey(a => a.CodAu);

                entity.Property(a => a.Nome)
                    .HasColumnType("VARCHAR(40)")
                    .IsRequired();
            });

            // Configuração para a tabela Assunto
            modelBuilder.Entity<Assunto>(entity =>
            {
                entity.ToTable("Assunto");
                entity.HasKey(a => a.CodAs);

                entity.Property(a => a.Descricao)
                    .HasColumnType("VARCHAR(20)")
                    .IsRequired();
            });

            // Configuração da tabela LivroAutor com chaves compostas
            modelBuilder.Entity<LivroAutor>(entity =>
            {
                entity.ToTable("Livro_Autor");
                entity.HasKey(la => new { la.Livro_Codl, la.Autor_CodAu });

                entity.HasOne(la => la.Livro)
                    .WithMany(l => l.LivrosAutores)
                    .HasForeignKey(la => la.Livro_Codl)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(la => la.Autor)
                    .WithMany(a => a.LivrosAutores)
                    .HasForeignKey(la => la.Autor_CodAu)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração da tabela LivroAssunto com chaves compostas
            modelBuilder.Entity<LivroAssunto>(entity =>
            {
                entity.ToTable("Livro_Assunto");
                entity.HasKey(la => new { la.Livro_Codl, la.Assunto_CodAs });

                entity.HasOne(la => la.Livro)
                    .WithMany(l => l.LivrosAssuntos)
                    .HasForeignKey(la => la.Livro_Codl)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(la => la.Assunto)
                    .WithMany(a => a.LivrosAssuntos)
                    .HasForeignKey(la => la.Assunto_CodAs)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
