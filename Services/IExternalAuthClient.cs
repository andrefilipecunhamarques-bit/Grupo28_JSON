namespace Grupo28_JSON.Services;

public interface IExternalAuthClient
{
    Task<LoginResponse> AutenticarAsync(LoginRequest pedido);
}
