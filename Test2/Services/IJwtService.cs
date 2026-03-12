namespace Test2.Services;

public interface IJwtService
{
    string GenerateToken(string userId, string email, string name, string role);
}
