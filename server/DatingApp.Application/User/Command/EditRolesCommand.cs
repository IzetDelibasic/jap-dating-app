using MediatR;

public class EditRolesCommand : IRequest<bool>
{
    public string Username { get; set; } = string.Empty;
    public string Roles { get; set; } = string.Empty;
}