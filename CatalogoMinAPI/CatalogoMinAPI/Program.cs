using CatalogoMinAPI.ApiEndpoints;
using CatalogoMinAPI.AppServicesExtensions;
using CatalogoMinAPI.Context;
using CatalogoMinAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -- Configure Service --
// Usando serviços
builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors(); 
builder.AddAutenticationJwt();

var app = builder.Build();

// - Métodos Endpoints -
app.MapAutenticacaoEndpoints(); // LOGIN 
app.MapCategoriaEndpoint(); // CATEGORIA
app.MapProdutoEndpoints(); // PRODUTO

// -- Configure --
var environment = app.Environment;
app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();