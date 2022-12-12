using CatalogoMinAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMinAPI.Context
{
    public class AppDbContext : DbContext
    {
        DbSet<Produto>? Produtos { get; set; }
        DbSet<Categoria>? Categorias { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        // Definindo valores específicos para os atributos dos modelos para o EF Core
        protected override void OnModelCreating(ModelBuilder mb)
        {
            // -- Categoria --
            mb.Entity<Categoria>().HasKey(c => c.CategoriaId); // Definindo chave primária
            mb.Entity<Categoria>().Property(c => c.Nome)
                                  .HasMaxLength(100)
                                  .IsRequired(); // Definindo tamanho máximo da propriedade e que é obrigatória
            mb.Entity<Categoria>().Property(c => c.Descricao).HasMaxLength(150).IsRequired();

            // -- Produto --
            mb.Entity<Produto>().HasKey(p => p.ProdutoId);
            mb.Entity<Produto>().Property(p => p.Nome).HasMaxLength(100).IsRequired();
            mb.Entity<Produto>().Property(p => p.Descricao).HasMaxLength(150).IsRequired();
            mb.Entity<Produto>().Property(p => p.Imagem).HasMaxLength(100);
            mb.Entity<Produto>().Property(p => p.Preco).HasPrecision(14, 2);

            // -- Relacionamento --
            mb.Entity<Produto>()
                .HasOne<Categoria>(c => c.Categoria) // Produto tem uma categoria
                .WithMany(p => p.Produtos) // Muitos produtos
                .HasForeignKey(c => c.CategoriaId); // Chave estrangeira - CategoriaId
        }
    }
}
