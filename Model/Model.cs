namespace Grupo28_JSON;

public sealed class Model
{
    private readonly Dictionary<string, string> baseDadosUtilizadores;

    public Model()
    {
        baseDadosUtilizadores = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["admin"] = "1234",
            ["equipa28"] = "json"
        };
    }

    public LoginResponse Autenticar(LoginRequest? pedido)
    {
        if (pedido is null)
        {
            return CriarErro("INVALID_REQUEST", "Pedido JSON inválido.");
        }

        if (pedido.Username.Equals("db_offline", StringComparison.OrdinalIgnoreCase))
        {
            return CriarErro("DB_OFFLINE", "Base de dados inoperacional");
        }

        if (ValidarCredenciais(pedido.Username, pedido.Password))
        {
            return new LoginResponse
            {
                Success = true,
                Code = "OK",
                Message = "Login efetuado com sucesso!"
            };
        }

        return CriarErro("INVALID_CREDENTIALS", "Username/Password incorreto/s");
    }

    private bool ValidarCredenciais(string username, string password)
    {
        return baseDadosUtilizadores.TryGetValue(username, out var passwordGuardada)
               && passwordGuardada == password;
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
