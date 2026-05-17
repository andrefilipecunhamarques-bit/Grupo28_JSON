using Grupo28_JSON.Services;

namespace Grupo28_JSON;

public sealed class Model
{
    private readonly IUserRepository _userRepo;

    public Model(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public Task<LoginResponse> AuthenticateAsync(LoginRequest? request)
    {
        if (request is null)
            return Task.FromResult(CreateLoginError("INVALID_REQUEST", "Invalid JSON request."));

        var user = _userRepo.FindUser(request.Username);
        if (user is null)
            return Task.FromResult(CreateLoginError("INVALID_CREDENTIALS", "Invalid username/password."));

        var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        return Task.FromResult(isValid
            ? new LoginResponse { Success = true, Code = "OK", Message = "Login successful." }
            : CreateLoginError("INVALID_CREDENTIALS", "Invalid username/password."));
    }

    public Task<RegisterResponse> RegisterAsync(RegisterRequest? request)
    {
        if (request is null)
            return Task.FromResult(CreateRegisterError("INVALID_REQUEST", "Invalid JSON request."));

        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.Email))
        {
            return Task.FromResult(CreateRegisterError("MISSING_FIELDS", "Username, password and email are required."));
        }

        if (_userRepo.UserExists(request.Username))
            return Task.FromResult(CreateRegisterError("USERNAME_TAKEN", "Username is already in use."));

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        _userRepo.AddUser(new UserRecord
        {
            Username = request.Username,
            PasswordHash = hash,
            Email = request.Email
        });

        return Task.FromResult(new RegisterResponse
        {
            Success = true,
            Code = "CREATED",
            Message = "User registered successfully."
        });
    }

    private static LoginResponse CreateLoginError(string code, string message) =>
        new() { Success = false, Code = code, Message = message };

    private static RegisterResponse CreateRegisterError(string code, string message) =>
        new() { Success = false, Code = code, Message = message };
}
