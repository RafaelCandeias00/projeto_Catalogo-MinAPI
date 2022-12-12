using CatalogoMinAPI.Models;
using CatalogoMinAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace CatalogoMinAPI.ApiEndpoints
{
    public static class AutenticacaoEndpoints
    {
        public static void MapAutenticacaoEndpoints(this WebApplication app)
        {
            app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
            {
                if (userModel == null)
                {
                    return Results.BadRequest("Login Inválido!");
                }
                if (userModel.Username == "rafael" && userModel.Password == "num123")
                {
                    var tokenString = tokenService.GerarToken(app.Configuration["Jwt:Key"],
                        app.Configuration["Jwt:Issuer"],
                        app.Configuration["Jwt:Audience"],
                        userModel);
                    return Results.Ok(new { token = tokenString });
                }
                else
                {
                    return Results.BadRequest("Login inválido!");
                }
            }).Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status200OK)
                .WithName("Login")
                .WithTags("Autenticacao");
        }
    }
}
