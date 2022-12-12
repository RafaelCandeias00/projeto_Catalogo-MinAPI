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
app.MapGet("/", () => "Cat�logo de Produtos - 2022").ExcludeFromDescription();

app.MapGet("/categorias", async (AppDbContext db) =>
    await db.Categorias.ToListAsync());

app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Categorias.FindAsync(id) 
        is Categoria categoria ? Results.Ok(categoria) : Results.NotFound("Categoria n�o encontrado!");
});

app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
{
    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
});
// C�digo Alternativo
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
        return Results.BadRequest();
    }

    var categoriaDB = await db.Categorias.FindAsync(id);

    if (categoriaDB is null) return Results.NotFound();

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
        return Results.NotFound();
    }

    db.Categorias.Remove(categoria);
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