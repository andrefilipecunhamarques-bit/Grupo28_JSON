using System.Net.Http.Json;

namespace Grupo28_JSON.Services;

// Cliente real: envia o pedido HTTP ao ator externo e recebe a resposta.
// Se o ator nao responder (rede, timeout, etc.) lanca HttpRequestException.
public sealed class ExternalAuthClient : IExternalAuthClient
{
    private readonly HttpClient _httpClient;

    public ExternalAuthClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResponse> AutenticarAsync(LoginRequest pedido)
    {
        var response = await _httpClient.PostAsJsonAsync("login", pedido);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<LoginResponse>()
               ?? throw new HttpRequestException("Resposta invalida recebida do servico externo.");
    }
}
