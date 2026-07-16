using MediatR;
using SoftlarePMS.Application.DTOs.User;

namespace SoftlarePMS.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;
