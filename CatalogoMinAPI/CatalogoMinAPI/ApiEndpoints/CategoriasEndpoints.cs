using CatalogoMinAPI.Context;
using CatalogoMinAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMinAPI.ApiEndpoints
{
    public static class CategoriasEndpoints
    {
        public static void MapCategoriaEndpoint(this WebApplication app)
        {
            app.MapGet("/categorias", async (AppDbContext db) =>
                await db.Categorias.ToListAsync()).WithTags("Categorias").RequireAuthorization();

            app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
            {
                return await db.Categorias.FindAsync(id)
                    is Categoria categoria ? Results.Ok(categoria) : Results.NotFound("Categoria não encontrado!");
            }).WithTags("Categorias").RequireAuthorization();

            app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
            {
                db.Categorias.Add(categoria);
                await db.SaveChangesAsync();

                return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
            }).WithTags("Categorias").RequireAuthorization();
            // Código Alternativo
            /*app.MapPost("/categorias", async ([FromBody] Categoria categoria, [FromServices] AppDbContext db) =>
            {
                db.Categorias.Add(categoria);
                await db.SaveChangesAsync();

                return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
            }).Accepts<Categoria>("application/json")
              .Produces<Categoria>(StatusCodes.Status201Created)
              .WithName("CriarNovaCategoria")
              .WithTags("Setter");*/

            app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDbContext db) =>
            {
                if (categoria.CategoriaId != id)
                {
                    return Results.BadRequest("Valor incorreto! | Digite um valor válido!");
                }

                var categoriaDB = await db.Categorias.FindAsync(id);

                if (categoriaDB is null) return Results.NotFound("Categoria não encontrado!");

                categoriaDB.Nome = categoria.Nome;
                categoriaDB.Descricao = categoria.Descricao;

                await db.SaveChangesAsync();
                return Results.Ok(categoriaDB);
            }).WithTags("Categorias").RequireAuthorization();

            app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
            {
                var categoria = await db.Categorias.FindAsync(id);

                if (categoria is null)
                {
                    return Results.NotFound("Categoria não encontrado!");
                }

                db.Categorias.Remove(categoria);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }).WithTags("Categorias").RequireAuthorization();
        }
    }
}
