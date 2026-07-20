using MediatR;
using SoftPMS.Application.DTOs.User;

namespace SoftPMS.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<IEnumerable<UserDto>>;
