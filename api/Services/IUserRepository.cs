namespace Grupo28_JSON.Services;

public interface IUserRepository
{
    UserRecord? FindUser(string username);
    bool UserExists(string username);
    void AddUser(UserRecord user);
}
