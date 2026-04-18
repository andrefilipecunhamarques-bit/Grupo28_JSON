using Grupo28_JSON.Services;

namespace Grupo28_JSON;

// O Model coordena o fluxo: valida o pedido e delega a autenticacao
// ao ator externo via IExternalAuthClient.
// Se o ator externo nao responder (HttpRequestException), sinaliza DB_OFFLINE.
public sealed class Model
{
    private readonly IExternalAuthClient _authClient;

    public Model(IExternalAuthClient authClient)
    {
        _authClient = authClient;
    }

    public async Task<LoginResponse> AutenticarAsync(LoginRequest? pedido)
    {
        if (pedido is null)
        {
            return CriarErro("INVALID_REQUEST", "Pedido JSON inválido.");
        }

        try
        {
            // Output para o ator externo; aguarda o input (resposta) dele.
            return await _authClient.AutenticarAsync(pedido);
        }
        catch (HttpRequestException)
        {
            // O ator externo estava incontactavel.
            return CriarErro("DB_OFFLINE", "Base de dados inoperacional");
        }
    }

    private static LoginResponse CriarErro(string code, string message)
    {
        return new LoginResponse
        {
            Success = false,
            Code = code,
            Message = message
        };
    }
}
