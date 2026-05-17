using Grupo28_JSON.Services;

namespace Grupo28_JSON;

public sealed class Model
{
    private readonly IUserRepository _userRepo;
    private readonly ILogger<Model> _logger;

    public Model(IUserRepository userRepo, ILogger<Model> logger)
    {
        _userRepo = userRepo;
        _logger = logger;
    }

    public Task<LoginResponse> AuthenticateAsync(LoginRequest? request)
    {
        if (request is null)
        {
            _logger.LogWarning("Login failed: request payload is null.");
            return Task.FromResult(CreateLoginError("INVALID_REQUEST", "Invalid JSON request."));
        }

        var user = _userRepo.FindUser(request.Username);
        if (user is null)
        {
            _logger.LogWarning("Login failed: user '{Username}' not found.", request.Username);
            return Task.FromResult(CreateLoginError("INVALID_CREDENTIALS", "Invalid username/password."));
        }

        var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isValid)
        {
            _logger.LogWarning("Login failed: invalid password for user '{Username}'.", request.Username);
        }
        else
        {
            _logger.LogInformation("Login successful for user '{Username}'.", request.Username);
        }

        return Task.FromResult(isValid
            ? new LoginResponse { Success = true, Code = "OK", Message = "Login successful." }
            : CreateLoginError("INVALID_CREDENTIALS", "Invalid username/password."));
    }

    public Task<RegisterResponse> RegisterAsync(RegisterRequest? request)
    {
        if (request is null)
        {
            _logger.LogWarning("Register failed: request payload is null.");
            return Task.FromResult(CreateRegisterError("INVALID_REQUEST", "Invalid JSON request."));
        }

        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.Email))
        {
            _logger.LogWarning("Register failed: missing required fields for user '{Username}'.", request.Username);
            return Task.FromResult(CreateRegisterError("MISSING_FIELDS", "Username, password and email are required."));
        }

        if (_userRepo.UserExists(request.Username))
        {
            _logger.LogWarning("Register failed: username '{Username}' already exists.", request.Username);
            return Task.FromResult(CreateRegisterError("USERNAME_TAKEN", "Username is already in use."));
        }

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        _userRepo.AddUser(new UserRecord
        {
            Username = request.Username,
            PasswordHash = hash,
            Email = request.Email
        });

        _logger.LogInformation("User '{Username}' registered successfully.", request.Username);

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
