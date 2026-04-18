namespace Grupo28_JSON.Services;

// Stub para desenvolvimento/testes locais.
// Simula o ator externo sem necessidade de um servico real.
// SimulateOffline = true  -> lanca HttpRequestException (ator incontactavel)
// SimulateOffline = false -> valida credenciais a partir de dados de teste
public sealed class StubExternalAuthClient : IExternalAuthClient
{
    private readonly bool _simulateOffline;

    private static readonly Dictionary<string, string> UtilizadoresTeste =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["admin"]     = "1234",
            ["equipa28"]  = "json"
        };

    public StubExternalAuthClient(bool simulateOffline)
    {
        _simulateOffline = simulateOffline;
    }

    public Task<LoginResponse> AutenticarAsync(LoginRequest pedido)
    {
        if (_simulateOffline)
            throw new HttpRequestException("Servico externo indisponivel (stub).");

        if (UtilizadoresTeste.TryGetValue(pedido.Username, out var password)
            && password == pedido.Password)
        {
            return Task.FromResult(new LoginResponse
            {
                Success = true,
                Code    = "OK",
                Message = "Login efetuado com sucesso!"
            });
        }

        return Task.FromResult(new LoginResponse
        {
            Success = false,
            Code    = "INVALID_CREDENTIALS",
            Message = "Username/Password incorreto/s"
        });
    }
}
