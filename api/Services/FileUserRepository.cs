using System.Text.Json;

namespace Grupo28_JSON.Services;

// Persists users in users.txt (one JSON line per user).
// Passwords are always stored as BCrypt hash, never as plain text.
public sealed class FileUserRepository : IUserRepository
{
    private readonly string _filePath;
    private readonly object _lock = new();

    public FileUserRepository(string filePath)
    {
        _filePath = filePath;
    }

    public UserRecord? FindUser(string username)
    {
        foreach (var line in ReadLines())
        {
            var user = JsonSerializer.Deserialize<UserRecord>(line);
            if (user is not null &&
                string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase))
            {
                return user;
            }
        }
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
    }

    private IEnumerable<string> ReadLines()
    {
        if (!File.Exists(_filePath))
            yield break;

        foreach (var line in File.ReadAllLines(_filePath))
        {
            if (!string.IsNullOrWhiteSpace(line))
                yield return line;
        }
    }
}
