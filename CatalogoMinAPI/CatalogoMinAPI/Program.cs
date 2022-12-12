using CatalogoMinAPI.Context;
using CatalogoMinAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -- Configure Service --
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(opt => 
    opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)
));

var app = builder.Build();

// -- Denifindo Endpoints --

// -- Categorias
app.MapGet("/", () => "Catálogo de Produtos - 2022").ExcludeFromDescription();

app.MapGet("/categorias", async (AppDbContext db) =>
    await db.Categorias.ToListAsync());

app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Categorias.FindAsync(id) 
        is Categoria categoria ? Results.Ok(categoria) : Results.NotFound("Categoria não encontrado!");
});

app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
{
    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
});
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

app.MapPut("/categorias/{id:int}", async(int id, Categoria categoria, AppDbContext db) =>
{
    if(categoria.CategoriaId != id)
    {
        return Results.BadRequest("Valor incorreto! | Digite um valor válido!");
    }

    var categoriaDB = await db.Categorias.FindAsync(id);

    if (categoriaDB is null) return Results.NotFound("Categoria não encontrado!");

    categoriaDB.Nome = categoria.Nome;
    categoriaDB.Descricao = categoria.Descricao;

    await db.SaveChangesAsync();
    return Results.Ok(categoriaDB);
});

app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    var categoria = await db.Categorias.FindAsync(id);

    if(categoria is null)
    {
        return Results.NotFound("Categoria não encontrado!");
    }

    db.Categorias.Remove(categoria);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// -- Produto
app.MapGet("/produtos", async (AppDbContext db) => 
    await db.Produtos.ToListAsync());

app.MapGet("/produtos/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Produtos.FindAsync(id)
        is Produto produto ? Results.Ok(produto) : Results.NotFound("Produto não encontrado!");
});

app.MapPost("/produtos", async(Produto produto, AppDbContext db) =>
{
    db.Produtos.Add(produto);
    await db.SaveChangesAsync();

    return Results.Created($"/produtos/{produto.ProdutoId}", produto);
});

app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDbContext db) =>
{
    if(produto.ProdutoId != id)
    {
        return Results.BadRequest("Valor incorreto! | Digite um valor válido!");
    }

    var produtoDB = await db.Produtos.FindAsync(id);

    if(produtoDB is null) return Results.NotFound("Produto não encontrado!");

    produtoDB.Nome = produto.Nome;
    produtoDB.Descricao= produto.Descricao;
    produtoDB.Preco= produto.Preco;
    produtoDB.Imagem= produto.Imagem;
    produtoDB.DataCompra=produto.DataCompra;
    produtoDB.Estoque= produto.Estoque;
    produtoDB.CategoriaId = produto.CategoriaId;

    await db.SaveChangesAsync();
    return Results.Ok(produtoDB);
});

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
});

// -- Configure --
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();