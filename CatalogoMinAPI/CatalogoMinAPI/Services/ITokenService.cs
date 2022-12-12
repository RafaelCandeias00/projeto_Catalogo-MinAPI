using CatalogoMinAPI.Models;

namespace CatalogoMinAPI.Services
{
    public interface ITokenService
    {
        string GerarToken(string key, string issuer, string audience, UserModel user);
    }
}
