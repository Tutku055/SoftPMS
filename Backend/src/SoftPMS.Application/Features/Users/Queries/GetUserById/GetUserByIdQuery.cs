using MediatR;
using SoftPMS.Application.DTOs.User;

namespace SoftPMS.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;
