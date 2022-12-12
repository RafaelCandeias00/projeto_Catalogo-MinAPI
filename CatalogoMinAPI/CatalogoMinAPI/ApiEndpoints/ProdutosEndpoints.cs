using CatalogoMinAPI.Context;
using CatalogoMinAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMinAPI.ApiEndpoints
{
    public static class ProdutosEndpoints
    {
        public static void MapProdutoEndpoints(this WebApplication app)
        {
            app.MapGet("/produtos", async (AppDbContext db) =>
                await db.Produtos.ToListAsync()).WithTags("Produtos").RequireAuthorization();

            app.MapGet("/produtos/{id:int}", async (int id, AppDbContext db) =>
            {
                return await db.Produtos.FindAsync(id)
                    is Produto produto ? Results.Ok(produto) : Results.NotFound("Produto não encontrado!");
            }).WithTags("Produtos").RequireAuthorization();

            app.MapPost("/produtos", async (Produto produto, AppDbContext db) =>
            {
                db.Produtos.Add(produto);
                await db.SaveChangesAsync();

                return Results.Created($"/produtos/{produto.ProdutoId}", produto);
            }).WithTags("Produtos").RequireAuthorization();

            app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDbContext db) =>
            {
                if (produto.ProdutoId != id)
                {
                    return Results.BadRequest("Valor incorreto! | Digite um valor válido!");
                }

                var produtoDB = await db.Produtos.FindAsync(id);

                if (produtoDB is null) return Results.NotFound("Produto não encontrado!");

                produtoDB.Nome = produto.Nome;
                produtoDB.Descricao = produto.Descricao;
                produtoDB.Preco = produto.Preco;
                produtoDB.Imagem = produto.Imagem;
                produtoDB.DataCompra = produto.DataCompra;
                produtoDB.Estoque = produto.Estoque;
                produtoDB.CategoriaId = produto.CategoriaId;

                await db.SaveChangesAsync();
                return Results.Ok(produtoDB);
            }).WithTags("Produtos").RequireAuthorization();

            app.MapDelete("/produtos/{id:int}", async (int id, AppDbContext db) =>
            {
                var produto = await db.Produtos.FindAsync(id);

                if (produto is null)
                {
                    return Results.NotFound("Produto não encontrado!");
                }

                db.Produtos.Remove(produto);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }).WithTags("Produtos").RequireAuthorization();
        }
    }
}
