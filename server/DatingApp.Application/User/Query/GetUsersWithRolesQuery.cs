using DatingApp.Common.Contracts.Response;
using MediatR;

public class GetUsersWithRolesQuery : IRequest<IEnumerable<UserWithRolesResponse>> { }