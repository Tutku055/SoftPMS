using MediatR;
using SoftlarePMS.Application.DTOs.User;

namespace SoftlarePMS.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<IEnumerable<UserDto>>;
