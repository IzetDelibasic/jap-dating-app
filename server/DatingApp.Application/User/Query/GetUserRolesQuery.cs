using MediatR;

public class GetUserRolesQuery : IRequest<IEnumerable<string>>
{
    public string Username { get; set; } = string.Empty;
}