namespace DatingApp.Common.Contracts.Response;

public class UserWithRolesResponse
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
