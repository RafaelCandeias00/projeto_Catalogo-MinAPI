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
    }
}
