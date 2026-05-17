using System.Text.Json;

namespace Grupo28_JSON.Services;

// Persists users in users.txt (one JSON line per user).
// Passwords are always stored as BCrypt hash, never as plain text.
public sealed class FileUserRepository : IUserRepository
{
    private readonly string _filePath;
    private readonly object _lock = new();
    private readonly ILogger<FileUserRepository> _logger;

    public FileUserRepository(string filePath, ILogger<FileUserRepository> logger)
    {
        _filePath = filePath;
        _logger = logger;
    }

    public UserRecord? FindUser(string username)
    {
        _logger.LogDebug("Searching for user '{Username}' in file repository.", username);
        foreach (var line in ReadLines())
        {
            UserRecord? user;
            try
            {
                user = JsonSerializer.Deserialize<UserRecord>(line);
            }
            catch (JsonException)
            {
                _logger.LogWarning("Skipping malformed line in users file.");
                continue;
            }

            if (user is not null &&
                string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogDebug("User '{Username}' found in file repository.", username);
                return user;
            }
        }

        _logger.LogDebug("User '{Username}' not found in file repository.", username);
        return null;
    }

    public bool UserExists(string username) => FindUser(username) is not null;

    public void AddUser(UserRecord user)
    {
        var line = JsonSerializer.Serialize(user);
        lock (_lock)
        {
            File.AppendAllText(_filePath, line + Environment.NewLine);
        }

        _logger.LogInformation("User '{Username}' appended to users file.", user.Username);
    }

    private IEnumerable<string> ReadLines()
    {
        if (!File.Exists(_filePath))
        {
            _logger.LogDebug("Users file not found at '{FilePath}'.", _filePath);
            yield break;
        }

        foreach (var line in File.ReadAllLines(_filePath))
        {
            if (!string.IsNullOrWhiteSpace(line))
                yield return line;
        }
    }
}
