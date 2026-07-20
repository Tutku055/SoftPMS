using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftPMS.Application.Common.Interfaces;
using SoftPMS.Application.DTOs.Role;
using SoftPMS.Domain.Entities;

namespace SoftPMS.Application.Features.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<CreateRoleCommand, RoleDto>
{
    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var exists = await context.Roles.AnyAsync(r => r.Name == request.Dto.Name, cancellationToken);
        if (exists)
            throw new Domain.Exceptions.DomainException($"Role '{request.Dto.Name}' already exists.");

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = request.Dto.Name,
            Description = request.Dto.Description
        };

        await context.Roles.AddAsync(role, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<RoleDto>(role);
    }
}
